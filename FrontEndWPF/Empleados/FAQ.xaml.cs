using FrontEndWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
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
using static FrontEndWPF.Modelos.InventarioModel;
using System.IO;
using System.Collections.ObjectModel;
using Xceed.Wpf.Toolkit.Primitives;
using System.Windows.Controls.Primitives;
using System.ComponentModel;

namespace FrontEndWPF.Empleados
{
	/// <summary>
	/// Lógica de interacción para FAQ.xaml
	/// </summary>
	public partial class FAQ : UserControl
	{
		FAQViewModel faqviewmodel = new FAQViewModel();
		public ObservableCollection<FAQS> FAQItems { get; set; }
		Conexion conexion = new Conexion();
		string rol;

		public string UserRole
		{
			get { return rol; }
			set
			{
				rol = value;
				OnPropertyChanged(nameof(UserRole));
			}
		}
		public FAQ()
		{
			InitializeComponent();
			FAQItems = new ObservableCollection<FAQS>();
			DataContext = this;
			PopulateFAQDataGrid();
			rol = SesionUsuario.Instance.rol;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public void PopulateFAQDataGrid()
		{
			List<FAQS> faq = new List<FAQS>();
			FAQItems.Clear();
			string query = @"SELECT Id, Pregunta, Respuesta, NombreDoc, Documento FROM FAQ";

			using (SqlConnection connection = conexion.OpenConnection())
			{
				try
				{
					SqlCommand command = new SqlCommand(query, connection);
					SqlDataReader reader = command.ExecuteReader();

					while (reader.Read())
					{
						faq.Add(new FAQS()
						{
							Id = reader.GetInt32(0),
							Pregunta = reader["Pregunta"].ToString(),
							Respuesta = reader["Respuesta"].ToString(),
							Nombre = reader["NombreDoc"].ToString() != "" ? reader["NombreDoc"].ToString() : "Sin Documento Asociado",
							Documento = (byte[])reader["Documento"]

						});
					}
					reader.Close();
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: " + ex.Message);
				}
			}

			foreach (var item in faq)
			{
				FAQItems.Add(item);
			}
		}


		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var nuevaFAQ = new nuevaFAQ();
			nuevaFAQ.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			nuevaFAQ.Titulo.Content = "Nueva Pregunta";
			if (nuevaFAQ.ShowDialog() == true)
			{
				string Pregunta = nuevaFAQ.pregunta;
				string Respuesta = nuevaFAQ.respuesta;
				byte[] Documento = nuevaFAQ.documento;
				string Nombre = nuevaFAQ.nombre;
				faqviewmodel.CrearFAQ(Pregunta, Respuesta, Documento, Nombre);
				PopulateFAQDataGrid();
			}
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			if (button != null)
			{
				FAQS faq = button.DataContext as FAQS;
				if (faq != null)
				{
					var nuevaFAQ = new nuevaFAQ();
					nuevaFAQ.WindowStartupLocation = WindowStartupLocation.CenterScreen;
					nuevaFAQ.Titulo.Content = "Editar Pregunta";
					nuevaFAQ.Pregunta.Text = faq.Pregunta;
					nuevaFAQ.Respuesta.Text = faq.Respuesta;
					nuevaFAQ.documento = faq.Documento;
					nuevaFAQ.nombre = faq.Nombre;
					if (faq.Nombre == "")
					{
						nuevaFAQ.Doc.Content = "Añadir Documento";
					}
					else
					{
						nuevaFAQ.Doc.Content = faq.Nombre;
					}
					if (nuevaFAQ.ShowDialog() == true)
					{
						string Pregunta = nuevaFAQ.pregunta;
						string Respuesta = nuevaFAQ.respuesta;
						byte[] Documento = nuevaFAQ.documento;
						string Nombre = nuevaFAQ.nombre;
						faqviewmodel.EditarFAQ(faq.Id, Pregunta, Respuesta, Documento, Nombre);
						PopulateFAQDataGrid();
					}
				}
			}
			//var selectedItem = FAQDataGrid.SelectedItem as FAQS;
			//if (selectedItem != null)
			//{
			//	var nuevaFAQ = new nuevaFAQ();
			//	nuevaFAQ.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			//	nuevaFAQ.Titulo.Content = "Editar Pregunta";
			//	nuevaFAQ.Pregunta.Text = selectedItem.Pregunta;
			//	nuevaFAQ.Respuesta.Text = selectedItem.Respuesta;
			//	if (selectedItem.Nombre == "") {
			//		nuevaFAQ.Doc.Content = "Añadir Documento";
			//	} else {
			//		nuevaFAQ.Doc.Content = "Documento Asociado:\n"+selectedItem.Nombre; 
			//	}

			//	if (nuevaFAQ.ShowDialog() == true)
			//	{
			//		string Pregunta = nuevaFAQ.pregunta;
			//		string Respuesta = nuevaFAQ.respuesta;
			//		byte[] Documento = nuevaFAQ.documento;
			//		string Nombre = nuevaFAQ.nombre;
			//		faqviewmodel.EditarFAQ(selectedItem.Id, Pregunta, Respuesta, Documento, Nombre);
			//		PopulateFAQDataGrid();
			//	}
			//}
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			if (button != null)
			{
				MessageBoxResult result = MessageBox.Show("¿Está seguro de eliminar la pregunta del FAQ?",
					"¡Confirmación!", MessageBoxButton.YesNo, MessageBoxImage.Question);
				if (result == MessageBoxResult.Yes)
				{
					FAQS faq = button.DataContext as FAQS;
					if (faq != null)
					{
						faqviewmodel.EliminarFAQ(faq.Id);
						PopulateFAQDataGrid();
					}
				}
			}
					//var selectedItem = FAQDataGrid.SelectedItem as FAQS;
					//if (selectedItem != null)
					//{
					//	faqviewmodel.EliminarFAQ(selectedItem.Id);
					//	PopulateFAQDataGrid();
					//}
				}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			if (button != null)
			{
				FAQS faq = button.DataContext as FAQS;
				if (faq != null && faq.Nombre != "Sin Documento Asociado")
				{
					var nombre = faqviewmodel.GetDocumento(faq.Id);
					Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
					saveFileDialog.FileName = nombre.Item1;
					string extension = System.IO.Path.GetExtension(nombre.Item1);
					saveFileDialog.DefaultExt = extension;
					saveFileDialog.Filter = "All Files|*.*";
					if (saveFileDialog.ShowDialog() == true)
					{
						File.WriteAllBytes(saveFileDialog.FileName, nombre.Item2);
						MessageBox.Show("Documento descargado exitosamente", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
					}
				}
					else
				{
					MessageBox.Show("Documento no asignado o no a seleccionado una opción de la tabla", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
			//	var selectedItem = FAQDataGrid.SelectedItem as FAQS;
			//	if (selectedItem != null && selectedItem.Nombre != "Sin Documento Asociado")
			//	{
			//		var nombre = faqviewmodel.GetDocumento(selectedItem.Id);
			//		// Guardar el archivo localmente
			//		Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
			//		saveFileDialog.FileName = nombre.Item1;
			//		string extension = System.IO.Path.GetExtension(nombre.Item1);
			//		saveFileDialog.DefaultExt = extension;
			//		saveFileDialog.Filter = "All Files|*.*";

			//		if (saveFileDialog.ShowDialog() == true)
			//		{
			//			File.WriteAllBytes(saveFileDialog.FileName, nombre.Item2);
			//			MessageBox.Show("Documento descargado exitosamente", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
			//		}
			//	}
			//	else
			//	{
			//		MessageBox.Show("Documento no asignado o no a seleccionado una opción de la tabla", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			//	}
		}
	}
}
