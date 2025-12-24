using ECommerceRazor.Datos;
using ECommerceRazor.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceRazor.Pages.Categorias
{
    public class CrearModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CrearModel(ApplicationDbContext context)
        {
            this._context = context;
        }

        [BindProperty]
        // Esto es para enlazar el modelo Categoria con el formulario de la página Razor
        public Categoria Categoria { get; set; } = default!;

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            // Asignar la fecha de creación antes de guardar
            Categoria.FechaCreacion = DateTime.Now;
            _context.Categorias.Add(Categoria);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
