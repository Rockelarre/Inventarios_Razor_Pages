using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventarios.Data;
using RPInventarios.Helpers;
using RPInventarios.ViewModels;

namespace RPInventarios.Pages.Usuarios
{
    public class CambiarContrasenaModel : PageModel
    {
        private readonly InventariosContext _context;
        private readonly INotyfService _servicioNotificacion;
        private readonly UsuarioFactoria _usuarioFactoria;

        public CambiarContrasenaModel(InventariosContext context, INotyfService servicioNotificacion,
            UsuarioFactoria usuarioFactoria)
        {
            _context = context;
            _servicioNotificacion = servicioNotificacion;
            _usuarioFactoria = usuarioFactoria;
        }

        [BindProperty]
        public UsuarioCambioContrasenaViewModel Usuario { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                _servicioNotificacion.Warning($"El identificador del usuario debe tener un valor diferente a nulo.");
                return NotFound();
            }

            var usuarioBd = await _context.Usuario
                                        .FirstOrDefaultAsync(m => m.Id == id);

            if (usuarioBd == null)
            {
                _servicioNotificacion.Warning($"No se ha encontrado el usuario con el identificador proporcionado.");
                return NotFound();
            }

            Usuario = _usuarioFactoria.CrearUsuarioCambioContrasena(usuarioBd);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var usuarioBd = await _context.Usuario.FindAsync(Usuario.Id);
            _usuarioFactoria.ActualizarContrasenaUsuario(Usuario, usuarioBd);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
            _servicioNotificacion.Success($"La contraseña del usuario {Usuario.Username} fue actualizada exitosamente.");
            return RedirectToPage("./Index");
        }

    }
}