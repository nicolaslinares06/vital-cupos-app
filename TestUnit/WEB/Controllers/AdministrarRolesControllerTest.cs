using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Web.Controllers;
using Web.Models;
using static CUPOS_FRONT.Models.Requests;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace TestUnit.WEB
{
	public class AdministrarRolesControllerTest
	{
		//Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
		private AdministrarRolesController controller;
        private Fixture _fixture;
        private readonly string token;

        public AdministrarRolesControllerTest()
        {
            controller = new AdministrarRolesController(new LoggerFactory().CreateLogger<AdministrarRolesController>());
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
		public void CrearRol()
		{
			var r = controller.CrearRol();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.CrearRol();
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
		public void FiltroRoles()
		{
			var r = controller.filtroRoles("", "");
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.filtroRoles("", "");
            Assert.True(r != null);

        }

		[Fact]
		public void ActualizarEstado()
		{
			ReqRol datos = new ReqRol();
			datos.rolId = 10023;
			datos.name = _fixture.Create<string>();
			datos.position = _fixture.Create<string>();
			datos.description = _fixture.Create<string>();
			datos.estate = _fixture.Create<bool>();

			var r = controller.ActualizarEstado(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ActualizarEstado(datos);
            Assert.True(r != null);
        }

		[Fact]
		public void CreacionRol()
		{
			CreateRolRequest datos = new CreateRolRequest();
			datos.rolId = _fixture.Create<int>();
			datos.name = _fixture.Create<string>();
			datos.position = _fixture.Create<string>();
			datos.description = _fixture.Create<string>();
			datos.estate = _fixture.Create<bool>();

			var r = controller.CreacionRol(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.CreacionRol(datos);
            Assert.True(r != null);
        }

		[Fact]
		public void BuscarRoles()
		{
            var term = "a";
            controller.HttpContext.Request.QueryString = new QueryString($"?term={term}");

            var r = controller.BuscarRoles();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.BuscarRoles();
            Assert.True(r != null);
        }
	}
}
