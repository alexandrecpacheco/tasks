using System.Diagnostics.CodeAnalysis;

namespace Tasks.Domain.DTO.Request
{
    [ExcludeFromCodeCoverage]
    public class TaskRequest
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
    }
}
