using AutoFixture;
using CUPOS_FRONT.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Security.Claims;
using Web.Controllers;
using static CUPOS_FRONT.Models.ReportesCuposEmpresasMarcajeModels;
using static CUPOS_FRONT.Models.ReportesPrecintosModels;
using static CUPOS_FRONT.Models.Requests;
using static Web.Models.ResolucionPeces;

namespace TestUnit.WEB
{
	public class ReportesCuposEmpresasMarcajeControllerTest
	{
		//Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
		private ReportesCuposEmpresasMarcajeController controller;
		private Fixture _fixture;
        private readonly string token;

        public ReportesCuposEmpresasMarcajeControllerTest()
		{
			controller = new ReportesCuposEmpresasMarcajeController(null, null, new LoggerFactory().CreateLogger<ReportesCuposEmpresasMarcajeController>());
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
		public void ObtenerDatosCuposEmpresas()
		{
			ReporteCuposEmpresaViewModel filtro = new ReporteCuposEmpresaViewModel();
			filtro.TipoEmpresa = _fixture.Create<int>();
			filtro.NombreEmpresa = _fixture.Create<string>();
			filtro.NIT = _fixture.Create<int>();
			filtro.Estado = _fixture.Create<int>();
			filtro.EstadoEmisionCITES = _fixture.Create<int>();
			filtro.NumeroResolucion = _fixture.Create<int>();
			filtro.FechaEmisionResolucionDesde = DateTime.Now;
			filtro.FechaEmisionResolucionHasta = DateTime.Now;
			filtro.NumeroResolucion = _fixture.Create<int>();

			var r = controller.ObtenerDatosCuposEmpresas(filtro);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ObtenerDatosCuposEmpresas(filtro);
            Assert.True(r != null);
        }

		[Fact]
		public void ExportarExcelDataEmpresas()
		{
			ReporteCuposEmpresaViewModel filtro = new ReporteCuposEmpresaViewModel();
			filtro.TipoEmpresa = _fixture.Create<int>();
			filtro.NombreEmpresa = _fixture.Create<string>();
			filtro.NIT = _fixture.Create<int>();
			filtro.Estado = _fixture.Create<int>();
			filtro.EstadoEmisionCITES = _fixture.Create<int>();
			filtro.NumeroResolucion = _fixture.Create<int>();
			filtro.FechaEmisionResolucionDesde = DateTime.Now;
			filtro.FechaEmisionResolucionHasta = DateTime.Now;
			filtro.NumeroResolucion = _fixture.Create<int>();

			var r = controller.ExportarExcelDataEmpresas(filtro);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ExportarExcelDataEmpresas(filtro);
            Assert.True(r != null);
        }

		[Fact]
		public void ExportarPDFDataEmpresas()
		{
			ReporteCuposEmpresaViewModel filtro = new ReporteCuposEmpresaViewModel();
			filtro.TipoEmpresa = _fixture.Create<int>();
			filtro.NombreEmpresa = _fixture.Create<string>();
			filtro.NIT = _fixture.Create<int>();
			filtro.Estado = _fixture.Create<int>();
			filtro.EstadoEmisionCITES = _fixture.Create<int>();
			filtro.NumeroResolucion = _fixture.Create<int>();
			filtro.FechaEmisionResolucionDesde = DateTime.Now;
			filtro.FechaEmisionResolucionHasta = DateTime.Now;
			filtro.BusquedaEspecifica = _fixture.Create<int>();

			var r = controller.ExportarPDFDataEmpresas(filtro);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ExportarPDFDataEmpresas(filtro);
            Assert.True(r != null);
        }

		[Fact]
		public void ObtenerDatosEmpresaAPI()
		{
			ReporteCuposEmpresaViewModel filtro = new ReporteCuposEmpresaViewModel();
			filtro.TipoEmpresa = _fixture.Create<int>();
			filtro.NombreEmpresa = _fixture.Create<string>();
			filtro.NIT = _fixture.Create<int>();
			filtro.Estado = _fixture.Create<int>();
			filtro.EstadoEmisionCITES = _fixture.Create<int>();
			filtro.NumeroResolucion = _fixture.Create<int>();
			filtro.FechaEmisionResolucionDesde = DateTime.Now;
			filtro.FechaEmisionResolucionHasta = DateTime.Now;
			filtro.BusquedaEspecifica = _fixture.Create<int>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ObtenerDatosEmpresaAPI", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { filtro });
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = methodInfo.Invoke(controller, new object[] { filtro });
            Assert.True(r != null);
        }

		[Fact]
		public void GenerarExcel()
		{
			DatosEmpresasModel datos = new DatosEmpresasModel();
			datos.TipoEmpresa = _fixture.Create<string>();
			datos.NombreEmpresa = _fixture.Create<string>();
			datos.NIT = _fixture.Create<int>();
			datos.Estado = _fixture.Create<string>();
			datos.EstadoEmisionCITES = _fixture.Create<string>();
			datos.NumeroResolucion = _fixture.Create<string>();
			datos.FechaEmisionResolucion = DateTime.Now.ToString();
			datos.Especies = _fixture.Create<string>();
			datos.Machos = _fixture.Create<int>();
			datos.Hembras = _fixture.Create<int>();
			datos.PoblacionTotalParental = _fixture.Create<int>();
			datos.AnioProduccion = _fixture.Create<int>();
			datos.CuposComercializacion = _fixture.Create<int>();
			datos.CuotaRepoblacion = _fixture.Create<string>();
			datos.CuposAsignadosTotal = _fixture.Create<int>();
			datos.SoportesRepoblacion = _fixture.Create<bool>();
			datos.CupoUtilizado = _fixture.Create<int>();
			datos.CupoDisponible = _fixture.Create<int>();
			string nombreArchivo = _fixture.Create<string>();

			List<DatosEmpresasModel> listdatos = new List<DatosEmpresasModel>();
			listdatos.Add(datos);

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("GenerarExcel", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { nombreArchivo, listdatos });
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = methodInfo.Invoke(controller, new object[] { nombreArchivo, listdatos });
            Assert.True(r != null);

        }

        [Fact]
		public void ObtenerBusquedaEspecifica()
		{
			ReporteCuposEmpresaViewModel filtro = new ReporteCuposEmpresaViewModel();
			filtro.TipoEmpresa = _fixture.Create<int>();
			filtro.NombreEmpresa = _fixture.Create<string>();
			filtro.NIT = _fixture.Create<int>();
			filtro.Estado = _fixture.Create<int>();
			filtro.EstadoEmisionCITES = _fixture.Create<int>();
			filtro.NumeroResolucion = _fixture.Create<int>();
			filtro.FechaEmisionResolucionDesde = DateTime.Now;
			filtro.FechaEmisionResolucionHasta = DateTime.Now;
			filtro.BusquedaEspecifica = _fixture.Create<int>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ObtenerBusquedaEspecifica", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { filtro });

			Assert.NotNull(r);
		}

		[Fact]
		public void ObtenerFechaHastaMediaNoche()
		{
            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ObtenerFechaHastaMediaNoche", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { DateTime.Now });

            Assert.NotNull(r);
		}

        [Fact]
        public void NormalizarFiltros()
        {
            ReporteCuposEmpresaViewModel filtro = new ReporteCuposEmpresaViewModel();
            filtro.TipoEmpresa = _fixture.Create<int>();
            filtro.NombreEmpresa = _fixture.Create<string>();
            filtro.NIT = _fixture.Create<int>();
            filtro.Estado = _fixture.Create<int>();
            filtro.EstadoEmisionCITES = _fixture.Create<int>();
            filtro.NumeroResolucion = _fixture.Create<int>();
            filtro.FechaEmisionResolucionDesde = DateTime.Now;
            filtro.FechaEmisionResolucionHasta = DateTime.Now;
            filtro.BusquedaEspecifica = _fixture.Create<int>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("NormalizarFiltros", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { filtro });

            Assert.NotNull(r);
        }


        [Fact]
        public void ObtenerTiposEstablecimientosSelect()
        {
            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ObtenerTiposEstablecimientosSelect", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, null);

			Assert.NotNull(r);
        }
    }
}
