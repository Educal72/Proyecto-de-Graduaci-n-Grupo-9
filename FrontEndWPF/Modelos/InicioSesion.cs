using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndWPF.Modelos
{
    public class InicioSesion
    {
        public int IdUsuario { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaInicioSesion { get; set; }
        public DateTime? UltimaDesconexion { get; set; }  // Permitir nulos
    }
}
