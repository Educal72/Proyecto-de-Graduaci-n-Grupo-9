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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FrontEndWPF.Reporteria
{
    /// <summary>
    /// Interaction logic for InformeNormativa.xaml
    /// </summary>
    public partial class InformeNormativa : UserControl
	{
        public InformeNormativa()
        {
            InitializeComponent();
            CargarDatosNormativa();
        }

        private void CargarDatosNormativa()
        {
            var normativas = DatosNormativa.ObtenerNormativas();
            normativaDataGrid.ItemsSource = normativas;
        }

        private void DescargarNormativa_Click(object sender, RoutedEventArgs e)
        {
            // Lógica para descargar la normativa
            // Por ejemplo:
            MessageBox.Show("Descargando normativa...");
        }
    }

    public static class DatosNormativa
    {
        public static List<Normativa> ObtenerNormativas()
        {
            return new List<Normativa>()
            {
                new Normativa()
                {
                    NombreNormativa = "Normativa A",
                    CodigoNormativa = "NRM001",
                    Descripcion = "Descripción de la normativa A",
                    FechaAplicacion = new DateTime(2022, 1, 10),
                    Estado = "Activa"
                },
                new Normativa()
                {
                    NombreNormativa = "Normativa B",
                    CodigoNormativa = "NRM002",
                    Descripcion = "Descripción de la normativa B",
                    FechaAplicacion = new DateTime(2022, 2, 15),
                    Estado = "Inactiva"
                },
                new Normativa()
                {
                    NombreNormativa = "Normativa C",
                    CodigoNormativa = "NRM003",
                    Descripcion = "Descripción de la normativa C",
                    FechaAplicacion = new DateTime(2022, 3, 20),
                    Estado = "Activa"
                },
            };
        }
    }

    public class Normativa
    {
        public string NombreNormativa { get; set; }
        public string CodigoNormativa { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaAplicacion { get; set; }
        public string Estado { get; set; }
    }
}
