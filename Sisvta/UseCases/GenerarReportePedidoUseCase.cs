using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sisvta.Infrastructure.Repositories;

namespace Sisvta.UseCases
{

   

    public class GenerarReportePedidosUseCase
    {
        private readonly ReportePedidoRepository _repository;

        public GenerarReportePedidosUseCase(ReportePedidoRepository repository)
        {
            _repository = repository;
        }

        public string Ejecutar(out int totalRegistros)
        {
            var datos = _repository.ObtenerDatosReporte().ToList();
            totalRegistros = datos.Count;

            if (totalRegistros == 0) return string.Empty;

            StringBuilder sb = new StringBuilder();
            int pedidoActual = -1;
            decimal totalPedido = 0;
            decimal totalGeneral = 0;

            sb.AppendLine("================================================================================");
            sb.AppendLine("                 REPORTE DE PEDIDOS Y DETALLE DE ÍTEMS                          ");
            sb.AppendLine($"Fecha de emisión: {DateTime.Now:dd/MM/yyyy HH:mm}");
            sb.AppendLine("================================================================================");
            sb.AppendLine();

            foreach (var row in datos)
            {
                string descripcion = row.Descripcion;
                if (descripcion.Length > 22)
                {
                    descripcion = descripcion.Substring(0, 22);
                }

                if (pedidoActual != row.Id_Factura)
                {
                    if (pedidoActual != -1)
                    {
                        sb.AppendLine("  ------------------------------------------------------------------------------");
                        sb.AppendLine(string.Format("  TOTAL PEDIDO #{0}: {1,57:C2}", pedidoActual, totalPedido));
                        sb.AppendLine("================================================================================");
                        sb.AppendLine();
                        totalPedido = 0;
                    }

                    pedidoActual = row.Id_Factura;

                    sb.AppendLine($"PEDIDO N°: {row.Id_Factura,-10} FECHA: {row.FechaFactura:dd/MM/yyyy HH:mm}");
                    sb.AppendLine($"CLIENTE  : [{row.IdCliente}] {row.NombreCliente}");
                    sb.AppendLine("--------------------------------------------------------------------------------");
                    sb.AppendLine(string.Format("  {0,-4} {1,-12} {2,-22} {3,8} {4,11} {5,13}", "Item", "Cód. Barras", "Descripción", "Cant.", "Precio U.", "Importe"));
                    sb.AppendLine("  ------------------------------------------------------------------------------");
                }

                sb.AppendLine(string.Format("  {0,-4} {1,-12} {2,-22} {3,8:N2} {4,11:C2} {5,13:C2}",
                    row.Detallefatura, row.Clave, descripcion, row.Cantidad, row.Precio, row.Importe));

                totalPedido += row.Importe;
                totalGeneral += row.Importe;
            }

            if (pedidoActual != -1)
            {
                sb.AppendLine("  ------------------------------------------------------------------------------");
                sb.AppendLine(string.Format("  TOTAL PEDIDO #{0}: {1,57:C2}", pedidoActual, totalPedido));
                sb.AppendLine("================================================================================");
                sb.AppendLine();
            }

            sb.AppendLine("================================================================================");
            sb.AppendLine(string.Format("   TOTAL GENERAL REPORTE: {0,54:C2}", totalGeneral));
            sb.AppendLine("================================================================================");

            return sb.ToString();
        }
    }
}
