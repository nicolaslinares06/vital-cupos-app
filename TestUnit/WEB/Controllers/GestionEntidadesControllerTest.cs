using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Repository.Helpers.Models;
using System.Security.Claims;
using Web.Controllers;
using Web.Models;
using static CUPOS_FRONT.Models.Requests;
using Microsoft.Extensions.Logging;

namespace TestUnit.WEB
{
	public class GestionEntidadesControllerTest
	{
		//Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
		private GestionEntidadesController controller;
        private Fixture _fixture;
        private readonly string token;

        public GestionEntidadesControllerTest()
		{
			controller = new GestionEntidadesController(new LoggerFactory().CreateLogger<GestionEntidadesController>());
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
		public void returnAdministracion()
		{
			var r = controller.returnAdministracion();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.returnAdministracion();
            Assert.True(r != null);
        }

		[Fact]
		public void Actualizar()
		{
			ReqEntidad datos = new ReqEntidad();
			datos.codigoEmpresa = _fixture.Create<int>();
			datos.idtipoDocumento = _fixture.Create<int>();
			datos.idtipoEntidad = _fixture.Create<int>();
			datos.nombreEmpresa = _fixture.Create<string>();
			datos.nit = _fixture.Create<int>();
			datos.idciudad = _fixture.Create<int>();
			datos.direccion = _fixture.Create<string>();
			datos.telefono = _fixture.Create<int>();
			datos.correo = _fixture.Create<string>();
			datos.matriculaMercantil = _fixture.Create<string>();

			var r = controller.Actualizar(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.Actualizar(datos);
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultNovedades()
		{
			decimal codigoEmpresa = _fixture.Create<int>(); 
			decimal? idNovedad = _fixture.Create<int>();

			var r = controller.ConsultNovedades(codigoEmpresa, idNovedad);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultNovedades(codigoEmpresa, idNovedad);
            Assert.True(r != null);
        }

		[Fact]
		public void GuardarNovedad()
		{
			ReqNovedad datos = new ReqNovedad();
			datos.codigo = _fixture.Create<int>();
			datos.codigoEmpresa = _fixture.Create<int>();
			datos.idTipoNovedad = _fixture.Create<int>();
			datos.idEstadoEmpresa = _fixture.Create<int>();
			datos.idEstadoEmisionCITES = _fixture.Create<int>();
			datos.fechaRegistroNovedad = DateTime.Now;
			datos.observaciones = _fixture.Create<string>();
			datos.saldoProduccionDisponible = _fixture.Create<int>();
			datos.cuposDisponibles = _fixture.Create<int>();
			datos.inventarioDisponible = _fixture.Create<int>();
			datos.numeroCupospendientesportramitar = _fixture.Create<int>();
			datos.idDisposicionEspecimen = _fixture.Create<int>();
			datos.idEmpresaZoo = _fixture.Create<int>();
			datos.otroCual = _fixture.Create<string>();
			datos.observacionesDetalle = _fixture.Create<string>();
			datos.documentos = null;
			datos.documentosAeliminar = null;

			var r = controller.GuardarNovedad(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.GuardarNovedad(datos);
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultCupos()
		{
			decimal idEmpresa = _fixture.Create<int>();

			var r = controller.ConsultCupos(idEmpresa);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultCupos(idEmpresa);
            Assert.True(r != null);
        }
	}
}
