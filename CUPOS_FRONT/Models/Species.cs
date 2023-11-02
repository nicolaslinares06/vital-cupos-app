using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Models
{
    public class Species
    {
        public int Code { get; set; }
        public string CommonName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string NameFamily { get; set; } = string.Empty;
        public string? ScientificName { get; set; }
    }
}
