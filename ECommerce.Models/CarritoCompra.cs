using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models
{
    public class CarritoCompra
    {
        public int Id { get; set; }
        public int ProductoId { get; set; } // Foreign key a Producto
        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; } // Navegación a Producto
        public int Cantidad { get; set; }
        public string ApplicationUserId { get; set; } // Foreign key a GUID de la tabla Usuario de Identity
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; } // Navegación a Usuario
    }
}
