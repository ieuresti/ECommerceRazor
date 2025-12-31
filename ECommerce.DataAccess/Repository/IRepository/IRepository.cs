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
        IEnumerable<T> GetAll(string? includeProperties = null); // Puede recibir una cadena con los nombres de las propiedades de navegacion a incluir
        T GetFirstOrDefault(Expression<Func<T, bool>> ? filter = null, string? includeProperties = null); // Puede recibir un filtro y una cadena con los nombres de las propiedades de navegacion a incluir
        bool Exists(Expression<Func<T, bool>> ? filter = null);
    }
}
