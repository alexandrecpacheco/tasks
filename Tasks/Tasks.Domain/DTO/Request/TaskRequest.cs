namespace Tasks.Domain.DTO.Request
{
    public class TaskRequest
    {
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
    }
}
