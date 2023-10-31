using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Repository.Helpers.Models;
using System.Security.Claims;
using Web.Controllers;
using Web.Models;
using static CUPOS_FRONT.Models.Requests;
using static Web.Models.TrayForNationalSealsExternUsers;

namespace TestUnit.WEB
{
	public class TrayForNationalSealsExternUsersControllerTest
	{
		//Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
		private TrayForNationalSealsExternUsersController controller;
		private readonly Mock<TrayForNationalSealsExternUsersController> _logger;
        private Fixture _fixture;

        public TrayForNationalSealsExternUsersControllerTest()
		{
			_logger = new Mock<TrayForNationalSealsExternUsersController>();
			controller = new TrayForNationalSealsExternUsersController(new LoggerFactory().CreateLogger<TrayForNationalSealsExternUsersController>());
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

            var token = "token";
            controller.ControllerContext.HttpContext.Session.SetString("token", token);
        }

		[Fact]
		public void TrayForNationalSealsExternUsers()
		{
			decimal codigoEmpresa = _fixture.Create<int>();
			decimal nitEmpresa = _fixture.Create<int>();
            decimal tipoEmpresa = _fixture.Create<int>();

			var r = controller.TrayForNationalSealsExternUsers(codigoEmpresa, nitEmpresa, tipoEmpresa);

			var viewResult = Assert.IsType<ViewResult>(r);

			Assert.NotNull(viewResult);
		}

		[Fact]
		public void ConsultBussiness()
		{
			var r = controller.ConsultBussiness();

			var datos = Assert.IsType<List<ElementTypes>>(r);

			Assert.NotNull(datos);

		}

		[Fact]
		public void ConsultBussinesAndLegalRepresentant()
		{
			decimal codigoEmpresa = 5;
			var r = controller.ConsultBussinesAndLegalRepresentant(codigoEmpresa);
			Assert.True(r != null);

		}

		[Fact]
		public void ConsultCities()
		{
			var r = controller.ConsultCities();

			var datos = Assert.IsType<List<ElementTypesEspecies>>(r);

			Assert.NotNull(datos);

		}

		[Fact]
		public void RegisterRequest()
		{
			soportsDocuments adjun = new soportsDocuments();
			adjun.codigo = _fixture.Create<int>();
			adjun.adjuntoBase64 = _fixture.Create<string>();
			adjun.nombreAdjunto = _fixture.Create<string>();
			adjun.tipoAdjunto = _fixture.Create<string>();

			numeracion numer = new numeracion();
			numer.inicial = _fixture.Create<int>();
			numer.final = _fixture.Create<int>();
			numer.origen = _fixture.Create<int>();

			Request datos = new Request();
			datos.codigoEmpresa = _fixture.Create<int>();
			datos.codigoSolicitud = _fixture.Create<int>();
			datos.fecha = DateTime.Now;
			datos.ciudadRepresentante = _fixture.Create<int>();
			datos.direccionEntrega = _fixture.Create<string>();
			datos.cantidad = _fixture.Create<int>();
			datos.especimenes = _fixture.Create<int>();
			datos.codigoInicialPieles = _fixture.Create<int>();
			datos.codigoFinalPieles = _fixture.Create<int>();
			datos.longitudMenor = _fixture.Create<int>();
			datos.longitudMayor = _fixture.Create<int>();
			datos.fechaRepresentante = DateTime.Now;
			datos.observaciones = _fixture.Create<string>();
			datos.respuesta = _fixture.Create<string>();
			datos.estadoSolicitud = _fixture.Create<string>();
			datos.fechaRadicado = DateTime.Now;
			datos.fechaCambioEstado = DateTime.Now;
			datos.observacionesDesistimiento = _fixture.Create<string>();
			datos.numeraciones = null;
			datos.facturaAdjunto = adjun;
			datos.adjuntoCarta = adjun;
			datos.AnexosAdjuntos = null;
			datos.anexosAdjuntosEliminar = null;
			datos.adjuntosRespuesta = null;
			datos.adjuntosRespuestaEliminar = null;

			var r = controller.RegisterRequest(datos);
			Assert.True(r != null);
		}

