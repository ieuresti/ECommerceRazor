using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Repository.IRepository
{
    // Interfaz especifica para el repositorio de Orden
    public interface IOrdenRepository : IRepository<Orden>
    {
        void Update(Orden orden);
    }
}
