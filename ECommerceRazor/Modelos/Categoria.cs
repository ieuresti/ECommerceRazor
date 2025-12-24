using System.ComponentModel.DataAnnotations;

namespace ECommerceRazor.Modelos
{
    public class Categoria
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(100, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Nombre de la Categoría")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El Orden debe ser mayor a 0")]
        [Display(Name = "Orden de Visualización")]
        public int OrdenVisualizacion { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
