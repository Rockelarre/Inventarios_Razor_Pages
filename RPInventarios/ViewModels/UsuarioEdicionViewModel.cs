using RPInventarios.Models;
using System.ComponentModel.DataAnnotations;

namespace RPInventarios.ViewModels;

public class UsuarioEdicionViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del usuario es obligatorio.")]
    [MinLength(2, ErrorMessage = "El nombre del usuario debe ser mayor o igual a 2 caracteres.")]
    [MaxLength(25, ErrorMessage = "El nombre del usuario no debe exceder los 25 caracteres.")]
    public string Nombre { get; set; }

    public string Apellidos { get; set; }

    [Display(Name = "Cuenta del Usuario")]
    public string Username { get; set; }

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [DataType(DataType.EmailAddress)]
    public string CorreoElectronico { get; set; }

    public string Celular { get; set; }

    [Required(ErrorMessage = "El perfil del usuario es obligatorio.")]
    [Display(Name = "Perfil")]
    public int PerfilId { get; set; }

    public byte[] Foto { get; set; }
}
