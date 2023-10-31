using Repository.Helpers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Models
{
    public class SaleDocumentModel
    {
        public decimal Code { get; set; }
        public int? Numeration { get; set; } //pendiente revisar, si se usa?
        public string? CarteNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal NumberSold { get; set; }
        public string BusinessSale { get; set; } = string.Empty;
        public decimal TypeCarte { get; set; }
        public decimal? TypeDocumentSeller { get; set; }
        public string? DocumentNumberSeller { get; set; }
        public string? ReasonSocial { get; set; }
        public decimal InitialBalanceBusiness { get; set; }
        public decimal FinalBalanceBusiness { get; set; }
        public string Observations { get; set; } = string.Empty;
        public string BusinessShopper { get; set; } = string.Empty;
        public decimal InventoryAvailability { get; set; }
        public decimal? TypeDocumentShopper { get; set; }
        public string? DocumentNumberShopper { get; set; }
        public string? ReasonSocialShopper { get; set; }
        public decimal InitialBalanceBusinessShopper { get; set; }
        public decimal FinalBalanceBusinessShopper { get; set; }
        public string ObservationsShopper { get; set; } = string.Empty;
        public int? Quota { get; set; }
        public int? Solds { get; set; }
        public decimal? QuotasSold { get; set; }
        public string? NitCompanySeller { get; set; }
        public string? NitCompanyShopper { get; set; }
        public decimal CompanySellerCode { get; set; }
        public decimal CompanyShopperCode { get; set; }
        public DateTime RegistrationDateCarteSale { get; set; }
        public List<SupportDocuments> SupportDocuments { get; set; } = new List<SupportDocuments>();
        public List<SupportDocuments>? SupportDocumentsRemoved { get; set; }
        public List<Quota>? Quotas { get; set; }
        public List<Inventory>? QuotasInventory { get; set; }
        public string? TypeSpecimenSeller { get; set; }
        public string? TypeSpecimenShopper { get; set; }
    }

    public class Seal
    {
        public decimal numeracionInicial { get; set; }
        public decimal numeracionFinal { get; set; }
        public int? codigoCupo { get; set; }
    }

    public class numbersSeals
    {
        public int initial { get; set; }
        public int final { get; set; }
        public int initialRep { get; set; }
        public int finalRep { get; set; }
    }

    public class InfoSalvoConducto
    {
        public string AutoridadAmbiental { get; set; } = string.Empty;
        public DateTime? FechaExpedicion { get; set; }
        public string DepartamentoOrigen { get; set; } = string.Empty;
        public string MunicipioOrigen { get; set; } = string.Empty;
        public string DepartamentoDestino { get; set; } = string.Empty;
        public string MunicipioDestino { get; set; } = string.Empty;
        public string TipoSalvoconducto { get; set; } = string.Empty;
        public int NumeracionInterna { get; set; }
        public DateTime? VigenciaDesde { get; set; }
        public DateTime? VigenciaHasta { get; set; }
        public string NombreTitular { get; set; } = string.Empty;
        public string IdentificacionTitular { get; set; } = string.Empty;
        public string MunicipioDomicilioTitular { get; set; } = string.Empty;
        public string ClaseRecurso { get; set; } = string.Empty;
        public List<InfoEspecies> InfoEspecies { get; set; } = new List<InfoEspecies>();
        public List<InfoRuta> InfoRuta { get; set; } = new List<InfoRuta>();
        public List<InfoTransporte> InfoTransporte { get; set; } = new List<InfoTransporte>();
    }

    public class InfoEspecies
    {
        public string NombreCientifico { get; set; } = string.Empty;
        public string NombreComun { get; set; } = string.Empty;
        public string TipoProducto { get; set; } = string.Empty;
        public string UnidadMedida { get; set; } = string.Empty;
        public decimal CantidadDisponible { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }

    public class InfoRuta
    {
        public string Departamento { get; set; } = string.Empty;
        public string Municipio { get; set; } = string.Empty;
    }

    public class InfoTransporte
    {
        public string MedioTransporte { get; set; } = string.Empty;
        public string TipoTransporte { get; set; } = string.Empty;
        public string IdentificacionTransporte { get; set; } = string.Empty;
        public string NombreTransportador { get; set; } = string.Empty;
        public string IdentificacionTransportador { get; set; } = string.Empty;
    }

}
