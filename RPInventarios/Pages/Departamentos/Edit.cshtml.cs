using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventarios.Data;
using RPInventarios.Models;

namespace RPInventarios.Pages.Departamentos;

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
    public Departamento Departamento { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Departamento == null)
        {
            _servicioNotificacion.Warning($"El identificador del departamento debe tener un valor diferente a nulo.");
            return NotFound();
        }

        var departamento =  await _context.Departamento.FirstOrDefaultAsync(m => m.Id == id);
        
        if (departamento == null)
        {
            _servicioNotificacion.Warning($"No se ha encontrado el departamento con el identificador proporcionado.");
            return NotFound();
        }
        Departamento = departamento;
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            _servicioNotificacion.Error($"Es necesario corregir los problemas para poder editar el departamento {Departamento.Nombre}");
            return Page();
        }

        var existeDepartamentoBd = _context.Departamento.Any(u => u.Nombre.ToLower().Trim() == Departamento.Nombre.ToLower().Trim()
                                                            && u.Id != Departamento.Id);

        _context.Attach(Departamento).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DepartamentoExists(Departamento.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        _servicioNotificacion.Success($"Éxito al actualizar el departamento {Departamento.Nombre}");
        return RedirectToPage("./Index");
    }

    private bool DepartamentoExists(int id)
    {
      return _context.Departamento.Any(e => e.Id == id);
    }
}
