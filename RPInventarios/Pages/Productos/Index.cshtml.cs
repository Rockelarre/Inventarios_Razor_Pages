using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventarios.Data;
using RPInventarios.Models;
using X.PagedList;

namespace RPInventarios.Pages.Productos;

public class IndexModel : PageModel
{
    private readonly InventariosContext _context;
    private readonly IConfiguration _configuration;

    public IndexModel(InventariosContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public IPagedList<Producto> Productos { get; set; }
    [BindProperty(SupportsGet = true)]
    public int? Pagina { get; set; }
    [BindProperty(SupportsGet = true)]
    public string TerminoBusqueda { get; set; }
    public int TotalRegistros { get; set; }

    public async Task OnGetAsync()
    {
        var registrosPorPagina = _configuration.GetValue("RegistrosPorPagina", 3);

        var consulta = _context.Producto
                                .Include(u => u.Marca)
                                .AsNoTracking()
                                .Select(u => u);

        if (!String.IsNullOrEmpty(TerminoBusqueda))
        {
            consulta = consulta.Where(u => u.Nombre.Contains(TerminoBusqueda));
        }

        TotalRegistros = consulta.Count();
        var numeroPagina = Pagina ?? 1;

        Productos = await consulta.ToPagedListAsync(numeroPagina, registrosPorPagina);
    }
}