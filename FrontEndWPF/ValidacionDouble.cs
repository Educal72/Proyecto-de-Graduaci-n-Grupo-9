using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Controls;

namespace FrontEndWPF
{
    class ValidacionDouble : ValidationRule
    {
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			double result;
			if (double.TryParse(value as string, out result))
			{
				return ValidationResult.ValidResult;
			}
			else
			{
				return new ValidationResult(false, "Precio Invalido, entre un precio correcto.");
			}
		}
	}
}
