using AutoFixture;
using DocumentFormat.OpenXml.Spreadsheet;
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
	public class LoginControllerTest
	{
		//Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
		private LoginController controller;
		private readonly Mock<IHttpClientFactory> _httpClientFactory;
        private Fixture _fixture;
        private readonly string token;


        public LoginControllerTest()
		{
			_httpClientFactory = new Mock<IHttpClientFactory>();
			controller = new LoginController(new LoggerFactory().CreateLogger<LoginController>());
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
		public void CambioContrasena()
		{
			var r = controller.CambioContrasena();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.CambioContrasena();
            Assert.True(r != null);
        }

		[Fact]
		public void CambioContrasenaOlvido()
		{
			var r = controller.CambioContrasenaOlvido();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.CambioContrasenaOlvido();
            Assert.True(r != null);
        }

		[Fact]
		public void EnvioCorreoView()
		{
			var r = controller.EnvioCorreo("");
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.EnvioCorreo("");
            Assert.True(r != null);
        }

		[Fact]
		public void Ingresar()
		{
			LoginRequest datos = new LoginRequest();
			datos.user = "Administrador";
			datos.password = "123456";
			datos.newPassword = "132456";
			datos.acceptsTerms = _fixture.Create<bool>();
			datos.acceptsProcessingPersonalData = _fixture.Create<bool>();
			datos.captcha1 = "6LcAwVMiAAAAAHajtt7BdYU7rIG1JV1g54zjFaLV";
			datos.captcha2 = "6LcAwVMiAAAAAN3KzqdzSmUMrhBBrLw4DaaIdHxw";

			var r = controller.Ingresar(datos);
			Assert.True(r != null);

			datos.user = "Administradores";
			datos.password = "123";
			datos.acceptsTerms = false;
			datos.acceptsProcessingPersonalData = false;

			r = controller.Ingresar(datos);
			Assert.True(r != null);

			datos.user = null;

			r = controller.Ingresar(datos);
			Assert.True(r != null);

			datos.acceptsTerms = false;
			datos.acceptsProcessingPersonalData = false;

			r = controller.Ingresar(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.Ingresar(datos);
            Assert.True(r != null);
        }

		[Fact]
		public void IsReCaptchValid()
		{
			var r = controller.IsReCaptchValid();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.IsReCaptchValid();
            Assert.True(r != null);
        }

		[Fact]
		public void Cambiar()
		{
			LoginRequest datos = new LoginRequest();
			datos.user = _fixture.Create<string>();
			datos.password = _fixture.Create<string>();
			datos.newPassword = _fixture.Create<string>();
			datos.acceptsTerms = _fixture.Create<bool>();
			datos.acceptsProcessingPersonalData = _fixture.Create<bool>();
			datos.captcha1 = _fixture.Create<string>();
			datos.captcha2 = _fixture.Create<string>();

			var r = controller.Cambiar(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.Cambiar(datos);
            Assert.True(r != null);
        }

		[Fact]
		public void CambiarOlvido()
		{
			LoginRequest datos = new LoginRequest();
			datos.user = _fixture.Create<string>();
			datos.password = _fixture.Create<string>();
			datos.newPassword = _fixture.Create<string>();
			datos.acceptsTerms = _fixture.Create<bool>();
			datos.acceptsProcessingPersonalData = _fixture.Create<bool>();
			datos.captcha1 = _fixture.Create<string>();
			datos.captcha2 = _fixture.Create<string>();

			var r = controller.CambiarOlvido(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.CambiarOlvido(datos);
            Assert.True(r != null);
        }

		[Fact]
		public void EnvioCorreo()
		{
			string user = _fixture.Create<string>();

			var r = controller.EnvioCorreo(user);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.EnvioCorreo(user);
            Assert.True(r != null);
        }

		[Fact]
		public void CerrarSesion()
		{
			var r = controller.CerrarSesion();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.CerrarSesion();
            Assert.True(r != null);
        }
	}
}
