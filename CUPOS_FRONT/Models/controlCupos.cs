
using Newtonsoft.Json;
using Web.Models;

namespace Web.Models
{
    public class Cupos
    {
        public decimal? codigoCupo { get; set; }
        public string autoridadEmiteResolucion { get; set; }
        public string? codigoZoocriadero { get; set; }
        public string? numeroResolucion { get; set; }
        public DateTime? fechaResolucion { get; set; }
        public DateTime? fechaRegistroResolucion { get; set; }
        public DateTime? fechaRadicado { get; set; }
        public decimal? cuposOtorgados { get; set; }
        public decimal? cuposPorAnio { get; set; }
        public decimal? cuposTotal { get; set; }
        public DateTime fechaProduccion { get; set; }
        public decimal? cuposAprovechamientoComercializacion { get; set; }
        public string? cuotaRepoblacion { get; set; }
        public decimal? cuposDisponibles { get; set; }
        public decimal? anioProduccion { get; set; }
        public string? observaciones { get; set; }
        public string? nitEmpresa { get; set; }
        public string? codigoEspecie { get; set; }
        public decimal? numeroInternoFinalCuotaRepoblacion { get; set; }
        public string? numeroInternoFinal { get; set; }
        public decimal? numeroInternoInicialCuotaRepoblacion { get; set; }
        public string? NumeroInternoInicial { get; set; }
        public List<cupoPorAnios>? cuposPorAnioProduccion { get; set; }
        public decimal? cuposUtilizados { get; set; }
        public decimal? codigoParametricaPagoCuotaDerepoblacion { get; set; }
        public string? tipoEspecimen { get; set; }
        public string? nombreEmnpresa { get; set; }
        public decimal? tipoEmpresa { get; set; }
        public bool? soporteRepoblacion { get; set; }

    }

    public class InventarioCupos
    {
        public decimal? especie { get; set; }
        public decimal codigo { get; set; }
        public string? numeroCartaVendeFactura { get; set; }
        public string quienVende { get; set; }
        public DateTime fechaVenta { get; set; }
        public int disponibilidadInventario { get; set; }
        public DateTime fechaProduccion { get; set; }
        public int anio { get; set; }
        public int inventarioDisponible { get; set; }
    }

    public class DatosEntidad
    {
        public decimal codigoEmpresa { get; set; }
        public string nombreEntidad { get; set; }
        public string nombreEmpresa { get; set; }
        public decimal nit { get; set; }
        public string tipoDocumento { get; set; }
        public decimal telefono { get; set; }
        public string correo { get; set; }
        public string ciudad { get; set; }
        public string departamento { get; set; }
        public string pais { get; set; }
        public string direccion { get; set; }
    }

    public class ElementTypes
    {
        public decimal? code { get; set; }
        public string? name { get; set; }
    }

    public class resolutionQuota
    {
        public cupoGuardar resolucionInfo { get; set; }
        public exportSpecimenss especiesExportar { get; set; }
    }

    public class soportDocuments
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

    public class exportSpecimenss
    {

        [JsonProperty("PkT005code")]
        public decimal? PkT005codigo { get; set; }

        [JsonProperty("quotaCode")]
        public decimal? codigoCupo { get; set; }

        [JsonProperty("year")]
        public decimal? anio { get; set; }

        [JsonProperty("quotas")]
        public decimal? cupos { get; set; }

        [JsonProperty("id")]
        public decimal? id { get; set; }

        [JsonProperty("markingTypeParametricCode")]
        public string? codigoParametricaTipoMarcaje { get; set; }

        [JsonProperty("speciesCode")]
        public string? codigoEspecie { get; set; }

        [JsonProperty("repopulationQuotaPaymentParametricCode")]
        public string? codigoParametricaPagoCuotaDerepoblacion { get; set; } = null!;

        [JsonProperty("filingDate")]
        public DateTime? fechaRadicado { get; set; }

        [JsonProperty("specimenType")]
        public string? tipoEspecimen { get; set; } = null!;

        [JsonProperty("productionYear")]
        public decimal? añoProduccion { get; set; }

        [JsonProperty("batchCode")]
        public string? marcaLote { get; set; } = null!;

        [JsonProperty("markingConditions")]
        public string? condicionesMarcaje { get; set; } = null!;

