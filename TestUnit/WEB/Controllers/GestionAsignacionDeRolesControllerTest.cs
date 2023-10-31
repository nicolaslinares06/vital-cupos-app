using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Repository.Helpers.Models;
using System.Security.Claims;
using Web.Controllers;
using Web.Models;
using Microsoft.Extensions.Logging;
using static ClosedXML.Excel.XLPredefinedFormat;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TestUnit.WEB
{
	public class GestionAsignacionDeRolesControllerTest
	{
		//Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
		private GestionAsignacionDeRolesController controller;
        private Fixture _fixture;
        private readonly string token;

        public GestionAsignacionDeRolesControllerTest()
		{
			controller = new GestionAsignacionDeRolesController(new LoggerFactory().CreateLogger<GestionAsignacionDeRolesController>());
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
		public void returnAdministracion()
		{
			var r = controller.returnAdministracion();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.returnAdministracion();
            Assert.True(r != null);
        }

        [Fact]
        public void AsignarRol()
        {
            string nombre = _fixture.Create<string>();

            var r = controller.AsignarRol(nombre);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.AsignarRol(nombre);
            Assert.True(r != null);
        }

        [Fact]
		public void filtroValor()
		{
			string nombre = _fixture.Create<string>();

			var r = controller.filtroValor(nombre);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.filtroValor(nombre);
            Assert.True(r != null);
        }

		[Fact]
		public void Actualizar()
		{
			AssignmentUpdateReq datos = new AssignmentUpdateReq();
			datos.id = _fixture.Create<int>();
			datos.estadoSolicitud = _fixture.Create<string>();

			var r = controller.Actualizar(datos);
			Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.Actualizar(datos);
            Assert.True(r != null);
        }

        [Fact]
        public void ConsultarRoles()
        {

            var r = controller.ConsultarRoles();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultarRoles();
            Assert.True(r != null);
        }

        [Fact]
        public void SolicitarRol()
        {
            VitalReq datos = new VitalReq();
            datos.code = _fixture.Create<int>();
            datos.status = _fixture.Create<string>();
            datos.permissions = _fixture.Create<string>();
            datos.message = _fixture.Create<string>();
            datos.ID = _fixture.Create<decimal>();
            datos.User = _fixture.Create<string>();
            datos.Name = _fixture.Create<string>();
            datos.Document = _fixture.Create<decimal>();
            datos.EMail = _fixture.Create<string>();
            datos.LastLogin = System.DateTime.Now;
            datos.Active = _fixture.Create<string>();
            datos.Enabled = _fixture.Create<string>();
            datos.Module = _fixture.Create<string>();
            datos.Url = _fixture.Create<string>();
            datos.Token = _fixture.Create<string>();
            datos.UrlError = _fixture.Create<string>();
            datos.rol = _fixture.Create<decimal>();
            datos.rols = _fixture.CreateMany<SelectListItem>().ToList();

            var r = controller.SolicitarRol(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.SolicitarRol(datos);
            Assert.True(r != null);
        }
    }
}
