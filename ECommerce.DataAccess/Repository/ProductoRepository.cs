using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Repository
{
    public class ProductoRepository : Repository<Producto>, IProductoRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductoRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public void Update(Producto producto)
        {
            var objetoDesdeDb = _context.Productos.FirstOrDefault(c => c.Id == producto.Id);
            if (objetoDesdeDb != null)
            {
                objetoDesdeDb.Nombre = producto.Nombre;
                objetoDesdeDb.Descripcion = producto.Descripcion;
                objetoDesdeDb.Precio = producto.Precio;
                objetoDesdeDb.CantidadDisponible = producto.CantidadDisponible;
                objetoDesdeDb.CategoriaId = producto.CategoriaId;
                objetoDesdeDb.Imagen = producto.Imagen;
            }
        }
    }
}
