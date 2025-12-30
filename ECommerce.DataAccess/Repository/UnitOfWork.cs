using ECommerce.DataAccess.Repository.IRepository;
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
        }

        public ICategoriaRepository Categoria { get; private set; } // Propiedad para acceder al repositorio de Categorias
        public IProductoRepository Producto { get; private set; } // Propiedad para acceder al repositorio de Productos

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
