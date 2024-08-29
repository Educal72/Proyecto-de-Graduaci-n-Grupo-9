using FrontEndWPF.Modelos;
using FrontEndWPF.PuntoDeVenta;
using FrontEndWPF.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static FrontEndWPF.ViewModel.CierreViewModel;


namespace FrontEndWPF.Reporteria
{
    /// <summary>
    /// Interaction logic for InicioDeSesion.xaml
    /// </summary>
    public partial class InicioDeSesion : UserControl
    {
        private InicioSesionViewModel viewModel = new InicioSesionViewModel();
        private List<InicioSesion> inicioSesions = new List<InicioSesion>();

        public InicioDeSesion()
        {
            InitializeComponent();
            LoadInicioSesiones();
           
        }


		private void LoadInicioSesiones()
		{
			Conexion conexion = new Conexion();
			var sesionesList = conexion.ListarInicioSesion();

			foreach (var sesionDict in sesionesList)
			{
				InicioSesion sesion = new InicioSesion
				{
					IdUsuario = (int)sesionDict["IdUsuario"],
					Nombre = GetUserById((int)sesionDict["IdUsuario"]),
					FechaIngreso = (DateTime)sesionDict["FechaIngreso"],
					FechaInicioSesion = (DateTime)sesionDict["FechaInicioSesion"],
					UltimaDesconexion = sesionDict["UltimaDesconexion"] != null ? (DateTime)sesionDict["UltimaDesconexion"] : (DateTime?)null
				};
				dataGrid.Items.Add(sesion);
                inicioSesions.Add(sesion);
			}
		}

		public string GetUserById(int id)
		{
			Conexion conexion = new Conexion();
			string NombreCompleto = "";
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = @"SELECT 
    Cedula,
    Nombre, 
    Apellido
FROM 
    Usuario
WHERE 
    Id = @Id";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.Add(new SqlParameter("@Id", id));
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							NombreCompleto = reader["Nombre"].ToString() + " " + reader["Apellido"].ToString() + " (" + reader["Cedula"].ToString() + ")";
							return NombreCompleto;
						}
					}
				}
			}
			return NombreCompleto;
		}

		

        private void Actualizar_Click(object sender, RoutedEventArgs e)
        {

            modalPopup.IsOpen = false;
        }

        private void GenerarInforme_Click(object sender, RoutedEventArgs e)
        {
			viewModel.GenerarPDF(inicioSesions);
        }

        private void CerrarInformePopup_Click(object sender, RoutedEventArgs e)
        {
            informePopup.IsOpen = false;
        }

        private void ActualizarInforme_Click(object sender, RoutedEventArgs e)
        {
           
            if (fechaInforme.SelectedDate.HasValue)
            {
                DateTime fechaSeleccionada = fechaInforme.SelectedDate.Value;
                CargarRegistrosPorFecha(fechaSeleccionada);
            }
            else
            {
                MessageBoxResult resultado = MessageBox.Show(
                    "No se ha seleccionado una fecha. ¿Desea continuar sin filtrar los registros por fecha?",
                    "Confirmar",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (resultado == MessageBoxResult.Yes)
                {
                    MessageBox.Show("Cargando todos los registros.");
                    viewModel.CargarRegistrosPorFecha(DateTime.MinValue); // Puedes manejar el caso de `DateTime.MinValue` como prefieras
                }
            }
        }

		public void CargarRegistrosPorFecha(DateTime fecha)
		{
			dataGrid.Items.Clear();
			Conexion conexion = new Conexion();
			var registrosList = conexion.BuscarInicioSesionPorFecha(fecha);

			foreach (var registroDict in registrosList)
			{
				InicioSesion registro = new InicioSesion
				{
					IdUsuario = (int)registroDict["IdUsuario"],
					Nombre = GetUserById((int)registroDict["IdUsuario"]),
					FechaIngreso = (DateTime)registroDict["FechaIngreso"],
					FechaInicioSesion = (DateTime)registroDict["FechaInicioSesion"],
					UltimaDesconexion = registroDict.ContainsKey("UltimaDesconexion")
										? (DateTime?)registroDict["UltimaDesconexion"]
										: null
				};
				dataGrid.Items.Add(registro);
			}
		}
	}
}
