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
        public string CommonName { get; set; }
        public string Name { get; set; }
        public string NameFamily { get; set; }
        public string? ScientificName { get; set; }
    }
}
