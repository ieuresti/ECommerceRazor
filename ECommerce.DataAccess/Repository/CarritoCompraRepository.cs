using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Repository
{
    public class CarritoCompraRepository : Repository<CarritoCompra>, ICarritoCompraRepository
    {
        private readonly ApplicationDbContext _context;

        public CarritoCompraRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public int DecrementarContador(CarritoCompra carritoCompra, int contador)
        {
            carritoCompra.Cantidad -= contador; // Disminuir la cantidad en el carrito de compra
            _context.SaveChanges(); // Guardar los cambios en la base de datos
            return carritoCompra.Cantidad; // Retornar la nueva cantidad
        }

        public int IncrementarContador(CarritoCompra carritoCompra, int contador)
        {
            carritoCompra.Cantidad += contador; // Aumentar la cantidad en el carrito de compra
            _context.SaveChanges(); // Guardar los cambios en la base de datos
            return carritoCompra.Cantidad; // Retornar la nueva cantidad
        }
    }
}
