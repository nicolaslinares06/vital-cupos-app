using System;
using System.Collections.Generic;

namespace Repository.Models
{
    public partial class CuposV002GestionPrecintosNacionales
    {
        public decimal ID { get; set; }
        public string? NUMERORADICADO { get; set; }
        public DateTime? FECHARADICADO { get; set; }
        public string PRECINTOSNACIONALES { get; set; } = string.Empty;
        public string ENTIDAD { get; set; } = string.Empty;
        public DateTime FECHASOLICITUD { get; set; }
        public string ESTADO { get; set; } = string.Empty;
        public decimal? ANALISTA { get; set; }
        public string? NUMERORADICADOSALIDA { get; set; }
        public DateTime? FECHARADICADOSALIDA { get; set; }
        public string TIPOSOLICITUD { get; set; } = string.Empty;

    }
}
