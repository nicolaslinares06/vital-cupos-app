using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Web.Controllers;
using Web.Models;
using Microsoft.Extensions.Logging;
using WebFront.Controllers;
using CUPOS_FRONT.Controllers;
using static CUPOS_FRONT.Models.ReportesMarquillasModels;

namespace TestUnit.WEB.Controllers
{
    public class ReportesMarquillasControllerTest
    {
        //Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
        private ReportesMarquillasController controller;
        private Fixture _fixture;

        public ReportesMarquillasControllerTest()
        {
            controller = new ReportesMarquillasController(null, null, new LoggerFactory().CreateLogger<ReportesMarquillasController>());
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

            var viewResult = Assert.IsType<ViewResult>(r);

            Assert.NotNull(viewResult);
        }

        [Fact]
        public void Crear()
        {
            FiltrosMarquillas datos = new FiltrosMarquillas();
            datos.radicationNumber = _fixture.Create<string>();
            datos.dateFrom = DateTime.Now;
            datos.dateTo = DateTime.Now;

            var r = controller.ObtenerDatosMarquillas(datos);
            Assert.True(r != null);
        }
    }
}
