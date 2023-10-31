using System;
using System.Collections.Generic;

namespace Repository.Models
{
    public partial class CuposV001Precintoymarquilla
    {
        public decimal PKV001CODIGO { get; set; }
        public string? TIPODOCUMENTO { get; set; }
        public decimal? NUMERO { get; set; }
        public string? NOMBRE { get; set; }
        public string? NUMERORADICADO { get; set; }
        public string? NUMEROPERMISOCITES { get; set; }
        public DateTime? FECHAINICIAL { get; set; }
        public DateTime? FECHAFINAL { get; set; }
        public string? NUMEROINICIAL { get; set; }
        public string? NUMEROFINAL { get; set; }
        public decimal? NUMEROINTERNOINICIAL { get; set; }
        public decimal? NUMEROINTERNOFINAL { get; set; }
        public DateTime? VIGENCIA { get; set; }
        public decimal? CANTIDAD { get; set; }
        public string? COLOR { get; set; }
        public string? ESPECIE { get; set; }
        public int? CUPOSDISPONIBLES { get; set; }
        public int? CUPOSTOTAL { get; set; }
        public DateTime? FECHACREACION { get; set; }
    }
}
