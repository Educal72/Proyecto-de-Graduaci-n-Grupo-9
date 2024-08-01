using FrontEndWPF.Empleados;
using Microsoft.Win32;
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
using System.Windows.Shapes;
using System.IO;

namespace FrontEndWPF
{
    /// <summary>
    /// Lógica de interacción para nuevoItemVentana.xaml
    /// </summary>
    public partial class nuevaFAQ : Window
    {

		public string titulo { get; set; }
		public string pregunta { get; set; }
		public string respuesta { get; set; }
		public string nombre { get; set; } = string.Empty;
		public byte[] documento { get; set; } = new byte[0];



		public nuevaFAQ()
        {
            InitializeComponent();
        }

		private void Button_Click(object sender, RoutedEventArgs e)
		{
				pregunta = Pregunta.Text;
				respuesta = Respuesta.Text; ;
				DialogResult = true;
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
			dlg.Filter = "All Files|*.*";

			if (dlg.ShowDialog() == true)
			{
				string filePath = dlg.FileName;
				documento = File.ReadAllBytes(filePath);
				nombre = System.IO.Path.GetFileName(filePath);
				Doc.Content = "¡Documento Subido Exitosamente!";
			}
		}
	}
}
