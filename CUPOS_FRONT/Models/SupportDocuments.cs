using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Helpers.Models
{
    public class SupportDocuments
    {
        [JsonProperty("code")]
        public decimal? codigo { get; set; }

        [JsonProperty("base64Attachment")]
        public string? adjuntoBase64 { get; set; }

        [JsonProperty("attachmentName")]
        public string? nombreAdjunto { get; set; }

        [JsonProperty("attachmentType")]
        public string? tipoAdjunto { get; set; }
        public string? ActionTemp { get; set; }
    }
}
