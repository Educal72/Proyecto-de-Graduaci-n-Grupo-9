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
    /// Interaction logic for InformeAuditoria.xaml
    /// </summary>
    public partial class InformeAuditoria : UserControl
	{
        public InformeAuditoria()
        {
            InitializeComponent();
            CargarDatosAuditoria();
        }

        private void CargarDatosAuditoria()
        {
            var auditorias = DatosAuditoria.ObtenerAuditorias();
            auditoriaDataGrid.ItemsSource = auditorias;
        }

        private void DescargarInforme_Click(object sender, RoutedEventArgs e)
        {
            // Lógica para descargar el informe
            // Por ejemplo:
            MessageBox.Show("Descargando informe...");
        }
    }

    public static class DatosAuditoria
    {
        public static List<Auditoria> ObtenerAuditorias()
        {
            return new List<Auditoria>()
            {
                new Auditoria()
                {
                    CodigoAuditoria = "AUD001",
                    Descripcion = "Auditoría interna de procesos",
                    TipoAuditoria = "Interna",
                    FechaCreacion = new DateTime(2022, 1, 10),
                    Estado = "Concluida"
                },
                new Auditoria()
                {
                    CodigoAuditoria = "AUD002",
                    Descripcion = "Auditoría de cumplimiento legal",
                    TipoAuditoria = "Externa",
                    FechaCreacion = new DateTime(2022, 2, 15),
                    Estado = "En proceso"
                },
                new Auditoria()
                {
                    CodigoAuditoria = "AUD003",
                    Descripcion = "Auditoría de seguridad informática",
                    TipoAuditoria = "Interna",
                    FechaCreacion = new DateTime(2022, 3, 20),
                    Estado = "Programada"
                },
            };
        }
    }

    public class Auditoria
    {
        public string CodigoAuditoria { get; set; }
        public string Descripcion { get; set; }
        public string TipoAuditoria { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; }
    }
}
