using AutoFixture;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Org.BouncyCastle.Crypto;
using Repository.Helpers.Models;
using System.Security.Claims;
using Web.Controllers;
using Web.Models;
using WebFront.Controllers;
using static CUPOS_FRONT.Models.Requests;

namespace TestUnit.WEB
{
	public class RegistrarResolucionControllerTest
	{
		//Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
		private RegistrarResolucionController controller;
        private Fixture _fixture;
        private readonly string token;

        public RegistrarResolucionControllerTest()
		{
			controller = new RegistrarResolucionController(new LoggerFactory().CreateLogger<RegistrarResolucionController>());
            _fixture = new Fixture();

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "Administrador")
        };
            var identity = new ClaimsIdentity(claims, "someAuthTypeName");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = claimsPrincipal,
                Session = new FakeSession()
            };

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            token = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Development.json").Build().GetValue<string>("Variables:Token");

        }

        [Fact]
		public void RegistrarResolucion()
		{
			decimal documentType = 1; 
			string nitBussines = _fixture.Create<string>();

			var r = controller.RegistrarResolucion(documentType, nitBussines);
			Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.RegistrarResolucion(documentType, nitBussines);
            Assert.True(r != null);
        }

        [Fact]
		public void ConsultEntityDates()
		{
			decimal documentType = 95;
			string nitBussines = "897564231";
			decimal? entityType = 1;

			var r = controller.ConsultEntityDates(documentType, nitBussines, entityType);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultEntityDates(documentType, nitBussines, entityType);
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultQuotas()
		{
			string nitBussines = "897564231";

			var r = controller.ConsultQuotas(nitBussines);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultQuotas(nitBussines);
            Assert.True(r != null);
        }

		[Fact]
		public void SearchQuotas()
		{
			decimal ResolutionNumbre  = 1;

			var r = controller.SearchQuotas(ResolutionNumbre);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.SearchQuotas(ResolutionNumbre);
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultInventory()
		{
			var r = controller.ConsultInventory();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultInventory();
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultMarkingType()
		{
			var r = controller.ConsultMarkingType();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultMarkingType();
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultEntityTypes()
		{
			var r = controller.ConsultEntityTypes();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultEntityTypes();
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultRepoblationPay()
		{
			var r = controller.ConsultRepoblationPay();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultRepoblationPay();
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultEspecimensTypes()
		{
			var r = controller.ConsultEspecimensTypes();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultEspecimensTypes();
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultOneQuota()
		{
			decimal quotaCode = 1;

			var r = controller.ConsultOneQuota(quotaCode);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultOneQuota(quotaCode);
            Assert.True(r != null);
        }

		[Fact]
		public void EditaEliminarResolucionEspecieExportar()
		{
            cupoGuardar cupo = new cupoGuardar();
            cupo.codigoCupo = _fixture.Create<decimal?>();
            cupo.autoridadEmiteResolucion = _fixture.Create<string>();
            cupo.codigoZoocriadero = _fixture.Create<string>();
            cupo.numeroResolucion = _fixture.Create<decimal?>();
            cupo.fechaResolucion = _fixture.Create<DateTime?>();
            cupo.fechaRegistroResolucion = _fixture.Create<DateTime?>();
            cupo.observaciones = _fixture.Create<string>();
            cupo.nitEmpresa = _fixture.Create<decimal?>();

            List<soportDocuments> listdoc = new List<soportDocuments>();

            // Crear una instancia de soportDocuments y asignar valores aleatorios
            soportDocuments doc = new soportDocuments();
            doc.codigo = _fixture.Create<decimal?>();
            doc.adjuntoBase64 = _fixture.Create<string>();
            doc.nombreAdjunto = _fixture.Create<string>();
            doc.tipoAdjunto = _fixture.Create<string>();

            listdoc.Add(doc);

            List<exportSpecimenss> listexp = new List<exportSpecimenss>();

            // Crear una instancia de exportSpecimenss y asignar valores aleatorios
            exportSpecimenss exp = new exportSpecimenss();
            exp.PkT005codigo = _fixture.Create<decimal?>();
            exp.codigoCupo = _fixture.Create<decimal?>();
            exp.anio = _fixture.Create<decimal?>();
            exp.cupos = _fixture.Create<decimal?>();
            exp.id = _fixture.Create<decimal?>();
            exp.codigoParametricaTipoMarcaje = _fixture.Create<string>();
            exp.codigoEspecie = _fixture.Create<string>();
            exp.codigoParametricaPagoCuotaDerepoblacion = _fixture.Create<string>();
            exp.fechaRadicado = _fixture.Create<DateTime?>();
            exp.tipoEspecimen = _fixture.Create<string>();
            exp.añoProduccion = _fixture.Create<decimal?>();
            exp.marcaLote = _fixture.Create<string>();
            exp.condicionesMarcaje = _fixture.Create<string>();
            exp.poblacionParentalMacho = _fixture.Create<decimal?>();
            exp.poblacionParentalHembra = _fixture.Create<decimal?>();
            exp.poblacionParentalTotal = _fixture.Create<decimal?>();
            exp.poblacionSalioDeIncubadora = _fixture.Create<decimal?>();
            exp.poblacionDisponibleParaCupos = _fixture.Create<decimal?>();
            exp.individuosDestinadosARepoblacion = _fixture.Create<string>();
            exp.cupoAprovechamientoOtorgados = _fixture.Create<decimal?>();
            exp.tasaReposicion = _fixture.Create<string>();
            exp.numeroMortalidad = _fixture.Create<decimal?>();
            exp.cuotaRepoblacionParaAprovechamiento = _fixture.Create<bool?>();
            exp.cupoAprovechamientoOtorgadosPagados = _fixture.Create<string>();
            exp.observaciones = _fixture.Create<string>();
            exp.cuotaRepoblacion = _fixture.Create<string>();
            exp.numeroInternoInicialCuotaRepoblacion = _fixture.Create<string>();
            exp.numeroInternoFinalCuotaRepoblacion = _fixture.Create<string>();
            exp.numeroInternoInicial = _fixture.Create<decimal?>();
            exp.numeroInternoFinal = _fixture.Create<decimal?>();
            exp.totalCupos = _fixture.Create<decimal?>();
            exp.cuposDisponibles = _fixture.Create<decimal?>();
            exp.sePagoCuotaRepoblacion = _fixture.Create<bool?>();
            exp.seRegistroEspecieComercializar = _fixture.Create<bool?>();
            exp.documentosSoporte = listdoc;
            exp.documentosSoporteNuevos = listdoc;
            exp.documentosSoporteEliminar = listdoc;

            listexp.Add(exp);


            saveResolutionQuotas datos = new saveResolutionQuotas();
            datos.datoGuardar = cupo;
            datos.datosEspecieExportarNuevo = listexp;

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            var r = controller.EditaEliminarResolucionEspecieExportar(datos);
            Assert.True(r != null);
        }

		[Fact]
		public void GuardarResolucionEspecieExportar()
		{
			saveResolutionQuotas datos = new saveResolutionQuotas();
			datos.datoGuardar = null;
			datos.datoGuardar = null;

			var r = controller.GuardarResolucionEspecieExportar(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.GuardarResolucionEspecieExportar(datos);
            Assert.True(r != null);
        }

		[Fact]
		public void DisableResolution()
		{
			decimal quotaCode = 1;

			var r = controller.DisableResolution(quotaCode);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.DisableResolution(quotaCode);
            Assert.True(r != null);
        }
	}
}
