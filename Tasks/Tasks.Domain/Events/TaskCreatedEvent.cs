namespace Tasks.Domain.Events
{
    public class TaskCreatedEvent : BaseCompletedEvent
    {
        public string? Email { get; set; }
        public string? Description { get; set; }
    }
}
