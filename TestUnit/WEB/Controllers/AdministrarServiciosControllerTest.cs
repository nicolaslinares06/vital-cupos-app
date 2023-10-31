using AutoFixture;
using CUPOS_FRONT.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Web.Controllers;
using Web.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using CUPOS_FRONT.Models;

namespace TestUnit.WEB
{
	public class AdministrarServiciosControllerTest
	{
		//Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
		private AdministrarServiciosController controller;
        private Fixture _fixture;
        private readonly string token;

        public AdministrarServiciosControllerTest()
        {
            controller = new AdministrarServiciosController(new LoggerFactory().CreateLogger<AdministrarServiciosController>());
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
        public void DetalleServicio()
        {
            var r = controller.DetalleServicio("Servicio");
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.DetalleServicio("Servicio");
            Assert.True(r != null);
        }

        [Fact]
        public void EvaluarServicios()
        {
            List<Servicio> datos = new List<Servicio>();

            Servicio servicio = new Servicio();
            servicio.Nombre = _fixture.Create<string>();
            servicio.Url = _fixture.Create<string>();
            servicio.Estado = _fixture.Create<string>();

            datos.Add(servicio);    

            var r = controller.EvaluarServicios(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.EvaluarServicios(datos);
            Assert.True(r != null);
        }
    }
}
