using System.Text.Json;
using FluentValidation.Results;
using Mapster;
using MediatR;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Listener;
using WebApi.Common.Exceptions;
using WebApi.Core.Auth.Models;
using WebApi.Core.Auth.Services;
using WebApi.Core.Cryptography.Models;
using WebApi.Core.Cryptography.Services;
using WebApi.Features.Auth.Jobs;
using WebApi.Features.Auth.Models;
using WebApi.Features.Auth.Services;
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

        public DateTime? EmailVerifiedAt { get; set; }

        public string? RealName { get; set; }
        public string? Class { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly EncryptionManager _encryptionManager;
        private readonly RefreshService _refreshService;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly SessionManager _sessionManager;

        public Handler(ISchedulerFactory schedulerFactory, ApplicationDbContext context,
            SessionManager sessionManager, EncryptionManager encryptionManager, RefreshService refreshService)
        {
            _schedulerFactory = schedulerFactory;
            _context = context;
            _sessionManager = sessionManager;
            _encryptionManager = encryptionManager;
            _refreshService = refreshService;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.RefreshToken is null)
                throw new ValidationException(new[]
                    { new ValidationFailure(nameof(request.RefreshToken), "Missing refresh token or cookie") });

            RefreshTokenContents? refreshTokenContents;
            try
            {
                refreshTokenContents = _refreshService.DecryptRefreshToken(request.RefreshToken);
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

            var token = await _sessionManager.StartSessionAsync(user.Id);

            var masterKey = new EncryptableKey(user.MasterKeyEncrypted);
            var unlockedMasterKey = masterKey.Unlock(refreshTokenContents.Password, user.MasterKeySalt);

            _encryptionManager.MasterKey = unlockedMasterKey; // Set the master key this way saves it in the session

            var refreshToken = _refreshService.GenerateRefreshToken(user.Id, refreshTokenContents.Password);

            await ScheduleSessionCreatedJobsAsync(user, unlockedMasterKey, cancellationToken);

            return new Response
            {
                User = user.Adapt<ResponseUser>(),
                Token = token,
                RefreshToken = refreshToken,
                RefreshTokenExpiration = DateTime.Now.Add(_refreshService.GetRefreshTokenExpiry())
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

            var updateGradesTrigger = TriggerBuilder.Create()
                .WithIdentity("updateGradesTrigger", "sessionCreatedJobs")
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