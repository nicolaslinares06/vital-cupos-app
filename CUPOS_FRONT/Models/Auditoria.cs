namespace Web.Models
{
    public class Auditoria
    {
        public string nombre { get; set; }  = string.Empty;
        public string accion { get; set; } = string.Empty;
        public DateTime fecha { get; set; }
        public string subModulo { get; set; } = string.Empty;
        public string ip { get; set; } = string.Empty;
        public string rol { get; set; } = string.Empty;
        public string? a013camposModificados { get; set; }
        public string? a013estadoAnterior { get; set; }
        public string? a013estadoActual { get; set; }
        public string? a013registroModificado { get; set; }
    }
}
