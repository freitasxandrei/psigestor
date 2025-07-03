using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using psigestor.Data; 

namespace psigestor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request == null)
            {
                return BadRequest("Dados inválidos.");
            }

            var userExists = await _context.Users.AnyAsync(u => u.Name == request.Username);
            if (userExists)
            {
                return BadRequest("Usuário já existe.");
            }

            var user = new User
            {
                Name = request.Username,
                Password = request.Password,
                Gender = request.Gender,
                Age = request.Age,
                RegistrationDate = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuário registrado com sucesso!" });
        }

[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginRequest request)
{
    var user = await _context.Users
        .FirstOrDefaultAsync(u => u.Name == request.Username && u.Password == request.Password);

    if (user == null)
    {
        return Unauthorized(new { message = "Usuário ou senha inválidos." });
    }

    // Aqui você geraria o token JWT para autenticar o usuário
    var token = "example-token"; // Isso deve ser gerado dinamicamente

    return Ok(new { token });
}

    }

    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
