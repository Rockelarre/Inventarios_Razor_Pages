using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RPInventarios.Data;
using RPInventarios.Helpers;
using RPInventarios.Models;
using RPInventarios.ViewModels;

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
        Producto = new ProductoCreacionEdicionViewModel();
        return Page();
    }

    [BindProperty]
    public ProductoCreacionEdicionViewModel Producto { get; set; }
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

        var nuevoProducto = new Producto()
        {
            Id = Producto.Id,
            Costo = Producto.Costo,
            Descripcion = Producto.Descripcion,
            Estatus = Producto.Estatus,
            MarcaId = Producto.MarcaId,
            Nombre = Producto.Nombre
        };

        if (Request.Form.Files.Count > 0)
        {
            IFormFile archivo = Request.Form.Files.FirstOrDefault();
            nuevoProducto.Imagen = await Utilerias.LeerImagen(archivo);
        }

        _context.Producto.Add(nuevoProducto);
        await _context.SaveChangesAsync();
        _servicioNotificacion.Success($"Éxito al crear el producto {nuevoProducto.Nombre}");

        return RedirectToPage("./Index");
    }
}
