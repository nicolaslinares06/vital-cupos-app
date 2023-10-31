using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Helpers.Models
{
    public class Quota
    {
        public decimal Code { get; set; }
        public decimal? NumberResolution { get; set; }
        public decimal QuotasGrant { get; set; }
        public decimal? QuotasAdvantageCommercialization { get; set; }
        public string? QuotasRePoblation { get; set; }
        public decimal? QuotasAvailable { get; set; }
        public DateTime ProductionDate { get; set; }
        public decimal? YearProduction { get; set; }
        public decimal SpeciesCode { get; set; }
        public string? SpeciesName { get; set; }
        public decimal? QuotasSold { get; set; }
        public decimal? InitialNumeration { get; set; }
        public decimal? FinalNumeration { get; set; }
        public decimal CompanyCode { get; set; }
        public decimal? InitialNumerationRePoblation { get; set; }
        public decimal? FinalNumerationRePoblation { get; set; }
        public decimal? InitialNumerationSeal { get; set; }
        public decimal? FinalNumerationSeal { get; set; }
    }
}
