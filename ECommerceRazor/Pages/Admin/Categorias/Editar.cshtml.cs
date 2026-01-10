using ECommerce.DataAccess;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceRazor.Pages.Admin.Categorias
{
    [Authorize(Roles = "Administrador")]
    public class EditarModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public EditarModel(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [BindProperty]
        public Categoria Categoria { get; set; } = default!;

        public IActionResult OnGet(int id)
        {
            // Buscar la categoría por su ID
            Categoria = _unitOfWork.Categoria.GetFirstOrDefault(c => c.Id == id);
            if (Categoria == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            // Buscar la categoría en la base de datos
            var categoriaEnDb = _unitOfWork.Categoria.GetFirstOrDefault(c => c.Id == Categoria.Id);
            if (categoriaEnDb == null)
            {
                return NotFound();
            }
            // Actualizar los campos de la categoría
            categoriaEnDb.Nombre = Categoria.Nombre;
            categoriaEnDb.OrdenVisualizacion = Categoria.OrdenVisualizacion;

            _unitOfWork.Save();

            TempData["Success"] = "Categoría actualizada exitosamente.";

            return RedirectToPage("Index");
        }
    }
}
