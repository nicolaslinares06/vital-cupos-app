using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Helpers.Models
{
    public class ReqParametric
    {
        public int? code { get; set; }
        public string? name { get; set; }
        public string? value { get; set; }
        public string? description { get; set; }
        public decimal? estate { get; set; }
    }

    public class ReqIdParametric
    {
        public decimal id { get; set; }
    }
}
