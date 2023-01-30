using Microsoft.AspNetCore.Identity;
using RPInventarios.Models;
using RPInventarios.ViewModels;

namespace RPInventarios.Helpers;

public class UsuarioFactoria
{
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public UsuarioFactoria(IPasswordHasher<Usuario> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public Usuario CrearUsuario(UsuarioRegistroViewModel usuarioVM)
    {
        var usuario = new Usuario()
        {
            Id = usuarioVM.Id,
            Apellidos = usuarioVM.Apellidos,
            Celular = usuarioVM.Celular,
            CorreoElectronico = usuarioVM.CorreoElectronico,
            Nombre = usuarioVM.Nombre,
            PerfilId = usuarioVM.PerfilId,
            Username = usuarioVM.Username
        };

        usuario.Contrasena = _passwordHasher.HashPassword(usuario,usuarioVM.Contrasena);
        return usuario;
    }

    public UsuarioEdicionViewModel CrearUsuarioEdicion(Usuario usuario)
    {
        return new UsuarioEdicionViewModel()
        {
            Id = usuario.Id,
            Apellidos = usuario.Apellidos,
            Celular = usuario.Celular,
            CorreoElectronico = usuario.CorreoElectronico,
            Nombre = usuario.Nombre,
            PerfilId = usuario.PerfilId,
            Username = usuario.Username,
            Foto = usuario.Foto
        };
    }

    public void ActualizarDatosUsuario(UsuarioEdicionViewModel usuario, Usuario usuarioBd)
    {
        usuarioBd.Celular = usuario.Celular;
        usuarioBd.CorreoElectronico = usuario.CorreoElectronico;
        usuarioBd.Nombre = usuario.Nombre;
        usuarioBd.Apellidos = usuario.Apellidos;
        usuarioBd.PerfilId = usuario.PerfilId; 
    }

    public UsuarioCambioContrasenaViewModel CrearUsuarioCambioContrasena(Usuario usuario)
    {
        return new UsuarioCambioContrasenaViewModel
        {
            Id = usuario.Id,
            Username = usuario.Username
        };
    }

    public void ActualizarContrasenaUsuario(UsuarioCambioContrasenaViewModel usuarioVM, Usuario usuarioBd)
    {
        usuarioBd.Contrasena = _passwordHasher.HashPassword(usuarioBd, usuarioVM.Contrasena);
    }
}
