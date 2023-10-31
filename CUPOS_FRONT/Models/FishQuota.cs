using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Helpers.Models
{
    public class FishQuota
    {
        public decimal Code { get; set; }
        public string Type { get; set; } = "";
        public decimal NumberResolution { get; set; }
        public DateTime ResolutionDate { get; set; }
        public DateTime? ValidityDate { get; set; }
        public int ValidityYear { get; set; }
        public string ResolutionObject { get; set; } = "";
        public decimal Document { get; set; }
        public DateTime InitialValidityDate { get; set; }
        public DateTime? FinalValidityDate { get; set; }
        public decimal CodeFishQuotaAmount { get; set; } = 0;
        public decimal Group { get; set; }
        public string? GroupName { get; set; }
        public string? speciesNameComun { get; set; }
        public decimal SpeciesCode { get; set; } = 0;
        public decimal Quota { get; set; } = 0;
        public decimal Total { get; set; } = 0;
        public decimal Region { get; set; } = 0;
        public string? SpeciesName { get; set; }
        public List<FishQuotaAmount>? FishQuotaAmounts { get; set; }
        public List<FishQuotaAmount>? FishQuotaAmountsRemoved { get; set; }
        public List<SupportDocuments> SupportDocuments { get; set; }
        public List<SupportDocuments>? SupportDocumentsRemoved { get; set; }
    }
}
