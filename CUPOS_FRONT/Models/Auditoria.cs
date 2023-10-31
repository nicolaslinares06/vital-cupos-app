namespace Web.Models
{
    public class Auditoria
    {
        public string nombre { get; set; }
        public string accion { get; set; }
        public DateTime fecha { get; set; }
        public string subModulo { get; set; }
        public string ip { get; set; }
        public string rol { get; set; }
        public string? a013camposModificados { get; set; }
        public string? a013estadoAnterior { get; set; }
        public string? a013estadoActual { get; set; }
        public string? a013registroModificado { get; set; }
    }
}
