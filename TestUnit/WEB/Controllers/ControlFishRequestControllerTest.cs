using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using Web.Controllers;
using Web.Models;
using WebFront.Controllers;
using static CUPOS_FRONT.Models.Requests;
using static Web.Models.ResolucionPeces;

namespace TestUnit.WEB
{
	public class ControlFishRequestControllerTest
	{
		//Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
		private ControlFishRequestController controller;
		private readonly Mock<ControlFishRequestController> _logger;
        private Fixture _fixture;
        private readonly string token;

        public ControlFishRequestControllerTest()
		{
			_logger = new Mock<ControlFishRequestController>();
			controller = new ControlFishRequestController(new LoggerFactory().CreateLogger<ControlFishRequestController>());
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
		public void InControlFishRequestdex()
		{
			var r = controller.ControlFishRequest();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ControlFishRequest();
            Assert.True(r != null);

        }

        [Fact]
		public void ConsultEntityDates()
		{
			decimal documentType = _fixture.Create<int>(); 
			decimal DocumentNumber = _fixture.Create<int>();

			var r = controller.ConsultEntityDates(documentType, DocumentNumber);
            r = controller.ControlFishRequest();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultEntityDates(documentType, DocumentNumber);
            Assert.True(r != null);

        }

		[Fact]
		public void ConsultPermitsReslution()
		{
			decimal codeBussines = _fixture.Create<int>();

			var r = controller.ConsultPermitsReslution(codeBussines);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultPermitsReslution(codeBussines);
            Assert.True(r != null);

        }

		[Fact]
		public void ConsultOnePermitResolution()
		{
			decimal codeReslution = _fixture.Create<int>();

			var r = controller.ConsultOnePermitResolution(codeReslution);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultOnePermitResolution(codeReslution);
            Assert.True(r != null);

        }

		[Fact]
		public void SaveResolution()
		{
			soportsDocuments archivo = new soportsDocuments();
			archivo.codigo = _fixture.Create<int>();
			archivo.adjuntoBase64 = _fixture.Create<string>();
			archivo.nombreAdjunto = _fixture.Create<string>();
			archivo.tipoAdjunto = _fixture.Create<string>();

			ResolucionPermisos datos = new ResolucionPermisos();
			datos.codigoResolucion = _fixture.Create<int>();
			datos.codigoEmpresa = _fixture.Create<int>();
			datos.numeroResolucion = _fixture.Create<int>();
			datos.fechaResolucion = DateTime.Now;
			datos.fechaInicio = DateTime.Now;
			datos.fechaFin = DateTime.Now;
			datos.adjunto = archivo;
			datos.objetoResolucion = _fixture.Create<string>();
            controller.ControllerContext.HttpContext.Session.SetString("token", token);

			var r = controller.SaveResolution(datos);
            Assert.True(r != null);
        }

		[Fact]
		public void EditResolution()
		{
			soportsDocuments archivo = new soportsDocuments();
			archivo.codigo = _fixture.Create<int>();
			archivo.adjuntoBase64 = _fixture.Create<string>();
			archivo.nombreAdjunto = _fixture.Create<string>();
			archivo.tipoAdjunto = _fixture.Create<string>();

			ResolucionPermisos datos = new ResolucionPermisos();
			datos.codigoResolucion = _fixture.Create<int>();
			datos.codigoEmpresa = _fixture.Create<int>();
			datos.numeroResolucion = _fixture.Create<int>();
			datos.fechaResolucion = DateTime.Now;
			datos.fechaInicio = DateTime.Now;
			datos.fechaFin = DateTime.Now;
			datos.adjunto = archivo;
			datos.objetoResolucion = _fixture.Create<string>();

			var r = controller.EditResolution(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.EditResolution(datos);
            Assert.True(r != null);
        }

		[Fact]
		public void DeleteResolution()
		{
			decimal codeResolution = _fixture.Create<int>();

			var r = controller.DeleteResolution(codeResolution);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.DeleteResolution(codeResolution);
            Assert.True(r != null);
        }
	}
}
