﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndWPF.Modelos
{
    public class PermisoDeTiempo
    {
        public int IdEmpleado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Motivo { get; set; }
        public string Estado { get; set; } // Cambiar de int a string por eso se me cae
    }
}