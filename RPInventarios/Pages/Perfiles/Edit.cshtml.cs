using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventarios.Data;
using RPInventarios.Models;

namespace RPInventarios.Pages.Perfiles;

public class EditModel : PageModel
{
    private readonly InventariosContext _context;
    private readonly INotyfService _servicioNotificacion;

    public EditModel(InventariosContext context, INotyfService servicioNotificacion)
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
            _servicioNotificacion.Warning($"El identificador del perfil debe tener un valor diferente a nulo.");
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

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            _servicioNotificacion.Error($"Es necesario corregir los problemas para poder editar el perfil {Perfil.Nombre}");
            return Page();
        }

        var existePerfilBd = _context.Marca.Any(u => u.Nombre.ToLower().Trim() == Perfil.Nombre.ToLower().Trim()
                                                        && u.Id != Perfil.Id);

        if (existePerfilBd)
        {
            _servicioNotificacion.Warning($"Ya existe un perfil con el nombre {Perfil.Nombre}");
            return Page();
        }

        _context.Attach(Perfil).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PerfilExists(Perfil.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        _servicioNotificacion.Success($"ÉXITO al actualizar el perfil {Perfil.Nombre}");
        return RedirectToPage("./Index");
    }

    private bool PerfilExists(int id)
    {
        return _context.Perfil.Any(e => e.Id == id);
    }
}