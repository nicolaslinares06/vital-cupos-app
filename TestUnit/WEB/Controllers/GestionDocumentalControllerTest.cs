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
using System.Security.Policy;

namespace TestUnit.WEB
{
	public class GestionDocumentalControllerTest
	{
		//Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
		private GestionDocumentalController controller;
        private Fixture _fixture; 
		private readonly string token;

        public GestionDocumentalControllerTest()
		{
			controller = new GestionDocumentalController(new LoggerFactory().CreateLogger<GestionDocumentalController>());
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
		public void EditarArchivo()
		{
			decimal id = _fixture.Create<int>(); 

			var r = controller.EditarArchivo(id);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.EditarArchivo(id);
            Assert.True(r != null);
        }

		[Fact]
		public void AdjuntarArchivo()
		{
			decimal id = _fixture.Create<int>();
			string nombre = _fixture.Create<string>(); 
			string url = _fixture.Create<string>();

			var r = controller.AdjuntarArchivo(id, nombre, url);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.AdjuntarArchivo(id, nombre, url);
            Assert.True(r != null);
        }

		[Fact]
		public void BusquedaDocumentos()
		{
			string cadBusqueda = _fixture.Create<string>();

			var r = controller.BusquedaDocumentos(cadBusqueda);
            Assert.True(r != null);

			controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.BusquedaDocumentos(cadBusqueda);
            Assert.True(r != null);
        }

		[Fact]
		public void ObtenerDocumentos()
		{
			decimal id = _fixture.Create<int>();

			var r = controller.ObtenerDocumentos(id);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ObtenerDocumentos(id);
            Assert.True(r != null);
        }

		[Fact]
		public void SaveDocument()
		{
            soportsDocuments doc = new soportsDocuments();
            doc.codigo = _fixture.Create<int>();
            doc.adjuntoBase64 = _fixture.Create<string>();
            doc.nombreAdjunto = _fixture.Create<string>();
            doc.tipoAdjunto = _fixture.Create<string>();

            ReqGuardarDoc datos = new ReqGuardarDoc();
			datos.id = _fixture.Create<int>();
			datos.archivo = doc;


            var r = controller.SaveDocument(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.SaveDocument(datos);
            Assert.True(r != null);
        }

		[Fact]
		public void LeerDocumento()
		{
			decimal id = _fixture.Create<int>();

			var r = controller.LeerDocumento(id);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.LeerDocumento(id);
            Assert.True(r != null);
        }
	}
}
