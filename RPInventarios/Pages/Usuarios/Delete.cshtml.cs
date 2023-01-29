using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventarios.Data;
using RPInventarios.Models;

namespace RPInventarios.Pages.Usuarios;

public class DeleteModel : PageModel
{
    private readonly InventariosContext _context;
    private readonly INotyfService _servicioNotificacion;

    public DeleteModel(InventariosContext context, INotyfService servicioNotificacion)
    {
        _context = context;
        _servicioNotificacion = servicioNotificacion;
    }

    [BindProperty]
    public Usuario Usuario { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            _servicioNotificacion.Warning($"El identificador debe tener un valor.");
            return NotFound();
        }

        Usuario = await _context.Usuario
            .Include(p => p.Perfil).FirstOrDefaultAsync(m => m.Id == id);

        if (Usuario == null)
        {
            _servicioNotificacion.Warning($"No se ha encontrado el usuario con el identificador proporcionado.");
            return NotFound();
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Usuario = await _context.Usuario.FindAsync(id);

        if (Usuario != null)
        {
            _context.Usuario.Remove(Usuario);
            await _context.SaveChangesAsync();
        }

        _servicioNotificacion.Success($"Éxito al eliminar el usuario {Usuario.Username}");

        return RedirectToPage("./Index");
    }
}