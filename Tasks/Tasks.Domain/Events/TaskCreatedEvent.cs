using System.Diagnostics.CodeAnalysis;

namespace Tasks.Domain.Events
{
    [ExcludeFromCodeCoverage]
    public class TaskCreatedEvent : BaseCompletedEvent
    {
        public string? Email { get; set; }
        public string? Description { get; set; }
    }
}
