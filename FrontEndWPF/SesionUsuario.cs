using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FrontEndWPF
{
    public class SesionUsuario
    {
        private static SesionUsuario instance = null;
        private static readonly object padlock = new object();
        public string correo { get; set; }
        public string rol { get; set; }
        public string nombre { get; set; }
        public int id { get; set; }

        SesionUsuario()
        {
        }

        public static SesionUsuario Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new SesionUsuario();
                    }
                    return instance;
                }
            }
        }
    }
}