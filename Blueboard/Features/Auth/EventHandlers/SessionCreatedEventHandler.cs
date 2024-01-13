using Blueboard.Features.Auth.Events;
using Blueboard.Features.Auth.Jobs;
using MediatR;
using Shimmer.Services;

namespace Blueboard.Features.Auth.EventHandlers;

public class SessionCreatedEventHandler(IShimmerJobFactory jobFactory) : INotificationHandler<SessionCreatedEvent>
{
    public async Task Handle(SessionCreatedEvent notification, CancellationToken cancellationToken)
    {
        var idSalt = Guid.NewGuid().ToString();

        var updateGradesJob =
            await jobFactory.CreateTreeAsync<UpdateGradesJob, UpdateGradesJob.Data>(cancellationToken);

        var updateLolosJob =
            await jobFactory.CreateTreeAsync<UpdateLolosJob, UpdateLolosJob.Data>(cancellationToken);

        updateLolosJob
            .Name("updateLolos" + idSalt)
            .Group("sessionCreatedJobs")
            .Data(new UpdateLolosJob.Data
            {
                User = notification.User,
                MasterKey = notification.MasterKey
            })
            .Concurrent(false);

        updateGradesJob
            .Name("updateGrades" + idSalt)
            .Group("sessionCreatedJobs")
            .Data(new UpdateGradesJob.Data
            {
                User = notification.User,
                MasterKey = notification.MasterKey
            })
            .AddDependentJob(updateLolosJob)
            .Concurrent(false);

        await updateGradesJob.FireAsync(cancellationToken);
    }
}