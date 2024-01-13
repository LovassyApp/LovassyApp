using Blueboard.Core.Auth.Services;
using Blueboard.Features.Auth.Jobs;
using MediatR;
using Shimmer.Services;

namespace Blueboard.Features.Auth.Commands;

public static class ResendVerifyEmail
{
    public class Command : IRequest
    {
        public string VerifyUrl { get; set; }
        public string VerifyTokenQueryKey { get; set; }
    }

    internal sealed class Handler(UserAccessor userAccessor, IShimmerJobFactory jobFactory)
        : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = userAccessor.User;

            var sendVerifyEmailJob =
                await jobFactory.CreateAsync<SendVerifyEmailJob, SendVerifyEmailJob.Data>(cancellationToken);

            sendVerifyEmailJob.Data(new SendVerifyEmailJob.Data
            {
                User = user!,
                VerifyUrl = request.VerifyUrl,
                VerifyTokenQueryKey = request.VerifyTokenQueryKey
            });

            await sendVerifyEmailJob.FireAsync(cancellationToken);

            return Unit.Value;
        }
    }
}