using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    public static class EstiloMenuOperador
    {
        public static void Aplicar(
            Control menu,
            Control panelSesion,
            Button inicio,
            Button voz,
            Button historial,
            Button cerrarSesion,
            Button seleccionado)
        {
            Color fondo = Color.FromArgb(17, 24, 34);

            menu.BackColor = fondo;
            panelSesion.BackColor = fondo;

            ConfigurarBoton(inicio, "\uE80F", fondo);
            ConfigurarBoton(voz, "\uE720", fondo);
            ConfigurarBoton(historial, "\uE81C", fondo);

            ConfigurarBoton(
                cerrarSesion,
                "\uE8BB",
                Color.FromArgb(32, 41, 56));

            seleccionado.BackColor = Color.FromArgb(50, 43, 96);
        }

        private static void ConfigurarBoton(
            Button boton,
            string simbolo,
            Color fondo)
        {
            Bitmap imagen = new Bitmap(24, 24);

            using (Graphics dibujo = Graphics.FromImage(imagen))
            using (Font fuente = new Font(
                "Segoe MDL2 Assets",
                16,
                FontStyle.Regular,
                GraphicsUnit.Pixel))
            using (SolidBrush pincel = new SolidBrush(Color.White))
            using (StringFormat formato = new StringFormat())
            {
                dibujo.Clear(Color.Transparent);

                dibujo.TextRenderingHint =
                    TextRenderingHint.AntiAliasGridFit;

                formato.Alignment = StringAlignment.Center;
                formato.LineAlignment = StringAlignment.Center;

                dibujo.DrawString(
                    simbolo,
                    fuente,
                    pincel,
                    new RectangleF(0, 0, 24, 24),
                    formato);
            }

            boton.Image = imagen;
            boton.ImageAlign = ContentAlignment.MiddleLeft;
            boton.TextAlign = ContentAlignment.MiddleLeft;
            boton.TextImageRelation = TextImageRelation.ImageBeforeText;

            boton.Padding = new Padding(10, 0, 0, 0);
            boton.ForeColor = Color.White;
            boton.BackColor = fondo;
            boton.UseVisualStyleBackColor = false;

            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderSize = 0;

            boton.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(43, 40, 74);

            boton.FlatAppearance.MouseDownBackColor =
                Color.FromArgb(65, 55, 110);

            boton.Cursor = Cursors.Hand;

            boton.Disposed += (s, e) => imagen.Dispose();
        }
    }
}
