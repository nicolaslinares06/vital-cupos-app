using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repository.Helpers.Models;
using System.Security.Claims;
using Web.Controllers;
using Web.Models;
using static CUPOS_FRONT.Models.Requests;
using static Web.Models.ResolucionPeces;

namespace TestUnit.WEB
{
	public class CrearUsuarioControllerTest
	{
		//Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
		private CrearUsuarioController controller;
        private Fixture _fixture;
        private readonly string token;

        public CrearUsuarioControllerTest()
		{
			controller = new CrearUsuarioController(new LoggerFactory().CreateLogger<CrearUsuarioController>());
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
		public void CreacionUsuario()
		{
			var r = controller.CreacionUsuario();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.CreacionUsuario();
            Assert.True(r != null);
        }

		[Fact]
		public void Crear()
		{
			CreacionUsu usuario = new CreacionUsu();
			usuario.firstName = _fixture.Create<string>();
			usuario.secondName = _fixture.Create<string>();
			usuario.firstLastName = _fixture.Create<string>();
			usuario.SecondLastName = _fixture.Create<string>();
			usuario.codeParametricDocumentType = _fixture.Create<int>();
			usuario.identification = _fixture.Create<int>();
			usuario.address = _fixture.Create<string>();
			usuario.dependence = _fixture.Create<string>();
			usuario.phone = _fixture.Create<int>();
			usuario.email = _fixture.Create<string>();
			usuario.phone = _fixture.Create<int>();
			usuario.contractStartDate = DateTime.Now;
			usuario.contractFinishDate = DateTime.Now;
			usuario.acceptsTerms = _fixture.Create<bool>();
			usuario.acceptsProcessingPersonalData = _fixture.Create<bool>();
			usuario.rol = _fixture.Create<string>();

			var r = controller.Crear(usuario);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.Crear(usuario);
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultarDocumentos()
		{
			var r = controller.ConsultarDocumentos();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultarDocumentos();
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultarDependencia()
		{
			var r = controller.ConsultarDependencia();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultarDependencia();
            Assert.True(r != null);
        }
	}
}