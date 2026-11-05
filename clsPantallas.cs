using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    /// <summary>
    /// Es la clase que hace que las pantallas se vean dentro de la ventana principal.
    /// La usan el menú del administrador (frmPrincipal) y el del operador (FrmPrincipalOperador).
    /// </summary>
    public static class clsPantallas
    {
        /// <summary>
        /// Mete un formulario dentro de un panel, como si fuera una página.
        /// nombreMenu: el menú lateral que trae el formulario desde el diseñador;
        /// se quita porque la ventana principal ya tiene el suyo.
        /// </summary>
        public static void mtdMostrarEnPanel(Panel panel, Form pantalla, string nombreMenu)
        {
            mtdQuitarMenuRepetido(pantalla, nombreMenu);

            pantalla.TopLevel = false;                        // deja de ser una ventana suelta
            pantalla.FormBorderStyle = FormBorderStyle.None;  // sin bordes ni barra de título
            pantalla.Dock = DockStyle.Fill;                   // ocupa todo el panel

            panel.Controls.Add(pantalla);
            panel.Visible = true;
            panel.BringToFront();

            pantalla.Show();
            pantalla.BringToFront();
        }

        /// <summary>Cierra y libera la pantalla que se estaba mostrando (si hay una).</summary>
        public static void mtdCerrar(Form pantalla)
        {
            if (pantalla == null) return;

            pantalla.Close();
            pantalla.Dispose();
        }

        /// <summary>
        /// Quita el menú lateral del formulario y corre el resto de controles
        /// a la izquierda para que no quede un espacio vacío.
        /// </summary>
        private static void mtdQuitarMenuRepetido(Form pantalla, string nombreMenu)
        {
            Control[] encontrados = pantalla.Controls.Find(nombreMenu, true);
            if (encontrados.Length == 0) return;

            Control menu = encontrados[0];
            if (menu.Parent != pantalla) return;

            int anchoMenu = menu.Right;

            // Se guardan las posiciones antes de quitar el menú.
            Dictionary<Control, Rectangle> posiciones = new Dictionary<Control, Rectangle>();
            foreach (Control control in pantalla.Controls)
            {
                if (control != menu)
                    posiciones.Add(control, control.Bounds);
            }

            pantalla.SuspendLayout();

            pantalla.Controls.Remove(menu);
            menu.Dispose();

            pantalla.ClientSize = new Size(
                Math.Max(1, pantalla.ClientSize.Width - anchoMenu),
                pantalla.ClientSize.Height);

            foreach (KeyValuePair<Control, Rectangle> item in posiciones)
            {
                Control control = item.Key;
                Rectangle posicion = item.Value;

                // Los controles con Dock se acomodan solos; solo se mueven los demás.
                if (control.Dock == DockStyle.None)
                    control.Bounds = new Rectangle(Math.Max(0, posicion.X - anchoMenu),
                                                   posicion.Y, posicion.Width, posicion.Height);
            }

            pantalla.ResumeLayout(true);
        }
    }
}
