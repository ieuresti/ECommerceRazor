using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Repository
{
    public class OrdenRepository : Repository<Orden>, IOrdenRepository
    {
        private readonly ApplicationDbContext _context;

        public OrdenRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public void Update(Orden orden)
        {
            var objetoDesdeDb = _context.Ordenes.FirstOrDefault(c => c.Id == orden.Id);
            if (objetoDesdeDb != null)
            {
                // Actualiza las propiedades del objeto desde la base de datos con los valores del objeto proporcionado
                _context.Ordenes.Update(objetoDesdeDb);
            }
        }
    }
}
