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
    public class ResumenModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public IEnumerable<CarritoCompra> ListaCarritoCompra { get; set; }
        [BindProperty]
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

        public IActionResult OnPost()
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

                // Rellenar los datos de la orden
                Orden.Estatus = CNT.EstadoPendiente;
                Orden.FechaOrden = DateTime.Now;
                Orden.UsuarioId = claim.Value;
                Orden.NombreUsuario = applicationUser.Nombres + " " + applicationUser.Apellidos;
                Orden.Telefono = Orden.Telefono;
                Orden.Direccion = Orden.Direccion;
                Orden.InstruccionesAdicionales = Orden.InstruccionesAdicionales;

                _unitOfWork.Orden.Add(Orden);
                _unitOfWork.Save();

                // Aqui se agrega el detalle de la orden
                foreach (var item in ListaCarritoCompra)
                {
                    DetalleOrden detalleOrden = new DetalleOrden()
                    {
                        ProductoId = item.ProductoId,
                        OrdenId = Orden.Id,
                        NombreProducto = item.Producto.Nombre,
                        Precio = (double)item.Producto.Precio,
                        Cantidad = item.Cantidad
                    };
                    _unitOfWork.DetalleOrden.Add(detalleOrden);
                    _unitOfWork.Save();
                }

                // Limpiar el carrito de compras
                _unitOfWork.CarritoCompra.RemoveRange(ListaCarritoCompra);
                _unitOfWork.Save();
            }

            return Page();
        }
    }
}
