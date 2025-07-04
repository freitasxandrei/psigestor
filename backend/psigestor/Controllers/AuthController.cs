using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using psigestor.Data;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity; // Necessário para PasswordHasher

namespace psigestor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly string _secretKey = "this_is_a_very_secure_secret_key_256bits!"; // Chave secreta de 256 bits
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthController(AppDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request == null)
            {
                return BadRequest("Dados inválidos.");
            }

            // Verifica se o usuário já existe
            var userExists = await _context.Users.AnyAsync(u => u.Name == request.Username);
            if (userExists)
            {
                return BadRequest("Usuário já existe.");
            }

            // Criptografando a senha do usuário
            var hashedPassword = _passwordHasher.HashPassword(null, request.Password);

            var user = new User
            {
                Name = request.Username,
                Password = hashedPassword, // Armazenando a senha criptografada
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
                .FirstOrDefaultAsync(u => u.Name == request.Username); // Encontra o usuário pelo nome

            if (user == null)
            {
                return Unauthorized(new { message = "Usuário ou senha inválidos." });
            }

            // Verificando se a senha fornecida corresponde à senha armazenada
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);
            
            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                return Unauthorized(new { message = "Usuário ou senha inválidos." });
            }

            // Gerando o token JWT para o usuário autenticado
            var token = GenerateJwtToken(user);

            return Ok(new { token });
        }

        // Método para gerar o token JWT
        private string GenerateJwtToken(User user)
        {
            var claims = new[] 
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)); // Certifique-se de que a chave tem 256 bits
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yourIssuer",
                audience: "yourAudience",
                claims: claims,
                expires: DateTime.Now.AddHours(1), // O token expira em 1 hora
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
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
