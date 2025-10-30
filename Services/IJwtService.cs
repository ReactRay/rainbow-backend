using RainbowProject.Models.Domain;

namespace RainbowProject.Services
{
    public interface IJwtService
    {
        string CreateToken(User user);
    }
}
