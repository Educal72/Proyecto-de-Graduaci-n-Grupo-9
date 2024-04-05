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

namespace FrontEndWPF
{
	/// <summary>
	/// Lógica de interacción para FichajesControl.xaml
	/// </summary>
	public partial class FichajesControl : UserControl
	{
		public FichajesControl()
		{
			InitializeComponent();
			PopulateFichajesDataGrid();
		}

		private void PopulateFichajesDataGrid()
		{
			// Sample data for fichajes
			var fichajes = new List<Fichajes>
			{
				new Fichajes { Id = 1, EmpleadoId = 1, Fecha = System.DateTime.Now, Tipo = "Entrada" },
				new Fichajes { Id = 2, EmpleadoId = 2, Fecha = System.DateTime.Now, Tipo = "Salida" },
				new Fichajes { Id = 3, EmpleadoId = 1, Fecha = System.DateTime.Now, Tipo = "Entrada" },
                // Add more sample fichajes as needed
            };

			// Bind data to the DataGrid
			FichajesDataGrid.ItemsSource = fichajes;
		}
	}
}
