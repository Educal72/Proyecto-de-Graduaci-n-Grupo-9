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
    /// Lógica de interacción para eliminarPlanilla.xaml
    /// </summary>
    public partial class eliminarPlanilla : Window
    {
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public string cedula { get; set; }
        public string puesto { get; set; }
        public string correo { get; set; }
        public DateTime fechaContratación { get; set; }
        public double salario { get; set; }


        public eliminarPlanilla()
        {
            InitializeComponent();
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Quieres eliminar esta planilla?",
                "¡Advertencia!", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Se cancelo la solicitud, muchas gracias",
                    "¡Aviso!", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = false;
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Se cancelo la solicitud, muchas gracias.",
                    "¡Aviso!", MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = false;
        }
    }
}


