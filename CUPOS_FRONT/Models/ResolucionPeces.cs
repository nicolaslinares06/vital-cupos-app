using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Models
{
    public class ResolucionPeces
    {
        public class ResolucionPermisos
        {
            [JsonProperty("resolutionCode")]
            public decimal? codigoResolucion { get; set; }

            [JsonProperty("companyCode")]
            public decimal? codigoEmpresa { get; set; }

            [JsonProperty("resolutionNumber")]
            public decimal? numeroResolucion { get; set; }

            [JsonProperty("resolutionDate")]
            public DateTime fechaResolucion { get; set; }

            [JsonProperty("startDate")]
            public DateTime fechaInicio { get; set; }

            [JsonProperty("endDate")]
            public DateTime fechaFin { get; set; }

            [JsonProperty("attachment")]
            public soportsDocuments adjunto { get; set; } = new soportsDocuments();

            [JsonProperty("resolutionObject")]
            public string? objetoResolucion { get; set; }
        }
    }
}
