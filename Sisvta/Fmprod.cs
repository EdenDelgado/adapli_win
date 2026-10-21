using Microsoft.EntityFrameworkCore;
using Sisvta.Infrastructure.Persistence.Contexts;
using Sisvta.Infrastructure.Repositories;
using Sisvta.UseCases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sisvta
{
    public partial class Fmprod : Form
    {
        private readonly GestionarProductosUseCase _useCase;
        private readonly ApplicationDbContext _context;
        private Fprodd _finv2;
        private Fcgru _fcgru;
        private Fclin _fclin;
        private Fprod _fprod;

        public Fmprod(GestionarProductosUseCase useCase, ApplicationDbContext context)
        {
            InitializeComponent();
            _useCase = useCase;
            _context = context;

            // Inicializamos Finv2 inyectándole el mismo caso de uso
            _finv2 = new Fprodd(_useCase, _context);

            // Corregido: Usamos el repositorio y el contexto correspondientes
            _fcgru = new Fcgru(new GestionarGruposLineasUseCase(new GrupoLineaRepository(_context)));
        }

        /// <summary>
        /// Constructor vacío requerido para el diseñador visual
        /// </summary>
        public Fmprod()
        {
            InitializeComponent();
        }

        // AQUÍ HACEMOS QUE SE ABRA MAXIMIZADO AUTOMÁTICAMENTE
        private void Fmprod_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void btlinea_Click(object sender, EventArgs e)
        {
            try
            {
                if (_context == null)
                {
                    MessageBox.Show("El contexto de la base de datos no está inicializado en Fmprod.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (_fclin == null || _fclin.IsDisposed)
                {
                    var repositorio = new LineaArticuloRepository(_context);
                    var useCase = new GestionarLineasArticulosUseCase(repositorio);
                    _fclin = new Fclin(useCase, _context);
                }

                if (_fclin.MdiParent == null && this.MdiParent != null)
                {
                    _fclin.MdiParent = this.MdiParent;
                }
                _fclin.Show();
                _fclin.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el formulario de líneas: {ex.Message}", "Excepción", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btGrupos_Click(object sender, EventArgs e)
        {
            try
            {
                if (_context == null)
                {
                    MessageBox.Show("El contexto de la base de datos no está inicializado en Fmprod.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (_fcgru == null || _fcgru.IsDisposed)
                {
                    _fcgru = new Fcgru(new GestionarGruposLineasUseCase(new GrupoLineaRepository(_context)));
                }

                if (_fcgru.MdiParent == null && this.MdiParent != null)
                {
                    _fcgru.MdiParent = this.MdiParent;
                }

                _fcgru.Show();
                _fcgru.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el formulario de grupos: {ex.Message}", "Excepción", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btarticulo_Click(object sender, EventArgs e)
        {
            try
            {
                if (_context == null)
                {
                    MessageBox.Show("El contexto de la base de datos no está inicializado en Fmprod.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // CORREGIDO: Fprod requiere _useCase y _context por su constructor inyectado
                if (_fprod == null || _fprod.IsDisposed)
                {
                    _fprod = new Fprod(_useCase, _context);
                }

                if (_fprod.MdiParent == null && this.MdiParent != null)
                {
                    _fprod.MdiParent = this.MdiParent;
                }

                _fprod.Show();
                _fprod.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el formulario de artículos: {ex.Message}", "Excepción", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btregresar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}