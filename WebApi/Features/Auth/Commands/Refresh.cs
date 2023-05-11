using System.Text.Json;
using FluentValidation.Results;
using Mapster;
using MediatR;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Listener;
using WebApi.Common.Exceptions;
using WebApi.Core.Auth.Models;
using WebApi.Core.Auth.Services;
using WebApi.Core.Cryptography.Models;
using WebApi.Core.Cryptography.Services;
using WebApi.Features.Auth.Jobs;
using WebApi.Features.Auth.Options;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Auth.Commands;

public static class Refresh
{
    public class Command : IRequest<Response>
    {
        public string? RefreshToken { get; set; }
    }

    public class Response
    {
        public ResponseUser User { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }

    public class ResponseUser
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        public string? RealName { get; set; }
        public string? Class { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly EncryptionManager _encryptionManager;
        private readonly EncryptionService _encryptionService;
        private readonly HashManager _hashManager;
        private readonly RefreshOptions _refreshOptions;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly SessionManager _sessionManager;

        public Handler(ISchedulerFactory schedulerFactory, ApplicationDbContext context,
            SessionManager sessionManager, EncryptionManager encryptionManager, EncryptionService encryptionService,
            HashManager hashManager, IOptions<RefreshOptions> refreshOptions)
        {
            _schedulerFactory = schedulerFactory;
            _context = context;
            _sessionManager = sessionManager;
            _encryptionManager = encryptionManager;
            _encryptionService = encryptionService;
            _hashManager = hashManager;
            _refreshOptions = refreshOptions.Value;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.RefreshToken is null)
                throw new ValidationException(new[]
                    { new ValidationFailure(nameof(request.RefreshToken), "Missing refresh token or cookie") });

            RefreshTokenContents? refreshTokenContents;
            try
            {
                refreshTokenContents =
                    _encryptionService.DeserializeUnprotect<RefreshTokenContents>(request.RefreshToken, out _);
            }
            catch
            {
                throw new ValidationException(new[]
                    { new ValidationFailure(nameof(request.RefreshToken), "Invalid refresh token") });
            }

            if (refreshTokenContents is null)
                throw new ValidationException(new[]
                    { new ValidationFailure(nameof(request.RefreshToken), "Invalid refresh token") });

            var user = await _context.Users.FindAsync(refreshTokenContents.UserId);

            if (user is null)
                throw new ValidationException(new[]
                    { new ValidationFailure(nameof(request.RefreshToken), "Invalid refresh token") });

            var token = await _sessionManager.StartSessionAsync(user);

            var masterKey = new EncryptableKey(user.MasterKeyEncrypted);
            var unlockedMasterKey = masterKey.Unlock(refreshTokenContents.Password, user.MasterKeySalt);

            _encryptionManager.MasterKey = unlockedMasterKey;
            _hashManager.Init(user);

            var refreshToken =
                _encryptionService.SerializeProtect(
                    new RefreshTokenContents { Password = refreshTokenContents.Password, UserId = user.Id },
                    TimeSpan.FromDays(_refreshOptions.ExpiryDays));

            await ScheduleSessionCreatedJobsAsync(user, unlockedMasterKey, cancellationToken);

            return new Response
            {
                User = user.Adapt<ResponseUser>(),
                Token = token,
                RefreshToken = refreshToken,
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshOptions.ExpiryDays)
            }; //TODO: Maybe add warden permissions to response
        }

        private async Task ScheduleSessionCreatedJobsAsync(User user, string masterKey,
            CancellationToken cancellationToken)
        {
            var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);

            var updateGradesJob = JobBuilder.Create<UpdateGradesJob>()
                .WithIdentity("updateGrades", "sessionCreatedJobs")
                .UsingJobData("userJson", JsonSerializer.Serialize(user))
                .UsingJobData("masterKey", masterKey)
                .Build();

            var updateGradesTrigger = TriggerBuilder.Create().WithIdentity("updateGradesTrigger", "sessionCreatedJobs")
                .StartNow()
                .Build();

            var updateLolosJob = JobBuilder.Create<UpdateLolosJob>()
                .WithIdentity("updateLolos", "sessionCreatedJobs")
                .UsingJobData("userJson", JsonSerializer.Serialize(user))
                .UsingJobData("masterKey", masterKey)
                .Build();

            var chainingListener = new JobChainingJobListener("sessionCreatedJobsPipeline");
            chainingListener.AddJobChainLink(updateGradesJob.Key, updateLolosJob.Key);

            scheduler.ListenerManager.AddJobListener(chainingListener,
                GroupMatcher<JobKey>.GroupEquals("sessionCreatedJobs"));

            await scheduler.ScheduleJob(updateGradesJob, updateGradesTrigger, cancellationToken);
            await scheduler.AddJob(updateLolosJob, false, true, cancellationToken);
        }
    }
}