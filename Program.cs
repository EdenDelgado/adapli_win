using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sisvta;
using Sisvta.Domain.Interfaces;
using Sisvta.Infrastructure.Persistence.Contexts;
using Sisvta.Infrastructure.Repositories;
using Sisvta.Infrastructure.Repositories;
using Sisvta.UseCases;
using System;
using System.Windows.Forms;

namespace Sisvta;

static class Program
{
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
                services.AddScoped<ILineaArticuloRepository, LineaArticuloRepository>(); // <-- Asegúrate de que esté mapeado a su clas
                services.AddScoped<IGrupoLineaRepository, GrupoLineaRepository>();
                services.AddScoped<IProveedoresRepository, ProveedoresRepository>();

                // --- 4. Formularios de la Aplicación ---
                services.AddTransient<Fmain>();
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
                services.AddTransient<Fcprov>(); services.AddTransient<Fprov>();
                services.AddTransient<Fclispre>(); services.AddTransient<Fccampre>();
                services.AddTransient<Fcinvfis>(); services.AddTransient<Fimpinv>();
                services.AddTransient<Fcdev>();
            })
            .Build();

        // Iniciamos el formulario principal resolviéndolo a través del contenedor
        using var scope = host.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        var mainForm = serviceProvider.GetRequiredService<Fmain>();
        Application.Run(mainForm);
        //Application.Run(ServiceProvider.GetRequiredService<Form99>());
    }
}