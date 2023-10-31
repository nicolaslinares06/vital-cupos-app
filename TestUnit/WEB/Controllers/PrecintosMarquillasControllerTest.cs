using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web.Controllers;
using Web.Models;

namespace TestUnit.WEB
{
    public class PrecintosMarquillasControllerTest
    {
        //Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
        private PrecintosMarquillasController precintosMarquillas;
        private Fixture _fixture;
        private readonly string token;


        public PrecintosMarquillasControllerTest()
        {
            precintosMarquillas = new PrecintosMarquillasController(new LoggerFactory().CreateLogger<PrecintosMarquillasController>());
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

            precintosMarquillas.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            token = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Development.json").Build().GetValue<string>("Variables:Token");

        }

        [Fact]
        public void Index()
        {
            var r = precintosMarquillas.Index();
            Assert.True(r != null);

            precintosMarquillas.ControllerContext.HttpContext.Session.SetString("token", token);

            r = precintosMarquillas.Index();
            Assert.True(r != null);

        }

        [Fact]
        public void Home()
        {
            var r = precintosMarquillas.Home();
            Assert.True(r != null);

            precintosMarquillas.ControllerContext.HttpContext.Session.SetString("token", token);

            r = precintosMarquillas.Home();
            Assert.True(r != null);
        }

        [Fact]
        public void Filtrar()
        {
            var tipoDocumento = _fixture.Create<string>();
            var fechaInicial = DateTime.Now;
            var numero = _fixture.Create<string>();
            var numeroDocumento = _fixture.Create<string>();
            var fechaFinal = DateTime.Now;
            var color = _fixture.Create<string>();
            var nombreEmpresa = _fixture.Create<string>();
            var vigencia = _fixture.Create<string>();

            var r = precintosMarquillas.Filtrar(tipoDocumento, fechaInicial, numero, numeroDocumento, fechaFinal, color, nombreEmpresa, vigencia);
            Assert.True(r != null);

            precintosMarquillas.ControllerContext.HttpContext.Session.SetString("token", token);

            r = precintosMarquillas.Filtrar(tipoDocumento, fechaInicial, numero, numeroDocumento, fechaFinal, color, nombreEmpresa, vigencia);
            Assert.True(r != null);
        }

        [Fact]
        public void TipoDocumentoEmpresa()
        {
            var r = precintosMarquillas.TipoDocumentoEmpresa();
            Assert.True(r != null);

            precintosMarquillas.ControllerContext.HttpContext.Session.SetString("token", token);

            r = precintosMarquillas.TipoDocumentoEmpresa();
            Assert.True(r != null);
        }

        [Fact]
        public void Color()
        {
            var r = precintosMarquillas.Color();
            Assert.True(r != null);

            precintosMarquillas.ControllerContext.HttpContext.Session.SetString("token", token);

            r = precintosMarquillas.Color();
            Assert.True(r != null);
        }
    }
}
