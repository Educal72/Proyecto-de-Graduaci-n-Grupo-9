using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace FrontEndWPF.Index
{
	/// <summary>
	/// Lógica de interacción para CambiarContrasenaVentana.xaml
	/// </summary>
	public partial class CambiarContrasenaVentana : Window
	{
		Conexion conexion = new Conexion();
		public CambiarContrasenaVentana()
		{
			InitializeComponent();
		}

		public string getContrasenaAntigua()
		{

			using (SqlConnection connection = conexion.OpenConnection())
			{
				if (connection != null)
				{
					// Find the role name
					string roleQuery = "SELECT Contraseña FROM Usuario WHERE Id = @Id";
					string conAntigua;

					using (SqlCommand roleCommand = new SqlCommand(roleQuery, connection))
					{
						roleCommand.Parameters.AddWithValue("@Id", SesionUsuario.Instance.id);
						try
						{
							object result = roleCommand.ExecuteScalar();
							if (result != null)
							{
								conAntigua = result.ToString();
								return conAntigua;
							}
							else
							{
								Console.WriteLine("Empelado no encontrado.");
								return "";
							}
						}
						catch (Exception ex)
						{
							Console.WriteLine("Error: " + ex.Message);
							return "";
						}
					}
				}
				else
				{
					return "";

				}
			}
		}

		public bool UpdateContrasena(string nuevaConstraseña)
		{

			using (SqlConnection connection = conexion.OpenConnection())
			{
				if (connection != null)
				{
					// Find the role name
					string roleQuery = "UPDATE Usuario SET Contraseña = @Contraseña WHERE Id = @Id";

					using (SqlCommand roleCommand = new SqlCommand(roleQuery, connection))
					{
						roleCommand.Parameters.AddWithValue("@Contraseña", nuevaConstraseña);
						roleCommand.Parameters.AddWithValue("@Id", SesionUsuario.Instance.id);
						try
						{
							int result = roleCommand.ExecuteNonQuery();
							if (result > 0)
							{
								return true;
							}
							else
							{
								Console.WriteLine("Empelado no encontrado.");
								return false;
							}
						}
						catch (Exception ex)
						{
							Console.WriteLine("Error: " + ex.Message);
							return false;
						}
					}
				}
				else
				{
					return false;

				}
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			string conAntigua = getContrasenaAntigua();
			string conAntiguaPassBox = conexion.HashPassword(contrasenaAntigua.Password);
			if (conAntigua == conAntiguaPassBox && nuevaContrasena.Password == confirmarNuevaContrasena.Password) {
				bool result = UpdateContrasena(conexion.HashPassword(nuevaContrasena.Password));
				if (result)
				{
					MessageBox.Show("Contraseña cambiada exitosamente.", "Cambio de Contraseña", MessageBoxButton.OK, MessageBoxImage.Information);
					DialogResult = true;
				}
			}
			else if (conAntigua != conAntiguaPassBox)
			{
				MessageBox.Show("Contraseña actual no es la correcta.", "Error al cambiar la contraseña", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else if (nuevaContrasena.Password != confirmarNuevaContrasena.Password)
			{
				MessageBox.Show("Contraseña nueva no coincide en los espacios.", "Error al cambiar la contraseña", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
