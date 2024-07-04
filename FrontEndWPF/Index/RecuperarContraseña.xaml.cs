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
using System.IO.Packaging;

namespace FrontEndWPF
{
    /// <summary>
    /// Lógica de interacción para RecuperarContraseña.xaml
    /// </summary>
    public partial class RecuperarContraseña : Page
    {       
        /*
         * Método que captura el correo ingresado por los usuarios.
         */
        public string correo { get; set; }

        /*
         * Este dato global sirve para poder validar -
         * si el método: ProcedimientoParaElCorreo() -
         * no genera ningún error [Ver 0.1].
         */
        static bool Paso_El_Dato;

        /*
         * Instancia para poder usar los procedimientos almacenados -
         * ubicados en los métodos de la clase Conexión.
         */
        Conexion conexion = new Conexion();


        /*
         * Método que permite poder inicializar los -
         * componentes que uno valla a colocar.
         */
        public RecuperarContraseña()
        {
            InitializeComponent();
        }


        /*
         * Método enlazado al botón: "Recuperar Contraseña".
         * Lo que se hace en este método es que valida si el -
         * correo esta en la BD de Molino Central de Coronado, -
         * esto por medio de 2 partes internas y 3 partes externas (3 métodos).
         * 
         * La 1ra parte toma el dato que el usuario esta poniendo -
         * en el TextBox del correo, luego de eso, dicho dato es enviado -
         * a otro método que se llama ValidarCorreo(). Aqui es donde entra la 2da parte, -
         * si dicho método da como resultado un true, entonces la condición le permitira -
         * mandar una solicitud de restablecimiento de contraseña al correo del usuario, -
         * luego de eso lo regresa al login.
         * 
         * Pero si resulta que da como resultado un false, entonces la condición le mostrara -
         * un cuadro de mensaje mencionando que hubo un error en la solicitud, y que lo revise nuevamente.
         */
        private void Button_RecuperarContraseña(object sender, RoutedEventArgs e)
        {
            //1ra parte de la validación:
            correo = Correo.Text;
            var valida = conexion.ValidarCorreo(correo);

            //2da parte de la validación:
            if (valida == true)
            {
                ProcedimientoParaElCorreo(correo);
                
                if(Paso_El_Dato == true)
                {
                    MessageBox.Show("Instrucciones enviadas al correo electrónico, favor revisarlo.",
                    "Recuperación de Contraseña", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.Navigate(new Uri("Index/Login.xaml", UriKind.Relative));
                }

                NavigationService.Navigate(new Uri("Index/Login.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("Lo sentimos, la transacción que solicito tuvo un error, favor revisarlo, gracias.",
                    "¡Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        /* |============| 1ra parte externa |============|
         * Este método lo que hace es que manda la solicitud de recuperación -
         * de contraseña al usuario que lo esta solicitando, en este caso, en -
         * un correo de perteneciente a un office 365.
         */
        public static void ProcedimientoParaElCorreo(string correo)
        {
            /* 
             * Instancia de la clase: RecuperarContraseña, esto para poder llevar -
             * la contraseña temporal. 
             */
            var claseParcial = new RecuperarContraseña();


            /*
             * Aqui coloca los datos la persona administradora, -
             * el cual quiere enviar el mensaje al usuario que esta -
             * solicitando un restablecimiento de contraseña.
             */
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("vicky_mora1321@hotmail.com"),
                Subject = "Recuperación de contraseña.",
                Body = $"¡Hola!, gracias por mandar tu solicitud de restablecimiento de contraseña.\r\r\r\r" +
                       $"Aqui te proporcionamos una contraseña temporal para que puedas ingresar a la aplicación y puedas cambiarla después.\r\r" +
                       $"Advertencia: Esta contraseña es de uso confidencial, no compartas esta contraseña con ningún otro, muchas gracias por usar nuestros servicios.\r\r" +
                       $"Contraseña Temporal: {claseParcial.ContraseñaTemporal()}",
                IsBodyHtml = true
            };


            /* 
             * Aqui lo que se hace es que se pone las credenciales de la -
             * persona administradora que quiere mandar el mensaje.
             */
            SmtpClient client = new SmtpClient("smtp.office365.com", 587)
            {
                Credentials = new NetworkCredential("vicky_mora1321@hotmail.com", "vickymora1977"),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            try
            {
                /*
                 * Aqui lo que se hace es que se manda el correo -
                 * al usuario que solicito un restablecimiento de -
                 * cuenta.
                 * 
                 * Además de que cuenta con un try catch para tener -
                 * un control adecuado de posibles errores que pueden -
                 * presentarse. 
                 */
                mailMessage.To.Add(correo);
                client.Send(mailMessage);
                Debug.WriteLine("Sent");
                client.Dispose();//Esto limpia todo, por buenas prácticas es recomendable colocarlo.
                Paso_El_Dato = true;
            }
            catch (Exception ex)
            {               
                MessageBox.Show("Parece que hubo un error durante el envio del correo para la recuperar la contraseña.",
                            "¡Error! - " + ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                Paso_El_Dato = false;
            }
        }


        /* |============| 2da parte externa |============|
         * Este método lo que hace es que se coloca la longitud (o largo) -
         * que va a tener la contraseña, por lo general son 8 digitos como minimo -
         * y luego de eso lo manda al 3 método externo para que pueda crear una -
         * contraseña temporal.
         * 
         * Después de que genera la contraseña temporal, este lo manda a un método -
         * llamado: Guardado(), que esta ubicado en la clase llamada: Login, y luego de eso, -
         * simplemente le da esa contraseña al correo que se esta creando para la solicitud de -
         * restablecimiento de contraseña.
         */
        public string ContraseñaTemporal()
        {
            int longitud = 8;
            string password = Generador_ContraseñaTemporal(longitud);
            var login = new Login();
            login.Guardado(password);
            return password;
        }


        /* |============| 3ra parte externa |============|
         * Este método lo que hace es que crea la contraseña temporal -
         * que sera enviado al usuario que esta solicitando una recuperación -
         * de contraseña.
         * 
         * Además de que esta contraseña temporal va a ser diferente cada vez -
         * que se solicite una, esto por buenas prácticas de desarrollo y seguridad.
         */
        public static string Generador_ContraseñaTemporal(int longitud)
        {
            const string cadenaCaracteres = "&4]NE2Gm[uY$-]/W}+@y+SL%]30unq{?3N/H6ScXaZidMh&0jeiX*m_;#LKp/10c";

            StringBuilder constructorString = new StringBuilder();
            Random aleatorio = new Random();

            for (int i = 0; i < longitud; i++)
            {
                int index = aleatorio.Next(cadenaCaracteres.Length);
                constructorString.Append(cadenaCaracteres[index]);
            }

            return constructorString.ToString();

        }


        /*
         * Método que sirve para devolverse al login.
         */
        private void BotonParaVolver(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Index/Login.xaml", UriKind.Relative));
        }




    }
}
