using System;
using System.Collections.Generic;
using System.IO;
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
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO.Ports;
using FrontEndWPF.Index;
using FrontEndWPF.ViewModel;
//using Microsoft.PointOfService;

namespace FrontEndWPF
{

    /// <summary>
    /// Lógica de interacción para ExtraPagina.xaml
    /// </summary>
    public partial class Login : Page
    {
        //private PosExplorer _posExplorer;
        //private PosPrinter _printer;
		/*
		 * Instancia para poder usar los procedimientos almacenados -
		 * ubicados en los métodos de la clase Conexión.
		 */
		Conexion conexion = new Conexion();
        InicioSesionViewModel inicioSesionViewModel = new InicioSesionViewModel();


		/*
		 * Instancia para poder enviar el correo y la contraseña -
		 * temporal a la clase llamada: CambioContraseña.
		 */
		CambioContraseña cambioContraseña = new CambioContraseña();


        /*
		 * Este dato global sirve para poder guardar -
		 * las contraseñas temporales para poder ayudar -
		 * en una validación [Ver 1.0].
		 */
        static string ContraseñaTemporalGuardada;



		public Login()
		{
			InitializeComponent();
			conexion.OpenConnection();
			if (conexion.HasEntries() || !File.Exists(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/db_config.txt"))
			{
				Opcion1.Content = "¿Olvidaste tu contraseña?";
			}
			else
			{
				Opcion1.Content = "Crear Usuario Admin";
			}
		}


		/*
         * Método que esta asociado al botón llamado: Iniciar sesión.
         * 
         * Este método lo que hace es que primero registra la contraseña -
         * ingresada, esto por medio de un tipo de dato conocido como: "var", -
         * esto por el hecho de que el var puede ser cambiado por otro dato de -
         * manera constante, luego de eso, entra a una condición que verifica si -
         * esa contraseña es igual a la contraseña temporal que se le mando -
         * por correo, esto por medio de la variable: ContraseñaTemporalGuardada.
         * 
         * Si resulta que es la misma contraseña temporal, entonces tomara el correo -
         * y la contraseña temporal y lo envia a un metódo llamado: Carga() y -
         * lo redirecciona a una página llamada: CambioContraseña, el cual fue creado -
         * para que se pueda hacer el cambio de contraseña del usuario.
         * 
         * Ahora, si en la condición sale que la contraseña temporal no es la misma o que -
         * esta colocando un dato nulo, entonces el sistema toma acentado que el usuario no -
         * necesita cambiar su contraseña por el momento.
         */
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var verif = cont.Password;

            // [1.0]
            if (verif == ContraseñaTemporalGuardada)
            {
                var Correo = correo.Text;
                cambioContraseña.Carga(Correo, verif);
                NavigationService.Navigate(new Uri("Index/CambioContraseña.xaml", UriKind.Relative));
            }
            else
            {
                var con = conexion.SelectUser(correo.Text, conexion.HashPassword(cont.Password));
                if (con.Count() > 0)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Usuario = con["Correo"].ToString()!;
                    if (con != null)
                    {
                        SesionUsuario.Instance.correo = con["Correo"].ToString()!;
                        conexion.getRoleName(Convert.ToInt32(con["IdRol"]));
                        SesionUsuario.Instance.rol = conexion.getRoleName(Convert.ToInt32(con["IdRol"]));
                        SesionUsuario.Instance.nombre = con["Nombre"].ToString()!;
                        var conexionEmpleado = new ConexionEmpleado();
                        SesionUsuario.Instance.id = Convert.ToInt32(con["Id"]);
						//Envia el nombre del usuario a la clase de conexion empleado.
						conexionEmpleado.NombreUsuario(SesionUsuario.Instance.nombre = con["Nombre"].ToString()!);
                        inicioSesionViewModel.CrearRegistroInicio(Convert.ToInt32(con["Id"]), DateTime.Today, DateTime.Now);
                    }
                    NavigationService.Navigate(new Uri("Index/MenuPrincipal.xaml", UriKind.Relative));
                }
                else
                {
                    MessageBox.Show("Usuario o Contraseña Incorrecta", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /*
         * Método que esta asociado al botón: ¿Olvidaste tu contraseña?
         * 
         * Lo que hace este método es que valida si el usuario quiere recuperar -
         * su contraseña de su cuenta ó si quiere registrar un nuevo usuario.
         */
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (conexion.HasEntries())
            {
                NavigationService.Navigate(new Uri("Index/RecuperarContraseña.xaml", UriKind.Relative));
            }
            else
            {
                var nuevoEmpleado = new añadirEmpleado();
                nuevoEmpleado.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                if (nuevoEmpleado.ShowDialog() == true)
                {

                    string Cedula = nuevoEmpleado.cedula_añadirEmpleado;
                    string Nombre = nuevoEmpleado.nombre_añadirEmpleado;
                    string Apellidos = nuevoEmpleado.apellidos_añadirEmpleado;
                    string Puesto = nuevoEmpleado.puesto_añadirEmpleado;
                    DateTime Fecha = nuevoEmpleado.fechaContratacion_añadirEmpleado;
                    double Salario = nuevoEmpleado.salario_añadirEmpleado;
                    string Correo = nuevoEmpleado.correo_añadirEmpleado;
                    string Contraseña = nuevoEmpleado.contraseña_añadirEmpleado;
                    string Telefono = nuevoEmpleado.telefono_añadirEmpleado;
                    string Direccion = nuevoEmpleado.direccion_añadirEmpleado;
                    string Rol = nuevoEmpleado.rol_añadirEmpleado;

                    conexion.AddUser(Nombre, Apellidos, Cedula, Telefono,
                        Correo, Contraseña, Rol, Fecha, Puesto, Salario, Direccion);
                    var con = conexion.SelectUser(Correo, conexion.HashPassword(Contraseña));
                    if (con != null)
                    {
                        SesionUsuario.Instance.correo = con["Correo"].ToString()!;
                        SesionUsuario.Instance.id = Convert.ToInt32(con["Id"]);
                        conexion.getRoleName(Convert.ToInt32(con["IdRol"]));
                        SesionUsuario.Instance.rol = conexion.getRoleName(Convert.ToInt32(con["IdRol"]));
                        SesionUsuario.Instance.nombre = con["Nombre"].ToString()!;
                        NavigationService.Navigate(new Uri("Index/MenuPrincipal.xaml", UriKind.Relative));
                    }
                }
            }
        }



        /*
         *  [1.0]
         *  Este método lo que hace es que recibe la contraseña -
         *  temporal que fue creada en la clase: RecuperarContraseña, -
         *  esto para poder hacer la validación que se encuentra en el método -
         *  que esta asociado al botón llamado: Iniciar sesión.
         */
        public void Guardado(string dato)
        {
            ContraseñaTemporalGuardada = dato;
        }


        private void SaveConfigToFile(string server, string puerto, string nombre, string usuario, string contraseña)
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            // Crear una carpeta específica para tu aplicación
            string appFolder = System.IO.Path.Combine(appDataPath, "YourAppName");
            if (!Directory.Exists(appFolder))
            {
                Directory.CreateDirectory(appFolder);
            }

            // Especificar la ruta completa del archivo
            string filePath = System.IO.Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "db_config.txt");

            string content = $"Server: {server}\n" +
                             $"Puerto: {puerto}\n" +
                             $"Nombre: {nombre}\n" +
                             $"Usuario: {usuario}\n" +
                             $"Contraseña: {contraseña}\n";

            try
            {
                File.WriteAllText(filePath, content);
                MessageBox.Show($"Configuración guardada exitosamente en el archivo!\nRuta: {filePath}", "Resultado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la configuración en el archivo: {ex.Message}", "Resultado", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BDButton_Click(object sender, RoutedEventArgs e)
        {
            var conexionBd = new BaseDeDatosPrimera();
            conexionBd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (conexionBd.ShowDialog() == true)
            {

                string Servidor = conexionBd.Servidor;
                string Puerto = conexionBd.Puerto;
                string NombreBD = conexionBd.NombreBD;
                string Usuario = conexionBd.Usuario;
                string Contraseña = conexionBd.Contraseña;
                SaveConfigToFile(Servidor, Puerto, NombreBD, Usuario, Contraseña);
                NavigationService.Navigate(new Uri("Index/Login.xaml", UriKind.Relative));
            }
        }

        private void PrintHelloWorldAndOpenCashDrawer(string printerName)
        {
            // ESC/POS command to print "Hello World"
            string printHelloWorldCommand = "\n";
            // ESC/POS command to open the cash drawer
            string openDrawerCommand = "\x1B\x70\x01\x32\x32";

            // Send print command
            RawPrinterHelper.SendStringToPrinter(printerName, printHelloWorldCommand);

            // Send open drawer command
            RawPrinterHelper.SendStringToPrinter(printerName, openDrawerCommand);


        }

        

    }
}



