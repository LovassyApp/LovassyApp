using Blueboard.Features.Auth.Jobs;
using Blueboard.Features.Users.Events;
using MediatR;
using Shimmer.Services;

namespace Blueboard.Features.Auth.EventHandlers;

public class UserCreatedEventHandler(IShimmerJobFactory jobFactory) : INotificationHandler<UserCreatedEvent>
{
    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var sendVerifyEmailJob =
            await jobFactory.CreateAsync<SendVerifyEmailJob, SendVerifyEmailJob.Data>(cancellationToken);

        sendVerifyEmailJob.Data(new SendVerifyEmailJob.Data
        {
            User = notification.User,
            VerifyUrl = notification.VerifyUrl,
            VerifyTokenQueryKey = notification.VerifyTokenQueryKey
        });
    }
}