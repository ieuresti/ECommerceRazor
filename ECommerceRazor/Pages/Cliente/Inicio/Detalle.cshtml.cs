using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceRazor.Pages.Cliente.Inicio
{
    [Authorize]
    public class DetalleModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DetalleModel(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [BindProperty]
        public CarritoCompra CarritoCompra { get; set; }

        public IActionResult OnGet(int id)
        {
            // Inicializar CarritoCompra con el producto correspondiente
            CarritoCompra = new() {
                Producto = _unitOfWork.Producto.GetFirstOrDefault(p => p.Id == id, "Categoria")
            };
            if (CarritoCompra == null)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public IActionResult OnPostAgregarAlCarrito()
        {
            //if (Cantidad < 1 || Cantidad > Producto.CantidadDisponible)
            //{
            //    ModelState.AddModelError("Cantidad", $"Debe ingresar un valor entre 1 y {Producto.CantidadDisponible}.");
            //    return Page();
            //}

            //TempData["Success"] = $"{Cantidad} unidad(es) del producto {Producto.Nombre} agregado al carrito exitosamente.";
            return RedirectToPage("/Cliente/Inicio/Index");
        }
    }
}
