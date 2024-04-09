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
using static FrontEndWPF.empleadosAdmin;

namespace FrontEndWPF.Empleados
{
    /// <summary>
    /// Lógica de interacción para Metricas.xaml
    /// </summary>
    public partial class Metricas : UserControl
    {
		
		public Metricas()
        {
			InitializeComponent();
			LlenarDataGrid();
		}

		private void LlenarDataGrid()
		{
			List<HistorialLaboral> listaHistorial = new List<HistorialLaboral>
	{
		new HistorialLaboral { FechaInicio = new DateTime(2010, 1, 1), FechaFin = new DateTime(2012, 12, 31), Empresa = "Empresa A", Cargo = "Desarrollador" },
		new HistorialLaboral { FechaInicio = new DateTime(2013, 5, 15), FechaFin = null, Empresa = "Empresa B", Cargo = "Gerente de Proyectos" },
		new HistorialLaboral { FechaInicio = new DateTime(2015, 8, 1), FechaFin = new DateTime(2018, 6, 30), Empresa = "Empresa C", Cargo = "Analista de Negocios" }
	};

			dataGrid.ItemsSource = listaHistorial;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var navigationService = NavigationService.GetNavigationService(this);

			// Verificar si el servicio de navegación es nulo y si podemos volver atrás
			if (navigationService != null && navigationService.CanGoBack)
			{
				// Volver a la página anterior
				navigationService.GoBack();
			}
		}
		}
	}

