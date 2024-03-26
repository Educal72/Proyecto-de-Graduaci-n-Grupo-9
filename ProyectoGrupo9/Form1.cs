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
	public partial class Form1 : Form
	{
		int resultado;
		public Form1()
		{
			InitializeComponent();
			
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void label2_Click(object sender, EventArgs e)
		{

		}

		private void label3_Click(object sender, EventArgs e)
		{

		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			string pattern = @"^\d+$";

			Regex regex = new Regex(pattern);

			if (!regex.IsMatch(textBox1.Text))
			{
				textBox1.Clear();
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

		private void button1_Click(object sender, EventArgs e)
		{
			
			string textBoxValue1 = textBox1.Text;
			string textBoxValue2 = textBox2.Text;
			if (textBoxValue1.Equals("") || textBoxValue2.Equals(""))
			{
				label3.Text = "¡Faltan numeros!";
			}
			else { 
				int.TryParse(textBoxValue1, out int num1);
				int.TryParse(textBoxValue2, out int num2);
				resultado = num1 + num2;
				label3.Text = resultado.ToString();
			}
			
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Form2 form2 = new Form2(resultado);
			form2.Show();
		}
	}
}
