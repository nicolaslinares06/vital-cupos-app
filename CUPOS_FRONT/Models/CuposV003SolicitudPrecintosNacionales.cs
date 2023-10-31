using System;
using System.Collections.Generic;

namespace Repository.Models
{
    public partial class CuposV003SolicitudPrecintosNacionales
    {
        public decimal ID { get; set; }
        public string? CIUDAD { get; set; }
        public string? DEPARTAMENTO { get; set; }
        public DateTime? FECHA { get; set; }
        public string? ESTABLECIMIENTO { get; set; }
        public string? PRIMERNOMBRE { get; set; }
        public string? SEGUNDONOMBRE { get; set; }
        public string? PRIMERAPELLIDO { get; set; }
        public string? SEGUNDOAPELLIDO { get; set; }
        public string? TIPOIDENTIFICACION { get; set; }
        public decimal? NUMEROIDENTIFICACION { get; set; }
        public string? DIRECCIONENTREGA { get; set; }
        public string? CIUDADENTREGA { get; set; }
        public decimal? TELEFONO { get; set; }
        public string? CORREOELECTRONICO { get; set; }
        public decimal? CANTIDAD { get; set; }
        public string? ESPECIE { get; set; }
        public decimal? CODIGOINICIAL { get; set; }
        public decimal? CODIGOFINAL { get; set; }
        public decimal? LONGITUDMENOR { get; set; }
        public decimal? LONGITUDMAYOR { get; set; }
        public DateTime? FECHALEGAL { get; set; }
        public string? OBSERVACIONES { get; set; }
        public string? RESPUESTA { get; set; }
        public decimal? CUPOSDISPONIBLES { get; set; }
        public DateTime? FECHADESISTIMIENTO { get; set; }
        public string? NUMERORADICACION { get; set; }
        public DateTime? FECHARADICACION { get; set; }
        public DateTime? FECHAESTADO { get; set; }
        public string? OBSERVACIONESDESISTIMIENTO { get; set; }
        public string? ANALISTA { get; set; }
        public DateTime? FECHARESOLUCION { get; set; }
        public decimal? NUMERORESOLUCION { get; set; }
        public decimal? CODIGOESPECIE { get; set; }
        public string? NUMERORADICACIONSALIDA { get; set; }
        public DateTime? FECHARADICACIONSALIDA { get; set; }
        public decimal? VALORCONSIGNACION { get; set; }
        public string? TIPOSOLICITUD { get; set; }
        public decimal? CODIGONUMERACIONES { get; set; }
        public decimal? TYPEREQUESTCODE { get; set; }
        public int? CODIGOCORTEPIELSOLICITUD { get; set; }
        public decimal? NITEMPRESA { get; set; }
        public string? CODIGOZOOCRIADERO { get; set; }
    }
}
