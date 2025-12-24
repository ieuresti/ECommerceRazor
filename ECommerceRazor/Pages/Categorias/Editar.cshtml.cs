using ECommerceRazor.Datos;
using ECommerceRazor.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceRazor.Pages.Categorias
{
    public class EditarModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditarModel(ApplicationDbContext context)
        {
            this._context = context;
        }

        [BindProperty]
        public Categoria Categoria { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Buscar la categoría por su ID
            Categoria = await _context.Categorias.FindAsync(id);
            if (Categoria == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            // Buscar la categoría en la base de datos
            var categoriaEnDb = await _context.Categorias.FindAsync(Categoria.Id);
            if (categoriaEnDb == null)
            {
                return NotFound();
            }
            // Actualizar los campos de la categoría
            categoriaEnDb.Nombre = Categoria.Nombre;
            categoriaEnDb.OrdenVisualizacion = Categoria.OrdenVisualizacion;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Categoría actualizada exitosamente.";

            return RedirectToPage("Index");
        }
    }
}
