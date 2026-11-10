using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sisvta.Infrastructure.Persistence.Contexts;

namespace Sisvta
{
    public partial class Fsrep : Form
    {
        private readonly ApplicationDbContext _context;

        public Fsrep(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private void regresabt_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btrepinv_Click(object sender, EventArgs e)
        {
            // Instanciamos pasándole el contexto inyectado si Fent lo requiere
            var formFrepinv = new Frepinv(_context);

            if (formFrepinv.ShowDialog() == DialogResult.OK)
            {
                // Lógica post-cierre si se realizaron cambios
            }
        }

        private void btrepcosv_Click(object sender, EventArgs e)
        {
            var formFrepcosv = new Frepcosv(_context);

            if (formFrepcosv.ShowDialog() == DialogResult.OK)
            {
                // Lógica post-cierre si se realizaron cambios
            }
        }

        private void btrepconin_Click(object sender, EventArgs e)
        {
            var formFrepconin = new Frepconin(_context);

            if (formFrepconin.ShowDialog() == DialogResult.OK)
            {
                // Lógica post-cierre si se realizaron cambios
            }
        }

        private void btsugcompra_Click(object sender, EventArgs e)
        {
            var formFcsug = new Fcsug(_context);

            if (formFcsug.ShowDialog() == DialogResult.OK)
            {
                // Lógica post-cierre si se realizaron cambios
            }
        }

        private void btrepsal_Click(object sender, EventArgs e)
        {
            var formFrepsal = new Frepsal(_context);

            formFrepsal.ShowDialog();
        }

        private void btrepexi_Click(object sender, EventArgs e)
        {
            var formFrepexi = new Frepexi(_context);

            formFrepexi.Show();
        }
    }
}