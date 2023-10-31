using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repository.Helpers;
using Repository.Helpers.Models;
using System.Security.Claims;
using Web.Controllers;
using Web.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace TestUnit.WEB
{
    public class ParametricaControllerTest
    {
        //Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
        private ParametricaController parametrica;
        private Fixture _fixture;
        private readonly string token;

        public ParametricaControllerTest()
        {
			parametrica = new ParametricaController(new LoggerFactory().CreateLogger<ParametricaController>());
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

            parametrica.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            token = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Development.json").Build().GetValue<string>("Variables:Token");
        }

        [Fact]
        public void Index()
        {

			var r = parametrica.Index();
            Assert.True(r != null);

            parametrica.ControllerContext.HttpContext.Session.SetString("token", token);

            r = parametrica.Index();
            Assert.True(r != null);

        }

		[Fact]
		public void HttpContextTest()
		{
			var httpContext = new DefaultHttpContext();
			httpContext.Request.Headers["X-Custom-Header"] = "88-test-tcb";
			var controller = new ParametricaController(new LoggerFactory().CreateLogger<ParametricaController>())
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = httpContext,
				}
			};
		}

		[Fact]
        public void Registrar()
        {
            var r = parametrica.Registrar();
            Assert.True(r != null);

            parametrica.ControllerContext.HttpContext.Session.SetString("token", token);

            r = parametrica.Registrar();
            Assert.True(r != null);
        }

        [Fact]
        public void Filtrar()
        {
            var r = parametrica.Filtrar("COLOR");
            Assert.True(r != null);

            parametrica.ControllerContext.HttpContext.Session.SetString("token", token);

            r = parametrica.Filtrar("COLOR");
            Assert.True(r != null);

        }

        [Fact]
        public void Crear()
        {
            ReqParametric datos = new ReqParametric();
            datos.code = _fixture.Create<int>();
            datos.name = "COLOR";
            datos.value = "CAFE";
            datos.description = "CAFE";
            datos.estate = StringHelper.estadoActivo;

            var r = parametrica.Crear(datos);
            Assert.True(r != null);

            parametrica.ControllerContext.HttpContext.Session.SetString("token", token);

            r = parametrica.Crear(datos);
            Assert.True(r != null);
        }

        [Fact]
        public void Actualizar()
        {
            ReqParametric datos = new ReqParametric();
            datos.code = 10165;
            datos.name = "COLOR";
            datos.value = "CAFE";
            datos.description = "CAFE";
            datos.estate = StringHelper.estadoActivo;

            var r = parametrica.Actualizar(datos);
            Assert.True(r != null);

            parametrica.ControllerContext.HttpContext.Session.SetString("token", token);

            r = parametrica.Actualizar(datos);
            Assert.True(r != null);
        }

        [Fact]
        public void ActivarInactivar()
        {
            ReqParametric datos = new ReqParametric();
            datos.code = 10165;
            datos.name = "COLOR";
            datos.value = "CAFE";
            datos.description = "CAFE";
            datos.estate = StringHelper.estadoActivo;

            var r = parametrica.ActivarInactivar(datos);
            Assert.True(r != null);

            parametrica.ControllerContext.HttpContext.Session.SetString("token", token);

            r = parametrica.ActivarInactivar(datos);
            Assert.True(r != null);
        }
        
        [Fact]
        public void Eliminar()
        {
			ReqIdParametric datos = new ReqIdParametric();
            datos.id = _fixture.Create<int>(); ;

            var r = parametrica.Eliminar(datos);
            Assert.True(r != null);

            parametrica.ControllerContext.HttpContext.Session.SetString("token", token);

            r = parametrica.Eliminar(datos);
            Assert.True(r != null);
        }

        [Fact]
        public void BuscarParametro()
        {
            var term = "a";
            parametrica.HttpContext.Request.QueryString = new QueryString($"?term={term}");

            var r = parametrica.BuscarParametro();
            Assert.True(r != null);

            parametrica.ControllerContext.HttpContext.Session.SetString("token", token);

            r = parametrica.BuscarParametro();
            Assert.True(r != null);

        }

		[Fact]
		public void ConsultarParametrica()
		{
            string parametric = _fixture.Create<string>();
			var r = parametrica.ConsultarParametrica(parametric);
            Assert.True(r != null);

            parametrica.ControllerContext.HttpContext.Session.SetString("token", token);

            r = parametrica.ConsultarParametrica(parametric);
            Assert.True(r != null);

        }

		[Fact]
		public void ConsultarPaises()
		{
			var r = parametrica.ConsultarPaises();
            Assert.True(r != null);

            parametrica.ControllerContext.HttpContext.Session.SetString("token", token);

            r = parametrica.ConsultarPaises();
            Assert.True(r != null);

        }

		[Fact]
		public void ConsultarDepartamentos()
		{
            int idpais = _fixture.Create<int>();
			var r = parametrica.ConsultarDepartamentos(idpais);
            Assert.True(r != null);

            parametrica.ControllerContext.HttpContext.Session.SetString("token", token);

            r = parametrica.ConsultarDepartamentos(idpais);
            Assert.True(r != null);

        }

		[Fact]
		public void ConsultarCiudades()
		{
			int iddepartamento = _fixture.Create<int>();
			var r = parametrica.ConsultarCiudades(iddepartamento);
            Assert.True(r != null);

            parametrica.ControllerContext.HttpContext.Session.SetString("token", token);

            r = parametrica.ConsultarCiudades(iddepartamento);
            Assert.True(r != null);

        }
	}
}
