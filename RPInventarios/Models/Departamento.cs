using System.ComponentModel.DataAnnotations;

namespace RPInventarios.Models;
public class Departamento
{
    public int Id { get; set; }

    [Required]
    [MinLength(5, ErrorMessage = "El nombre debe ser mayor o igual a 5 caracteres")]
    [MaxLength(100, ErrorMessage = "El nombre no debe exceder los 100 caracteres")]
    public string Nombre { get; set; }

    [MinLength(5, ErrorMessage = "La descripción debe ser mayor o igual a 5 caracteres")]
    [MaxLength(200, ErrorMessage = "La descripción no debe exceder los 200 caracteres")]
    public string Descripcion { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime FechaCreacion { get; set; }
}