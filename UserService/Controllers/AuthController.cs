using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models;

namespace UserService.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserDbContext _context;

    public AuthController(UserDbContext context)
    {
        _context = context;
    }

    // REGISTER
    [HttpPost("register")]
    public async Task<IActionResult> Register(User user)
    {
        var exists = await _context.Users.AnyAsync(u => u.Email == user.Email);

        if (exists)
            return BadRequest("User already exists");

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(user);
    }

    // LOGIN
    [HttpPost("login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == password);

        if (user == null)
            return Unauthorized();

        return Ok(user);
    }
}