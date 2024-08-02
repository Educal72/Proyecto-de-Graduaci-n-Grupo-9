using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndWPF.Modelos
{
	public class UserModel
	{
        /* Entidades que almacenan los datos de las -
         * empleados.*/
        public class UsuarioEmpleado
		{        
            public int Id { get; set; }                             //ID del empleado.
            public string? Nombre { get; set; }                     //Nombre del empleado.
			public string? Apellido { get; set; }                   //Apellido del empleado.
			public string? Cedula { get; set; }                     //Cédula del empleado.
			public string? Telefono { get; set; }                   //Teléfono del empleado.
			public string? Correo { get; set; }
			public string? NombreRol { get; set; }   //Correo del empleado.
			public string? Contraseña { get; set; }                 //Contraseña del empleado.
            public int IdRol { get; set; }                          //Rol del empleado.
            public DateTime FechaCreacion { get; set; }             //Fecha cuando ingreso el empleado al sistema y el negocio.
			public string? Puesto { get; set; }                     //Posición del empleado (o puesto).
            public string? Direccion { get; set; }                  //Dirección del empleado.
            public bool Activo { get; set; }                        //Si el empleado sigue trabajando o no.
        }


        /* Entidades que almacenan los datos de las -
         * empleados, pero los que se usan para solo -
         * mostrar. */
        public class UsuarioEmpleadoSoloMostrar
		{
            public string? Nombre { get; set; }                     //Nombre del empleado.
            public string? Apellido { get; set; }                   //Apellido del empleado.
            public string? Cedula { get; set; }                     //Cédula del empleado.
            public string? Telefono { get; set; }                   //Teléfono del empleado.
            public string? Correo { get; set; }                     //Correo del empleado.
            public string? Rol { get; set; }                        //Rol del empleado.
            public DateTime FechaCreacion { get; set; }             //Fecha cuando ingreso el empleado al sistema y el negocio.
            public string? Puesto { get; set; }                     //Posición del empleado (o puesto).
            public string? Direccion { get; set; }                  //Dirección del empleado.
            public bool Activo { get; set; }                        //Si el empleado sigue trabajando o no.
        }


        /* Entidades que almacenan los datos de los -
         * permisos de autorización. */
        public class PermisosAutorizacion
		{
            public int IdUsuario { get; set; }                      //ID del usuario que tiene los permisos.
            public bool LeerDesvinculacion { get; set; }            //Si tiene permisos para leer las solicitudes de las desvinculaciones.
            public bool CrearDesvinculacion { get; set; }           //Si tiene permisos para crear las solicitudes de las desvinculaciones.
            public bool EliminarDesvinculacion { get; set; }        //Si tiene permisos para eliminar las solicitudes de las desvinculaciones.			
        }


        /* Entidades que almacenan los datos de las -
         * desvinculaciones. */
        public class EmpleadoDesvinculacion
        {
            public int Id { get; set; }                             //ID único de la desvinculación
            public string? Nombre { get; set; }                     //Nombre del empleado para la desvinculación.
            public string? Apellido { get; set; }                   //Apellido del empleado para la desvinculación.
            public DateTime FechaInicio { get; set; }               //Fecha de inicio de la desvinculación.
            public string? Motivo { get; set; }                     //Motivo de la desvinculación.
            public string? Comentarios { get; set; }                //Comentarios adicionales sobre la desvinculación.
            public DateTime FechaSalida { get; set; }               //Fecha de la desvinculación.
            public bool Reconocido { get; set; }                    //Si el estado es reconocido (true o 1) o si no es reconocido (false o 0) la desvinculación.
        }


        /* Entidades que almacenan los datos de las -
         * planillas. */
        public class ControlPlanilla
        {
            public string? Nombre { get; set; }                     //Nombre del empleado para la planilla.
            public string? Apellidos { get; set; }                  //Apellido del empleado para la planilla.
            public string? Cedula { get; set; }                     //Cédula del empleado para la planilla.
            public string? Puesto { get; set; }                     //Puesto del empleado para la planilla.
            public string? Correo { get; set; }                     //Correo del empleado para la planilla.
            public DateTime FechaCreacion { get; set; }             //Fecha en donde se creo la planilla.
            public decimal Salario { get; set; }                    //Salario del empleado para la planilla.
        }


        /* Entidades que almacenan los datos de los -
         * incidentes. */
        public class Incidente
        {
            public int? Id { get; set; }
            public string? Usuario { get; set; }
			public string? Apellido { get; set; }
			public string? Nombre { get; set; } //Nombre del usuario (empleado) que tendra ese incidente.
            public DateTime Fecha { get; set; }                     //Fecha en que ocurrió el incidente.
            public string? Hora { get; set; }                       //Hora del incidente.
            public string? Descripcion { get; set; }                //Descripción del incidente.
            public string? Tipo { get; set; }                       //Si es un estado interno ó externo.
            public string? Estado { get; set; }                     //Si el incidente esta resuelto o no esta resuelto.


        }


        /* Entidades que almacenan los datos de los -
         * perfiles competenciales. */
        public class PerfilCompentencial
        {
            public int Id { get; set; }                             //Identificador único del perfil.
            public string? Titulo { get; set; }                     //Título del perfil de empleo.
            public string? Descripcion { get; set; }                //Descripción del perfil de empleo.
            public string? Experiencia { get; set; }                //Nivel de experiencia requerido (Básico, Intermedio o Avanzado).
            public string? Requisitos { get; set; }                 //Requisitos del perfil de empleo.
            public string? Ubicacion { get; set; }                  //Ubicación del trabajo.
            public double Salario { get; set; }                     //Salario mínimo ofrecido.
        }
    
    
    
    
    
    }
}