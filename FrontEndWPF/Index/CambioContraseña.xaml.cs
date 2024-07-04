using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
using static FrontEndWPF.Modelos.UserModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FrontEndWPF.Index
{
    /// <summary>
    /// Lógica de interacción para CambioContraseña.xaml
    /// </summary>
    public partial class CambioContraseña : Page
    {
        /*
         * Método que captura la contraseña que fue ingresado -
         * por los usuarios.
         */
        public string contraseña { get; set; }


        /*
         * Estos datos globales sirven para poder guardar -
         * los correos y contraseñas temporales para poder -
         * mostrarlo en la página, además de ver si hay o no -
         * un error [Ver 1.1].
         */
        static string datocorreo;
        static string datocontraseña;
        static bool error;

        /*
         * Instancia para poder usar los procedimientos almacenados -
         * ubicados en los métodos de la clase Conexión.
         */
        Conexion conexion = new Conexion();


        public CambioContraseña()
        {
            InitializeComponent();
            CargarDatos(); // [1.1]
        }


        /* [1.1]
         * Método que sirve para poder mostrar el correo y -
         * la contraseña temporal al usuario.
         */
        private void CargarDatos()
        {
            Correo.Text = datocorreo;
            Temporal.Text = datocontraseña;
        }


        /* [1.1]
         * Método complementario que sirve para poder capturar el correo -
         * y la contraseña temporal, esto para poder ser enviado -
         * al método: CargarDatos().
         */
        public void Carga(string correo, string temporal)
        {
            datocorreo = correo;
            datocontraseña = temporal;
        }


        /* 
         * Método que esta asociado al botón llamado: Cambiar Contraseña.
         * Lo que hace este método es que guarda la nueva contraseña que -
         * digitara el usuario para poder recuperar su cuenta, luego, dicha -
         * contraseña junto con el correo asociado, es enviado al método llamado: -
         * InsertarNuevaContraseña().
         * 
         * Después de realizar ese proceso, se manda al método llamado: Guardado -
         * un string.Empty, esto para poder quitar la posibilidad de que después de cambiar -
         * la contraseña, el usuario pueda ser de uso nuevamente dicha contraseña temporal -
         * que le fue brindado(a) anteriormente, y luego se redirecciona al login.
         * 
         * Esto se realiza de esta manera para poder mantener las buenas prácticas en cuanto a la -
         * seguridad.
         */
        private void CambiarContraseña(object sender, RoutedEventArgs e)
        {
            contraseña = NuevaContraseña.Password;            
            conexion.InsertarNuevaContraseña(datocorreo, contraseña);
            if(error != false)
            {
                var login = new Login();
                login.Guardado(string.Empty);
                NavigationService.Navigate(new Uri("Index/Login.xaml", UriKind.Relative));
            }
            
            NavigationService.Navigate(new Uri("Index/Login.xaml", UriKind.Relative));
        }

        /* [1.1]
         * Método complementario de CambiarContraseña.
         * Sirve para validar si aparecio un error o no.
         */
        public void ErrorEnvio(bool datos)
        {
            error = datos;
        }

        /*
         * Método que esta asociado al botón llamado: Salir.
         * Este método sirve para poder regresar al login.
         */
        private void BotonParaVolver(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Index/Login.xaml", UriKind.Relative));
        }
    }
}
