using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Repository
{
    // Implementacion de la unidad de trabajo que agrupa los repositorios
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            this._context = context;
            Categoria = new CategoriaRepository(_context); // Inicializa el repositorio de Categorias
            Producto = new ProductoRepository(_context); // Inicializa el repositorio de Productos
            CarritoCompra = new CarritoCompraRepository(_context); // Inicializa el repositorio de CarritoCompra
            Orden = new OrdenRepository(_context); // Inicializa el repositorio de Ordenes
            DetalleOrden = new DetalleOrdenRepository(_context); // Inicializa el repositorio de DetalleOrden
            ApplicationUser = new ApplicationUserRepository(_context); // Inicializa el repositorio de ApplicationUser
        }

        public ICategoriaRepository Categoria { get; private set; } // Propiedad para acceder al repositorio de Categorias
        public IProductoRepository Producto { get; private set; } // Propiedad para acceder al repositorio de Productos
        public ICarritoCompraRepository CarritoCompra { get; private set; } // Propiedad para acceder al repositorio de CarritoCompra
        public IOrdenRepository Orden { get; private set; } // Propiedad para acceder al repositorio de Ordenes
        public IDetalleOrdenRepository DetalleOrden { get; private set; } // Propiedad para acceder al repositorio de DetalleOrden
        public IApplicationUserRepository ApplicationUser { get; private set; } // Propiedad para acceder al repositorio de ApplicationUser

        public void Dispose()
        {
            _context.Dispose(); // Libera los recursos del contexto de la base de datos
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
