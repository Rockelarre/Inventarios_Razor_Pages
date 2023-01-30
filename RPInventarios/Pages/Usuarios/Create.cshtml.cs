using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RPInventarios.Data;
using RPInventarios.Helpers;
using RPInventarios.ViewModels;

namespace RPInventarios.Pages.Usuarios;

public class CreateModel : PageModel
{
    private readonly InventariosContext _context;
    private readonly INotyfService _servicioNotificacion;
    private readonly UsuarioFactoria _usuarioFactoria;

    public CreateModel(InventariosContext context, INotyfService servicioNotificacion, UsuarioFactoria usuarioFactoria)
    {
        _context = context;
        _servicioNotificacion = servicioNotificacion;
        _usuarioFactoria = usuarioFactoria;
    }

    public IActionResult OnGet()
    {
        Perfiles = new SelectList(_context.Perfil.AsNoTracking(), "Id", "Nombre");
        Usuario = new UsuarioRegistroViewModel();
        return Page();
    }

    [BindProperty]
    public UsuarioRegistroViewModel Usuario { get; set; }
    public SelectList Perfiles { get; set; }


    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Perfiles = new SelectList(_context.Perfil.AsNoTracking(), "Id", "Nombre");
            _servicioNotificacion.Error($"Es necesario corregir los problemas para poder crear el usuario {Usuario.Username}");
            return Page();
        }

        var existeUsuarioBd = _context.Usuario.Any(u => u.Username.ToLower().Trim() == Usuario.Username.ToLower().Trim());

        if (existeUsuarioBd)
        {
            Perfiles = new SelectList(_context.Marca.AsNoTracking(), "Id", "Nombre");
            _servicioNotificacion.Warning($"Ya existe un usuario con la cuenta {Usuario.Username}");
            return Page();
        }

        var usuarioAgregar = _usuarioFactoria.CrearUsuario(Usuario);

        if (Request.Form.Files.Count > 0)
        {
            IFormFile archivo = Request.Form.Files.FirstOrDefault();
            using var dataStream = new MemoryStream();
            await archivo.CopyToAsync(dataStream);
            usuarioAgregar.Foto = dataStream.ToArray();
        }

        _context.Usuario.Add(usuarioAgregar);
        await _context.SaveChangesAsync();
        _servicioNotificacion.Success($"Éxito al crear el usuario {Usuario.Username}");

        return RedirectToPage("./Index");
    }

    public async Task<JsonResult> OnGetExisteUsername(string username)
    {
        var existeUsuarioBd = await _context.Usuario
                              .AnyAsync(u => u.Username.ToLower().Trim() == username.ToLower().Trim());

        var existeUsuario = existeUsuarioBd ? true : false;

        return new JsonResult(new { existeUsuario });
    }
}