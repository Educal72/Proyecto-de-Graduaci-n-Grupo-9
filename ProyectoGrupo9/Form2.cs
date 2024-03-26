using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoGrupo9
{
	public partial class Form2 : Form
	{
		public Form2(int receivedValue)
		{
			InitializeComponent();
			textBox1.Text = receivedValue.ToString();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			string textBoxValue1 = textBox1.Text;
			string textBoxValue2 = textBox2.Text;
			if (textBoxValue1.Equals("") || textBoxValue2.Equals(""))
			{
				label3.Text = "¡Faltan numeros!";
			}
			else
			{
				int.TryParse(textBoxValue1, out int num1);
				int.TryParse(textBoxValue2, out int num2);
				int resultado = num1 * num2;
				label3.Text = resultado.ToString();
			}
		}

		private void textBox2_TextChanged(object sender, EventArgs e)
		{
			string pattern = @"^\d+$";

			Regex regex = new Regex(pattern);

			if (!regex.IsMatch(textBox2.Text))
			{
				textBox2.Clear();
			}
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{

		}
	}
}
