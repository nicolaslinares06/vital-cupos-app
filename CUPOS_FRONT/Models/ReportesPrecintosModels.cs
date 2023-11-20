using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CUPOS_FRONT.Models
{
    public class ReportesPrecintosModels
    {
        public class FiltrosPrecintos
        {
            public string ResolutionNumber { get; set; } = "";
            public decimal Establishment { get; set; } = 0;
            public decimal? NIT { get; set; }
            [DataType(DataType.Date)]
            public DateTime? RadicationDate { get; set; }
            public int SpecificSearch { get; set; }
        }


        public class  ReportesPrecintosViewModel : FiltrosPrecintos
        {
            public List<DatosPrecintosModel> listadoPrecintos { get; set; } =  new List<DatosPrecintosModel>(); 
            public IEnumerable<SelectListItem> Establecimientos { get; set; } = new List<SelectListItem>();
        }

        public class DatosPrecintosModel
        {
            public string? RadicationNumber { get; set; } = "";
            public string? RadicationDate { get; set; }
            public string? CompanyName { get; set; } = "";
            public decimal? NIT { get; set; } = 0;
            public string? City { get; set; } = "";
            public string? DeliveryAddress { get; set; } = "";
            public string? Telephone { get; set; } = "";
            public string? Species { get; set; } = "";
            public decimal? LesserLength { get; set; } = 0;
            public decimal? GreaterLength { get; set; } = 0;
            public decimal? Quantity { get; set; } = 0;
            public string? Color { get; set; } = "";
            public decimal? ProductionYear { get; set; } = 0;
            public decimal? InitialInternalNumber { get; set; } = 0;
            public decimal? FinalInternalNumber { get; set; } = 0;
            public string? InitialNumber { get; set; } = "";
            public string? FinalNumber { get; set; } = "";
            public decimal? CompanyCode { get; set; } = 0;
            public string? DepositValue { get; set; } = "";
            public string? Analyst { get; set; } = "";

        }

        public class EstablecimientosPrecintos
        {

            public decimal EstablishmentId { get; set; }
            public string EstablishmentName { get; set; } = "";
        }

    }
}
