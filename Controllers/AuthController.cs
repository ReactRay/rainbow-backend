using Microsoft.AspNetCore.Mvc;
using RainbowProject.DTOs;
using RainbowProject.Models.Domain;
using RainbowProject.Repositories;
using RainbowProject.Services;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity.Data;

namespace RainbowProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtService _jwt;

        public AuthController(IUserRepository userRepo, IJwtService jwt)
        {
            _userRepo = userRepo;
            _jwt = jwt;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto req)
        {
            if (await _userRepo.GetByEmailAsync(req.Email) != null)
                return BadRequest("Email already in use.");

            var user = new User
            {
                UserName = req.UserName,
                Email = req.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
                UserImage = req.UserImage,
            };

            await _userRepo.CreateAsync(user);

            var token = _jwt.CreateToken(user);
            return Ok(new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                UserName = user.UserName
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto req)
        {
            var user = await _userRepo.GetByEmailAsync(req.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
                return Unauthorized("Invalid email or password.");

            var token = _jwt.CreateToken(user);
            return Ok(new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                UserName = user.UserName
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Stateless JWT — client handles logout by deleting the token.
            return Ok(new { message = "Logout successful (delete token on client side)." });
        }
    }
}
