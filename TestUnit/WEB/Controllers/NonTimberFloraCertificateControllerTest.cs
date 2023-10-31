using AutoFixture;
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

namespace TestUnit.WEB
{
	public class NonTimberFloraCertificateControllerTest
	{
		//Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
		private NonTimberFloraCertificateController controller;
        private Fixture _fixture;
        private readonly string token;

        public NonTimberFloraCertificateControllerTest()
		{
			controller = new NonTimberFloraCertificateController(new LoggerFactory().CreateLogger<NonTimberFloraCertificateController>());
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
		public void NonTimberFloraCertificate()
		{
			var r = controller.NonTimberFloraCertificate();
			Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.NonTimberFloraCertificate();
            Assert.True(r != null);
        }

        [Fact]
		public void ConsultAuthority()
		{
			var r = controller.ConsultAuthority();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultAuthority();
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultEspecimensProductsType()
		{
			var r = controller.ConsultEspecimensProductsType();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultEspecimensProductsType();
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultDocumentsTypes()
		{
			var r = controller.ConsultDocumentsTypes();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultDocumentsTypes();
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultCertificates()
		{
			var r = controller.ConsultCertificates();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultCertificates();
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultEntityDates()
		{
			decimal documentType = _fixture.Create<int>();
			decimal ResolutionNumbre = _fixture.Create<int>();

			var r = controller.ConsultEntityDates(documentType, ResolutionNumbre);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultEntityDates(documentType, ResolutionNumbre);
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultCertificatesForNit()
		{
			decimal documentType = _fixture.Create<int>();
			decimal DocumentNumber = _fixture.Create<int>();
			string CertificateNumber = "100";

			var r = controller.ConsultCertificatesForNit(documentType, DocumentNumber, CertificateNumber);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultCertificatesForNit(documentType, DocumentNumber, CertificateNumber);
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultEspecimensTypesNon()
		{
			var r = controller.ConsultEspecimensTypes();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultEspecimensTypes();
            Assert.True(r != null);
        }

		[Fact]
		public void SaveCertificate()
		{
			CertificatesDatas datos = new CertificatesDatas();
			datos.codigo = _fixture.Create<int>();
			datos.autoridadEmiteCertificacion = _fixture.Create<string>();
			datos.numeroCertificado = _fixture.Create<string>();
			datos.fechaCertificacion = DateTime.Now;
			datos.vigenciaCertificacion = DateTime.Now;
			datos.tipoPermiso = _fixture.Create<string>();
			datos.tipoEspecimenProductoImpExp = _fixture.Create<string>();
			datos.observacionesCertificado = _fixture.Create<string>();
			datos.nitEmpresa = _fixture.Create<int>();
			datos.documentosSoporte = null;
			datos.documentosSoporteNuevo = null;
			datos.documentosSoporteEliminar = null;

			var r = controller.SaveCertificate(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.SaveCertificate(datos);
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultDatasCertificate()
		{
			decimal codeCertificate = _fixture.Create<int>();

			var r = controller.ConsultDatasCertificate(codeCertificate);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultDatasCertificate(codeCertificate);
            Assert.True(r != null);
        }

		[Fact]
		public void SaveEditCertificate()
		{
			CertificatesDatas datos = new CertificatesDatas();
			datos.codigo = _fixture.Create<int>();
			datos.autoridadEmiteCertificacion = _fixture.Create<string>();
			datos.numeroCertificado = _fixture.Create<string>();
			datos.fechaCertificacion = DateTime.Now;
			datos.vigenciaCertificacion = DateTime.Now;
			datos.tipoPermiso = _fixture.Create<string>();
			datos.tipoEspecimenProductoImpExp = _fixture.Create<string>();
			datos.observacionesCertificado = _fixture.Create<string>();
			datos.nitEmpresa = _fixture.Create<int>();
			datos.documentosSoporte = null;
			datos.documentosSoporteNuevo = null;
			datos.documentosSoporteEliminar = null;

			var r = controller.SaveEditCertificate(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.SaveEditCertificate(datos);
            Assert.True(r != null);
        }

		[Fact]
		public void DeleteCertificate()
		{
			decimal codeCertificate = _fixture.Create<int>();

			var r = controller.DeleteCertificate(codeCertificate);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.DeleteCertificate(codeCertificate);
            Assert.True(r != null);
        }
	}
}
