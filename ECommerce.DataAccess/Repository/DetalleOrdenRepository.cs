using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Repository
{
    public class DetalleOrdenRepository : Repository<DetalleOrden>, IDetalleOrdenRepository
    {
        private readonly ApplicationDbContext _context;

        public DetalleOrdenRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public void Update(DetalleOrden detalleOrden)
        {
            var objetoDesdeDb = _context.DetalleOrdenes.FirstOrDefault(c => c.Id == detalleOrden.Id);
            if (objetoDesdeDb != null)
            {
                // Actualiza las propiedades del objeto desde la base de datos con los valores del objeto proporcionado
                _context.DetalleOrdenes.Update(objetoDesdeDb);
            }
        }
    }
}
