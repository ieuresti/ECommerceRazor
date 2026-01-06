using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Repository.IRepository
{
    // Interfaz especifica para el repositorio de Carrito Compra
    public interface ICarritoCompraRepository : IRepository<CarritoCompra>
    {
        int IncrementarContador(CarritoCompra carritoCompra, int contador);
        int DecrementarContador(CarritoCompra carritoCompra, int contador);
    }
}
