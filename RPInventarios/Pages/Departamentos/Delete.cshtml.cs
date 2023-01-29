using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventarios.Data;
using RPInventarios.Models;

namespace RPInventarios.Pages.Departamentos;

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
  public Departamento Departamento { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Departamento == null)
        {
            _servicioNotificacion.Warning($"El identificador debe tener un valor.");
            return NotFound();
        }   

        var departamento = await _context.Departamento.FirstOrDefaultAsync(m => m.Id == id);

        if (departamento == null)
        {
            _servicioNotificacion.Warning($"No se ha encontrado con el identificador proporcionado.");
            return NotFound();
        }
        else 
        {
            Departamento = departamento;
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null || _context.Departamento == null)
        {
            return NotFound();
        }
        var departamento = await _context.Departamento.FindAsync(id);

        if (departamento != null)
        {
            Departamento = departamento;
            _context.Departamento.Remove(Departamento);
            await _context.SaveChangesAsync();
        }
        _servicioNotificacion.Success($"Éxito al eliminar el departamento {Departamento.Nombre}");
        return RedirectToPage("./Index");
    }
}
