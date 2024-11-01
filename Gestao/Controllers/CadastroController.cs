using Microsoft.AspNetCore.Mvc;
using Gestao.Data;
using Gestao.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity; // Adicione esta linha
using System.Threading.Tasks; // Certifique-se de ter esta linha para usar Task

namespace Gestao.Controllers
{
    public class CadastroController : Controller
    {
        private readonly GestaoDbContext _context;
        private readonly PasswordHasher<Usuario> _passwordHasher; // Adicione esta linha

        public CadastroController(GestaoDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Usuario>(); // Inicialize o PasswordHasher
        }

        // Renderiza a tela de Cadastro
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(usuarios model)
        {
            if (ModelState.IsValid)
            {
                // Verifica se o email já está cadastrado
                var usuarioExistente = await _context.usuarios.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (usuarioExistente != null)
                {
                    ModelState.AddModelError(string.Empty, "Email já cadastrado.");
                    return View(model);
                }

                // Cria um novo usuário
                var usuario = new Usuario
                {
                    Nome = model.Nome,
                    Email = model.Email
                };

                // Hash da senha
                usuario.Senha = _passwordHasher.HashPassword(usuario, model.Senha); // Armazena o hash da senha

                _context.usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // Redireciona para a página de confirmação
                return RedirectToAction("Confirmacao");
            }
            return View(model);
        }

        public IActionResult Confirmacao()
        {
            return View();
        }
    }
}