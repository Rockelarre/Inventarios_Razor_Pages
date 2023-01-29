using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventarios.Data;
using RPInventarios.Models;

namespace RPInventarios.Pages.Usuarios;

public class DetailsModel : PageModel
{
    private readonly InventariosContext _context;

    public DetailsModel(InventariosContext context)
    {
        _context = context;
    }

    public Usuario Usuario { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Usuario = await _context.Usuario
                                    .Include(p => p.Perfil)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(m => m.Id == id);

        if (Usuario == null)
        {
            return NotFound();
        }
        return Page();
    }
}