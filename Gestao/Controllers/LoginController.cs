using Azure;
using Gestao.Data;
using Gestao.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace Gestao.Controllers
{
    public class LoginController : Controller
    {

        private readonly GestaoDbContext _context;

        public LoginController(GestaoDbContext context)
        {
            _context = context;
        }

        // Método para renderizar a página de login
        public IActionResult Index()
        {
            Usuario model = new Usuario();
            return View(model);
        }

        // Método para realizar o login
        [HttpPost]
        public async Task<IActionResult> Login(Usuario usuario)
        {
            // Verificar se o usuário existe e a senha está correta
            var user = await _context.usuarios
                .FirstOrDefaultAsync(u => u.Email == usuario.Email && u.Senha == usuario.Senha);

            if (user == null)
            {
                // Se o usuário não for encontrado, retornar NotFound
                return NotFound("Usuário não encontrado");
            }
            else
            {
                // Se o usuário for encontrado, gerar token de acesso
                return GenerateToken(user);
            }
        }

        // Método para gerar token de acesso
        private IActionResult GenerateToken(Usuario user)
        {
            // Criar claims para o token
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Nome),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Senha)
            };

            // Gerar chave secreta mais segura
            using (var rng = new RNGCryptoServiceProvider())
            {
                var keyBytes = new byte[32]; // 256 bits
                rng.GetBytes(keyBytes);
                var key = new SymmetricSecurityKey(keyBytes);
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Criar token de acesso
                var token = new JwtSecurityToken(
                    issuer: "Gesao",
                    audience: "Gestao",
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: creds
                );

                // Armazenar token de acesso em um cookie seguro
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                };
                Response.Cookies.Append("AccessToken", tokenString, cookieOptions);

                // Redirecionar para a tela Home
                return RedirectToAction("Index", "Home");
            }
        }
    }
}