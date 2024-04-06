using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
	/// Lógica de interacción para FAQ.xaml
	/// </summary>
	public partial class FAQ : UserControl
	{
		List<FAQS> faq = new List<FAQS>();
		public FAQ()
		{
			InitializeComponent();
			PopulateFAQDataGrid();
		}

		public void PopulateFAQDataGrid()
		{
			faq = new List<FAQS>
		{
			new FAQS { Id = 1, Pregunta = "¿Qué es C#?", Respuesta = "C# es un lenguaje de programación desarrollado por Microsoft.", Documento = null },
			new FAQS { Id = 2, Pregunta = "¿Cuál es la versión más reciente de .NET Core?", Respuesta = "La versión más reciente de .NET Core es la 3.1.", Documento = null },
			new FAQS { Id = 3, Pregunta = "¿Qué es WPF?", Respuesta = "WPF (Windows Presentation Foundation) es un framework para construir aplicaciones de escritorio en Windows.", Documento = null },
			new FAQS { Id = 4, Pregunta = "¿Cuál es la diferencia entre una clase abstracta y una interfaz en C#?", Respuesta = "Una clase abstracta puede contener implementaciones, mientras que una interfaz solo puede tener la firma de métodos.", Documento = null }
		};

			FAQDataGrid.ItemsSource = faq;
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
				string Documento = nuevaFAQ.documento;
				faq.Add(new FAQS
				{
					Id = 1,
					Pregunta = Pregunta,
					Respuesta = Respuesta,
					Documento = Documento,
				});
				FAQDataGrid.Items.Refresh();
			}
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			var selectedItem = FAQDataGrid.SelectedItem as FAQS;
			if (selectedItem != null)
			{
				var nuevaFAQ = new nuevaFAQ();
				nuevaFAQ.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				nuevaFAQ.Titulo.Content = "Editar Pregunta";
				nuevaFAQ.Pregunta.Text = selectedItem.Pregunta;
				nuevaFAQ.Respuesta.Text = selectedItem.Respuesta;
				nuevaFAQ.documento = selectedItem.Documento;
				if (nuevaFAQ.ShowDialog() == true)
				{
					string Pregunta = nuevaFAQ.pregunta;
					string Respuesta = nuevaFAQ.respuesta;
					string Documento = nuevaFAQ.documento;
					selectedItem.Pregunta = Pregunta;
					selectedItem.Respuesta = Respuesta;
					selectedItem.Documento = Documento;
					FAQDataGrid.Items.Refresh();
				}
			}
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			var selectedItem = FAQDataGrid.SelectedItem as FAQS;
			if (selectedItem != null)
			{
				faq.Remove(selectedItem);
				FAQDataGrid.Items.Refresh();
			}
		}
	}
}
