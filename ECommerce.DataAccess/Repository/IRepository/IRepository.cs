using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Repository.IRepository
{
    // Interfaz generica para los repositorios
    public interface IRepository<T> where T : class
    {
        // Metodos CRUD
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity); // Eliminar una coleccion de objetos
        IEnumerable<T> GetAll();
        T GetFirstOrDefault(Expression<Func<T, bool>> ? filter = null); // Recibe como parametro una expresion lambda
        bool Exists(Expression<Func<T, bool>> ? filter = null);
    }
}
