namespace RainbowProject.Models.Domain
{
    public class ColorItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        // Example: "#00cc66"
        public string Hex { get; set; } = "#000000";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign key reference to User
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
