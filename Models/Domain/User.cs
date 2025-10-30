namespace RainbowProject.Models.Domain
{
    public class User
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string? UserImage { get; set; }

        // Optional: navigation property to colors they posted
        public ICollection<ColorItem>? Colors { get; set; }
    }
}
