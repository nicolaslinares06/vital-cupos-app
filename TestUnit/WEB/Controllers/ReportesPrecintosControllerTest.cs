using AutoFixture;
using CUPOS_FRONT.Controllers;
using CUPOS_FRONT.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;
using System.Security.Claims;
using Web.Controllers;
using Web.Models;
using WebFront.Controllers;
using static CUPOS_FRONT.Models.ReportesCuposEmpresasMarcajeModels;
using static CUPOS_FRONT.Models.ReportesPrecintosModels;
using static CUPOS_FRONT.Models.Requests;
using static Web.Models.ResolucionPeces;

namespace TestUnit.WEB
{
	public class ReportesPrecintosControllerTest
	{
		//Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
		private ReportesPrecintosController controller;
        private Fixture _fixture;

        public ReportesPrecintosControllerTest()
		{

			controller = new ReportesPrecintosController(new LoggerFactory().CreateLogger<ReportesPrecintosController>());
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
        }

		[Fact]
		public void Index()
		{
			var r = controller.Index();
			Assert.NotNull(r);
		}

		[Fact]
		public void ObtenerDatosPrecintos()
		{
			FiltrosPrecintos filtro = new FiltrosPrecintos();
			filtro.ResolutionNumber = _fixture.Create<string>();
			filtro.Establishment = _fixture.Create<int>();
			filtro.NIT = _fixture.Create<int>();
			filtro.SpecificSearch = _fixture.Create<int>();

			var r = controller.ObtenerDatosPrecintos(filtro);
			Assert.NotNull(r);

		}

		//[Fact]
		//public void ExportarExcelDataPrecintos()
		//{
		//	FiltrosPrecintos filtro = new FiltrosPrecintos();
		//	filtro.ResolutionNumber = _fixture.Create<string>();
		//	filtro.Establishment = _fixture.Create<int>();
		//	filtro.NIT = _fixture.Create<int>();
		//	filtro.SpecificSearch = _fixture.Create<int>();

		//	var r = controller.ExportarExcelDataPrecintos(filtro);
		//	Assert.NotNull(r);
		//}

	

		[Fact]
		public void ObtenerDatosPrecintosAPI()
		{
			FiltrosPrecintos filtro = new FiltrosPrecintos();
			filtro.ResolutionNumber = _fixture.Create<string>();
			filtro.Establishment = _fixture.Create<int>();
			filtro.NIT = _fixture.Create<int>();
			filtro.SpecificSearch = _fixture.Create<int>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ObtenerDatosPrecintosAPI", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { filtro });

			Assert.NotNull(r);
		}

		[Fact]
		public void GenerarExcel()
		{
			DatosPrecintosModel datos = new DatosPrecintosModel();
			datos.RadicationNumber = _fixture.Create<string>();
			datos.RadicationDate = _fixture.Create<string>();
			datos.CompanyName = _fixture.Create<string>();
			datos.NIT = _fixture.Create<int>();
			datos.City = _fixture.Create<string>();
			datos.DeliveryAddress = _fixture.Create<string>();
			datos.Telephone = _fixture.Create<string>();
			datos.Species = _fixture.Create<string>();
			datos.LesserLength = _fixture.Create<int>();
			datos.GreaterLength = _fixture.Create<int>();
			datos.Quantity = _fixture.Create<int>();
			datos.Color = _fixture.Create<string>();
			datos.ProductionYear = _fixture.Create<int>();
			datos.InitialInternalNumber = _fixture.Create<int>();
			datos.FinalInternalNumber = _fixture.Create<int>();
			datos.InitialNumber = "0";
			datos.FinalNumber = "0";
			datos.CompanyCode = _fixture.Create<int>();
			datos.DepositValue = _fixture.Create<string>();
			datos.Analyst = _fixture.Create<string>();
			string nombreArchivo = _fixture.Create<string>();

			List<DatosPrecintosModel> listdatos = new List<DatosPrecintosModel>();
			listdatos.Add(datos);

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("GenerarExcel", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { nombreArchivo, listdatos });

			Assert.NotNull(r);

		}

		[Fact]
		public void ObtenerBusquedaEspecifica()
		{
			FiltrosPrecintos filtro = new FiltrosPrecintos();
			filtro.ResolutionNumber = _fixture.Create<string>();
			filtro.Establishment = _fixture.Create<int>();
			filtro.NIT = _fixture.Create<int>();
			filtro.SpecificSearch = _fixture.Create<int>();

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
			FiltrosPrecintos filtro = new FiltrosPrecintos();
			filtro.ResolutionNumber = _fixture.Create<string>();
			filtro.Establishment = _fixture.Create<int>();
			filtro.NIT = _fixture.Create<int>();
			filtro.SpecificSearch = _fixture.Create<int>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("NormalizarFiltros", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { filtro });

			Assert.NotNull(r);
		}

		[Fact]
		public void ObtenerEstablecimientosSelect()
		{
            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ObtenerEstablecimientosSelect", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] {  });

			Assert.NotNull(r);
		}
	}
}
