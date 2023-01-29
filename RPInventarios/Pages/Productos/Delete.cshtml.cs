using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventarios.Data;
using RPInventarios.Models;

namespace RPInventarios.Pages.Productos;

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
    public Producto Producto { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            _servicioNotificacion.Warning($"El identificador debe tener un valor.");
            return NotFound();
        }

        Producto = await _context.Producto
            .Include(p => p.Marca).FirstOrDefaultAsync(m => m.Id == id);

        if (Producto == null)
        {
            _servicioNotificacion.Warning($"No se ha encontrado el producto con el identificador proporcionado.");
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

        Producto = await _context.Producto.FindAsync(id);

        if (Producto != null)
        {
            _context.Producto.Remove(Producto);
            await _context.SaveChangesAsync();
        }

        _servicioNotificacion.Success($"ÉXITO al eliminar el producto {Producto.Nombre}");

        return RedirectToPage("./Index");
    }
}
