using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndWPF.Modelos
{
    public class Inversion
    {
        public int Id { get; set; }               // Mapea a [Id] PK, INT, NOT NULL
        public string Financiera { get; set; }    // Mapea a [Financiera] NVARCHAR(100), NOT NULL
        public decimal MontoInvertido { get; set; }  // Mapea a [MontoInvertido] DECIMAL(18,2), NOT NULL
        public DateTime FechaCreacion { get; set; }  // Mapea a [FechaCreacion] DATE, NOT NULL
        public decimal? GananciaMensual { get; set; }  // Mapea a [GananciaMensual] DECIMAL(18,2), NULL
        public DateTime? FechaFinalizacion { get; set; }  // Mapea a [FechaFinalizacion] DATE, NULL
    }

    public class Prestamos
    {
        public int Id { get; set; }               // Mapea a [Id] PK, INT, NOT NULL
        public int IdEmpleado { get; set; }       // Mapea a [IdEmpleado] FK, INT, NOT NULL
        public decimal MontoTotal { get; set; }   // Mapea a [MontoTotal] DECIMAL(10,2), NOT NULL
        public decimal Pendiente { get; set; }    // Mapea a [Pendiente] DECIMAL(10,2), NOT NULL
        public decimal Interes { get; set; }      // Mapea a [Interes] DECIMAL(5,2), NOT NULL
        public string Estado { get; set; }        // Mapea a [Estado] NVARCHAR(50), NOT NULL
        public DateTime FechaCreacion { get; set; }  // Mapea a [FechaCreacion] DATE, NOT NULL
        public string? Descripcion { get; set; }  // Mapea a [Descripcion] NVARCHAR(MAX), NULL
    }

    public class Financiamiento
    {
        public int Id { get; set; }               // Mapea a [Id] PK, INT, NOT NULL
        public string NombreEmpresa { get; set; } // Mapea a [NombreEmpresa] NVARCHAR(100), NOT NULL
        public string Motivo { get; set; }        // Mapea a [Motivo] NVARCHAR(255), NOT NULL
        public DateTime Fecha { get; set; }       // Mapea a [Fecha] DATE, NOT NULL
        public string Estado { get; set; }        // Mapea a [Estado] NVARCHAR(50), NOT NULL
        public decimal Monto { get; set; }        // Mapea a [Monto] DECIMAL(10,2), NOT NULL
        public decimal? Cancelado { get; set; }   // Mapea a [Cancelado] DECIMAL(10,2), NULL
        public decimal? Interes { get; set; }     // Mapea a [Interes] DECIMAL(5,2), NULL
    }
}
