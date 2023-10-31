using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CUPOS_FRONT.Models
{
    public class ReportesCuposEmpresasMarcajeModels
    {
        public class ReporteCuposEmpresaViewModel
        {
            public decimal TipoEmpresa { get; set; } = 0;
            public string NombreEmpresa { get; set; } = "";
            public decimal NIT { get; set; } = 0;
            public decimal Estado { get; set; } = 0;
            public decimal EstadoEmisionCITES { get; set; } = 0;
            public decimal NumeroResolucion { get; set; } = 0;
            [DataType(DataType.Date)]
            public DateTime FechaEmisionResolucionDesde { get; set; } 
            [DataType(DataType.Date)]
            public DateTime FechaEmisionResolucionHasta { get; set; } 
            public int BusquedaEspecifica { get; set; }
            public int CombinacionTipo { get; set; }


        }

        public class PropsViewModel : ReporteCuposEmpresaViewModel
        {
            public IEnumerable<SelectListItem> TipoEstablecimientos { get; set; }
            public IEnumerable<SelectListItem> Estados { get; set; }



        }

        public class DatosEmpresasModel
        {
            public string? TipoEmpresa { get; set; } = "";
            public string? NombreEmpresa { get; set; } = "";
            public decimal? NIT { get; set; } = 0;
            public string? Estado { get; set; } = "";
            public string? EstadoEmisionCITES { get; set; } = "";
            public string? NumeroResolucion { get; set; } = "";
            public string? FechaEmisionResolucion { get; set; }
            public string? Especies { get; set; } = "";
            public decimal? Machos { get; set; } = 0;
            public decimal? Hembras { get; set; } = 0;
            public decimal? PoblacionTotalParental { get; set; } = 0;
            public decimal? AnioProduccion { get; set; } = 0;
            public decimal? CuposComercializacion { get; set; } = 0;
            public string? CuotaRepoblacion { get; set; } = "";
            public decimal? CuposAsignadosTotal { get; set; } = 0;
            public bool? SoportesRepoblacion { get; set; }
            public decimal? CupoUtilizado { get; set; } = 0;
            public decimal? CupoDisponible { get; set; } = 0;
        }

        public class FiltrosEmpresasBooleanos
        {
            public bool TipoEmpresa { get; set; }
            public bool NombreEmpresa { get; set; }
            public bool NIT { get; set; } 
            public bool Estado { get; set; } 
            public bool EstadoEmisionCITES { get; set; } 
            public bool NumeroResolucion { get; set; }          
            public bool FechaEmisionResolucionDesde { get; set; } 
            public bool FechaEmisionResolucionHasta { get; set; } 
            public int BusquedaEspecifica { get; set; }


        }




    }
}
