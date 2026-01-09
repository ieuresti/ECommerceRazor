using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Utility
{
    // Clase para definir constantes de roles (CNT diminutivo de constantes)
    public static class CNT
    {
        public const string Administrador = "Administrador";
        public const string Cliente = "Cliente";

        // Estados para las ordenes en el pago
        public const string EstadoPendiente = "Estado_PagoPendiente";
        public const string EstadoPagoEnviado = "Estado_PagoEnviado";
        public const string EstadoPagoRechazado = "Estado_PagoRechazado";

        public const string EstadoEnviado = "Estado_Enviado";
        public const string EstadoCompletado = "Estado_Completado";
        public const string EstadoCancelado = "Estado_Cancelado";
        public const string EstadoReembolsado = "Estado_Reembolsado";

        // Nombre de la session para el carrito de compra
        public const string CarritoSession = "CarritoSession";
    }
}
