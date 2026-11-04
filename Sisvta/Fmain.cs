using Microsoft.Extensions.DependencyInjection;
using Sisvta.UseCases;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Sisvta
{
    public partial class Fmain : Form
    {
        private readonly IServiceProvider? _serviceProvider;

        // Contenedores y Timer del menú lateral
        private FlowLayoutPanel flpMenuLateral;
        private Panel panelContenedor;
        private System.Windows.Forms.Timer timerEfectoMenu;

        private const int AnchoMinimo = 70;   // Ancho colapsado del menú (espacio fijo reservado)
        private const int AnchoMaximo = 220;  // Ancho expandido del menú al hacer hover

        // Botones principales y submenús
        private Button btnPDV, btnCxP, btnCxC, btnBancos, btnInventarios, btnReportes;
        private Panel pnlSubmenuPDV, pnlSubmenuCxP, pnlSubmenuCxC, pnlSubmenuBancos, pnlSubmenuInventarios, pnlSubmenuReportes;

        public Fmain()
        {
            InitializeComponent();
            this.IsMdiContainer = false;
        }

        public Fmain(IServiceProvider serviceProvider) : this()
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            ConfigurarMenuLateralDinamico();
        }

        private void ConfigurarMenuLateralDinamico()
        {
            this.Size = new Size(1400, 760);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Sisvta - Sistema de Control de Almacén y Ventas";

            // 1. Panel lateral izquierdo (menú flotante que empieza en 70px)
            flpMenuLateral = new FlowLayoutPanel();
            flpMenuLateral.Dock = DockStyle.Left;
            flpMenuLateral.Width = AnchoMinimo;
            flpMenuLateral.BackColor = Color.FromArgb(40, 40, 40);
            flpMenuLateral.FlowDirection = FlowDirection.TopDown;
            flpMenuLateral.WrapContents = false;
            flpMenuLateral.AutoScroll = true;
            this.Controls.Add(flpMenuLateral);

            // 2. Panel contenedor central donde se cargarán los formularios hijos vía DI
            // Se posiciona explícitamente a partir de X = 70 para que las pantallas hijas inicien exactamente ahí
            panelContenedor = new Panel();
            panelContenedor.Location = new Point(AnchoMinimo, 0);
            panelContenedor.Size = new Size(this.ClientSize.Width - AnchoMinimo, this.ClientSize.Height);
            panelContenedor.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelContenedor.BackColor = Color.FromArgb(245, 247, 245);
            this.Controls.Add(panelContenedor);

            // Aseguramos que el menú flote por encima del panel contenedor al expandirse
            flpMenuLateral.BringToFront();

            // --- CONSTRUCCIÓN DE SECCIONES Y SUBMENÚS ---

            // --- SECCIÓN BANCOS ---
            btnBancos = CrearBotonPrincipal("🏦", "Bancos");
            btnBancos.Click += (s, e) => AlternarSubmenu(pnlSubmenuBancos, 105);
            flpMenuLateral.Controls.Add(btnBancos);

            pnlSubmenuBancos = CrearPanelSubmenu();
            var btnDepositos = CrearBotonSubmenu("📥", "Depósitos");
            var btnRetiros = CrearBotonSubmenu("📤", "Retiros");
            var btnEstadosCuenta = CrearBotonSubmenu("📄", "Estados de Cta.");
            pnlSubmenuBancos.Controls.Add(btnEstadosCuenta);
            pnlSubmenuBancos.Controls.Add(btnRetiros);
            pnlSubmenuBancos.Controls.Add(btnDepositos);
            flpMenuLateral.Controls.Add(pnlSubmenuBancos);

            // --- SECCIÓN CxP ---
            btnCxP = CrearBotonPrincipal("📁", "CxP");
            btnCxP.Click += (s, e) => AlternarSubmenu(pnlSubmenuCxP, 80);
            flpMenuLateral.Controls.Add(btnCxP);

            pnlSubmenuCxP = CrearPanelSubmenu();
            var btnCompras = CrearBotonSubmenu("🛒", "Compras");
            var btnPagos = CrearBotonSubmenu("💳", "Pagos");
            var btnProveedores = CrearBotonSubmenu("👥", "Proveedores");

            btnProveedores.Click += (s, e) => AbrirFormularioEnPanel<Fcprov>();

            pnlSubmenuCxP.Controls.Add(btnPagos);
            pnlSubmenuCxP.Controls.Add(btnCompras);
            pnlSubmenuCxP.Controls.Add(btnProveedores);
            flpMenuLateral.Controls.Add(pnlSubmenuCxP);

            // --- SECCIÓN CxC ---
            btnCxC = CrearBotonPrincipal("📁", "CxC");
            btnCxC.Click += (s, e) => AlternarSubmenu(pnlSubmenuCxC, 80);
            flpMenuLateral.Controls.Add(btnCxC);

            pnlSubmenuCxC = CrearPanelSubmenu();
            var btnVentasCxC = CrearBotonSubmenu("🛒", "Ventas");
            var btnCobranza = CrearBotonSubmenu("💳", "Cobranza");

            pnlSubmenuCxC.Controls.Add(btnCobranza);
            pnlSubmenuCxC.Controls.Add(btnVentasCxC);
            flpMenuLateral.Controls.Add(pnlSubmenuCxC);

            // --- SECCIÓN INVENTARIOS ---
            btnInventarios = CrearBotonPrincipal("📦", "Inventarios");
            btnInventarios.Click += (s, e) => AlternarSubmenu(pnlSubmenuInventarios, 540);
            flpMenuLateral.Controls.Add(btnInventarios);

            pnlSubmenuInventarios = CrearPanelSubmenu();
            var btnAlmacenes = CrearBotonSubmenu("🏢", "Almacenes");
            var btnArticulos = CrearBotonSubmenu("📦", "Artículos");
            var btnCambioPrecio = CrearBotonSubmenu("🏷️", "Cambio de Precio");
            var btnDevoluciones = CrearBotonSubmenu("↩️", "Devoluciones");
            var btnEntradas = CrearBotonSubmenu("📥", "Entradas");
            var btnSalidas = CrearBotonSubmenu("📤", "Salidas");
            var btnGrupos = CrearBotonSubmenu("📁", "Grupos");
            var btnLineas = CrearBotonSubmenu("📑", "Líneas");
            var btnListaPrecios = CrearBotonSubmenu("💲", "Lista de Precios");
            var btnReportesInv = CrearBotonSubmenu("📊", "Reportes de Inventario");
            var btninvfis = CrearBotonSubmenu("📋", "Inventario Físico");
            var btnimpinv = CrearBotonSubmenu("📂", "Importar Inventario");
            var btnsugerencias = CrearBotonSubmenu("💡", "Sugerencias de Compra");
            var btntraspasos = CrearBotonSubmenu("🔀", "Traspasos");
            var btntransacciones = CrearBotonSubmenu("🔄", "Transacciones");

            // Enlaces con formularios usando DI (Entradas y Salidas integradas)
            btnAlmacenes.Click += (s, e) => AbrirFormularioEnPanel<Fcalm>();
            btnArticulos.Click += (s, e) => AbrirFormularioEnPanel<Fprod>();
            btnEntradas.Click += (s, e) => AbrirFormularioEnPanel<Fcent>();
            btnSalidas.Click += (s, e) => AbrirFormularioEnPanel<Fcsal>();
            btnGrupos.Click += (s, e) => AbrirFormularioEnPanel<Fcgru>();
            btnLineas.Click += (s, e) => AbrirFormularioEnPanel<Fclin>();
            btnListaPrecios.Click += (s, e) => AbrirFormularioEnPanel<Fclispre>();
            btnCambioPrecio.Click += (s, e) => AbrirFormularioEnPanel<Fccampre>();
            btntransacciones.Click += (s, e) => AbrirFormularioEnPanel<Fctac>();
            btntraspasos.Click += (s, e) => AbrirFormularioEnPanel<Fctra>();
            btnsugerencias.Click += (s, e) => AbrirFormularioEnPanel<Fcsug>();
            btnimpinv.Click += (s, e) => AbrirFormularioEnPanel<Fimpinv>();
            btninvfis.Click += (s, e) => AbrirFormularioEnPanel<Fcinvfis>();
            btnDevoluciones.Click += (s, e) => AbrirFormularioEnPanel<Fcdev>();
            btnReportesInv.Click += (s, e) => AbrirFormularioEnPanel<Fsrep>();

            pnlSubmenuInventarios.Controls.Add(btnListaPrecios);
            pnlSubmenuInventarios.Controls.Add(btnCambioPrecio);
            pnlSubmenuInventarios.Controls.Add(btntransacciones);
            pnlSubmenuInventarios.Controls.Add(btnSalidas);
            pnlSubmenuInventarios.Controls.Add(btnDevoluciones);
            pnlSubmenuInventarios.Controls.Add(btnReportesInv);
            pnlSubmenuInventarios.Controls.Add(btntraspasos);
            pnlSubmenuInventarios.Controls.Add(btnsugerencias);
            pnlSubmenuInventarios.Controls.Add(btnimpinv);
            pnlSubmenuInventarios.Controls.Add(btninvfis);
            pnlSubmenuInventarios.Controls.Add(btnLineas);
            pnlSubmenuInventarios.Controls.Add(btnGrupos);
            pnlSubmenuInventarios.Controls.Add(btnEntradas);
            pnlSubmenuInventarios.Controls.Add(btnArticulos);
            pnlSubmenuInventarios.Controls.Add(btnAlmacenes);

            flpMenuLateral.Controls.Add(pnlSubmenuInventarios);

            // --- SECCIÓN PDV ---
            btnPDV = CrearBotonPrincipal("🛒", "PDV");
            btnPDV.Click += (s, e) => AlternarSubmenu(pnlSubmenuPDV, 140);
            flpMenuLateral.Controls.Add(btnPDV);

            pnlSubmenuPDV = CrearPanelSubmenu();
            var btnProductos = CrearBotonSubmenu("🏷️", "Productos");
            var btnClientes = CrearBotonSubmenu("👥", "Clientes");
            var btnProdd = CrearBotonSubmenu("⚙️", "Prodd (Gestión)");
            var btnPedidos = CrearBotonSubmenu("📋", "Pedidos");

            btnProductos.Click += (s, e) => AbrirFormularioEnPanel<Fcprod>();
            btnClientes.Click += (s, e) => AbrirFormularioEnPanel<Fccli>();
            btnProdd.Click += (s, e) => AbrirFormularioEnPanel<Fmprod>();
            btnPedidos.Click += (s, e) => AbrirFormularioEnPanel<Fped>();

            pnlSubmenuPDV.Controls.Add(btnPedidos);
            pnlSubmenuPDV.Controls.Add(btnProdd);
            pnlSubmenuPDV.Controls.Add(btnClientes);
            pnlSubmenuPDV.Controls.Add(btnProductos);
            flpMenuLateral.Controls.Add(pnlSubmenuPDV);

            // --- SECCIÓN REPORTES ---
            btnReportes = CrearBotonPrincipal("📋", "REPORTES");
            btnReportes.Click += (s, e) => AlternarSubmenu(pnlSubmenuReportes, 80);
            flpMenuLateral.Controls.Add(btnReportes);

            pnlSubmenuReportes = CrearPanelSubmenu();
            var btnRepCliente = CrearBotonSubmenu("📊", "Ventas x Cliente");
            var btnRepPedido = CrearBotonSubmenu("📈", "Ventas x Pedido");

            btnRepCliente.Click += (s, e) => EjecutarReporteClientes();
            btnRepPedido.Click += (s, e) => EjecutarReportePedidos();

            pnlSubmenuReportes.Controls.Add(btnRepPedido);
            pnlSubmenuReportes.Controls.Add(btnRepCliente);
            flpMenuLateral.Controls.Add(pnlSubmenuReportes);

            // 3. Timer para controlar el efecto hover del menú lateral
            timerEfectoMenu = new System.Windows.Forms.Timer();
            timerEfectoMenu.Interval = 50;
            timerEfectoMenu.Tick += TimerEfectoMenu_Tick;
            timerEfectoMenu.Start();
        }

        private void TimerEfectoMenu_Tick(object sender, EventArgs e)
        {
            if (flpMenuLateral == null) return;

            Point mousePos = Cursor.Position;
            Rectangle menuRect = flpMenuLateral.RectangleToScreen(flpMenuLateral.ClientRectangle);
            bool mouseEncima = menuRect.Contains(mousePos);

            if (mouseEncima)
            {
                if (flpMenuLateral.Width < AnchoMaximo)
                {
                    flpMenuLateral.Width = AnchoMaximo;
                    MostrarTextos(true);
                }
            }
            else
            {
                if (flpMenuLateral.Width > AnchoMinimo)
                {
                    flpMenuLateral.Width = AnchoMinimo;
                    MostrarTextos(false);
                }
            }
        }

        private void MostrarTextos(bool mostrar)
        {
            foreach (Control ctrl in flpMenuLateral.Controls)
            {
                if (ctrl is Button btn && btn.Tag is Tuple<string, string> datosBoton)
                {
                    btn.Text = mostrar ? $"{datosBoton.Item1}    {datosBoton.Item2}" : datosBoton.Item1;
                }
                else if (ctrl is Panel pnl)
                {
                    foreach (Control subCtrl in pnl.Controls)
                    {
                        if (subCtrl is Button subBtn && subBtn.Tag is Tuple<string, string> datosSubBoton)
                        {
                            subBtn.Text = mostrar ? $"{datosSubBoton.Item1}    {datosSubBoton.Item2}" : datosSubBoton.Item1;
                        }
                    }
                }
            }
        }

        private void AlternarSubmenu(Panel panelSubmenu, int alturaDeseada)
        {
            panelSubmenu.Height = (panelSubmenu.Height == 0) ? alturaDeseada : 0;
        }

        private void AbrirFormularioEnPanel<TForm>() where TForm : notnull
        {
            if (_serviceProvider == null) return;

            var scope = _serviceProvider.CreateScope();
            var formularioHijo = scope.ServiceProvider.GetRequiredService<TForm>();

            if (formularioHijo is Form winForm)
            {
                if (panelContenedor.Controls.Count > 0)
                {
                    panelContenedor.Controls[0].Dispose();
                }

                winForm.TopLevel = false;
                winForm.FormBorderStyle = FormBorderStyle.None;
                winForm.Dock = DockStyle.Fill;

                panelContenedor.Controls.Add(winForm);
                panelContenedor.Tag = winForm;

                winForm.FormClosed += (sender, e) => scope.Dispose();
                winForm.Show();
            }
            else
            {
                scope.Dispose();
            }
        }

        private Button CrearBotonPrincipal(string icono, string texto)
        {
            Button btn = new Button();
            btn.Tag = new Tuple<string, string>(icono, texto);
            btn.Text = icono;
            btn.Size = new Size(AnchoMaximo, 45);
            btn.BackColor = Color.FromArgb(50, 50, 50);
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(15, 0, 0, 0);
            btn.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            return btn;
        }

        private Panel CrearPanelSubmenu()
        {
            Panel pnl = new Panel();
            pnl.Size = new Size(AnchoMaximo, 0);
            pnl.BackColor = Color.FromArgb(30, 30, 30);
            pnl.BorderStyle = BorderStyle.None;
            return pnl;
        }

        private Button CrearBotonSubmenu(string icono, string texto)
        {
            Button btn = new Button();
            btn.Tag = new Tuple<string, string>(icono, texto);
            btn.Text = icono;
            btn.Dock = DockStyle.Top;
            btn.Height = 35;
            btn.BackColor = Color.FromArgb(60, 60, 60);
            btn.ForeColor = Color.LightGray;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(30, 0, 0, 0);
            btn.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            return btn;
        }

        private void EjecutarReporteClientes()
        {
            if (_serviceProvider == null) return;
            using var scope = _serviceProvider.CreateScope();
            var useCase = scope.ServiceProvider.GetRequiredService<GenerarReporteClientesUseCase>();
            var datos = useCase.Ejecutar();
            ReportePedidosTexto.ProcesarEImprimir(datos);
        }

        private void EjecutarReportePedidos()
        {
            if (_serviceProvider == null) return;
            using var scope = _serviceProvider.CreateScope();
            var useCase = scope.ServiceProvider.GetRequiredService<ReportePedidoPorPedidoUseCase>();
            var datos = useCase.Ejecutar();
            if (!datos.Any())
            {
                MessageBox.Show("El reporte se ejecutó, pero la consulta no devolvió ningún registro.", "Sin Datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ReportePedidoPorPedidoTexto.ProcesarEImprimir(datos);
        }

        private void Fmain_Load(object sender, EventArgs e) { }
    }
}