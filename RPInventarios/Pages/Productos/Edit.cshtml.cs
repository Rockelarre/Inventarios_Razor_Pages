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
    public ProductoCreacionEdicionViewModel Producto { get; set; }
    public SelectList Marcas { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            _servicioNotificacion.Warning($"El identificador del producto debe tener un valor diferente a nulo.");
            return NotFound();
        }

        var productoBd = await _context.Producto
                                    //.Include(p => p.Marca)
                                    .FirstOrDefaultAsync(m => m.Id == id);

        if (productoBd == null)
        {
            _servicioNotificacion.Warning($"No se ha encontrado el producto con el identificador proporcionado.");
            return NotFound();
        }

        Marcas = new SelectList(_context.Marca.AsNoTracking(), "Id", "Nombre");

        Producto = new ProductoCreacionEdicionViewModel()
        {
            Id = productoBd.Id,
            Costo = productoBd.Costo,
            Descripcion = productoBd.Descripcion,
            Estatus = productoBd.Estatus,
            MarcaId = productoBd.MarcaId,
            Nombre = productoBd.Nombre
        };

        if (!String.IsNullOrEmpty(productoBd.Imagen))
        {
            Producto.Imagen = await Utilerias.ConvertirImagenABytes(productoBd.Imagen);
        }

        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Marcas = new SelectList(_context.Marca.AsNoTracking(), "Id", "Nombre");
            return Page();
        }

        var existeProductoBd = _context.Producto.Any(u => u.Nombre.ToLower().Trim() == Producto.Nombre.ToLower().Trim()
                                                   && u.Id != Producto.Id);

        if (existeProductoBd)
        {
            Marcas = new SelectList(_context.Marca.AsNoTracking(), "Id", "Nombre");
            _servicioNotificacion.Warning($"Ya existe un producto con el nombre {Producto.Nombre}");
            return Page();
        }

        var productoBd = await _context.Producto.FindAsync(Producto.Id);

        productoBd.Costo = Producto.Costo;
        productoBd.Descripcion = Producto.Descripcion;
        productoBd.Estatus = Producto.Estatus;
        productoBd.MarcaId = Producto.MarcaId;
        productoBd.Nombre = Producto.Nombre;

        if (Request.Form.Files.Count > 0)
        {
            IFormFile archivo = Request.Form.Files.FirstOrDefault();
            productoBd.Imagen = await Utilerias.LeerImagen(archivo);
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductoExists(Producto.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        _servicioNotificacion.Success($"ÉXITO al actualizar el producto {Producto.Nombre}");
        return RedirectToPage("./Index");
    }

    private bool ProductoExists(int id)
    {
        return _context.Producto.Any(e => e.Id == id);
    }
}
