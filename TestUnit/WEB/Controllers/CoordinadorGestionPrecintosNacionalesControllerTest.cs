using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Web.Controllers;
using Web.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace TestUnit.WEB
{
	public class CoordinadorGestionPrecintosNacionalesControllerTest
	{
		//Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
		private CoordinadorGestionPrecintosNacionalesController controller;
        private Fixture _fixture;
        private readonly string token;

        public CoordinadorGestionPrecintosNacionalesControllerTest()
		{
			controller = new CoordinadorGestionPrecintosNacionalesController(null, new LoggerFactory().CreateLogger<CoordinadorGestionPrecintosNacionalesController>());
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
		public void Index()
		{
			var r = controller.Index();
			Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

			r = controller.Index();
            Assert.True(r != null);
        }

        [Fact]
        public void Ayuda()
        {
            var r = controller.Ayuda();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.Ayuda();
            Assert.True(r != null);
        }

        [Fact]
		public void CoordinadorGPNAsignacion()
		{
			decimal codigoSolicitud = _fixture.Create<int>();

            var r = controller.CoordinadorGPNAsignacion(codigoSolicitud);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.CoordinadorGPNAsignacion(codigoSolicitud);
            Assert.True(r != null);
        }

        [Fact]
        public void CoordinadorGPNSolicitudAprobada()
        {
            decimal codigoSolicitud = _fixture.Create<int>();

            var r = controller.CoordinadorGPNSolicitudAprobada(codigoSolicitud);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.CoordinadorGPNSolicitudAprobada(codigoSolicitud);
            Assert.True(r != null);
        }

        [Fact]
        public void ObtenerListaNumeracionesSolicitud()
        {
            decimal codigoSolicitud = _fixture.Create<int>();

            var r = controller.ObtenerListaNumeracionesSolicitud(codigoSolicitud);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ObtenerListaNumeracionesSolicitud(codigoSolicitud);
            Assert.True(r != null);
        }


        [Fact]
		public void CoordinadorGPNAprobacion()
		{
			decimal codigoSolicitud = _fixture.Create<int>();

            var r = controller.CoordinadorGPNAprobacion(codigoSolicitud);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.CoordinadorGPNAprobacion(codigoSolicitud);
            Assert.True(r != null);
        }

		[Fact]
		public void CoordinadorGPNSolicitudDesisistido()
		{
			decimal codigoSolicitud = _fixture.Create<int>();

			var r = controller.CoordinadorGPNSolicitudDesisistido(codigoSolicitud);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.CoordinadorGPNSolicitudDesisistido(codigoSolicitud);
            Assert.True(r != null);
        }

		[Fact]
		public void ObtenerListado()
		{
			int tipoConsultaTabla = _fixture.Create<int>();

			var r = controller.ObtenerListado(tipoConsultaTabla);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ObtenerListado(tipoConsultaTabla);
            Assert.True(r != null);
        }

		[Fact]
		public void ObtenerDocumentoSolicitud()
		{
			decimal codigoSolicitud = _fixture.Create<int>();
			decimal tipoDocumento = _fixture.Create<int>();

			var r = controller.ObtenerDocumentoSolicitud(codigoSolicitud, tipoDocumento);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ObtenerDocumentoSolicitud(codigoSolicitud, tipoDocumento);
            Assert.True(r != null);
        }

		[Fact]
		public void EnviarDatosAsignacionAnalista()
		{
			ActualizacionAsignacionAnalista datos = new ActualizacionAsignacionAnalista();
			datos.AnalistaId = _fixture.Create<int>();
			datos.CodigoSolicitud = _fixture.Create<int>();

			var r = controller.EnviarDatosAsignacionAnalista(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.EnviarDatosAsignacionAnalista(datos);
            Assert.True(r != null);
        }

		[Fact]
		public void EnviarDatosAprobacionSolicitud()
		{
			ActualizacionAprobacionAnalista datos = new ActualizacionAprobacionAnalista();
			datos.IdSolicitud = _fixture.Create<int>();
			datos.EstadoSolicitud = _fixture.Create<int>();
			datos.CargoAsignacion = _fixture.Create<int>();
			datos.FechaAprobacion = DateTime.Now;
			datos.Observaciones = _fixture.Create<string>();

			var r = controller.EnviarDatosAprobacionSolicitud(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.EnviarDatosAprobacionSolicitud(datos);
            Assert.True(r != null);
        }
	}
}
