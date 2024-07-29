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

namespace FrontEndWPF
{
    /// <summary>
    /// Lógica de interacción para nuevoItemVentana.xaml
    /// </summary>
    public partial class nuevaFAQ : Window
    {
		public string? titulo {  get; set; }
		public string? pregunta { get; set; }
		public string? respuesta { get; set; }
		public string? documento { get; set; }


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
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if (openFileDialog.ShowDialog() == true)
			{
				documento = openFileDialog.FileName;
				Doc.Content = "Documento Subido Exitosamente";
			}
		}
	}
}
