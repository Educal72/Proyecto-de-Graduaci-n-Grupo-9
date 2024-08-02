using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndWPF.Modelos
{
    public class PermisoDeTiempo
    {
        public int IdEmpleado { get; set; }
        public string? NombreCompleto { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Motivo { get; set; }
        public string Estado { get; set; }
    }
}
