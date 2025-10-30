using ColorApi.Data;
using Microsoft.AspNetCore.Mvc;
using RainbowProject.Models.Domain;
using Microsoft.EntityFrameworkCore;
using RainbowProject.Models.Dto;


[ApiController]
[Route("api/[controller]")]
public class ColorController : ControllerBase
{
    private readonly AppDbContext _context;

    public ColorController(AppDbContext context)
    {
        _context = context;
    }

    public Guid Id { get; set; } = Guid.NewGuid();
    public string Hex { get; set; } = "#000000";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [HttpPost("add")]
    public async Task<IActionResult> AddColor([FromBody] ColorItemDtoRequest color)
    {
        if (string.IsNullOrEmpty(color.Hex) || color.UserId== Guid.Empty)
            return BadRequest("Invalid data");


        var colorDomain = new ColorItem
        {
            Id = Guid.NewGuid(),
            Hex = color.Hex,
            UserId = color.UserId
        };

        await _context.Colors.AddAsync(colorDomain);
        await _context.SaveChangesAsync();

        return Ok(color);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserColors(Guid userId)
    {
        var colors = await _context.Colors
            .Where(c => c.UserId == userId)
            .Include(c => c.User) // 👈 include user info
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new
            {
                c.Id,
                c.Hex,
                c.CreatedAt,
                User = new
                {
                    c.User.Id,
                    c.User.UserName,
                    c.User.Email,
                    c.User.UserImage
                }
            })
            .ToListAsync();

        return Ok(colors);
    }


    [HttpGet("all")]
    public async Task<IActionResult> GetAllColors()
    {
        var colors = await _context.Colors
      .Include(c => c.User)
      .Select(c => new {
          c.Id,
          c.Hex,
          c.CreatedAt,
          User = new
          {
              c.User.Id,
              c.User.UserName,
              c.User.Email,
              c.User.UserImage
          }
      })
      .OrderByDescending(c => c.CreatedAt)
      .ToListAsync();

        return Ok(colors);
    }

}
