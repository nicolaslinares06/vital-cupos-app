namespace Web.Models
{
    public class GestionParametrica
    {
        public decimal pkT008codigo { get; set; }
        public decimal a008codigoUsuarioCreacion { get; set; }
        public decimal? a008CodigoUsuarioModificacion { get; set; }
        public decimal? a008EstadoRegistro { get; set; }
        public DateTime a008FechaCreacion { get; set; }
        public DateTime? a008FechaModificacion { get; set; }
        public string? a008Modulo { get; set; }
        public string? a008Parametrica { get; set; }
        public string? a008Valor { get; set; }
        public string? a008Descripcion { get; set; }
    }

    public class ValorParametro
    {
        public string? a008Parametrica { get; set; }
    }
}
