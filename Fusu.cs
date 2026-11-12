using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Sisvta.Infrastructure.Persistence.Contexts;

namespace Sisvta
{
    public partial class Fusu : Form
    {
        private readonly ApplicationDbContext _context;

        // Inyección de dependencias: Recibe el ApplicationDbContext configurado desde Program.cs
        public Fusu(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context;
        }

        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private extern static void Sendmessage(System.IntPtr hWnd, int msg, int wParam, int lParam);

        private void txtuser_TextChanged(object sender, EventArgs e) { }

        private void txtuser_Leave(object sender, EventArgs e)
        {
            if (txtuser.Text == "")
            {
                txtuser.Text = "USUARIO";
                txtuser.ForeColor = Color.DarkGray;
            }
        }

        private void txtuser_Enter(object sender, EventArgs e)
        {
            if (txtuser.Text == "USUARIO")
            {
                txtuser.Text = "";
                txtuser.ForeColor = Color.LightGray;
            }
        }

        private void txtpas_Leave(object sender, EventArgs e)
        {
            if (txtpas.Text == "")
            {
                txtpas.Text = "CONTRASEÑA";
                txtpas.ForeColor = Color.DarkGray;
            }
        }

        private void txtpas_Enter(object sender, EventArgs e)
        {
            if (txtpas.Text == "CONTRASEÑA")
            {
                txtpas.Text = "";
                txtpas.ForeColor = Color.LightGray;
                txtpas.UseSystemPasswordChar = false;
            }
        }

        private void btncerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnminimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnlogin_MouseDown(object sender, MouseEventArgs e) { }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            Sendmessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            Sendmessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            string usuarioInput = txtuser.Text.Trim();
            string passwordInput = txtpas.Text.Trim();

            if (string.IsNullOrEmpty(usuarioInput) || usuarioInput == "USUARIO" ||
                string.IsNullOrEmpty(passwordInput) || passwordInput == "CONTRASEÑA")
            {
                MessageBox.Show("Por favor, ingrese el usuario y la contraseña.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Usamos la instancia inyectada (_context) en lugar de hacer "new ApplicationDbContext()"
            var usuarioEncontrado = _context.Usuarios
                .FirstOrDefault(u => u.Nombre == usuarioInput && u.Password == passwordInput);

            if (usuarioEncontrado != null)
            {
                if (usuarioEncontrado.IdStatusUser != 1)
                {
                    MessageBox.Show("Este usuario se encuentra inactivo.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show("¡Bienvenido al sistema, " + usuarioEncontrado.Nombre + "!", "Acceso Concedido", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Fmain fmainForm = new Fmain();
                this.Hide();
                fmainForm.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Error de Autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);

                txtpas.Text = "CONTRASEÑA";
                txtpas.ForeColor = Color.DarkGray;
                txtpas.UseSystemPasswordChar = false;
                txtuser.Focus();
            }
        }
    }
}