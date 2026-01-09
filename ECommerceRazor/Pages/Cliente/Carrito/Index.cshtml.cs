using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using ECommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace ECommerceRazor.Pages.Cliente.Carrito
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public IEnumerable<CarritoCompra> ListaCarritoCompra { get; set; }
        public double TotalCarrito;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            this.TotalCarrito = 0;
        }

        public IActionResult OnGet()
        {
            // Obtener el usuario
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                // Cargar los elementos del carrito de compra del usuario (incluyendo Producto y Categoria)
                ListaCarritoCompra = _unitOfWork.CarritoCompra.
                    GetAll(filter: u => u.ApplicationUserId == claim.Value, "Producto, Producto.Categoria");

                // Calcular el total del carrito
                foreach (var itemCarrito in ListaCarritoCompra)
                {
                    TotalCarrito += (double)(itemCarrito.Producto.Precio * itemCarrito.Cantidad);
                }
            }

            return Page();
        }

        public IActionResult OnPostMas(int carritoId)
        {
            var carrito = _unitOfWork.CarritoCompra.GetFirstOrDefault(filter: c => c.Id == carritoId);
            if (carrito != null)
            {
                _unitOfWork.CarritoCompra.IncrementarContador(carrito, 1);
            }
            return RedirectToPage("/Cliente/Carrito/Index");
        }

        public IActionResult OnPostMenos(int carritoId)
        {
            var carrito = _unitOfWork.CarritoCompra.GetFirstOrDefault(filter: c => c.Id == carritoId);
            if (carrito.Cantidad == 1)
            {
                // Almacenar en la session la cantidad actualizada de productos en el carrito
                var contador = _unitOfWork.CarritoCompra.GetAll(
                        u => u.ApplicationUserId == carrito.ApplicationUserId).ToList().Count - 1;

                _unitOfWork.CarritoCompra.Remove(carrito);
                _unitOfWork.Save();

                // Actualizar la session del carrito de compra para reflejar la cantidad actualizada
                HttpContext.Session.SetInt32(CNT.CarritoSession, contador);
            }
            else
            {
                _unitOfWork.CarritoCompra.DecrementarContador(carrito, 1);
            }
            return RedirectToPage("/Cliente/Carrito/Index");
        }

        public IActionResult OnPostEliminar(int carritoId)
        {
            var carrito = _unitOfWork.CarritoCompra.GetFirstOrDefault(filter: c => c.Id == carritoId);
            if (carrito != null)
            {
                _unitOfWork.CarritoCompra.Remove(carrito);

                // Almacenar en la session la cantidad actualizada de productos en el carrito
                var contador = _unitOfWork.CarritoCompra.GetAll(
                        u => u.ApplicationUserId == carrito.ApplicationUserId).ToList().Count - 1;

                _unitOfWork.Save();

                // Actualizar la session del carrito de compra para reflejar la cantidad actualizada
                HttpContext.Session.SetInt32(CNT.CarritoSession, contador);
            }
            return RedirectToPage("/Cliente/Carrito/Index");
        }
    }
}
