using AutoFixture;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Models;

namespace TestUnit.WEB.Models
{
	public class CvModelTest
	{
        private Fixture _fixture;

        [Fact]
		public void CvModel()
		{
			CvModel datos = new CvModel();
			datos.nombreEntidadhj = "string";
			datos.tipo_de_documento = "string";
			datos.nithj = 1;
			datos.telefonohj = "string";
			datos.paishj = "string";
			datos.ciudadhj = "string";
			datos.nombreEmpresahj = "string";
			datos.correohj = "string";
			datos.departamentohj = "string";
			datos.direccionhj = "string";
			datos.id = 1;
			datos.nitsitu = "string";
			datos.nombresitu = "string";
			datos.empresasitu = "string";
			datos.estadositu = "string";
			datos.observacionesitu = "string";
			datos.nombreTipoNovedad = "string";
			datos.ultimomodificado = "string";
			datos.codigoCupo = 1;
			datos.numeroResolucion = 1;
			datos.autoridadEmiteResolucion = "string";
			datos.fechaResolucion = DateTime.Now;
			datos.fechaRadicado = DateTime.Now;
			datos.cuposOtorgados = 1;
			datos.cuposPorAnio = 1;
			datos.fechaProduccion = DateTime.Now;
			datos.cuposAprovechamientoComercializacion = 1;
			datos.cuotaRepoblacion = "string";
			datos.cuposDisponibles = 1;
			datos.anioProduccion = 1;
			datos.observaciones = "string";
			datos.nitEmpresa = "string";
			datos.cuposTotal = 1;
			datos.codigoEspecie = "string";
			datos.resootorgadas = "string";
			datos.codigoCertificado = 1;
			datos.numeroCertificacion = "string";
			datos.fechaCertificacion = DateTime.Now;
			datos.nit = 1;
			datos.vigenciaCertificacion = DateTime.Now;
			datos.totalcertificados = 1;
			datos.codigoResolucion = 1;
			datos.codigoEmpresa = 1;
			datos.numeroResolucion2 = 1;
			datos.fechaResolucion2 = DateTime.Now;
			datos.fechaInicio = DateTime.Now;
			datos.fechaFin = DateTime.Now;
			datos.adjunto = null;
			datos.objetoResolucion = "string";
			datos.totalresolucionpeces = "string";
			datos.numeracion = 1;
			datos.numerodocumentoventa = "string";
			datos.quienvendedocumento = "string";
			datos.quienvenderazon = "string";
			datos.quiencompra = "string";
			datos.quiencomprarazon = "string";
			datos.fechaventa = DateTime.Now;
			datos.ncuposventa = "string";
			datos.totaldocumentos = "string";
			datos.factura = 1;
			datos.quienvenderazon2 = "string";
			datos.fechaventa2 = DateTime.Now;
			datos.disponibilidainventario = "string";
			datos.año = null;
			datos.inventariodispo = "string";
			datos.Code = 1;
			datos.Numeration = 1;
			datos.CarteNumber = "string";
			datos.SaleDate = DateTime.Now;
			datos.NumberSold = 1;
			datos.BusinessSale = "string";
			datos.TypeCarte = 1;
			datos.TypeDocumentSeller = 1;
			datos.DocumentNumberSeller = null;
			datos.ReasonSocial = "string";
			datos.InitialBalanceBusiness = 1;
			datos.FinalBalanceBusiness = 1;
			datos.Observations = "string";
			datos.BusinessShopper = "string";
			datos.InventoryAvailability = 1;
			datos.TypeDocumentShopper = 1;
			datos.DocumentNumberShopper = null;
			datos.ReasonSocialShopper = "string";
			datos.InitialBalanceBusinessShopper = 1;
			datos.FinalBalanceBusinessShopper = 1;
			datos.ObservationsShopper = "string";
			datos.Quota = 1;
			datos.Solds = 1;
			datos.QuotasSold = 1;
			datos.NitCompanySeller = "string";
			datos.NitCompanyShopper = "string";
			datos.CompanySellerCode = 1;
			datos.CompanyShopperCode = 1;
			datos.documentosSoporte = null;
			datos.SupportDocumentsRemoved = null;
			datos.codigo = 1;
			datos.idTipoNovedad = 1;
			datos.fechaRegistroNovedad = DateTime.Now;
			datos.idEstado = 1;
			datos.idEstadoEmisionCITES = 1;
			datos.estadoEmisionCITES = "string";
			datos.saldoProduccionDisponible = 1;
			datos.inventarioDisponible = 1;
			datos.numeroCupospendientesportramitar = 1;
			datos.idDisposicionEspecimen = 1;
			datos.idEmpresaZoo = 1;
			datos.NitEmpresaZoo = 1;
			datos.otroCual = "string";
			datos.observacionesDetalle = "string";

			var type = Assert.IsType<CvModel>(datos);
			Assert.NotNull(type);
		}

		[Fact]
		public void soportsDocuments()
		{
			soportsDocuments datos = new soportsDocuments();
			datos.codigo = 1;
			datos.adjuntoBase64 = "string";
			datos.nombreAdjunto = "string";
			datos.tipoAdjunto = "string";

			var type = Assert.IsType<soportsDocuments>(datos);
			Assert.NotNull(type);
		}
	}
}
