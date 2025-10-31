using ColorApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RainbowProject.Repositories;
using RainbowProject.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ✅ CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("https://rainbow-project-redw.vercel.app/")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// ✅ DB Context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// ✅ Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();

// ✅ JWT Setup
var jwt = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!))
        };
    });

// ✅ MVC + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ✅ Middleware order matters!
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

// ✅ Use CORS before authentication
app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
