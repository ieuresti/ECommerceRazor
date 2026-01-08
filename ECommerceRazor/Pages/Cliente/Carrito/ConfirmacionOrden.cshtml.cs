using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using ECommerce.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe.Checkout;

namespace ECommerceRazor.Pages.Cliente.Carrito
{
    public class ConfirmacionOrdenModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public int OrdenId { get; set; }

        public ConfirmacionOrdenModel(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public void OnGet(int id)
        {
            Orden orden = _unitOfWork.Orden.GetFirstOrDefault(o => o.Id == id);
            // Verificar si la orden tiene un SessionId asociado
            if (orden.SessionId != null)
            {
                var servicio = new SessionService();
                Session session = servicio.Get(orden.SessionId);
                // Verificar el estado del pago
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    // Actualizar el estado de la orden a "Pago Enviado"
                    orden.Estatus = CNT.EstadoPagoEnviado;
                    // Asignar el PaymentIntentId de Stripe a la orden
                    orden.PaymentIntentId = session.PaymentIntentId;
                    _unitOfWork.Save();
                }
            }

            List<CarritoCompra> listaCarritoCompra = _unitOfWork.CarritoCompra.
                GetAll(filter: c => c.ApplicationUserId == orden.UsuarioId).ToList();

            // Limpiar el carrito de compra del usuario
            _unitOfWork.CarritoCompra.RemoveRange(listaCarritoCompra);
            _unitOfWork.Save();
            // Asignar el Id de la orden para mostrar en la vista
            OrdenId = id;
        }
    }
}
