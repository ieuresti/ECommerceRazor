using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using ECommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

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
            // Obtener el usuario
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            // Inicializar el objeto CarritoCompra con la informacion de Producto y Usuario
            CarritoCompra = new() {
                ApplicationUserId = claims.Value,
                Producto = _unitOfWork.Producto.GetFirstOrDefault(p => p.Id == id, "Categoria"),
                ProductoId = id
            };
            if (CarritoCompra == null)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var producto = _unitOfWork.Producto.GetFirstOrDefault(p => p.Id == CarritoCompra.ProductoId);
                if (producto == null)
                {
                    return NotFound("No se encontró el producto relacionado.");
                }
                // Validar si el inventario es cero
                if (producto.CantidadDisponible <= 0)
                {
                    TempData["Error"] = "El producto no tiene inventario disponible.";
                    return RedirectToAction("Detalle", new { id = CarritoCompra.ProductoId });
                }
                // Validar que la cantidad ingresada no sea menor a 1 ni mayor a la cantidad disponible
                if (CarritoCompra.Cantidad < 1 || CarritoCompra.Cantidad > producto.CantidadDisponible)
                {
                    TempData["Error"] = $"Debe ingresar un valor entre 1 y {producto.CantidadDisponible}";
                    return RedirectToAction("Detalle", new { id = CarritoCompra.ProductoId });
                }
                // Reducir la cantidad disponible del producto
                producto.CantidadDisponible -= CarritoCompra.Cantidad;

                // Consultar si el producto ya existe en el carrito de compra del usuario
                CarritoCompra carritoDesdeDb = _unitOfWork.CarritoCompra.GetFirstOrDefault(
                    filter: c => c.ApplicationUserId == CarritoCompra.ApplicationUserId &&
                    c.ProductoId == CarritoCompra.ProductoId
                );
                if (carritoDesdeDb == null)
                {
                    // Si no existe, agregar el producto al carrito de compra
                    _unitOfWork.CarritoCompra.Add(CarritoCompra);
                    _unitOfWork.Save();
                    TempData["Success"] = $"{CarritoCompra.Cantidad} unidad(es) añadida(s) al carrito exitosamente.";

                    // Actualizar la session del carrito de compra para reflejar la cantidad actualizada
                    HttpContext.Session.SetInt32(CNT.CarritoSession,
                        _unitOfWork.CarritoCompra.GetAll(c => c.ApplicationUserId == CarritoCompra.ApplicationUserId).ToList().Count);
                }
                else
                {
                    // Si ya existe, incrementar la cantidad
                    _unitOfWork.CarritoCompra.IncrementarContador(carritoDesdeDb, CarritoCompra.Cantidad);
                    TempData["Success"] = $"{CarritoCompra.Cantidad} unidad(es) actualizada(s) en el carrito exitosamente.";
                }
                
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
