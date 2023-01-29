using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RPInventarios.Data;
using RPInventarios.Models;

namespace RPInventarios.Pages.Productos;

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
        Marcas = new SelectList(_context.Marca.AsNoTracking(), "Id", "Nombre");
        return Page();
    }

    [BindProperty]
    public Producto Producto { get; set; }
    public SelectList Marcas { get; set; }
    

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
        {
            Marcas = new SelectList(_context.Marca.AsNoTracking(), "Id", "Nombre");
            _servicioNotificacion.Error($"Es necesario corregir los problemas para poder crear el producto {Producto.Nombre}");
            return Page();
        }

      var existeProductoBd = _context.Producto.Any(u=>u.Nombre.ToLower().Trim() == Producto.Nombre.ToLower().Trim());

        if (existeProductoBd)
        {
            Marcas = new SelectList(_context.Marca.AsNoTracking(), "Id", "Nombre");
            _servicioNotificacion.Warning($"Ya existe un producto con el nombre {Producto.Nombre}");
            return Page();
        }

        _context.Producto.Add(Producto);
        await _context.SaveChangesAsync();
        _servicioNotificacion.Success($"Éxito al crear el producto {Producto.Nombre}");

        return RedirectToPage("./Index");
    }
}
