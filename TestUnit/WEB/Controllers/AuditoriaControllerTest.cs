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
	public class AuditoriaControllerTest
	{
		//Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
		private AuditoriaController controller;
        private Fixture _fixture;
        private readonly string token;

        public AuditoriaControllerTest()
        {
            controller = new AuditoriaController(new LoggerFactory().CreateLogger<AuditoriaController>());
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
		public void GestionAuditoria()
		{
			var r = controller.GestionAuditoria();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.GestionAuditoria();
            Assert.True(r != null);
        }

		[Fact]
		public void DetalleAuditoria()
		{

			string fecha = _fixture.Create<string>();
			var r = controller.DetalleAuditoria(fecha);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.DetalleAuditoria(fecha);
            Assert.True(r != null);
        }

		[Fact]
		public void ReturnAdministracion()
		{
			var r = controller.returnAdministracion();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.returnAdministracion();
            Assert.True(r != null);
        }

		[Fact]
		public void Consultar()
		{
			DateTime fechaInicio = DateTime.Now; 
			DateTime fechaFinal = DateTime.Now; 

			var r = controller.Consultar(fechaInicio, fechaFinal);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.Consultar(fechaInicio, fechaFinal);
            Assert.True(r != null);

        }

		[Fact]
		public void ConsultarDetalles()
		{
			string fecha = _fixture.Create<string>();

			var r = controller.ConsultarDetalles(fecha);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultarDetalles(fecha);
            Assert.True(r != null);

        }

		[Fact]
		public void ConsultarModulos()
		{
			var r = controller.ConsultarModulos();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultarModulos();
            Assert.True(r != null);
        }
	}
}
