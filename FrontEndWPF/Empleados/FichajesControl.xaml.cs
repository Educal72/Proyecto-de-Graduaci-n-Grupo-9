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
			var fichajes = new List<Fichajes>
			{
				new Fichajes { Id = 1, Cedula = 1234567890, Nombre = "Nombre Apellidos", Puesto = "Puesto", Fecha = System.DateTime.Now, Tipo = "Entrada" },
				new Fichajes { Id = 2, Cedula = 1357898654, Nombre = "Nombre Apellidos", Puesto = "Puesto", Fecha = System.DateTime.Now, Tipo = "Salida" },
				new Fichajes { Id = 3, Cedula = 1212344557, Nombre = "Nombre Apellidos", Puesto = "Puesto", Fecha = System.DateTime.Now, Tipo = "Entrada" },
				new Fichajes { Id = 4, Cedula = 1212344557, Nombre = "Nombre Apellidos", Puesto = "Puesto", Fecha = System.DateTime.Now, Tipo = "Entrada" },
				new Fichajes { Id = 5, Cedula = 1212344557, Nombre = "Nombre Apellidos", Puesto = "Puesto", Fecha = System.DateTime.Now, Tipo = "Entrada" },
				new Fichajes { Id = 6, Cedula = 1357898654, Nombre = "Nombre Apellidos", Puesto = "Puesto", Fecha = System.DateTime.Now, Tipo = "Salida" },
				new Fichajes { Id = 7, Cedula = 1357898654, Nombre = "Nombre Apellidos", Puesto = "Puesto", Fecha = System.DateTime.Now, Tipo = "Salida" }
			};

			FichajesDataGrid.ItemsSource = fichajes;
		}
	}
}
