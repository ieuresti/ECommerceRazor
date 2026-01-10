using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceRazor.Pages.Admin.Productos
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
        public IEnumerable<Producto> Productos { get; set; } = default!;

        public void OnGet()
        {
            // Cargar todos los productos junto con sus categorias relacionadas
            Productos = _unitOfWork.Producto.GetAll(filter: null, "Categoria");
        }

        public IActionResult OnPostDelete([FromBody] int id)
        {
            var producto = _unitOfWork.Producto.GetFirstOrDefault(c => c.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            _unitOfWork.Producto.Remove(producto);
            _unitOfWork.Save();

            TempData["Success"] = "Producto eliminado exitosamente.";

            // Retornar una respuesta JSON indicando éxito para manejarla del lado del cliente (sweetalert)
            return new JsonResult(new { success = true, message = "Producto eliminado correctamente" });
        }
    }
}
