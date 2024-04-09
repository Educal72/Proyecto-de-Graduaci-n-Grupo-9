using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace FrontEndWPF.Reporteria
{
    public partial class VisualizarInversiones : Page
    {

        public VisualizarInversiones()
        {
            InitializeComponent();
            CargarDatosInversiones();
        }

        private void CargarDatosInversiones()
        {
            var inversiones = DatosInversiones.ObtenerInversiones();
            inversionesDataGrid.ItemsSource = inversiones;
        }

        private void Descargar_Click(object sender, RoutedEventArgs e)
        {
            // Agregar código para descargar la inversión seleccionada
        }
    }

    public static class DatosInversiones
    {
        public static List<Inversion> ObtenerInversiones()
        {
            return new List<Inversion>()
            {
                new Inversion()
                {
                    Financiera = "Financiera A",
                    MontoInvertido = 10000.00m,
                    FechaInversion = new DateTime(2022, 1, 10),
                    PorcentajeGananciaMensual = 1.5m,
                    FechaFinalizacion = new DateTime(2023, 1, 10)
                },
                new Inversion()
                {
                    Financiera = "Financiera B",
                    MontoInvertido = 15000.00m,
                    FechaInversion = new DateTime(2022, 2, 15),
                    PorcentajeGananciaMensual = 2.0m,
                    FechaFinalizacion = new DateTime(2023, 2, 15)
                },
                new Inversion()
                {
                    Financiera = "Financiera C",
                    MontoInvertido = 20000.00m,
                    FechaInversion = new DateTime(2022, 3, 20),
                    PorcentajeGananciaMensual = 1.75m,
                    FechaFinalizacion = new DateTime(2023, 3, 20)
                },
            };
        }
    }

    public class Inversion
    {
        public string Financiera { get; set; }
        public decimal MontoInvertido { get; set; }
        public DateTime FechaInversion { get; set; }
        public decimal PorcentajeGananciaMensual { get; set; }
        public DateTime FechaFinalizacion { get; set; }
    }
}
