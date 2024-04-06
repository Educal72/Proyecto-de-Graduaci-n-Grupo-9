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
	/// Lógica de interacción para PerfileComp.xaml
	/// </summary>
	public partial class PerfilesComp : UserControl
	{
		List<PerfilEmpleoBuscado> perfiles = new List<PerfilEmpleoBuscado>();
		public PerfilesComp()
		{
			InitializeComponent();
			PopulatePerfilEmpleoBuscadoDataGrid();
		}

		public void PopulatePerfilEmpleoBuscadoDataGrid()
		{
			perfiles = new List<PerfilEmpleoBuscado>
		{
			new PerfilEmpleoBuscado { Id = 1, Titulo = "Desarrollador de software", Descripcion = "Responsable de desarrollar aplicaciones de software", NivelExperiencia = "Intermedio", Requisitos = "Conocimientos en C#, experiencia con bases de datos SQL", Ubicacion = "Ciudad de México", Salario = 25000},
			new PerfilEmpleoBuscado { Id = 2, Titulo = "Diseñador gráfico", Descripcion = "Encargado de crear diseños visuales para diversos proyectos", NivelExperiencia = "Avanzado", Requisitos = "Experiencia con herramientas de diseño como Adobe Photoshop e Illustrator", Ubicacion = "Guadalajara", Salario = 20000},
			new PerfilEmpleoBuscado { Id = 3, Titulo = "Contador público", Descripcion = "Responsable de llevar la contabilidad y realizar declaraciones fiscales", NivelExperiencia = "Avanzado", Requisitos = "Licenciatura en contabilidad, experiencia en el campo", Ubicacion = "Monterrey", Salario = 30000}
		};

			PerfilesDataGrid.ItemsSource = perfiles;
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			var selectedItem = PerfilesDataGrid.SelectedItem as PerfilEmpleoBuscado;
			if (selectedItem != null)
			{
				perfiles.Remove(selectedItem);
				PerfilesDataGrid.Items.Refresh();
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var nuevoPerfilComp = new nuevoPerfilComp();
			nuevoPerfilComp.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			nuevoPerfilComp.tituloPagina.Content = "Nuevo Perfil";
			if (nuevoPerfilComp.ShowDialog() == true)
			{

				perfiles.Add(new PerfilEmpleoBuscado
				{
					Id = 3,
					Titulo = nuevoPerfilComp.titulo,
					Descripcion = nuevoPerfilComp.descripcion,
					NivelExperiencia = nuevoPerfilComp.experiencia,
					Requisitos = nuevoPerfilComp.requisitos,
					Ubicacion = nuevoPerfilComp.ubicación,
					Salario = nuevoPerfilComp.salario
				});

				PerfilesDataGrid.Items.Refresh();
			}
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			var selectedItem = PerfilesDataGrid.SelectedItem as PerfilEmpleoBuscado;
			if (selectedItem != null)
			{
				var nuevoPerfil = new nuevoPerfilComp();
				nuevoPerfil.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				nuevoPerfil.tituloPagina.Content = "Editar Perfil";
				nuevoPerfil.Titulo.Text = selectedItem.Titulo;
				nuevoPerfil.Desc.Text = selectedItem.Descripcion;
				nuevoPerfil.Exp.Text = selectedItem.NivelExperiencia;
				nuevoPerfil.Req.Text = selectedItem.Requisitos;
				nuevoPerfil.Ubicacion.Text = selectedItem.Ubicacion;
				nuevoPerfil.Salario.Text = selectedItem.Salario.ToString();
				if (nuevoPerfil.ShowDialog() == true)
				{
					selectedItem.Titulo = nuevoPerfil.titulo;
					selectedItem.Descripcion = nuevoPerfil.descripcion;
					selectedItem.NivelExperiencia = nuevoPerfil.experiencia;
					selectedItem.Requisitos = nuevoPerfil.requisitos;
					selectedItem.Ubicacion = nuevoPerfil.ubicación;
					selectedItem.Salario = nuevoPerfil.salario;
					PerfilesDataGrid.Items.Refresh();
				}
			}
		}
	}
}

