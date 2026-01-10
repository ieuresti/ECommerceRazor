using ECommerce.DataAccess;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceRazor.Pages.Admin.Categorias
{
    [Authorize(Roles = "Administrador")]
    public class CrearModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public CrearModel(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [BindProperty]
        // Esto es para enlazar el modelo Categoria con el formulario de la página Razor
        public Categoria Categoria { get; set; } = default!;

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            // Validacion personalizada: comprobar si el nombre ya existe
            bool nombreExiste = _unitOfWork.Categoria.Exists(c => c.Nombre == Categoria.Nombre);
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

            _unitOfWork.Categoria.Add(Categoria);
            _unitOfWork.Save();

            // Usar TempData para mostrar un mensaje de éxito después de la redirección
            TempData["Success"] = "Categoría creada exitosamente.";

            return RedirectToPage("Index");
        }
    }
}
