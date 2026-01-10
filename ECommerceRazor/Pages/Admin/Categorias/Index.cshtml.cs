using ECommerce.DataAccess;
using ECommerce.DataAccess.Repository;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ECommerceRazor.Pages.Admin.Categorias
{
    [Authorize(Roles = "Administrador")]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        // Con en default! le indicamos al compilador que esta propiedad se inicializa en otro lugar
        public IEnumerable<Categoria> Categorias { get; set; } = default!;

        public void OnGet()
        {
            // Obtener todas las categorias desde el repositorio
            Categorias = _unitOfWork.Categoria.GetAll();
        }

        public IActionResult OnPostDelete([FromBody] int id)
        {
            var categoria = _unitOfWork.Categoria.GetFirstOrDefault(c => c.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            _unitOfWork.Categoria.Remove(categoria);
            _unitOfWork.Save();

            TempData["Success"] = "Categoría eliminada exitosamente.";

            // Retornar una respuesta JSON indicando éxito para manejarla del lado del cliente (sweetalert)
            return new JsonResult(new { success = true, message = "Categoría eliminada correctamente" });
        }
    }
}
