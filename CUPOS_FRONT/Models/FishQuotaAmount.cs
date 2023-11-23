using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Helpers.Models
{
    public class FishQuotaAmount
    {
        public decimal CodeFishQuotaAmount { get; set; }
        public decimal Group { get; set; }
        public decimal SpeciesCode { get; set; }
        public string? SpeciesName { get; set; }
        public string? speciesNameComun { get; set; }
        public decimal Quota { get; set; }
        public decimal Total { get; set; }
        public string? NameRegion { get; set; }
        public decimal Region { get; set; } = 0;
        public decimal ActionTemp { get; set; } = 0; 

    }

    public class ParametricaRegion
    {
        public int IdRegion { get; set; }
        public string NombreRegion { get; set; } = string.Empty;        
    }
}
