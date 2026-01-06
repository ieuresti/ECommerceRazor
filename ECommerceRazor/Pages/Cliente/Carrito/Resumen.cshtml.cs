using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace ECommerceRazor.Pages.Cliente.Carrito
{
    [Authorize]
    public class ResumenModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public IEnumerable<CarritoCompra> ListaCarritoCompra { get; set; }
        public Orden Orden { get; set; }

        public ResumenModel(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            this.Orden = new Orden();
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
                    Orden.TotalOrden += (double)(itemCarrito.Producto.Precio * itemCarrito.Cantidad);
                }

                // Obtener los datos del usuario del repositorio de ApplicationUser
                ApplicationUser applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);
                // Rellenar los datos de la orden con la informacion del usuario
                Orden.NombreUsuario = applicationUser.Nombres + " " + applicationUser.Apellidos;
                Orden.Direccion = applicationUser.Direccion;
                Orden.Telefono = applicationUser.PhoneNumber;
                Orden.InstruccionesAdicionales = Orden.InstruccionesAdicionales;
            }

            return Page();
        }
    }
}
