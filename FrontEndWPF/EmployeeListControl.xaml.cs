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
	/// Lógica de interacción para EmployeeListControl.xaml
	/// </summary>
	public partial class EmployeeListControl : UserControl
	{
		List<Empleado> employees = new List<Empleado>();
		public EmployeeListControl()
		{
			InitializeComponent();
			PopulateEmployeeDataGrid();
		}

		private void PopulateEmployeeDataGrid()
		{
			// Sample data for employees
			employees = new List<Empleado>
			{
				new Empleado { Cedula = "123456789",
	Nombre = "John",
	Apellidos = "Doe",
	Puesto = "Desarrollador",
	FechaContratacion = new DateTime(2020, 1, 1),
	Salario = 50000.00,
	CorreoElectronico = "john.doe@example.com",
	Telefono = "123456789",
	Contraseña = "password123",
	Activo = true },
				new Empleado { Cedula = "987654321",
	Nombre = "Jane",
	Apellidos = "Smith",
	Puesto = "Gerente",
	FechaContratacion = new DateTime(2018, 5, 1),
	Salario = 75000.00,
	CorreoElectronico = "jane.smith@example.com",
	Telefono = "987654321",
	Contraseña = "strongpassword",
	Activo = true },
				new Empleado { Cedula = "456789123",
	Nombre = "Alice",
	Apellidos = "Johnson",
	Puesto = "Diseñador",
	FechaContratacion = new DateTime(2019, 3, 15),
	Salario = 60000.00,
	CorreoElectronico = "alice.johnson@example.com",
	Telefono = "456789123",
	Contraseña = "securepass123",
	Activo = false },
			};

			EmployeeDataGrid.ItemsSource = employees;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var nuevoEmpleado = new añadirEmpleado();
			nuevoEmpleado.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			if (nuevoEmpleado.ShowDialog() == true)
			{

				string Cedula = nuevoEmpleado.cedula;
				string Nombre = nuevoEmpleado.nombre;
				string Apellidos = nuevoEmpleado.apellidos;
				string Puesto = nuevoEmpleado.puesto;
				DateTime Fecha = nuevoEmpleado.fechaContratación;
				double Salario = nuevoEmpleado.salario;
				string Correo = nuevoEmpleado.correo;
				string Contraseña = nuevoEmpleado.contraseña;
				string Telefono = nuevoEmpleado.telefono;
				bool Activo = true;



				employees.Add(new Empleado
				{
					Cedula = Cedula,
					Nombre = Nombre,
					Apellidos = Apellidos,
					Puesto = Puesto,
					FechaContratacion = Fecha,
					Salario = Salario,
					CorreoElectronico = Correo,
					Telefono = Telefono,
					Contraseña = Contraseña,
					Activo = Activo
				});
				EmployeeDataGrid.Items.Refresh();
			}
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Empleado selectedEmpleado = EmployeeDataGrid.SelectedItem as Empleado;
			if (selectedEmpleado != null)
			{
				employees.Remove(selectedEmpleado);
				EmployeeDataGrid.Items.Refresh();
			}
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			Empleado selectedEmpleado = EmployeeDataGrid.SelectedItem as Empleado;
			if (selectedEmpleado != null)
			{
				var editarEmpleado = new editarEmpleado();
				editarEmpleado.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				editarEmpleado.Cedula.Text = selectedEmpleado.Cedula;
				editarEmpleado.Nombre.Text = selectedEmpleado.Nombre;
				editarEmpleado.Apellidos.Text = selectedEmpleado.Apellidos;
				editarEmpleado.Puesto.Text = selectedEmpleado.Puesto;
				editarEmpleado.Fecha.SelectedDate = selectedEmpleado.FechaContratacion;
				editarEmpleado.Salario.Text = selectedEmpleado.Salario.ToString();
				editarEmpleado.Correo.Text = selectedEmpleado.CorreoElectronico;
				editarEmpleado.Contraseña.Text = selectedEmpleado.Contraseña;
				editarEmpleado.Telefono.Text = selectedEmpleado.Telefono;
				editarEmpleado.Activo.IsChecked = selectedEmpleado.Activo;
				if (editarEmpleado.ShowDialog() == true)
				{
					selectedEmpleado.Cedula = editarEmpleado.cedula;
					selectedEmpleado.Nombre = editarEmpleado.nombre;
					selectedEmpleado.Apellidos = editarEmpleado.apellidos;
					selectedEmpleado.Puesto = editarEmpleado.puesto;
					selectedEmpleado.FechaContratacion = editarEmpleado.fechaContratación;
					selectedEmpleado.Salario = editarEmpleado.salario;
					selectedEmpleado.CorreoElectronico = editarEmpleado.correo;
					selectedEmpleado.Contraseña = editarEmpleado.contraseña;
					selectedEmpleado.Telefono = editarEmpleado.telefono;
					selectedEmpleado.Activo = editarEmpleado.Activo.IsChecked ?? false;
					EmployeeDataGrid.Items.Refresh();
				}
			}
		}
	}
}

