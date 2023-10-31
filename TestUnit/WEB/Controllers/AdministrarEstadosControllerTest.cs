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
	public class AdministrarEstadosControllerTest
	{
		//Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
		private AdministrarEstadosController controller;
        private Fixture _fixture;
        private readonly string token;

        public AdministrarEstadosControllerTest()
        {
            controller = new AdministrarEstadosController(new LoggerFactory().CreateLogger<AdministrarEstadosController>());
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
		public void AgregarEstado()
		{
			var r = controller.AgregarEstado();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.AgregarEstado();
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
		public void FiltroEstados()
		{
			var r = controller.filtroEstados(_fixture.Create<string>(), _fixture.Create<string>());
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.filtroEstados(_fixture.Create<string>(), _fixture.Create<string>());
            Assert.True(r != null);
        }

		[Fact]
		public void ActualizarEstado()
		{
			ReqEstado datos = new ReqEstado();
			datos.id = _fixture.Create<int>();
			datos.position = _fixture.Create<int>();
			datos.idEstate = _fixture.Create<string>();
			datos.description = _fixture.Create<string>();
			datos.stage = _fixture.Create<string>();
			datos.estate = _fixture.Create<bool>();

			var r = controller.ActualizarEstado(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ActualizarEstado(datos);
            Assert.True(r != null);
        }

		[Fact]
		public void Crear()
		{
			ReqAdminEstados datos = new ReqAdminEstados();
			datos.etapa = _fixture.Create<string>();
			datos.descripcion = _fixture.Create<string>();
			datos.estado = _fixture.Create<int>();
			
			var r = controller.Crear(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.Crear(datos);
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultarEstadosCertificado()
		{
			var r = controller.ConsultarEstadosCertificado();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultarEstadosCertificado();
            Assert.True(r != null);
        }

		[Fact]
		public void BuscarEstados()
		{
            var term = "a";
            controller.HttpContext.Request.QueryString = new QueryString($"?term={term}");

            var r = controller.BuscarEstados();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.BuscarEstados();
            Assert.True(r != null);
        }
	}
}
