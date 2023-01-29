using System.ComponentModel.DataAnnotations;

namespace RPInventarios.Models;

    public class Marca
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La marca es requerida")]
        [MinLength(5,ErrorMessage = "La marca debe ser mayor o igual a 5 caracteres")]
        [MaxLength(50, ErrorMessage = "La marca no debe exceder los 50 caracteres")]
        [Display(Name ="Marca")]
        public string Nombre { get; set; }
        public virtual ICollection<Producto> Productos { get; set; }
        
    }

