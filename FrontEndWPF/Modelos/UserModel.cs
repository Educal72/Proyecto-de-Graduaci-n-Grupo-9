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
            public int Id { get; set; }
			public string Nombre { get; set; }
			public string PrimerApellido { get; set; }
			public string SegundoApellido { get; set; }
			public string Cedula { get; set; }
			public string Telefono { get; set; }
			public string Correo { get; set; }
			public string Contraseña { get; set; }
			public int IdRol { get; set; }
            public string? NombreRol {  get; set; }
			public DateTime FechaCreacion { get; set; }
			public string Puesto { get; set; }
			public decimal Salario { get; set; }
            public string Direccion { get; set; }
            public bool Activo { get; set; }


        }

        public class EmpleadoDesvinculacion
        {
            public int Id { get; set; } // Identificador único de la desvinculación
            public string Empleado { get; set; }
            public DateTime FechaInicio { get; set; } // Fecha de inicio.
            public string Motivo { get; set; } // Motivo de la desvinculación
            public string Comentarios { get; set; } // Comentarios adicionales sobre la desvinculación
            public DateTime FechaSalida { get; set; } // Fecha de la desvinculación
            public bool Reconocido { get; set; }
        }

        /*
         * Entidades que almacenan los datos de las -
         * planillas.
         */
		public class ControlPlanilla
		{
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string Cedula { get; set; } 
            public string Puesto { get; set; }
            public string Correo { get; set; }
            public DateTime FechaCreacion { get; set; } 
            public decimal Salario { get; set; }
        }
    }
}
