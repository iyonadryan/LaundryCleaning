using LaundryCleaning.Common.Domain;

namespace LaundryCleaning.Common.Models.Entities
{
    public class SystemPublisher : EntityBase
    {
        public string Topic { get; set; } = default!;
        public string Payload { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsPublished { get; set; } = false;
        public DateTime? PublishedAt { get; set; }
        public string? ErrorMessage { get; set; }
        public int RetryCount { get; set; } = 0;
    }
}