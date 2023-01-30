using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventarios.Data;
using RPInventarios.Helpers;
using RPInventarios.Models;
using RPInventarios.ViewModels;
using System.Security.Claims;

namespace RPInventarios.Pages.Account;

public class LoginModel : PageModel
{
    private readonly InventariosContext _context;
    private readonly IPasswordHasher<Usuario> _passwordHasher;
    private readonly INotyfService _servicioNotificacion;

    public LoginModel(InventariosContext context, IPasswordHasher<Usuario> passwordHasher, INotyfService servicioNotificacion)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _servicioNotificacion = servicioNotificacion;
    }

    [BindProperty]
    public LoginViewModel LoginVM { get; set; }
    public string ReturnUrl { get; set; }

    public async Task OnGetAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            var usuarioBd = _context.Usuario
                            .Include(u => u.Perfil)
                            .FirstOrDefault(u => u.Username.ToLower().Trim() == LoginVM.Username.ToLower().Trim());

            if (usuarioBd == null)
            {
                _servicioNotificacion.Warning("Lo sentimos, el usuario no existe.");
                return Page();
            }

            var result = _passwordHasher.VerifyHashedPassword(usuarioBd, usuarioBd.Contrasena, LoginVM.Password);

            if (result == PasswordVerificationResult.Success)
            {
                // La contraseña es correcta
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuarioBd.Username),
                    new Claim("FullName", usuarioBd.Nombre + " " + usuarioBd.Apellidos),
                    new Claim(ClaimTypes.Role,usuarioBd.Perfil.Nombre)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = LoginVM.Recordarme
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return LocalRedirect(Url.GetLocalUrl(returnUrl));
            }
            else
            {
                _servicioNotificacion.Warning("La constraseña es incorrecta.");
                return Page();
            }
        }

        return Page();
    }
}
