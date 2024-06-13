using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndWPF.Modelos
{
	public class UserModel
	{
		public class UsuarioEmpleado
		{
			public string Nombre { get; set; }
			public string PrimerApellido { get; set; }
			public string SegundoApellido { get; set; }
			public string Cedula { get; set; }
			public string Telefono { get; set; }
			public string Correo { get; set; }
			public string Contraseña { get; set; }
			public int IdRol { get; set; }
			public DateTime FechaCreacion { get; set; }
			public string Puesto { get; set; }
			public double Salario { get; set; }
            public string Direccion { get; set; }
            public bool Activo { get; set; }


        }
	}
}
