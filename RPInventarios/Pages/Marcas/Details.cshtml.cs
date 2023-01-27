using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventarios.Data;
using RPInventarios.Models;

namespace RPInventarios.Pages.Marcas;

public class DetailsModel : PageModel
{
    private readonly InventariosContext _context;

    public DetailsModel(InventariosContext context)
    {
        _context = context;
    }

  public Marca Marca { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Marca == null)
        {
            return NotFound();
        }

        var marca = await _context.Marca.FirstOrDefaultAsync(m => m.Id == id);
        if (marca == null)
        {
            return NotFound();
        }
        else 
        {
            Marca = marca;
        }
        return Page();
    }
}