		[Fact]
		public void ConsultRegisteredRecuest()
		{
			decimal codigoEmpresa = _fixture.Create<int>();

			var r = controller.ConsultRegisteredRecuest(codigoEmpresa);

			var datos = Assert.IsType<List<solicitudes>>(r);

			Assert.NotNull(datos);

		}

		[Fact]
		public void ConsultRequirements()
		{
			decimal codigoEmpresa = _fixture.Create<int>();

			var r = controller.ConsultRequirements(codigoEmpresa);

			var datos = Assert.IsType<List<solicitudes>>(r);

			Assert.NotNull(datos);

		}

		[Fact]
		public void ConsultApproved()
		{
			decimal codigoEmpresa = _fixture.Create<int>();

			var r = controller.ConsultApproved(codigoEmpresa);

			var datos = Assert.IsType<List<solicitudes>>(r);

			Assert.NotNull(datos);

		}

		[Fact]
		public void ConsultDesisted()
		{
			decimal codigoEmpresa = _fixture.Create<int>();

			var r = controller.ConsultDesisted(codigoEmpresa);

			var datos = Assert.IsType<List<solicitudes>>(r);

			Assert.NotNull(datos);

		}

		[Fact]
		public void ConsultOnePendientRegister()
		{
			decimal codigoEmpresa = 897564231;

			var r = controller.ConsultOnePendientRegister(codigoEmpresa);
			Assert.True(r != null);

		}

		[Fact]
		public void EditRequest()
		{
			soportsDocuments adjun = new soportsDocuments();
			adjun.codigo = _fixture.Create<int>();
			adjun.adjuntoBase64 = _fixture.Create<string>();
			adjun.nombreAdjunto = _fixture.Create<string>();
			adjun.tipoAdjunto = _fixture.Create<string>();

			numeracion numer = new numeracion();
			numer.inicial = _fixture.Create<int>();
			numer.final = _fixture.Create<int>();
			numer.origen = _fixture.Create<int>();

			Request datos = new Request();
			datos.codigoEmpresa = _fixture.Create<int>();
			datos.codigoSolicitud = _fixture.Create<int>();
			datos.fecha = DateTime.Now;
			datos.ciudadRepresentante = _fixture.Create<int>();
			datos.direccionEntrega = _fixture.Create<string>();
			datos.cantidad = _fixture.Create<int>();
			datos.especimenes = _fixture.Create<int>();
			datos.codigoInicialPieles = _fixture.Create<int>();
			datos.codigoFinalPieles = _fixture.Create<int>();
			datos.longitudMenor = _fixture.Create<int>();
			datos.longitudMayor = _fixture.Create<int>();
			datos.fechaRepresentante = DateTime.Now;
			datos.observaciones = _fixture.Create<string>();
			datos.respuesta = _fixture.Create<string>();
			datos.estadoSolicitud = _fixture.Create<string>();
			datos.fechaRadicado = DateTime.Now;
			datos.fechaCambioEstado = DateTime.Now;
			datos.observacionesDesistimiento = _fixture.Create<string>();
			datos.numeraciones = null;
			datos.facturaAdjunto = adjun;
			datos.adjuntoCarta = adjun;
			datos.AnexosAdjuntos = null;
			datos.anexosAdjuntosEliminar = null;
			datos.adjuntosRespuesta = null;
			datos.adjuntosRespuestaEliminar = null;

			var r = controller.EditRequest(datos);
			Assert.True(r != null);
		}

		[Fact]
		public void GetQuotas()
		{
			string documentNumber = "897564231";
			string especie = "2";

			var r = controller.GetQuotas(documentNumber, especie);
			Assert.True(r != null);

		}

		[Fact]
		public void GetInventory()
		{
			string documentNumber = "897564231";
			string especie = "2";

			var r = controller.GetInventory(documentNumber, especie);
			Assert.True(r != null);

		}
	}
}
