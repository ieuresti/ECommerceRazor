using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using ECommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceRazor.Pages.Admin.Ordenes
{
    [Authorize]
    public class DetalleOrdenModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DetalleOrdenModel(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [BindProperty(SupportsGet = true)] // Permitir enlazar en solicitudes GET
        public int Id { get; set; }
        public List<string> EstadosDisponibles { get; set; }
        public Orden Orden { get; set; }
        public IEnumerable<DetalleOrden> DetallesOrden { get; set; }

        public IActionResult OnGet(int id)
        {
            // Obtener la orden por Id
            Orden = _unitOfWork.Orden.GetFirstOrDefault(o => o.Id == id, "ApplicationUser");
            if (Orden == null)
            {
                return NotFound("No se encontró la orden");
            }
            // Obtener los detalles de la orden
            DetallesOrden = _unitOfWork.DetalleOrden.GetAll(d => d.OrdenId == Id, "Producto");
            EstadosDisponibles = new List<string>
            {
                CNT.EstadoCompletado,
                CNT.EstadoCancelado,
                CNT.EstadoReembolsado
            };
            return Page();
        }

        public IActionResult OnPostActualizarEstado(int id, string nuevoEstado)
        {
            // Buscar la orden por Id
            Orden = _unitOfWork.Orden.GetFirstOrDefault(o => o.Id == id);
            if (Orden == null)
            {
                return new JsonResult(new { success = false, message = "Orden no encontrada." });
            }
            // Definir los estados permitidos
            var EstadosPermitidos = new List<string>
            {
                CNT.EstadoCompletado,
                CNT.EstadoCancelado,
                CNT.EstadoReembolsado
            };
            if (!EstadosPermitidos.Contains(nuevoEstado)) {
                return new JsonResult(new { success = false, message = "Estado invalido." });
            }
            // Actualizar el estado de la orden
            Orden.Estatus = nuevoEstado;
            _unitOfWork.Save();
            // Retornar respuesta JSON exitosa
            return new JsonResult(new { success = true, message = "Estado de la orden actualizado exitosamente." });
        }
    }
}
