using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Web.Controllers;
using Web.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace TestUnit.WEB
{
	public class AdministracionTecnicaControllerTest
	{
		//Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
		private AdministracionTecnicaController controller;
        private Fixture _fixture;
        private readonly string token;

        public AdministracionTecnicaControllerTest()
        {
            controller = new AdministracionTecnicaController(new LoggerFactory().CreateLogger<AdministracionTecnicaController>());
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
		public void AgregarValor()
		{
			var r = controller.AgregarValor();
			Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.AgregarValor();
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
		public void FiltroValor()
		{
			var r = controller.filtroValor(_fixture.Create<string>());
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.filtroValor(_fixture.Create<string>());
            Assert.True(r != null);
        }

		[Fact]
		public void Actualizar()
		{
			AdminTecnicaReq datos = new AdminTecnicaReq();
			datos.codigo = _fixture.Create<int>();
            datos.nombre = _fixture.Create<string>();
            datos.valor = _fixture.Create<string>();
            datos.descripcion = _fixture.Create<string>();
            datos.estadoRegistro = _fixture.Create<bool>();

            var r = controller.Actualizar(datos);
			Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.Actualizar(datos);
            Assert.True(r != null);
        }

		[Fact]
		public void Eliminar()
		{
			ReqId datos = new ReqId();
			datos.id = _fixture.Create<int>();

			var r = controller.Eliminar(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.Eliminar(datos);
            Assert.True(r != null);
        }

		[Fact]
		public void Crear()
		{
			AdminTecnicaReq datos = new AdminTecnicaReq();
			datos.codigo = _fixture.Create<int>(); ;
			datos.nombre = _fixture.Create<string>(); ;
			datos.valor = _fixture.Create<string>(); ;
			datos.descripcion = _fixture.Create<string>(); ;
			datos.estadoRegistro = _fixture.Create<bool>();

			var r = controller.Crear(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.Crear(datos);
            Assert.True(r != null);
        }

        [Fact]
        public void BuscarValoresTecnicos()
        {
            var term = "a";
            controller.HttpContext.Request.QueryString = new QueryString($"?term={term}");

            var r = controller.BuscarValoresTecnicos();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.BuscarValoresTecnicos();
            Assert.True(r != null);
        }

    }
}