﻿using FrontEndWPF.ViewModel;
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

namespace FrontEndWPF.PuntoDeVenta
{
	/// <summary>
	/// Lógica de interacción para Gabinete.xaml
	/// </summary>

	public partial class Gabinete : Window
	{
		CierreViewModel cierreViewModel = new CierreViewModel();
		public string tipo { get; set; }

		public Gabinete()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (!decimal.TryParse(cantidadItem.Text, out decimal result))
			{
				MessageBox.Show("Por favor, introduzca un cantidad valida.", "Error de validación", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				decimal dinero = Convert.ToDecimal(cantidadItem.Text);
				if (tipoComboBox.Text == "Egreso" && cierreViewModel.CheckEgreso(dinero))
				{
					tipo = tipoComboBox.Text;
					cierreViewModel.Gabinete(tipoComboBox.Text, dinero);
					DialogResult = true;
				}
				else if (tipoComboBox.Text == "Ingreso")
				{
					tipo = tipoComboBox.Text;
					cierreViewModel.Gabinete(tipoComboBox.Text, dinero);
					DialogResult = true;
				}
				else if(tipoComboBox.Text == "Egreso" && !cierreViewModel.CheckEgreso(dinero)) {
					MessageBox.Show("¡Intenta retirar más del dinero disponible!", "Operación Invalida", MessageBoxButton.OK, MessageBoxImage.Error);
				}
				
			}
        }

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
