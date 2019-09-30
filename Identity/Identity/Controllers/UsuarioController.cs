using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LoginPage()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("UserPage");
            }

            return View();
        }

        [HttpPost]
        public IActionResult LoginPage(Usuario usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //aqui poderia ter alguma requisição para base de dados, estou usando dados estáticos para não complicar
                    if (usuario.Login == "Eschechola" && usuario.Senha == "123321")
                    {
                        Login(usuario);
                        return RedirectToAction("UserPage");
                    }
                    else
                    {
                        ViewBag.Erro = "Usuário e / ou senha incorretos!";
                    }
                }
            }
            catch (Exception)
            {
                ViewBag.Erro = "Ocorreu algum erro ao tentar se logar, tente novamente!";
            }
            return View();
        }

        private async void Login(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Login),
                new Claim(ClaimTypes.Role, "Usuario_Comum")
            };

            var identidadeDeUsuario = new ClaimsIdentity(claims, "Login");
            ClaimsPrincipal claimPrincipal = new ClaimsPrincipal(identidadeDeUsuario);

            var propriedadesDeAutenticacao = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTime.Now.ToLocalTime().AddHours(2),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal, propriedadesDeAutenticacao);
        }

        [Authorize]
        public IActionResult UserPage()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}