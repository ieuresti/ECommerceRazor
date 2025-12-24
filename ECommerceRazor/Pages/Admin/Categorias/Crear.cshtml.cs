using ECommerce.DataAccess;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceRazor.Pages.Admin.Categorias
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
            // Validacion personalizada: comprobar si el nombre ya existe
            bool nombreExiste = _context.Categorias.Any(c => c.Nombre == Categoria.Nombre);
            if (nombreExiste)
            {
                // Agregar un error de modelo personalizado
                ModelState.AddModelError("Categoria.Nombre", "El nombre de la categoría ya existe. Por favor, elija otro nombre.");
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }
            // Asignar la fecha de creación antes de guardar
            Categoria.FechaCreacion = DateTime.Now;
            _context.Categorias.Add(Categoria);
            await _context.SaveChangesAsync();

            // Usar TempData para mostrar un mensaje de éxito después de la redirección
            TempData["Success"] = "Categoría creada exitosamente.";

            return RedirectToPage("Index");
        }
    }
}
