using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms; // <-- ESTO FALTABA Y CAUSABA QUE SE DETUVIERA
using Sisvta.UseCases.DTO;

namespace Sisvta
{
    public static class ReportePedidoPorPedidoTexto
    {
        public static void ProcesarEImprimir(IEnumerable<ReportePedidoItemDto> datos)
        {
            var sb = new StringBuilder();
            sb.AppendLine("==================================================");
            sb.AppendLine("           REPORTE DE PEDIDOS POR PEDIDO          ");
            sb.AppendLine("==================================================");
            sb.AppendLine($"Fecha de generación: {DateTime.Now}");
            sb.AppendLine("--------------------------------------------------\n");

            int pedidoActual = -1;
            int totalRegistros = 0;

            foreach (var item in datos)
            {
                totalRegistros++;

                // Si cambia de pedido, agrupamos visualmente la cabecera
                if (pedidoActual != item.Id_Factura)
                {
                    pedidoActual = item.Id_Factura;
                    sb.AppendLine($"\nPedido ID: {pedidoActual} | Fecha: {item.FechaFactura:yyyy-MM-dd}");
                    sb.AppendLine($"Cliente: {item.IdCliente} - {item.NombreCliente}");
                    sb.AppendLine("  --- Detalle ---");
                }

                sb.AppendLine($"  [Item {item.Detallefatura}] Prod: {item.Clave} - {item.Descripcion}");
                sb.AppendLine($"    Cant: {item.Cantidad} x ${item.Precio:N2} = ${item.Importe:N2}");
            }

            // Validación por si no entró ningún registro al foreach
            if (totalRegistros == 0)
            {
                MessageBox.Show("El reporte se ejecutó, pero no hay datos para mostrar.", "Sin Registros", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            sb.AppendLine("\n==================================================");
            sb.AppendLine("Fin del reporte.");

            // Guardarlo en un archivo de texto temporal y abrir el Bloc de notas
            string rutaArchivo = Path.Combine(Path.GetTempPath(), "ReportePedidoPorPedido.txt");
            File.WriteAllText(rutaArchivo, sb.ToString(), Encoding.UTF8);

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = rutaArchivo,
                UseShellExecute = true
            });
        }
    }
}