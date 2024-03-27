using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoGrupo9
{

		public partial class Form1 : Form
	{


		public Form1()
		{
			InitializeComponent();
			
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void label3_Click(object sender, EventArgs e)
		{

		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{


		}

		private void panel1_Paint(object sender, PaintEventArgs e)
		{
		}

		private async void button1_Click(object sender, EventArgs e)
		{
			Form2 form2 = new Form2();
			this.Hide();
			form2.ShowDialog();
			this.Close();
		}

		private void textBox2_TextChanged(object sender, EventArgs e)
		{
			textBox2.Padding = new Padding(5);
		}
	}
}
