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

        /*
         * Entidades que almacenan los datos de los -
         * incidentes.
         */
        public class Incidente
        {
            public string Usuario { get; set; } //Nombre del usuario (empleado) que tendra ese incidente.
            public DateTime Fecha { get; set; }  // Fecha en que ocurrió el incidente.
            public string Hora { get; set; }  // Hora del incidente.
            public string Descripcion { get; set; }  // Descripción del incidente.
            public string Tipo { get; set; }  // Si es un estado interno ó externo.
            public string Estado { get; set; } // Si el incidente esta resuelto o no esta resuelto.


        }


        /*
         * Entidades que almacenan los datos de los -
         * perfiles competenciales.
         */
        public class PerfilCompentencial
        {
            public int Id { get; set; } // Identificador único del perfil.
            public string Titulo { get; set; } // Título del perfil de empleo.
            public string Descripcion { get; set; } // Descripción del perfil de empleo.
            public string Experiencia { get; set; } // Nivel de experiencia requerido (Básico, Intermedio o Avanzado).
            public string Requisitos { get; set; } // Requisitos del perfil de empleo.
            public string Ubicacion { get; set; } // Ubicación del trabajo.
            public double Salario { get; set; } // Salario mínimo ofrecido.
        }
    }
}