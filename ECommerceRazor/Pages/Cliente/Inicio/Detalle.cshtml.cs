using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceRazor.Pages.Cliente.Inicio
{
    public class DetalleModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DetalleModel(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [BindProperty]
        public Producto Producto { get; set; }

        [BindProperty]
        public int Cantidad { get; set; } // Cantidad para agregar al carrito

        public IActionResult OnGet(int id)
        {
            Producto = _unitOfWork.Producto.GetFirstOrDefault(p => p.Id == id, "Categoria");
            if (Producto == null)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public IActionResult OnPostAgregarAlCarrito()
        {
            if (Cantidad < 1 || Cantidad > Producto.CantidadDisponible)
            {
                ModelState.AddModelError("Cantidad", $"Debe ingresar un valor entre 1 y {Producto.CantidadDisponible}.");
                return Page();
            }

            TempData["Success"] = $"{Cantidad} unidad(es) del producto {Producto.Nombre} agregado al carrito exitosamente.";
            return RedirectToPage("/Cliente/Inicio/Index");
        }
    }
}
