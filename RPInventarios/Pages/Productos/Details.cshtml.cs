using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventarios.Data;
using RPInventarios.Models;

namespace RPInventarios.Pages.Productos;

public class DetailsModel : PageModel
{
    private readonly InventariosContext _context;

    public DetailsModel(InventariosContext context)
    {
        _context = context;
    }

    public Producto Producto { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Producto = await _context.Producto
                                    .Include(p => p.Marca)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(m => m.Id == id);

        if (Producto == null)
        {
            return NotFound();
        }
        return Page();
    }
}