using AutoFixture;
using DocumentFormat.OpenXml.Office2010.Word;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Repository.Helpers.Models;
using System.Security.Claims;
using Web.Controllers;
using Web.Models;
using WebFront.Controllers;
using static CUPOS_FRONT.Models.Requests;
using static Web.Models.ResolucionPeces;

namespace TestUnit.WEB
{
	public class CvControllerTest
	{
		//Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
		private CvController controller;
		private Roles model;
        private Fixture _fixture;
        private readonly string token;

        public CvControllerTest()
		{
			controller = new CvController(new LoggerFactory().CreateLogger<CvController>());
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
			decimal documentypecv = _fixture.Create<int>();
			decimal documentid = _fixture.Create<int>();

			var r = controller.Index(documentypecv, documentid);
			Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.Index(documentypecv, documentid);
            Assert.True(r != null);
        }

        [Fact]
		public void Index1()
		{
			decimal documentypecv = _fixture.Create<int>();
			decimal documentid = _fixture.Create<int>();

			var r = controller.Index1(documentypecv, documentid);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.Index1(documentypecv, documentid);
            Assert.True(r != null);
        }

		[Fact]
		public void Index2()
		{
			decimal documentypecv = _fixture.Create<int>();
			decimal documentid = _fixture.Create<int>();

			var r = controller.Index2(documentypecv, documentid);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.Index2(documentypecv, documentid);
            Assert.True(r != null);
        }

		[Fact]
		public void Buscar()
		{
			decimal documentypecv = _fixture.Create<int>();
			decimal documentid = _fixture.Create<int>();

			var r = controller.Buscar(documentypecv, documentid);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.Buscar(documentypecv, documentid);
            Assert.True(r != null);
        }

		[Fact]
		public void Situacion()
		{
			decimal documentypecv = _fixture.Create<int>();
			decimal documentid = _fixture.Create<int>();

			var r = controller.Situacion(documentypecv, documentid);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.Situacion(documentypecv, documentid);
            Assert.True(r != null);
        }

		[Fact]
		public void Resolucioncupos()
		{
			decimal documentypecv = _fixture.Create<int>();
			string documentid = _fixture.Create<string>();

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

			var r = controller.Resolucioncupos(null, documentypecv, documentid);
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultPeces()
		{
			decimal documentid = _fixture.Create<int>();

			var r = controller.ConsultPeces(null, documentid);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultPeces(null, documentid);
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultCertificateshj()
		{
			var r = controller.ConsultCertificateshj();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultCertificateshj();
            Assert.True(r != null);
        }

		[Fact]
		public void DocumentoVenta()
		{
            decimal nit = _fixture.Create<decimal>();

            var r = controller.DocumentoVenta(nit);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.DocumentoVenta(nit);
            Assert.True(r != null);
        }

		[Fact]
		public void DocumentoVenta2()
		{
			var r = controller.DocumentoVenta2();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.DocumentoVenta2();
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultOneQuota2()
		{
			decimal quotaCode = _fixture.Create<int>();

			var r = controller.ConsultOneQuota2(null, quotaCode);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultOneQuota2(null, quotaCode);
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultDocument2()
		{
			decimal docuid = _fixture.Create<int>();

			var r = controller.ConsultOneQuota2(null, docuid);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultOneQuota2(null, docuid);
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultDocumentid()
		{
			decimal docuid = _fixture.Create<int>();

			var r = controller.ConsultDocumentid(null, docuid);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultDocumentid(null, docuid);
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultCertificateshj2()
		{
			decimal idcertificado = _fixture.Create<int>();
			var r = controller.ConsultCertificateshj2(null, idcertificado);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultCertificateshj2(null, idcertificado);
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultSituacionpdf()
		{
			decimal situacionid = _fixture.Create<int>();

			var r = controller.ConsultSituacionpdf(null, situacionid);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultSituacionpdf(null, situacionid);
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultSituacionid()
		{
			decimal codigoEmpresa = _fixture.Create<int>();
			decimal situacionid = _fixture.Create<int>();

			var r = controller.ConsultSituacionid(null, codigoEmpresa, situacionid);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultSituacionid(null, codigoEmpresa, situacionid);
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultSituacionnovedad()
		{
			decimal codigoEmpresa = _fixture.Create<int>();

			var r = controller.ConsultSituacionnovedad(null, codigoEmpresa);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultSituacionnovedad(null, codigoEmpresa);
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultSituacionnovedadultima()
		{
			decimal codigoEmpresa = _fixture.Create<int>();

			var r = controller.ConsultSituacionnovedadultima(null, codigoEmpresa);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultSituacionnovedadultima(null, codigoEmpresa);
            Assert.True(r != null);
        }

		[Fact]
		public void Consultpecespdf()
		{
			decimal idresolucionp = _fixture.Create<int>();

			var r = controller.ConsultSituacionnovedadultima(null, idresolucionp);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultSituacionnovedadultima(null, idresolucionp);
            Assert.True(r != null);
        }
	}
}