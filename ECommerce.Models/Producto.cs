using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models
{
    public class Producto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(100, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Nombre del Producto")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(500, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres", MinimumLength = 3)]
        public string Descripcion { get; set; }
        //[Required(ErrorMessage = "El campo {0} es obligatorio")]
        //[StringLength(300, ErrorMessage = "La ruta {0} debe tener entre {2} y {1} caracteres", MinimumLength = 3)]
        public string? Imagen { get; set; } // Ruta de la imagen almacenada
        [NotMapped]
        public IFormFile? ImagenArchivo { get; set; } // Para manejar la carga de imágenes
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(0.01, int.MaxValue, ErrorMessage = "La cantidad no puede ser negativa")]
        [Display(Name = "Cantidad Disponible")]
        public int CantidadDisponible { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "La categoría es obligatoria")]
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }
    }
}
