using Microsoft.AspNetCore.Mvc;
using Gestao.Data;
using Gestao.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

namespace Gestao.Controllers
{
    public class EsqueciSenhaController : Controller
    {
        private readonly GestaoDbContext _context;

        public EsqueciSenhaController(GestaoDbContext context)
        {
            _context = context;
        }

        // Renderiza a tela de Esqueceu a Senha
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EnviarEmail(EsqueciSenhaModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _context.usuarios
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

                if (usuario != null)
                {
                    // Gerar token para redefinição de senha (simples para o exemplo)
                    var token = Guid.NewGuid().ToString();

                    // Aqui você pode enviar o email com o token
                    var mensagem = new MailMessage();
                    mensagem.To.Add(new MailAddress(usuario.Email));
                    mensagem.Subject = "Redefinição de Senha";
                    mensagem.Body = $"Seu token de redefinição de senha é: {token}";
                    // Enviar o email (configurações SMTP devem ser definidas)

                    // Redireciona para uma página de confirmação
                    return RedirectToAction("Confirmacao");
                }

                // Adiciona um erro ao ModelState se o e-mail não for encontrado
                ModelState.AddModelError(string.Empty, "Email não encontrado.");
            }

            // Retorna a view com os erros do ModelState
            return View("Index", model);
        }

        public IActionResult Confirmacao()
        {
            return View();
        }
    }
}