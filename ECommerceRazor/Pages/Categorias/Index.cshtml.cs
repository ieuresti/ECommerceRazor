using ECommerceRazor.Datos;
using ECommerceRazor.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ECommerceRazor.Pages.Categorias
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            this._context = context;
        }
        // Con en default! le indicamos al compilador que esta propiedad se inicializa en otro lugar
        public IList<Categoria> Categorias { get; set; } = default!;

        public async Task OnGetAsync()
        {
            // Obtener las categorías ordenadas por OrdenVisualizacion
            Categorias = await _context.Categorias.OrderBy(c => c.OrdenVisualizacion).ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync([FromBody] int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Categoría eliminada exitosamente.";

            // Retornar una respuesta JSON indicando éxito para manejarla del lado del cliente (sweetalert)
            return new JsonResult(new { success = true, message = "Categoría eliminada correctamente" });
        }
    }
}
