using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RPInventarios.Enums;
using RPInventarios.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RPInventarios.ViewModels;

public class ProductoCreacionEdicionViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del producto es requerido.")]
    [MinLength(5, ErrorMessage = "El nombre del producto debe ser mayor o igual a 5 caracteres")]
    [MaxLength(50, ErrorMessage = "El nombre del producto no debe exceder los 50 caracteres")]
    public string Nombre { get; set; }

    [Display(Name = "Descripción")]
    [StringLength(200, MinimumLength = 5,
       ErrorMessage = "La descripción del producto debe contener entre 5 y 200 caracteres.")]
    public string Descripcion { get; set; }

    [Display(Name = "Marca")]
    [Required(ErrorMessage = "La marca del producto es obligatoria.")]
    public int MarcaId { get; set; }
    public virtual Marca Marca { get; set; }

    [Required(ErrorMessage = "El costo es obligatorio.")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Costo { get; set; }

    [Display(Name = "Estatus")]
    [Required(ErrorMessage = "El estatus del producto es obligatorio.")]
    public EstatusProducto Estatus { get; set; } = EstatusProducto.Activo;

    [Display(Name = "Imagen del producto")]
    public byte[] Imagen { get; set; }
}
