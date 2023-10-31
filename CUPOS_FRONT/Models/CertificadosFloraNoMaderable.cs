using Newtonsoft.Json;

namespace Web.Models
{
    public class CertificadosFloraNoMaderable
    {
        public decimal? codigoCertificado { get; set; } 
        public string? numeroCertificacion { get; set; }
        public DateTime? fechaCertificacion { get; set; }
        public DateTime? fechaRegistroCertificacion { get; set; }
        public decimal? nit { get; set; }
        public DateTime? vigenciaCertificacion { get; set; }
    }

    public class ElementTypesEspecies
    {
        public decimal? id { get; set; }
        public string? text { get; set; }
        public string? NameFamily { get; set; }
        public string? CommonName { get; set; }
    }

    public class CertificatesDatas
    {
        [JsonProperty("code")]
        public decimal? codigo { get; set; }

        [JsonProperty("issuingAuthority")]
        public string? autoridadEmiteCertificacion { get; set; }

        [JsonProperty("certificateNumber")]
        public string? numeroCertificado { get; set; }

        [JsonProperty("certificationDate")]
        public DateTime? fechaCertificacion { get; set; }

        [JsonProperty("certificationValidity")]
        public DateTime? vigenciaCertificacion { get; set; }

        [JsonProperty("permissionType")]
        public string? tipoPermiso { get; set; }

        [JsonProperty("specimenProductImpExpType")]
        public string tipoEspecimenProductoImpExp { get; set; }

        [JsonProperty("certificateRemarks")]
        public string? observacionesCertificado { get; set; }

        [JsonProperty("companyNit")]
        public decimal? nitEmpresa { get; set; }

        [JsonProperty("supportingDocuments")]
        public List<soportsDocuments>? documentosSoporte { get; set; }

        [JsonProperty("newSupportingDocuments")]
        public List<soportsDocuments>? documentosSoporteNuevo { get; set; }

        [JsonProperty("deletedSupportingDocuments")]
        public List<soportsDocuments>? documentosSoporteEliminar { get; set; }
    }


    public class soportsDocuments
    {
        [JsonProperty("code")]
        public decimal? codigo { get; set; }

        [JsonProperty("base64Attachment")]
        public string? adjuntoBase64 { get; set; }

        [JsonProperty("attachmentName")]
        public string? nombreAdjunto { get; set; }

        [JsonProperty("attachmentType")]
        public string? tipoAdjunto { get; set; }
    }

    public class ReqGuardarDoc
    {
        public decimal id { get; set; }
        public soportsDocuments archivo { get; set; }
    }
}
