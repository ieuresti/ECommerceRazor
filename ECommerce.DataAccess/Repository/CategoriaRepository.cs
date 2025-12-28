using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoriaRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public void Update(Categoria categoria)
        {
            var objetoDesdeDb = _context.Categorias.FirstOrDefault(c => c.Id == categoria.Id);
            if (objetoDesdeDb != null)
            {
                objetoDesdeDb.Nombre = categoria.Nombre;
                objetoDesdeDb.OrdenVisualizacion = categoria.OrdenVisualizacion;
            }
        }
    }
}
