using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FrontEndWPF.Modelos.UserModel;

namespace FrontEndWPF.ViewModel
{
    class AuditoriaViewModel
    {
        public class InfoAuditoria
        {
            public string? Impuestos { get; set; }
            public string? Ingresos { get; set; }
            public string? Gastos { get; set; }
            public string? Retenciones { get; set; }
            public string? EstadoFinanciero { get; set; }
            public string? DatosEmpleado { get; set; }

            public string? RespImpuestos { get; set; }
            public string? RespIngresos { get; set; }
            public string? RespGastos { get; set; }
            public string? RespRetenciones { get; set; }
            public string? RespEstadoFinanciero { get; set; }
            public string? RespDatosEmpleado { get; set; }
        }

        public class Impuestos_InfoAuditoria
        {
            public string? Impuestos { get; set; }
        }

        public class Ingresos_InfoAuditoria
        {
            public string? Ingresos { get; set; }
        }

        public class Gastos_InfoAuditoria
        {
            public string? Gastos { get; set; }
        }

        public class Retenciones_InfoAuditoria
        {
            public string? Retenciones { get; set; }
        }

        public class EstadoFinanciero_InfoAuditoria
        {
            public string? EstadoFinanciero { get; set; }
        }

        public class DatosEmpleado_InfoAuditoria
        {
            public string? DatosEmpleado { get; set; }
        }

    
    }
}
