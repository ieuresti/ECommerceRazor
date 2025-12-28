using ECommerce.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Repository
{
    // Implementacion generica de la interfaz IRepository
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet; // DbSet para la entidad T

        public Repository(ApplicationDbContext context)
        {
            this._context = context;
            this.dbSet = _context.Set<T>(); // Inicializa el DbSet para la entidad T
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public IEnumerable<T> GetAll()
        {
            // Declaramos una variable de tipo IQueryable<T> llamada 'query' y la inicializamos
            // con el DbSet correspondiente. IQueryable permite componer consultas LINQ
            // que no se ejecutan inmediatamente contra la base de datos.
            IQueryable<T> query = dbSet;
            // Al llamar a ToList() ejecutamos la consulta construida hasta ahora contra la BD
            // y se materializan los resultados en una lista en memoria (List<T>).
            // Esto provoca que se ejecute la sentencia SQL equivalente (p. ej. SELECT * FROM ...)
            // y que los datos devueltos queden cargados en la colección que se retorna.
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;
            // Si se proporciona un filtro, aplicarlo a la consulta
            if (filter != null)
            {
                // Aplicamos el filtro a la consulta usando el método Where
                query = query.Where(filter);
            }
            // Retornamos el primer elemento que cumple con el filtro o el valor por defecto (null)
            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }

        public bool Exists(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query.Any();
        }
    }
}
