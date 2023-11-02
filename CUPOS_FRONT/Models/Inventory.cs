using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Helpers.Models
{
    public class Inventory
    {
        public decimal quotaCode { get; set; }
        public decimal Code { get; set; }
        public string? NumberSaleCarte { get; set; }
        public string ReasonSocial { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public decimal AvailabilityInventory { get; set; }
        public string Year { get; set; } = string.Empty;
        public decimal AvailableInventory { get; set; }
        public decimal? InitialNumeration { get; set; }
        public decimal? FinalNumeration { get; set; }
        public decimal? InitialNumerationRePoblation { get; set; }
        public decimal? FinalNumerationRePoblation { get; set; }
        public decimal? InitialNumerationSeal { get; set; }
        public decimal? FinalNumerationSeal { get; set; }
        public decimal SpeciesCode { get; set; }
        public string? SpeciesName { get; set; }
        public decimal InventorySold { get; set; }
    }
}
