using FrontEndWPF.Index;
using iText.Commons.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace FrontEndWPF.Reporteria
{
    /// <summary>
    /// Lógica de interacción para CambiarInforme.xaml
    /// </summary>
    public partial class CambiarInforme : Window
    {
        public string impuestos_cambiarInforme { get; set; }
        public string ingresos_cambiarInforme { get; set; }
        public string gastos_cambiarInforme { get; set; }
        public string retenciones_cambiarInforme { get; set; }
        public string estadoFinanciero_cambiarInforme { get; set; }
        public string datosEmpleado_cambiarInforme { get; set; }


        public CambiarInforme()
        {
            InitializeComponent();
        }


        public void Guardar_Click(object sender, RoutedEventArgs e)
        {
            impuestos_cambiarInforme = Impuestos.Text;
            ingresos_cambiarInforme = Ingresos.Text;

            gastos_cambiarInforme = Gastos.Text;
            retenciones_cambiarInforme = Retenciones.Text;

            estadoFinanciero_cambiarInforme = Estado_Financiero.Text;
            datosEmpleado_cambiarInforme = Datos_Empleado.Text;
            
            DialogResult = true;
        }


        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

    }
}