        [JsonProperty("parentalPopulationMale")]
        public decimal? poblacionParentalMacho { get; set; }

        [JsonProperty("parentalPopulationFemale")]
        public decimal? poblacionParentalHembra { get; set; }

        [JsonProperty("totalParentalPopulation")]
        public decimal? poblacionParentalTotal { get; set; }

        [JsonProperty("populationFromIncubator")]
        public decimal? poblacionSalioDeIncubadora { get; set; }

        [JsonProperty("populationAvailableForQuotas")]
        public decimal? poblacionDisponibleParaCupos { get; set; }

        [JsonProperty("individualsForRepopulation")]
        public string? individuosDestinadosARepoblacion { get; set; } = null!;

        [JsonProperty("grantedUtilizationQuotas")]
        public decimal? cupoAprovechamientoOtorgados { get; set; } = null!;

        [JsonProperty("replacementRate")]
        public string? tasaReposicion { get; set; } = null!;

        [JsonProperty("mortalityNumber")]
        public decimal? numeroMortalidad { get; set; }

        [JsonProperty("repopulationQuotaForUtilization")]
        public bool? cuotaRepoblacionParaAprovechamiento { get; set; }

        [JsonProperty("grantedPaidUtilizationQuotas")]
        public string? cupoAprovechamientoOtorgadosPagados { get; set; } = null!;

        [JsonProperty("observations")]
        public string? observaciones { get; set; } = null!;

        [JsonProperty("repopulationQuota")]
        public string? cuotaRepoblacion { get; set; }

        [JsonProperty("initialRepopulationQuotaInternalNumber")]
        public string? numeroInternoInicialCuotaRepoblacion { get; set; }

        [JsonProperty("finalRepopulationQuotaInternalNumber")]
        public string? numeroInternoFinalCuotaRepoblacion { get; set; }

        [JsonProperty("initialInternalNumber")]
        public decimal? numeroInternoInicial { get; set; }

        [JsonProperty("finalInternalNumber")]
        public decimal? numeroInternoFinal { get; set; }

        [JsonProperty("totalQuotas")]
        public decimal? totalCupos { get; set; }

        [JsonProperty("availableQuotas")]
        public decimal? cuposDisponibles { get; set; }

        [JsonProperty("repopulationQuotaPaid")]
        public bool? sePagoCuotaRepoblacion { get; set; }

        [JsonProperty("speciesRegisteredForCommercialization")]
        public bool? seRegistroEspecieComercializar { get; set; }

        [JsonProperty("supportingDocuments")]
        public List<soportDocuments>? documentosSoporte { get; set; }

        [JsonProperty("newSupportingDocuments")]
        public List<soportDocuments>? documentosSoporteNuevos { get; set; }

        [JsonProperty("deletedSupportingDocuments")]
        public List<soportDocuments>? documentosSoporteEliminar { get; set; }
    }

    public class saveResolutionQuotas
    {
        [JsonProperty("dataToSave")]
        public cupoGuardar datoGuardar { get; set; }

        [JsonProperty("newExportSpeciesData")]
        public List<exportSpecimenss>? datosEspecieExportarNuevo { get; set; }
    }

    public class cupoPorAnios
    {
        public decimal? anio { get; set; }
        public decimal? cupos { get; set; }
    }

    public class cupoGuardar
    {
        [JsonProperty("quotaCode")]
        public decimal? codigoCupo { get; set; }

        [JsonProperty("issuingAuthority")]
        public string? autoridadEmiteResolucion { get; set; }

        [JsonProperty("zoocriaderoCode")]
        public string? codigoZoocriadero { get; set; }

        [JsonProperty("resolutionNumber")]
        public decimal? numeroResolucion { get; set; }

        [JsonProperty("resolutionDate")]
        public DateTime? fechaResolucion { get; set; }

        [JsonProperty("resolutionRegistrationDate")]
        public DateTime? fechaRegistroResolucion { get; set; }

        [JsonProperty("observations")]
        public string? observaciones { get; set; }

        [JsonProperty("companyNit")]
        public decimal? nitEmpresa { get; set; }
    }

    public class cupoTotales
    {
        public decimal? availableQuotas { get; set; }
        public decimal? availableInventory { get; set; }
        public decimal? pendingQuotasForProcessing { get; set; }
    }
}
