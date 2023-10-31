namespace Web.Models
{
    public class PrecintosMarquillas
    {
        public int pkT006Codigo { get; set; }
        public int a006CodigoPrecintoymarquilla { get; set; }
        public int a006CodigoEspecieExportar { get; set; }
        public int a006CodigoUsuarioCreacion { get; set; }
        public int a006CodigoUsuarioModificacion { get; set; }
        public int a006CodigoParametricaTipoPrecintomarquilla { get; set; }
        public int a006codigoParametricaColorPrecintosymarquillas { get; set; }
        public string? a006EstadoRegistro { get; set; }
        public DateTime a006FechaCreacion { get; set; }
        public DateTime a006FechaModificacion { get; set; }
        public string? a006Observacion { get; set; }
    }

    public class TiposDocumentosEmpresas
    {
        public int pkT008Codigo { get; set; }
        public string? a008Parametrica { get; set; }
        public string? a008Valor { get; set; }
    }

    public class Colores
    {
        public int pkT008Codigo { get; set; }
        public string? a008Parametrica { get; set; }
        public string? a008Valor { get; set; }
    }

    public class FiltrosPrecintosMarquillas
    {
        public string documentType { get; set; } = string.Empty;
        public DateTime initialDate { get; set; }
        public string number { get; set; } = string.Empty;
        public string documentNumber { get; set; } = string.Empty;
        public DateTime finalDate { get; set; }
        public string color { get; set; } = string.Empty;
        public string companyName { get; set; } = string.Empty;
        public string validity { get; set; } = string.Empty;

    }
    
}
