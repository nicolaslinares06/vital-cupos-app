using System.ComponentModel.DataAnnotations;

namespace CUPOS_FRONT.Models
{
    public class ReportesMarquillasModels
    {
        public class DatosMarquillaModel
        {
            public string? RadicationNumber { get; set; }
            public DateTime? RadicationDate { get; set; }
            public string? CompanyName { get; set; }
            public decimal? NIT { get; set; }
            public string? City { get; set; }
            public string? Address { get; set; }
            public decimal? Phone { get; set; }
            public string? Species { get; set; }
            public string? Type { get; set; }
            public string? SpeciesTags { get; set; }
            public decimal? Amount { get; set; }
            public string? Color { get; set; }
            public string? InitialNumber { get; set; }
            public string? FinalNumber { get; set; }
            public decimal? ConsignmentValue { get; set; }
            public string? Evaluator { get; set; }
            public DateTime? AnswerDate { get; set; }
            public string? InitialNumberTags { get; set; }
            public string? FinalNumberTags { get; set; }
        }


        public class FiltrosMarquillas
        {
            [DataType(DataType.Date)]
            public  DateTime? dateFrom { get; set; }
            [DataType(DataType.Date)]
            public DateTime? dateTo { get; set; }
            public string? radicationNumber { get; set; }
        }
    }
}
