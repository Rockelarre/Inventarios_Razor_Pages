using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventarios.Data;
using RPInventarios.Models;

namespace RPInventarios.Pages.Perfiles;

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
    public Perfil Perfil { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            _servicioNotificacion.Warning($"El identificador debe tener un valor.");
            return NotFound();
        }

        Perfil = await _context.Perfil.FirstOrDefaultAsync(m => m.Id == id);

        if (Perfil == null)
        {
            _servicioNotificacion.Warning($"No se ha encontrado el perfil con el identificador proporcionado.");
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

        Perfil = await _context.Perfil.FindAsync(id);

        if (Perfil != null)
        {
            _context.Perfil.Remove(Perfil);
            await _context.SaveChangesAsync();
        }

        _servicioNotificacion.Success($"ÉXITO al eliminar el perfil {Perfil.Nombre}");
        return RedirectToPage("./Index");
    }
}