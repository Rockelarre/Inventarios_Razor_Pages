using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RPInventarios.Data;
using RPInventarios.Models;

namespace RPInventarios.Pages.Perfiles;

public class CreateModel : PageModel
{
    private readonly InventariosContext _context;
    private readonly INotyfService _servicioNotificacion;

    public CreateModel(InventariosContext context, INotyfService servicioNotificacion)
    {
        _context = context;
        _servicioNotificacion = servicioNotificacion;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public Perfil Perfil { get; set; }

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            _servicioNotificacion.Error($"Es necesario corregir los problemas para poder crear el perfil {Perfil.Nombre}");
            return Page();
        }

        var existePerfilBd = _context.Perfil.Any(u => u.Nombre.ToLower().Trim() == Perfil.Nombre.ToLower().Trim());
        if (existePerfilBd)
        {
            _servicioNotificacion.Warning($"Ya existe un perfil con el nombre {Perfil.Nombre}");
            return Page();
        }

        _context.Perfil.Add(Perfil);
        await _context.SaveChangesAsync();
        _servicioNotificacion.Success($"ÉXITO al crear el perfil {Perfil.Nombre}");
        return RedirectToPage("./Index");
    }
}
