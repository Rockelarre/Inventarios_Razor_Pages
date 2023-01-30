using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RPInventarios.Data;
using RPInventarios.Helpers;
using RPInventarios.ViewModels;

namespace RPInventarios.Pages.Usuarios;

public class EditModel : PageModel
{
    private readonly InventariosContext _context;
    private readonly INotyfService _servicioNotificacion;
    private readonly UsuarioFactoria _usuarioFactoria;

    public EditModel(InventariosContext context, INotyfService servicioNotificacion, UsuarioFactoria usuarioFactoria)
    {
        _context = context;
        _servicioNotificacion = servicioNotificacion;
        _usuarioFactoria = usuarioFactoria;
    }

    [BindProperty]
    public UsuarioEdicionViewModel Usuario { get; set; }
    public SelectList Perfiles { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            _servicioNotificacion.Warning($"El identificador del usuario debe tener un valor diferente a nulo.");
            return NotFound();
        }

        var usuarioBd = await _context.Usuario
                                    //.Include(p => p.Marca)
                                    .FirstOrDefaultAsync(m => m.Id == id);

        if (usuarioBd == null)
        {
            _servicioNotificacion.Warning($"No se ha encontrado el usuario con el identificador proporcionado.");
            return NotFound();
        }

        Perfiles = new SelectList(_context.Perfil.AsNoTracking(), "Id", "Nombre");
        Usuario = _usuarioFactoria.CrearUsuarioEdicion(usuarioBd);
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Perfiles = new SelectList(_context.Perfil.AsNoTracking(), "Id", "Nombre");
            return Page();
        }

        var existeUsuarioBd = _context.Usuario.Any(u => u.Username.ToLower().Trim() == Usuario.Username.ToLower().Trim()
                                                   && u.Id != Usuario.Id);

        if (existeUsuarioBd)
        {
            Perfiles = new SelectList(_context.Perfil.AsNoTracking(), "Id", "Nombre");
            _servicioNotificacion.Warning($"Ya existe un usuario con la cuenta {Usuario.Username}");
            return Page();
        }

        var usuarioBd = await _context.Usuario.FindAsync(Usuario.Id);
        _usuarioFactoria.ActualizarDatosUsuario(Usuario, usuarioBd);

        if (Request.Form.Files.Count > 0)
        {
            IFormFile archivo = Request.Form.Files.FirstOrDefault();
            using var dataStream = new MemoryStream();
            await archivo.CopyToAsync(dataStream);
            usuarioBd.Foto = dataStream.ToArray();
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UsuarioExists(Usuario.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        _servicioNotificacion.Success($"Éxito al actualizar el usuario {Usuario.Username}");
        return RedirectToPage("./Index");
    }

    private bool UsuarioExists(int id)
    {
        return _context.Usuario.Any(e => e.Id == id);
    }
}