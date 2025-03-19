namespace API.Entities
{
    public class Message
    {
        public long Id { get; set; }

        public required string Content { get; set; }

        public required DateTime CreatedAt { get; set; }

        public required User User { get; set; } // Navigation property
    }

}
