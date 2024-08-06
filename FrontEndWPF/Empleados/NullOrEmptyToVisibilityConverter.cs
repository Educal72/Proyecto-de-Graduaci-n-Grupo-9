﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace FrontEndWPF.Empleados
{
	public class NullOrEmptyToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || string.IsNullOrWhiteSpace(value.ToString()) || value.ToString() == "Sin Documento Asociado")
			{
				return Visibility.Collapsed;
			}
			return Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

	}
}
