using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrontEndWPF.Modelos;
using static FrontEndWPF.Reporteria.VisualizarInversiones;
using static FrontEndWPF.Reporteria.VisualizarFinanciamientos;
using static FrontEndWPF.Reporteria.VisualizarPrestamos;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FrontEndWPF.ViewModel
{
    public class FlujosFinancierosViewModel
    {
        Conexion conexion = new Conexion();

        public List<Inversion> GetAllInversiones()
        {
            Conexion conexion = new Conexion();
            List<Inversion> inversiones = new List<Inversion>();

            using (SqlConnection connection = conexion.OpenConnection())
            {
                string query = "SELECT * FROM Inversiones";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Inversion inversion = new Inversion
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Financiera = reader["Financiera"].ToString(),
                                MontoInvertido = Convert.ToDecimal(reader["MontoInvertido"]),
                                FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]),
                                GananciaMensual = reader["GananciaMensual"] != DBNull.Value ? Convert.ToDecimal(reader["GananciaMensual"]) : (decimal?)null,
                                FechaFinalizacion = reader["FechaFinalizacion"] != DBNull.Value ? Convert.ToDateTime(reader["FechaFinalizacion"]) : (DateTime?)null
                            };
                            inversiones.Add(inversion);
                        }
                    }
                }
            }
            return inversiones;
        }

        public List<Inversion> GetInversionesByFinanciera(string financiera)
        {
            Conexion conexion = new Conexion();
            List<Inversion> inversiones = new List<Inversion>();

            using (SqlConnection connection = conexion.OpenConnection())
            {
                string query = "SELECT * FROM Inversiones WHERE Financiera = @Financiera";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@Financiera", financiera));
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Inversion inversion = new Inversion
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Financiera = reader["Financiera"].ToString(),
                                MontoInvertido = Convert.ToDecimal(reader["MontoInvertido"]),
                                FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]),
                                GananciaMensual = reader["GananciaMensual"] != DBNull.Value ? Convert.ToDecimal(reader["GananciaMensual"]) : (decimal?)null,
                                FechaFinalizacion = reader["FechaFinalizacion"] != DBNull.Value ? Convert.ToDateTime(reader["FechaFinalizacion"]) : (DateTime?)null
                            };
                            inversiones.Add(inversion);
                        }
                    }
                }
            }
            return inversiones;
        }

        public List<Prestamos> GetAllPrestamos()
        {
            Conexion conexion = new Conexion();
            List<Prestamos> prestamos = new List<Prestamos>();

            using (SqlConnection connection = conexion.OpenConnection())
            {
                string query = "SELECT * FROM Prestamos";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Prestamos prestamo = new Prestamos
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                IdEmpleado = Convert.ToInt32(reader["IdEmpleado"]),
                                MontoTotal = Convert.ToDecimal(reader["MontoTotal"]),
                                Pendiente = Convert.ToDecimal(reader["Pendiente"]),
                                Interes = Convert.ToDecimal(reader["Interes"]),
                                Estado = reader["Estado"].ToString(),
                                FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]),
                                Descripcion = reader["Descripcion"] != DBNull.Value ? reader["Descripcion"].ToString() : null
                            };
                            prestamos.Add(prestamo);
                        }
                    }
                }
            }
            return prestamos;
        }

       
            // Método para obtener todos los financiamientos
            public List<Financiamiento> GetAllFinanciamientos()
            {
                Conexion conexion = new Conexion();
                List<Financiamiento> financiamientos = new List<Financiamiento>();

                using (SqlConnection connection = conexion.OpenConnection())
                {
                    string query = "SELECT * FROM Financiamiento";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Financiamiento financiamiento = new Financiamiento
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    NombreEmpresa = reader["NombreEmpresa"].ToString(),
                                    Motivo = reader["Motivo"].ToString(),
                                    Fecha = Convert.ToDateTime(reader["Fecha"]),
                                    Estado = reader["Estado"].ToString(),
                                    Monto = Convert.ToDecimal(reader["Monto"]),
                                    Cancelado = reader["Cancelado"] != DBNull.Value ? Convert.ToDecimal(reader["Cancelado"]) : (decimal?)null,
                                    Interes = reader["Interes"] != DBNull.Value ? Convert.ToDecimal(reader["Interes"]) : (decimal?)null
                                };
                                financiamientos.Add(financiamiento);
                            }
                        }
                    }
                }
                return financiamientos;
            }

            // Método para obtener financiamientos por NombreEmpresa
            public List<Financiamiento> GetFinanciamientosByNombreEmpresa(string nombreEmpresa)
            {
                Conexion conexion = new Conexion();
                List<Financiamiento> financiamientos = new List<Financiamiento>();

                using (SqlConnection connection = conexion.OpenConnection())
                {
                    string query = "SELECT * FROM Financiamiento WHERE NombreEmpresa = @NombreEmpresa";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@NombreEmpresa", nombreEmpresa));
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Financiamiento financiamiento = new Financiamiento
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    NombreEmpresa = reader["NombreEmpresa"].ToString(),
                                    Motivo = reader["Motivo"].ToString(),
                                    Fecha = Convert.ToDateTime(reader["Fecha"]),
                                    Estado = reader["Estado"].ToString(),
                                    Monto = Convert.ToDecimal(reader["Monto"]),
                                    Cancelado = reader["Cancelado"] != DBNull.Value ? Convert.ToDecimal(reader["Cancelado"]) : (decimal?)null,
                                    Interes = reader["Interes"] != DBNull.Value ? Convert.ToDecimal(reader["Interes"]) : (decimal?)null
                                };
                                financiamientos.Add(financiamiento);
                            }
                        }
                    }
                }
                return financiamientos;
            }


        public List<Prestamos> GetPrestamosByIdUsuario(int id)
        {
            Conexion conexion = new Conexion();
            List<Prestamos> prestamos = new List<Prestamos>();

            using (SqlConnection connection = conexion.OpenConnection())
            {
                string query = "SELECT * FROM Prestamos WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@Id", id));
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Prestamos prestamo = new Prestamos
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                MontoTotal = Convert.ToDecimal(reader["MontoTotal"]),
                                Pendiente = Convert.ToDecimal(reader["Pendiente"]),
                                Interes = Convert.ToDecimal(reader["Interes"]),
                                Estado = reader["Estado"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),
                                FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"])
                            };
                            prestamos.Add(prestamo);
                        }
                    }
                }
            }
            return prestamos;
        }

        public List<string> GetAllNombresEmpresa()
        {
            Conexion conexion = new Conexion();
            List<string> nombresEmpresa = new List<string>();

            using (SqlConnection connection = conexion.OpenConnection())
            {
                string query = "SELECT DISTINCT NombreEmpresa FROM Financiamiento";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            nombresEmpresa.Add(reader["NombreEmpresa"].ToString());
                        }
                    }
                }
            }

            return nombresEmpresa;
        }

        public List<string> GetAllNombresFinancieras()
        {
            Conexion conexion = new Conexion();
            List<string> nombresFinancieras = new List<string>();

            using (SqlConnection connection = conexion.OpenConnection())
            {
                string query = "SELECT DISTINCT Financiera FROM Inversiones";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            nombresFinancieras.Add(reader["Financiera"].ToString());
                        }
                    }
                }
            }

            return nombresFinancieras;
        }

        public List<int> GetAllIdEmpleados()
        {
            Conexion conexion = new Conexion();
            List<int> idEmpleados = new List<int>();

            using (SqlConnection connection = conexion.OpenConnection())
            {
                string query = "SELECT DISTINCT IdEmpleado FROM Prestamos";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            idEmpleados.Add(Convert.ToInt32(reader["IdEmpleado"]));
                        }
                    }
                }
            }

            return idEmpleados;
        }


    }
}
