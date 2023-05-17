using System.Diagnostics.CodeAnalysis;

namespace Tasks.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class BaseEntity
    {
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; }
    }
}
