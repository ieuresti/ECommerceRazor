using ECommerceRazor.Modelos;
using Microsoft.EntityFrameworkCore;

namespace ECommerceRazor.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets de las entidades de la aplicación
        public DbSet<Categoria> Categorias { get; set; }
    }
}
