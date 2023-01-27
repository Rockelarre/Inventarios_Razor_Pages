namespace RPInventarios.Models;

    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int MarcaId { get; set; }
        public virtual Marca Marca { get; set; }
        public decimal Costo { get; set; }
    }

