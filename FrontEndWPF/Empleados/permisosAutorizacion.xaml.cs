using FrontEndWPF.Index;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FrontEndWPF
{
    /// <summary>
    /// Lógica de interacción para permisosAutorizacion.xaml
    /// </summary>
    public partial class permisosAutorizacion : Window
    {
        //Instancia de la clase de EmployeeListControl.
        EmployeeListControl employeeListControl = new EmployeeListControl();    


        /* Propiedades para poder guardar todos los datos solicitados -
		 * al usuario administrador para agregarle o quitarle permisos de autorización -
		 * a un empleado del sistema.*/
        public bool desvinculacion_permiso_leer_permisosAutorizacion { set; get; }
        public bool desvinculacion_permiso_crear_permisosAutorizacion { set; get; }
        public bool desvinculacion_permiso_eliminar_permisosAutorizacion { set; get; }


        //Constructor vacio.
        public permisosAutorizacion()
        {
            InitializeComponent();
        }


        /* Este método se encarga de enviar los nuevos datos digitados por el usuario -
         * administrador al método relacionado al registro de los permisos de un empleado, el -
         * cual esta ubica en la clase: EmployeeListControl. Además de estar enlazado -
         * al botón: "Guardar". */
        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            desvinculacion_permiso_leer_permisosAutorizacion = (bool)Permiso_Leer_Desvinculacion.IsChecked!;
            desvinculacion_permiso_crear_permisosAutorizacion = (bool)Permiso_Crear_Desvinculacion.IsChecked!;
            desvinculacion_permiso_eliminar_permisosAutorizacion = (bool)Permiso_Eliminar_Desvinculacion.IsChecked!;
            employeeListControl.PermisoValidacion(true, false);
            DialogResult = true;
        }


        /*  Este método se encarga de cancelar el apartado de los permisos si el -
         *  usuario administrador lo desea. Además de estar enlazado al botón: -
         *  "Cancelar".*/
        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }


        /* Este método se encarga de enviar los nuevos datos digitados por el usuario -
         * administrador al método relacionado a la actualización de los permisos de -
         * un empleado, el cual esta ubica en la clase: EmployeeListControl. 
         * Además de estar enlazado al botón: "Actualizar". */
        private void Actualizar_Click(object sender, RoutedEventArgs e)
        {
            desvinculacion_permiso_leer_permisosAutorizacion = (bool)Permiso_Leer_Desvinculacion.IsChecked!;
            desvinculacion_permiso_crear_permisosAutorizacion = (bool)Permiso_Crear_Desvinculacion.IsChecked!;
            desvinculacion_permiso_eliminar_permisosAutorizacion = (bool)Permiso_Eliminar_Desvinculacion.IsChecked!;
            employeeListControl.PermisoValidacion(false, true);
            DialogResult = true;
        }
    }
}
