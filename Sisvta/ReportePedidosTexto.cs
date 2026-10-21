using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using Sisvta.UseCases.DTO;

namespace Sisvta
{
    public static class ReportePedidosTexto
    {
        public static void ProcesarEImprimir(IEnumerable<ReportePedidoItemDto> datos)
        {
            var lista = new List<ReportePedidoItemDto>(datos);

            if (lista.Count == 0)
            {
                MessageBox.Show("No se encontraron registros para imprimir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MessageBox.Show("Reporte Pedidos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            string textoReporte = GenerarTexto(lista);
            GuardarOAbrirTexto(textoReporte);
        }

        private static string GenerarTexto(List<ReportePedidoItemDto> datos)
        {
            StringBuilder sb = new StringBuilder();
            string clienteActual = string.Empty;
            decimal subtotalCliente = 0;
            decimal totalGeneral = 0;

            sb.AppendLine("============================================================");
            sb.AppendLine("              REPORTE DE PEDIDOS POR CLIENTE                ");
            sb.AppendLine($"Fecha de Emisión: {DateTime.Now:dd/MM/yyyy HH:mm}");
            sb.AppendLine("============================================================");
            sb.AppendLine();
            MessageBox.Show("Reporte Pedidos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            foreach (var row in datos)
            {
                if (clienteActual != row.IdCliente)
                {
                    if (!string.IsNullOrEmpty(clienteActual))
                    {
                        sb.AppendLine("------------------------------------------------------------");
                        sb.AppendLine(string.Format("  SUBTOTAL CLIENTE: {0,38:C2}", subtotalCliente));
                        sb.AppendLine("------------------------------------------------------------");
                        sb.AppendLine();
                        subtotalCliente = 0;
                    }

                    clienteActual = row.IdCliente;
                    sb.AppendLine($"CLIENTE: [{row.IdCliente}] {row.NombreCliente}");
                    sb.AppendLine(string.Format("{0,-12} {1,-20} {2,24}", "ID Pedido", "Fecha", "Total"));
                    sb.AppendLine("------------------------------------------------------------");
                }

                sb.AppendLine(string.Format("{0,-12} {1,-20:dd/MM/yyyy HH:mm} {2,24:C2}", row.Id_Factura, row.FechaFactura, row.Importe));

                subtotalCliente += row.Importe;
                totalGeneral += row.Importe;
            }

            sb.AppendLine("------------------------------------------------------------");
            sb.AppendLine(string.Format("  SUBTOTAL CLIENTE: {0,38:C2}", subtotalCliente));
            sb.AppendLine("------------------------------------------------------------");
            sb.AppendLine();

            sb.AppendLine("============================================================");
            sb.AppendLine(string.Format("  TOTAL GENERAL:    {0,38:C2}", totalGeneral));
            sb.AppendLine("============================================================");

            return sb.ToString();
        }

        private static void GuardarOAbrirTexto(string contenidoTexto)
        {
            string rutaArchivo = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                $"ReportePedidos_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
            );

            System.IO.File.WriteAllText(rutaArchivo, contenidoTexto, Encoding.UTF8);

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = rutaArchivo,
                UseShellExecute = true
            });
        }
    }
}