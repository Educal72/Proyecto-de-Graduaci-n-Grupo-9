using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace FrontEndWPF
{
    /// <summary>
    /// Lógica de interacción para añadirEmpleado.xaml
    /// </summary>
    public partial class editarEmpleado : Window
    {
        /* Propiedades que permitiran el almacenamiento de los nuevos datos -
         * ingresados por el usuario administrador.*/
        public string nombre_editarEmpleado { get; set; }
        public string apellidos_editarEmpleado { get; set; }
        public string cedula_editarEmpleado { get; set; }
        public string puesto_editarEmpleado { get; set; }
        public DateTime fechaContratacion_editarEmpleado { get; set; }
        public string correo_editarEmpleado { get; set; }
        public string contraseña_editarEmpleado { get; set; }
        public string telefono_editarEmpleado { get; set; }
        public bool activo_editarEmpleado { get; set; }
        public string rol_editarEmpleado { get; set; }
        public string direccion_editarEmpleado { get; set; }


        //Constructor vacio.
        public editarEmpleado()
        {
            InitializeComponent();
        }


        /* Método que se encarga de validar los espacios correspondientes en -
		 * donde el usuario administrador agregara los datos del nuevo empleado -
		 * al sistema.*/
        private bool ValidateInputs(out string errorMessage)
        {
            errorMessage = string.Empty;

            // Validar Nombre:
            if (string.IsNullOrWhiteSpace(Nombre.Text) && Nombre.Text.Length > 3)
            {
                errorMessage = "El campo Nombre es obligatorio.";
                return false;
            }

            // Validar Apellidos:
            if (string.IsNullOrWhiteSpace(Apellidos.Text) && Apellidos.Text.Length > 6)
            {
                errorMessage = "El campo Apellidos es obligatorio.";
                return false;
            }

            // Validar Cedula:
            if (!int.TryParse(Cedula.Text, out _) && Cedula.Text.Length > 5)
            {
                errorMessage = "El campo Cedula es obligatorio y debe ser válido (sin guiones).";
                return false;
            }

            // Validar Puesto:
            if (string.IsNullOrWhiteSpace(Puesto.Text) || Puesto.Text.Length < 3)
            {
                errorMessage = "El campo Puesto es obligatorio.";
                return false;
            }


            // Validar Correo:
            int index = Correo.Text.IndexOf("@");
            if (string.IsNullOrWhiteSpace(Correo.Text) && index > 0)
            {
                errorMessage = "El campo Correo es obligatorio.";
                return false;
            }

            /* Esta valicación en la historia CTE001, corresponde al 4rto escenario, -
             * en donde se explica que debe de dar un mensaje de advertencia porque -
             * hay datos que están haciendo conflictos en la BD. */
            Conexion conexion = new Conexion();
            var con = conexion.SelectUserCedula(Correo.Text, Convert.ToInt32(Cedula.Text));
            if (con.Count() > 1)
            {
                errorMessage = "El usuario a crear ya existe.";
                return false;
            }

            // Validar Teléfono:
            if (!int.TryParse(Telefono.Text, out _) && Telefono.Text.Length > 8)
            {
                errorMessage = "El campo Telefono es obligatorio y debe ser válido.";
                return false;
            }

            // Validar Rol:
            if (string.IsNullOrWhiteSpace(Rol.Text))
            {
                errorMessage = "El campo Rol es obligatorio.";
                return false;
            }

            // Todas las validaciones realizadas
            return true;
        }


        /* Este método se encarga de enviar los nuevos datos digitados por el usuario -
         * administrador al método relacionado a la actualización de un empleado, el -
         * cual esta ubicado en la clase: EmployeeListControl. Además de estar enlazado -
         * al botón: "Guardar". */
        public void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs(out string errorMessage))
            {
                MessageBox.Show("Hay un conflicto de datos o estás omitiendo un campo obligatorio.\n" 
                    + errorMessage, "Alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

           
            nombre_editarEmpleado = Nombre.Text;
            apellidos_editarEmpleado = Apellidos.Text;
            
            cedula_editarEmpleado = Cedula.Text;
            puesto_editarEmpleado = Puesto.Text;
            
            correo_editarEmpleado = Correo.Text;
            telefono_editarEmpleado = Telefono.Text;
            
            activo_editarEmpleado = (bool)Activo.IsChecked!;
            rol_editarEmpleado = Rol.Text;
            
            direccion_editarEmpleado = DireccionTo.Text;
            DialogResult = true;
        }


        /*  Este método se encarga de cancelar el apartado de actualización si el -
         *  usuario administrador lo desea. Además de estar enlazado al botón: -
         *  "Cancelar".*/
        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }


        /* Este método esta enlazado al botón: "Actual", el cual da la fecha actual en que -
         * se encuentra. */
    }
}
