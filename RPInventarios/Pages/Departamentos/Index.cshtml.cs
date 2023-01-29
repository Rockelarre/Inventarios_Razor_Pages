using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventarios.Data;
using RPInventarios.Models;
using X.PagedList;

namespace RPInventarios.Pages.Departamentos;

public class IndexModel : PageModel
{
    private readonly InventariosContext _context;
    private readonly IConfiguration _configuration;

    public IndexModel(InventariosContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public IPagedList<Departamento> Departamento { get;set; }

    [BindProperty(SupportsGet = true)]
    public int? Pagina { get; set; }

    [BindProperty(SupportsGet = true)]
    public string TerminoBusqueda { get; set; }
    public int TotalRegistros { get; set; }
    public async Task OnGetAsync()
    {
        var registrosPorPagina = _configuration.GetValue("RegistrosPorPagina", 3);

        var consulta = _context.Departamento
                                .AsNoTracking()
                                .Select(u => u);

        if (!String.IsNullOrEmpty(TerminoBusqueda))
        {
            consulta = consulta.Where(u=>u.Nombre.Contains(TerminoBusqueda));
        }

        TotalRegistros = consulta.Count();
        var numeroPagina = Pagina ?? 1;

        Departamento = await consulta.ToPagedListAsync(numeroPagina, registrosPorPagina);
    }
}
