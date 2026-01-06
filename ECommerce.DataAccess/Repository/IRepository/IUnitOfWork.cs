using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Repository.IRepository
{
    // Interfaz para la unidad de trabajo que agrupa los repositorios
    public interface IUnitOfWork : IDisposable // Hereda de IDisposable para liberar recursos
    {
        ICategoriaRepository Categoria { get; }
        IProductoRepository Producto { get; }
        ICarritoCompraRepository CarritoCompra { get; }
        IOrdenRepository Orden { get; }
        IDetalleOrdenRepository DetalleOrden { get; }
        void Save();
    }
}
