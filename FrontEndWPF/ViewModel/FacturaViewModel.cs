using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrontEndWPF.Index;
using System.Windows.Media.Media3D;

namespace FrontEndWPF.ViewModel
{
    class FacturaViewModel
    {
		Conexion conexion = new Conexion();
		public int CrearFactura(int IdOrden, decimal CantidadPagada, int Impuestos, int Servicio, DateTime FechaCreacion, DateTime FechaCierre, string Cajero, int IdEmpleado, int IdCliente, decimal Descuento, decimal PuntosGanados, string MetodoPago, string TipoVenta, decimal Total)
		{
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = "INSERT INTO Factura (IdOrden, CantidadPagada, Impuestos, Servicio, FechaCreacion, FechaCierre, Cajero, IdEmpleado, IdCliente, Descuento, PuntosGanados, MetodoPago, TipoVenta, Total)  OUTPUT INSERTED.Id " +
					"VALUES (@IdOrden, @CantidadPagada, @Impuestos, @Servicio, @FechaCreacion, @FechaCierre, @Cajero, @IdEmpleado, @IdCliente, @Descuento, @PuntosGanados, @MetodoPago, @TipoVenta, @Total)";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.Add(new SqlParameter("@IdOrden", IdOrden ));
					command.Parameters.Add(new SqlParameter("@CantidadPagada", CantidadPagada));
					command.Parameters.Add(new SqlParameter("@Impuestos", Impuestos));
					command.Parameters.Add(new SqlParameter("@Servicio", Servicio));
					command.Parameters.Add(new SqlParameter("@FechaCreacion", FechaCreacion));
					command.Parameters.Add(new SqlParameter("@FechaCierre", FechaCierre));
					command.Parameters.Add(new SqlParameter("@Cajero", Cajero));
					command.Parameters.Add(new SqlParameter("@IdEmpleado", IdEmpleado));
					command.Parameters.Add(new SqlParameter("@IdCliente", IdCliente));
					command.Parameters.Add(new SqlParameter("@Descuento", Descuento));
					command.Parameters.Add(new SqlParameter("@PuntosGanados", PuntosGanados));
					command.Parameters.Add(new SqlParameter("@MetodoPago", MetodoPago));
					command.Parameters.Add(new SqlParameter("@TipoVenta", TipoVenta));
					command.Parameters.Add(new SqlParameter("@Total", Total));
					int idFactura = (int)command.ExecuteScalar();
					return idFactura;
				}
			}
		}
		public void TerminarOrden(int id)
		{
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = "UPDATE Orden SET Estado = @Estado WHERE Id = @Id";

				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@Estado", "Facturada");
					command.Parameters.AddWithValue("@Id", id);
					command.ExecuteNonQuery();
				}
			}
		}
	}
}
