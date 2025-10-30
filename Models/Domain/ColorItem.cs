namespace RainbowProject.Models.Domain
{
    public class ColorItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Hex { get; set; } = "#000000";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relationship
        public Guid UserId { get; set; }
        public User User { get; set; }
    }

}
