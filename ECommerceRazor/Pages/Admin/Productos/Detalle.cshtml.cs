using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceRazor.Pages.Admin.Productos
{
    public class DetalleModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DetalleModel(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [BindProperty]
        public Producto Producto { get; set; } = default!;
        public Categoria Categoria { get; set; } = default!;

        public IActionResult OnGet(int id)
        {
            Producto = _unitOfWork.Producto.GetFirstOrDefault(p => p.Id == id);
            if (Producto == null)
            {
                return NotFound();
            }
            Categoria = _unitOfWork.Categoria.GetFirstOrDefault(c => c.Id == Producto.CategoriaId);

            return Page();
        }
    }
}
