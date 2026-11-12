using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sisvta;
using Sisvta.Domain.Interfaces;
using Sisvta.Infrastructure.Persistence.Contexts;
using Sisvta.Infrastructure.Repositories;
using Sisvta.UseCases;
using System;
using System.Windows.Forms;

namespace Sisvta
{
    static class Program
    {
        // Propiedad estática para exponer el Host de forma global
        public static IHost HostInstance { get; private set; }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // --- 1. Base de Datos (Entity Framework Core) ---
                    string connString = "Server=localhost\\SQLEXPRESS; Database=adaplirenelomeli; Integrated Security=True; Encrypt=False; TrustServerCertificate=True;";

                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(connString));

                    // --- 2. Repositorios (Con su cadena de conexión explícita) ---
                    services.AddScoped<IClienteRepository, ClienteRepository>();
                    services.AddScoped<IProductoRepository, ProductoRepository>();

                    services.AddScoped<IVentaRepository>(provider => new VentaRepository(connString));
                    services.AddScoped<IPedidoRepository>(provider => new PedidoRepository(connString));
                    services.AddScoped<IReportePedidoRepository, ReportePedidoRepository>();
                    services.AddScoped<IReportePedidoPorPedidoRepository>(provider => new ReportePedidoPorPedidoRepository(connString));

                    // --- 3. Casos de Uso ---
                    services.AddScoped<IGestionarPedidosUseCase, GestionarPedidosUseCase>();
                    services.AddScoped<GestionarPedidosUseCase>();
                    services.AddScoped<IConsultarClientesUseCase, ConsultarClientesUseCase>();
                    services.AddScoped<GestionarClientesUseCase>();
                    services.AddScoped<IConsultarProveedoresUseCase, ConsultarProveedoresUseCase>();
                    services.AddScoped<GestionarProveedoresUseCase>();
                    services.AddScoped<IConsultarProductosUseCase, ConsultarProductosUseCase>();
                    services.AddScoped<GestionarProductosUseCase>();
                    services.AddScoped<IObtenerTotalVentasQuery, ObtenerTotalVentasQuery>();
                    services.AddScoped<GenerarReporteClientesUseCase>();
                    services.AddScoped<ReportePedidoPorPedidoUseCase>();
                    services.AddScoped<GestionarGruposLineasUseCase>();
                    services.AddScoped<GestionarLineasArticulosUseCase>();
                    services.AddScoped<ILineaArticuloRepository, LineaArticuloRepository>();
                    services.AddScoped<IGrupoLineaRepository, GrupoLineaRepository>();
                    services.AddScoped<IProveedoresRepository, ProveedoresRepository>();

                    // --- 4. Formularios de la Aplicación ---
                    services.AddTransient<Fmain>();
                    services.AddTransient<Fusu>();
                    services.AddTransient<Fcli>();
                    services.AddTransient<Fccli>();
                    services.AddTransient<Fprod>();
                    services.AddTransient<Fcprod>();
                    services.AddTransient<Fped>();
                    services.AddTransient<Fcped>();
                    services.AddTransient<Fvta>();
                    services.AddTransient<Fprodd>();
                    services.AddTransient<Fmprod>();
                    services.AddTransient<Fcsal>();
                    services.AddTransient<Fdped>();
                    services.AddTransient<Fcmov>();
                    services.AddTransient<Fcgru>();
                    services.AddTransient<Fcalm>();
                    services.AddTransient<Flin>();
                    services.AddTransient<Fclin>();
                    services.AddTransient<Fcent>();
                    services.AddTransient<Fctra>();
                    services.AddTransient<Fcsug>();
                    services.AddTransient<Fctac>();
                    services.AddTransient<Fcprov>();
                    services.AddTransient<Fprov>();
                    services.AddTransient<Fclispre>();
                    services.AddTransient<Fccampre>();
                    services.AddTransient<Fcinvfis>();
                    services.AddTransient<Fimpinv>();
                    services.AddTransient<Fcdev>();
                    services.AddTransient<Fsrep>();
                })
                .Build();

            // Asignamos el host construido a la propiedad estática
            HostInstance = host;

            // Iniciamos el contenedor de servicios
            using var scope = host.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            // 1. Mostramos el Login (Fusu) de forma modal resolviéndolo por DI
            var loginForm = serviceProvider.GetRequiredService<Fusu>();

            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // 2. Si el acceso es exitoso, lanzamos Fmain como la ventana principal del sistema
                var mainForm = serviceProvider.GetRequiredService<Fmain>();
                Application.Run(mainForm);
            }
            else
            {
                // Si el usuario cancela o cierra el login, la aplicación finaliza limpiamente
                Application.Exit();
            }
        }
    }
}