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
    /// Lógica de interacción para eliminarPerfilCompetencial.xaml
    /// </summary>
    public partial class eliminarPerfilCompetencial : Window
    {

        public eliminarPerfilCompetencial()
        {
            InitializeComponent();
        }


        /* Método relacionado al botón: Guardar.
         * El cual se encarga de guardar los datos que están siendo -
         * ingresados por el usuario.*/
        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Quieres eliminar este perfil competencial?",
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


        /* Método relacionado al botón: Cancelar.
         * El cual se encarga de cancelar los datos que están siendo -
         * ingresados por el usuario.*/
        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Se cancelo la solicitud, muchas gracias.",
                    "¡Aviso!", MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = false;
        }

    }
}
