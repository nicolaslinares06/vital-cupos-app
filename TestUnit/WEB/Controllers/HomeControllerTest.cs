using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Repository.Helpers.Models;
using System.Security.Claims;
using Web.Controllers;
using Web.Models;
using WebFront.Controllers;
using static CUPOS_FRONT.Models.Requests;

namespace TestUnit.WEB
{
	public class HomeControllerTest
	{
		//Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
		private HomeController controller;
		private readonly Mock<HomeController> _logger;
        private Fixture _fixture;
        private readonly string token;

        public HomeControllerTest()
		{
			_logger = new Mock<HomeController>();
			controller = new HomeController(new LoggerFactory().CreateLogger<HomeController>());
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
			string usuario = _fixture.Create<string>();

			var r = controller.Index(usuario);
			Assert.True(r != null);

            r = controller.Index(null);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.Index(usuario);
            Assert.True(r != null);
        }

        [Fact]
		public void Administracion()
		{
			var r = controller.Administracion();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.Administracion();
            Assert.True(r != null);
        }

		[Fact]
		public void FlujoNegocio()
		{
			int validar = _fixture.Create<int>();
			int cv = _fixture.Create<int>();

			var r = controller.FlujoNegocio(validar, cv);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.FlujoNegocio(validar, cv);
            Assert.True(r != null);
        }

		[Fact]
		public void Reportes()
		{
			var r = controller.Reportes();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.Reportes();
            Assert.True(r != null);
        }

		[Fact]
		public void Privacy()
		{
			var r = controller.Privacy();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.Privacy();
            Assert.True(r != null);
        }

		[Fact]
		public void MapaSitio()
		{
			var r = controller.MapaSitio();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.MapaSitio();
            Assert.True(r != null);
        }

		[Fact]
		public void Error()
		{
			var r = controller.Error();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.Error();
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultarRol()
		{
			var r = controller.ConsultarRol();
			Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultarRol();
            Assert.True(r != null);
        }
	}
}
