using MediatR;
using Serilog;
using Tasks.Domain.Events;

namespace Tasks.Handler.EventHandlers
{
    public class TaskCreatedEventHandler : IRequestHandler<TaskCreatedEvent>
    {
        Task IRequestHandler<TaskCreatedEvent>.Handle(TaskCreatedEvent request, CancellationToken cancellationToken)
        {
            Log.Information($"{nameof(TaskCreatedEvent)}, Email = {request.Email} Description = {request.Description}");

            return Task.FromResult(Unit.Value);
        }
    }
}
