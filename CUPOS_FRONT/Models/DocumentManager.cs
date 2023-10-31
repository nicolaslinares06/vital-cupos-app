
using System.Net;
using Web.Models;

namespace Repository.Helpers
{
    public class DocumentManager
    {
        public class Archivo
        {
            public decimal? codigo { get; set; }
            public string? adjuntoBase64 { get; set; }
            public string? nombreAdjunto { get; set; }
            public string? tipoAdjunto { get; set; }
            public string? urlFTP { get; set; }
        }
    }
}
