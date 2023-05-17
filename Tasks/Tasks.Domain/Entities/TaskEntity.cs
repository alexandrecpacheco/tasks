using System.Diagnostics.CodeAnalysis;

namespace Tasks.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class TaskEntity : BaseEntity
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
    }
}
