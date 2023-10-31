using Microsoft.AspNetCore.Mvc;
using Repository.Helpers.Models;
using System.Xml.Linq;

namespace Web.Models
{
    public class CvModel
    {
        public string nombreEntidadhj { get; set; } = string.Empty;
        public string tipo_de_documento { get; set; } = string.Empty;
        public int nithj { get; set; }
        public string? telefonohj { get; set; }
        public string? paishj { get; set; }
        public string? ciudadhj { get; set; }
        public string? nombreEmpresahj { get; set; }
        public string? correohj { get; set; }
        public string? departamentohj { get; set; }
        public string? direccionhj { get; set; }

        public int id { get; set; }
        public string nitsitu { get; set; } = string.Empty;
        public string nombresitu { get; set; } = string.Empty;

        public string empresasitu { get; set; } = string.Empty;
        public string estadositu { get; set; } = string.Empty;
        public string observacionesitu { get; set; } = string.Empty;
        public string nombreTipoNovedad { get; set; } = string.Empty;
        public string ultimomodificado { get; set; } = string.Empty;

        






        public decimal? codigoCupo { get; set; }
        public decimal? numeroResolucion { get; set; }
        public string autoridadEmiteResolucion { get; set; } = string.Empty;
        public DateTime? fechaResolucion { get; set; }
        public DateTime? fechaRadicado { get; set; }
        public decimal? cuposOtorgados { get; set; }
        public decimal? cuposPorAnio { get; set; }
        public DateTime fechaProduccion { get; set; }
        public decimal? cuposAprovechamientoComercializacion { get; set; }
        public string? cuotaRepoblacion { get; set; }
        public decimal? cuposDisponibles { get; set; }
        public decimal? anioProduccion { get; set; }
        public string? observaciones { get; set; }
        public string? nitEmpresa { get; set; }
        public decimal? cuposTotal { get; set; }
        public string? codigoEspecie { get; set; }
        public string? resootorgadas { get; set; } 

        public decimal? codigoCertificado { get; set; }
        public string? numeroCertificacion { get; set; }
        public DateTime? fechaCertificacion { get; set; }
        public decimal? nit { get; set; }
        public DateTime? vigenciaCertificacion { get; set; }

        public int? totalcertificados { get; set; }



            public decimal? codigoResolucion { get; set; }
            public decimal? codigoEmpresa { get; set; }
            public decimal? numeroResolucion2 { get; set; }
            public DateTime fechaResolucion2 { get; set; }
            public DateTime fechaInicio { get; set; }
            public DateTime fechaFin { get; set; }
            public soportsDocuments? adjunto { get; set; }
            public string? objetoResolucion { get; set; }
            public string? totalresolucionpeces { get; set; }


        public decimal numeracion { get; set; }
        public string? numerodocumentoventa { get; set; }
        public string? quienvendedocumento { get; set; }
        public string? quienvenderazon { get; set; }
        public string? quiencompra { get; set; }
        public string? quiencomprarazon { get; set; }
        public DateTime? fechaventa { get; set; }
        public string? ncuposventa { get; set; }
        public string? totaldocumentos { get; set; }


        public decimal factura { get; set; }
        public string? quienvenderazon2 { get; set; }
        public DateTime? fechaventa2 { get; set; }
        public string? disponibilidainventario { get; set; }
        public DateTime? año { get; set; }
        public string? inventariodispo { get; set; }

        //consulta documento

        public decimal Code { get; set; }
        public int? Numeration { get; set; } //pendiente revisar, si se usa?
        public string? CarteNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal NumberSold { get; set; }
        public string BusinessSale { get; set; } = string.Empty;
        public decimal TypeCarte { get; set; }
        public int? TypeDocumentSeller { get; set; }
        public string? DocumentNumberSeller { get; set; }
        public string? ReasonSocial { get; set; }
        public decimal InitialBalanceBusiness { get; set; }
        public decimal FinalBalanceBusiness { get; set; }
        public string Observations { get; set; } = string.Empty;
        public string BusinessShopper { get; set; } = string.Empty;
        public decimal InventoryAvailability { get; set; }
        public int? TypeDocumentShopper { get; set; }
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
        public List<soportsDocuments>? documentosSoporte { get; set; }
        public List<SupportDocuments>? SupportDocumentsRemoved { get; set; }

      
        public class soportsDocuments
        {
            public decimal? codigo { get; set; }
            public string? adjuntoBase64 { get; set; }
            public string? nombreAdjunto { get; set; }
            public string? tipoAdjunto { get; set; }
        }

        public decimal codigo { get; set; }
        
        public decimal idTipoNovedad { get; set; }
        
        public DateTime fechaRegistroNovedad { get; set; }
        public decimal idEstado { get; set; }
        
        public decimal? idEstadoEmisionCITES { get; set; }
        public string? estadoEmisionCITES { get; set; }
        
        public decimal? saldoProduccionDisponible { get; set; }
        
        public decimal? inventarioDisponible { get; set; }
        public decimal? numeroCupospendientesportramitar { get; set; }
        public decimal? idDisposicionEspecimen { get; set; }
        public decimal? idEmpresaZoo { get; set; }
        public decimal? NitEmpresaZoo { get; set; }
        public string? otroCual { get; set; }
        public string? observacionesDetalle { get; set; }
    }


    public class CVDEnityData
    {
        public string nombreEntidadhj { get; set; } = string.Empty;
        public string tipo_de_documento { get; set; } = string.Empty;
        public int nithj { get; set; }
        public string? telefonohj { get; set; }
        public string? paishj { get; set; }
        public string? ciudadhj { get; set; }
        public string? nombreEmpresahj { get; set; }
        public string? correohj { get; set; }
        public string? departamentohj { get; set; }
        public string? direccionhj { get; set; }

    }

    

   

}
