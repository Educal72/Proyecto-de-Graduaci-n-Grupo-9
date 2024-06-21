using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;

namespace FrontEndWPF
{
    /// <summary>
    /// Lógica de interacción para RecuperarContraseña.xaml
    /// </summary>
    public partial class RecuperarContraseña : Page
    {
        Conexion conexion = new Conexion();
        public string correo { get; set; }
        bool num;

        public RecuperarContraseña()
        {
            InitializeComponent();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //1ra parte:
            correo = Correo.Text;
            var valida = conexion.ValidarCorreo(correo);

            if (valida != null)
            {
                num = true;
            }
            else
            {
                num = false;
            }

            //2da parte:
            if (num == true)
            {
                ProcedimientoParaElCorreo(correo);
                MessageBox.Show("Instrucciones enviadas al correo electrónico, favor revisarlo.",
                    "Recuperación de Contraseña", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new Uri("Index/Login.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("Lo sentimos, la transacción que solicito tuvo un error, favor revisarlo, gracias.",
                    "¡Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        /*
         * Sirve para devolverse al login.
         */
        private void BotonParaVolver(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Index/Login.xaml", UriKind.Relative));
        }

        public static void ProcedimientoParaElCorreo(string correo)
        {
            string to = "Molino Central Coronado";
            string from = correo;
            string subject = "Recuperación de contraseña.";
            string body =
                @"¡Hola!, gracias por mandar tu solicitud de restablecimiento de contraseña, aqui te proporcionamos una contraseña temporal para que puedas ingresar a la aplicación y puedas cambiarla después.
                  Advertencia: Esta contraseña es de uso confidencial, no compartas esta contraseña con ningún otro, además, de que dicha contraseña tiene un limite de tiempo, muchas gracias por usar nuestros servicios.";

            MailMessage message = new MailMessage(from, to, subject, body);
            var smtpClient = new SmtpClient();
            

            try
            {
                smtpClient.Send(message);
                Debug.WriteLine("Sent");
                smtpClient.Dispose();
            }
            catch
            {
                throw;
            }
        }
    }
}
