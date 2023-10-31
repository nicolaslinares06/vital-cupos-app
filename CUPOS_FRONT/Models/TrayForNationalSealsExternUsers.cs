using Newtonsoft.Json;

namespace Web.Models
{
    public class TrayForNationalSealsExternUsers
    {
        public class Request
        {
            [JsonProperty("companyCode")]
            public decimal codigoEmpresa { get; set; }

            [JsonProperty("requestCode")]
            public decimal? codigoSolicitud { get; set; }

            [JsonProperty("date")]
            public DateTime fecha { get; set; }

            [JsonProperty("representativeCity")]
            public decimal ciudadRepresentante { get; set; }

            [JsonProperty("deliveryAddress")]
            public string direccionEntrega { get; set; } = string.Empty;

            [JsonProperty("quantity")]
            public decimal cantidad { get; set; }

            [JsonProperty("specimens")]
            public decimal? especimenes { get; set; }

            [JsonProperty("initialSkinCode")]
            public decimal codigoInicialPieles { get; set; }

            [JsonProperty("finalSkinCode")]
            public decimal codigoFinalPieles { get; set; }

            [JsonProperty("minorLength")]
            public decimal longitudMenor { get; set; }

            [JsonProperty("majorLength")]
            public decimal longitudMayor { get; set; }

            [JsonProperty("representativeDate")]
            public DateTime fechaRepresentante { get; set; }

            [JsonProperty("observations")]
            public string? observaciones { get; set; }

            [JsonProperty("response")]
            public string? respuesta { get; set; }

            [JsonProperty("requestStatus")]
            public string? estadoSolicitud { get; set; }

            [JsonProperty("registrationDate")]
            public DateTime? fechaRadicado { get; set; }

            [JsonProperty("requestType")]
            public decimal? tipoSolicitud { get; set; }

            [JsonProperty("statusChangeDate")]
            public DateTime? fechaCambioEstado { get; set; }

            [JsonProperty("withdrawalObservations")]
            public string? observacionesDesistimiento { get; set; }

            [JsonProperty("numerations")]
            public List<numeracion>? numeraciones { get; set; }

            [JsonProperty("invoiceAttachment")]
            public soportsDocuments facturaAdjunto { get; set; } = new soportsDocuments();

            [JsonProperty("letterAttachment")]
            public soportsDocuments? adjuntoCarta { get; set; }

            [JsonProperty("attachedAnnexes")]
            public List<soportsDocuments>? AnexosAdjuntos { get; set; }

            [JsonProperty("attachedAnnexesToDelete")]
            public List<soportsDocuments>? anexosAdjuntosEliminar { get; set; }

            [JsonProperty("responseAttachments")]
            public List<soportsDocuments>? adjuntosRespuesta { get; set; }

            [JsonProperty("responseAttachmentsToDelete")]
            public List<soportsDocuments>? adjuntosRespuestaEliminar { get; set; }

            [JsonProperty("representativeDepartment")]
            public decimal departamentoRepresentante { get; set; }
            public List<safeGuardNumbersModel>? safeGuardNumbers { get; set; }
            public List<cuttingSaveModel>? cuttingSave { get; set; }
        }

        public class safeGuardNumbersModel
        {
            public int id { get; set; }
            public int idCutting { get; set; }
            public string number { get; set; } = string.Empty;
        }

        public class cuttingSaveModel
        {
            public int id { get; set; }
            public int idCutting { get; set; }
            public string? fractionType { get; set; }
            public int amountSelected { get; set; }
            public int totalAreaSelected { get; set; }
        }

        public class numeracion
        {
            [JsonProperty("code")]
            public decimal? codigo { get; set; }

            [JsonProperty("initial")]
            public decimal inicial { get; set; }

            [JsonProperty("final")]
            public decimal final { get; set; }

            [JsonProperty("origen")]
            public decimal? origen { get; set; }
        }
        public class cuttingSafeGuar
        {
            public List<decimal> salvoCon { get; set; } = new List<decimal>();
            public List<valCut> Cut { get; set; } = new List<valCut>();

        }
        public class valCut
        {
            public decimal? cantCut { get; set; }
            public decimal areaCut { get; set; }
            public string? tipoPart { get; set; }
        }
        public class RepresentaeLegalEmpresa
        {
            public decimal codigoEmpresa { get; set; }
            public decimal ciudad { get; set; }
            public string primerNombre { get; set; } = string.Empty;
            public string? segundoNombre { get; set; } = string.Empty;
            public string primerApellido { get; set; } = string.Empty;
            public string segundoApellido { get; set; } = string.Empty;
            public decimal tipoIdentificacion { get; set; }
            public string numeroIdentificacion { get; set; } = string.Empty;
            public string telefono { get; set; } = string.Empty;
            public string? fax { get; set; }
        }

        public class solicitudes
        {
            [JsonProperty("code")]
            public decimal codigo { get; set; }

            [JsonProperty("filingNumber")]
            public string? numeroRadicado { get; set; }

            [JsonProperty("sealLabelRequest")]
            public string solicitudPrecintoMarquilla { get; set; } = string.Empty;

            [JsonProperty("requestingEntityName")]
            public string? nombreEntidadSolicitante { get; set; }

            [JsonProperty("requestDate")]
            public DateTime fechaSolicitud { get; set; }

            [JsonProperty("filingDate")]
            public DateTime? fechaRadicacion { get; set; }

            [JsonProperty("status")]
            public string estado { get; set; } = string.Empty;

            [JsonProperty("observations")]
            public string? observaciones { get; set; }

            [JsonProperty("outgoingFilingNumber")]
            public string? numeroRadicadoSalida { get; set; }

            [JsonProperty("outgoingFilingDate")]
            public DateTime? fechaRadicadoSalida { get; set; }
        }

        public class registerPending
        {
            public decimal codigoSolicitud { get; set; }
            public DateTime fechaRadicado { get; set; }
            public decimal numeroRadicado { get; set; }
        }

        public class EspecimenQuota
        {
            public decimal codigoCupo { get; set; }
            public string codigoEspecie { get; set; } = string.Empty;
            public decimal? cuposDisponibles { get; set; }
            public bool validacion { get; set; }
        }

        public class ConsultUnableNumbersModel
        {
            public int code { get; set; }
            public int nitEmpresa { get; set; }
            public bool cupo { get; set; }
            public bool inventario { get; set; }
        }

        public class validateNumbersModel
        {
            public decimal codeCompany { get; set; }
            public numbers numeros { get; set; } = new numbers();
            public decimal origen { get; set; }
        }
        public class numbers
        {
            public int initial { get; set; }
            public int final { get; set; }
        }

        public class cuttingReport
        {
            public decimal code { get; set; }
            public DateTime dateVisit { get; set; }
            public DateTime dateRegister { get; set; }
            public string visitNumber { get; set; } = string.Empty;
        }

        public class cutting
        {
            public decimal? code { get; set; }
            public string? fractionsType { get; set; }
            public decimal? amount { get; set; }
            public decimal? totalArea { get; set; }
        }

        public class Safeguard
        {
            public decimal? code { get; set; }
            public decimal codSafeguard { get; set; }

        }

    }

}
