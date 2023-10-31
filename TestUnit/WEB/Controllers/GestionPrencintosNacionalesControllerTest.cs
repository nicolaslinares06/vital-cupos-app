using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Repository.Helpers.Models;
using Repository.Helpers;
using Repository.Models;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using Web.Controllers;
using Web.Models;
using Microsoft.Extensions.Logging;
using AutoFixture;
using Xunit;

namespace TestUnit.WEB
{
    public class GestionPrencintosNacionalesControllerTest
    {
        //Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
        private GestionPrencintosNacionalesController gestionPrencintosNacionales;
        private Fixture _fixture;
        private readonly string token;

        public GestionPrencintosNacionalesControllerTest()
        {
            gestionPrencintosNacionales = new GestionPrencintosNacionalesController(new LoggerFactory().CreateLogger<GestionPrencintosNacionalesController>(), null);
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

            gestionPrencintosNacionales.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            token = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Development.json").Build().GetValue<string>("Variables:Token");
        }

        [Fact]
        public void Index()
        {
            var r = gestionPrencintosNacionales.Index();
            Assert.True(r != null);

            gestionPrencintosNacionales.ControllerContext.HttpContext.Session.SetString("token", token);

            r = gestionPrencintosNacionales.Index();
            Assert.True(r != null);
        }

        [Fact]
        public void GenerarNumeroRadicado()
        {
            var r = gestionPrencintosNacionales.GenerarNumeroRadicado(3);
            Assert.True(r != null);

            gestionPrencintosNacionales.ControllerContext.HttpContext.Session.SetString("token", token);

            r = gestionPrencintosNacionales.GenerarNumeroRadicado(3);
            Assert.True(r != null);
        }

        [Fact]
        public void SolicitudRadicadaPendiente()
        {
            var r = gestionPrencintosNacionales.SolicitudRadicadaPendiente(3);
            Assert.True(r != null);

            gestionPrencintosNacionales.ControllerContext.HttpContext.Session.SetString("token", token);

            r = gestionPrencintosNacionales.SolicitudRadicadaPendiente(3);
            Assert.True(r != null);
        }

        [Fact]
        public void GuardarCodigoPrecintosPendiente()
        {
            var r = gestionPrencintosNacionales.GuardarCodigoPrecintosPendiente();
            Assert.True(r != null);

            gestionPrencintosNacionales.ControllerContext.HttpContext.Session.SetString("token", token);

            r = gestionPrencintosNacionales.GuardarCodigoPrecintosPendiente();
            Assert.True(r != null);
        }

        [Fact]
        public void GenerarCartaSolicitantePendiente()
        {
            var code = _fixture.Create<int>();
            var amount = _fixture.Create<int>();
            var color = _fixture.Create<string>();
            var colorCode = _fixture.Create<decimal>();
            var codeIni = _fixture.Create<decimal>();
            var codeFin = _fixture.Create<decimal>();
            var nameSpecie = _fixture.Create<string>();
            var valueConsignment = _fixture.Create<decimal>();
            var observations = _fixture.Create<string>();
            var tipoSolicitud = _fixture.Create<string>();

            var r = gestionPrencintosNacionales.GenerarCartaSolicitantePendiente(code, amount, color, colorCode, codeIni, codeFin, nameSpecie, valueConsignment, observations, tipoSolicitud);
            Assert.True(r != null);

            gestionPrencintosNacionales.ControllerContext.HttpContext.Session.SetString("token", token);

            r = gestionPrencintosNacionales.GenerarCartaSolicitantePendiente(code, amount, color, colorCode, codeIni, codeFin, nameSpecie, valueConsignment, observations, tipoSolicitud);
            Assert.True(r != null);
        }

        [Fact]
        public void Requerimiento()
        {
            var r = gestionPrencintosNacionales.Requerimiento(3);
            Assert.True(r != null);

            gestionPrencintosNacionales.ControllerContext.HttpContext.Session.SetString("token", token);

            r = gestionPrencintosNacionales.Requerimiento(3);
            Assert.True(r != null);
        }

        [Fact]
        public void SolicitudRadicadaAprobada()
        {
            var r = gestionPrencintosNacionales.SolicitudRadicadaAprobada(3);
            Assert.True(r != null);

            gestionPrencintosNacionales.ControllerContext.HttpContext.Session.SetString("token", token);

            r = gestionPrencintosNacionales.SolicitudRadicadaAprobada(3);
            Assert.True(r != null);
        }

        [Fact]
        public void SolicitudRadicadaDesistimiento()
        {
            var r = gestionPrencintosNacionales.SolicitudRadicadaDesistimiento(3);
            Assert.True(r != null);

            gestionPrencintosNacionales.ControllerContext.HttpContext.Session.SetString("token", token);

            r = gestionPrencintosNacionales.SolicitudRadicadaDesistimiento(3);
            Assert.True(r != null);
        }

        [Fact]
        public void DesistimientoSolicitudTest()
        {
            var r = gestionPrencintosNacionales.DesistimientoSolicitud(3);
            Assert.True(r != null);

            gestionPrencintosNacionales.ControllerContext.HttpContext.Session.SetString("token", token);

            r = gestionPrencintosNacionales.DesistimientoSolicitud(3);
            Assert.True(r != null);
        }

        [Fact]
        public void ConsultarPendiente()
        {
            var r = gestionPrencintosNacionales.ConsultarPendiente();
            Assert.True(r != null);

            gestionPrencintosNacionales.ControllerContext.HttpContext.Session.SetString("token", token);

            r = gestionPrencintosNacionales.ConsultarPendiente();
            Assert.True(r != null);

        }

        [Fact]
        public void ConsultarRequerimiento()
        {
            var r = gestionPrencintosNacionales.ConsultarRequerimiento();
            Assert.True(r != null);

            gestionPrencintosNacionales.ControllerContext.HttpContext.Session.SetString("token", token);

            r = gestionPrencintosNacionales.ConsultarRequerimiento();
            Assert.True(r != null);
        }

        [Fact]
        public void ConsultarAprobada()
        {
            var r = gestionPrencintosNacionales.ConsultarAprobada();
            Assert.True(r != null);

            gestionPrencintosNacionales.ControllerContext.HttpContext.Session.SetString("token", token);

            r = gestionPrencintosNacionales.ConsultarAprobada();
            Assert.True(r != null);
        }

        [Fact]
        public void ConsultarDesistimiento()
        {
            var r = gestionPrencintosNacionales.ConsultarDesistimiento();
            Assert.True(r != null);

            gestionPrencintosNacionales.ControllerContext.HttpContext.Session.SetString("token", token);

            r = gestionPrencintosNacionales.ConsultarDesistimiento();
            Assert.True(r != null);
        }

        [Fact]
        public void ConsultarFirmada()
        {
            var r = gestionPrencintosNacionales.ConsultarAsignada();
            Assert.True(r != null);

            gestionPrencintosNacionales.ControllerContext.HttpContext.Session.SetString("token", token);

            r = gestionPrencintosNacionales.ConsultarAsignada();
            Assert.True(r != null);
        }

        [Fact]
        public void ConsultarAsignada()
        {
            var r = gestionPrencintosNacionales.ConsultarAsignada();
            Assert.True(r != null);

            gestionPrencintosNacionales.ControllerContext.HttpContext.Session.SetString("token", token);

            r = gestionPrencintosNacionales.ConsultarAsignada();
            Assert.True(r != null);
        }

        [Fact]
        public void RadicarSolicitud()
        {
            SettledNationalSealsManagement datos = new SettledNationalSealsManagement();
            datos.code = 3;
            datos.codeSettled = "1234656";
            datos.date = DateTime.Now;

            var r = gestionPrencintosNacionales.RadicarSolicitud(datos);
            Assert.True(r != null);

            gestionPrencintosNacionales.ControllerContext.HttpContext.Session.SetString("token", token);

            r = gestionPrencintosNacionales.RadicarSolicitud(datos);
            Assert.True(r != null);
        }

        [Fact]
        public void Color()
        {
            var r = gestionPrencintosNacionales.Color();
            Assert.True(r != null);

            gestionPrencintosNacionales.ControllerContext.HttpContext.Session.SetString("token", token);

            r = gestionPrencintosNacionales.Color();
            Assert.True(r != null);
        }

        [Fact]
        public void Especie()
        {
            var r = gestionPrencintosNacionales.Especie();
            Assert.True(r != null);

            gestionPrencintosNacionales.ControllerContext.HttpContext.Session.SetString("token", token);

            r = gestionPrencintosNacionales.Especie();
            Assert.True(r != null);
        }

        [Fact]
        public void GenerarCodigosPrecintos()
        {
            int codeSpecies = _fixture.Create<int>();
            int initialNumber = _fixture.Create<int>();
            int finalNumber = _fixture.Create<int>();
            int color = _fixture.Create<int>();
            int amount = _fixture.Create<int>();
            int worth = _fixture.Create<int>();
            string speciesName = _fixture.Create<string>();
            string colorName = _fixture.Create<string>();
            int idSolicitud = _fixture.Create<int>();
            string tipoSolicitud = _fixture.Create<string>();
            string observaciones = _fixture.Create<string>();

            var r = gestionPrencintosNacionales.GenerarCodigosPrecintos(codeSpecies, initialNumber, finalNumber, color, amount, speciesName, colorName, worth, idSolicitud, observaciones, tipoSolicitud);
            Assert.True(r != null);

            gestionPrencintosNacionales.ControllerContext.HttpContext.Session.SetString("token", token);

            r = gestionPrencintosNacionales.GenerarCodigosPrecintos(codeSpecies, initialNumber, finalNumber, color, amount, speciesName, colorName, worth, idSolicitud, observaciones, tipoSolicitud);
            Assert.True(r != null);
        }

        [Fact]
        public void DetalleSolicitud()
        {
            var r = gestionPrencintosNacionales.DetalleSolicitud(3);
            Assert.True(r != null);

            gestionPrencintosNacionales.ControllerContext.HttpContext.Session.SetString("token", token);

            r = gestionPrencintosNacionales.DetalleSolicitud(3);
            Assert.True(r != null);
        }

        [Fact]
        public void DevolverSolicitud()
        {
            ReturnSettledNationalSealsManagement datos = new ReturnSettledNationalSealsManagement();
            datos.code = _fixture.Create<int>();
            datos.observations = _fixture.Create<string>();

            var r = gestionPrencintosNacionales.DevolverSolicitud(datos);
            Assert.True(r != null);

            gestionPrencintosNacionales.ControllerContext.HttpContext.Session.SetString("token", token);

            r = gestionPrencintosNacionales.DevolverSolicitud(datos);
            Assert.True(r != null);
        }

        [Fact]
        public void GuardarCodigosPrecintos()
        {
            GenerateSealCodes datos = new GenerateSealCodes();
            datos.code = "3";
            datos.codeSpecies = 30145;
            datos.color = 10165;
            datos.initialNumber = "1";
            datos.finalNumber = "10";

            var r = gestionPrencintosNacionales.GuardarCodigosPrecintos(datos);
            Assert.True(r != null);

            gestionPrencintosNacionales.ControllerContext.HttpContext.Session.SetString("token", token);

            r = gestionPrencintosNacionales.GuardarCodigosPrecintos(datos);
            Assert.True(r != null);
        }

        [Fact]
        public void CrearCartaSolicitantePendiente()
        {
            int code = _fixture.Create<int>();
            int amount = _fixture.Create<int>();
            string color = _fixture.Create<string>();

            var r = gestionPrencintosNacionales.CrearCartaSolicitantePendiente(code, amount, color, _fixture.Create<int>(), _fixture.Create<int>(), _fixture.Create<string>());
            Assert.True(r != null);

            gestionPrencintosNacionales.ControllerContext.HttpContext.Session.SetString("token", token);

            r = gestionPrencintosNacionales.CrearCartaSolicitantePendiente(code, amount, color, _fixture.Create<int>(), _fixture.Create<int>(), _fixture.Create<string>());
            Assert.True(r != null);
        }

        [Fact]
        public void DesistimientoSolicitud()
        {
            string base64 = "JVBERi0xLjMNCiXi48/TDQoNCjEgMCBvYmoNCjw8DQovVHlwZSAvQ2F0YWxvZw0KL091dGxpbmVzIDIgMCBSDQovUGFnZXMgMyAwIFINCj4+DQplbmRvYmoNCg0KMiAwIG9iag0KPDwNCi9UeXBlIC9PdXRsaW5lcw0KL0NvdW50IDANCj4+DQplbmRvYmoNCg0KMyAwIG9iag0KPDwNCi9UeXBlIC9QYWdlcw0KL0NvdW50IDINCi9LaWRzIFsgNCAwIFIgNiAwIFIgXSANCj4+DQplbmRvYmoNCg0KNCAwIG9iag0KPDwNCi9UeXBlIC9QYWdlDQovUGFyZW50IDMgMCBSDQovUmVzb3VyY2VzIDw8DQovRm9udCA8PA0KL0YxIDkgMCBSIA0KPj4NCi9Qcm9jU2V0IDggMCBSDQo+Pg0KL01lZGlhQm94IFswIDAgNjEyLjAwMDAgNzkyLjAwMDBdDQovQ29udGVudHMgNSAwIFINCj4+DQplbmRvYmoNCg0KNSAwIG9iag0KPDwgL0xlbmd0aCAxMDc0ID4+DQpzdHJlYW0NCjIgSg0KQlQNCjAgMCAwIHJnDQovRjEgMDAyNyBUZg0KNTcuMzc1MCA3MjIuMjgwMCBUZA0KKCBBIFNpbXBsZSBQREYgRmlsZSApIFRqDQpFVA0KQlQNCi9GMSAwMDEwIFRmDQo2OS4yNTAwIDY4OC42MDgwIFRkDQooIFRoaXMgaXMgYSBzbWFsbCBkZW1vbnN0cmF0aW9uIC5wZGYgZmlsZSAtICkgVGoNCkVUDQpCVA0KL0YxIDAwMTAgVGYNCjY5LjI1MDAgNjY0LjcwNDAgVGQNCigganVzdCBmb3IgdXNlIGluIHRoZSBWaXJ0dWFsIE1lY2hhbmljcyB0dXRvcmlhbHMuIE1vcmUgdGV4dC4gQW5kIG1vcmUgKSBUag0KRVQNCkJUDQovRjEgMDAxMCBUZg0KNjkuMjUwMCA2NTIuNzUyMCBUZA0KKCB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiApIFRqDQpFVA0KQlQNCi9GMSAwMDEwIFRmDQo2OS4yNTAwIDYyOC44NDgwIFRkDQooIEFuZCBtb3JlIHRleHQuIEFuZCBtb3JlIHRleHQuIEFuZCBtb3JlIHRleHQuIEFuZCBtb3JlIHRleHQuIEFuZCBtb3JlICkgVGoNCkVUDQpCVA0KL0YxIDAwMTAgVGYNCjY5LjI1MDAgNjE2Ljg5NjAgVGQNCiggdGV4dC4gQW5kIG1vcmUgdGV4dC4gQm9yaW5nLCB6enp6ei4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kICkgVGoNCkVUDQpCVA0KL0YxIDAwMTAgVGYNCjY5LjI1MDAgNjA0Ljk0NDAgVGQNCiggbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiApIFRqDQpFVA0KQlQNCi9GMSAwMDEwIFRmDQo2OS4yNTAwIDU5Mi45OTIwIFRkDQooIEFuZCBtb3JlIHRleHQuIEFuZCBtb3JlIHRleHQuICkgVGoNCkVUDQpCVA0KL0YxIDAwMTAgVGYNCjY5LjI1MDAgNTY5LjA4ODAgVGQNCiggQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgKSBUag0KRVQNCkJUDQovRjEgMDAxMCBUZg0KNjkuMjUwMCA1NTcuMTM2MCBUZA0KKCB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBFdmVuIG1vcmUuIENvbnRpbnVlZCBvbiBwYWdlIDIgLi4uKSBUag0KRVQNCmVuZHN0cmVhbQ0KZW5kb2JqDQoNCjYgMCBvYmoNCjw8DQovVHlwZSAvUGFnZQ0KL1BhcmVudCAzIDAgUg0KL1Jlc291cmNlcyA8PA0KL0ZvbnQgPDwNCi9GMSA5IDAgUiANCj4+DQovUHJvY1NldCA4IDAgUg0KPj4NCi9NZWRpYUJveCBbMCAwIDYxMi4wMDAwIDc5Mi4wMDAwXQ0KL0NvbnRlbnRzIDcgMCBSDQo+Pg0KZW5kb2JqDQoNCjcgMCBvYmoNCjw8IC9MZW5ndGggNjc2ID4+DQpzdHJlYW0NCjIgSg0KQlQNCjAgMCAwIHJnDQovRjEgMDAyNyBUZg0KNTcuMzc1MCA3MjIuMjgwMCBUZA0KKCBTaW1wbGUgUERGIEZpbGUgMiApIFRqDQpFVA0KQlQNCi9GMSAwMDEwIFRmDQo2OS4yNTAwIDY4OC42MDgwIFRkDQooIC4uLmNvbnRpbnVlZCBmcm9tIHBhZ2UgMS4gWWV0IG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gKSBUag0KRVQNCkJUDQovRjEgMDAxMCBUZg0KNjkuMjUwMCA2NzYuNjU2MCBUZA0KKCBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSApIFRqDQpFVA0KQlQNCi9GMSAwMDEwIFRmDQo2OS4yNTAwIDY2NC43MDQwIFRkDQooIHRleHQuIE9oLCBob3cgYm9yaW5nIHR5cGluZyB0aGlzIHN0dWZmLiBCdXQgbm90IGFzIGJvcmluZyBhcyB3YXRjaGluZyApIFRqDQpFVA0KQlQNCi9GMSAwMDEwIFRmDQo2OS4yNTAwIDY1Mi43NTIwIFRkDQooIHBhaW50IGRyeS4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gKSBUag0KRVQNCkJUDQovRjEgMDAxMCBUZg0KNjkuMjUwMCA2NDAuODAwMCBUZA0KKCBCb3JpbmcuICBNb3JlLCBhIGxpdHRsZSBtb3JlIHRleHQuIFRoZSBlbmQsIGFuZCBqdXN0IGFzIHdlbGwuICkgVGoNCkVUDQplbmRzdHJlYW0NCmVuZG9iag0KDQo4IDAgb2JqDQpbL1BERiAvVGV4dF0NCmVuZG9iag0KDQo5IDAgb2JqDQo8PA0KL1R5cGUgL0ZvbnQNCi9TdWJ0eXBlIC9UeXBlMQ0KL05hbWUgL0YxDQovQmFzZUZvbnQgL0hlbHZldGljYQ0KL0VuY29kaW5nIC9XaW5BbnNpRW5jb2RpbmcNCj4+DQplbmRvYmoNCg0KMTAgMCBvYmoNCjw8DQovQ3JlYXRvciAoUmF2ZSBcKGh0dHA6Ly93d3cubmV2cm9uYS5jb20vcmF2ZVwpKQ0KL1Byb2R1Y2VyIChOZXZyb25hIERlc2lnbnMpDQovQ3JlYXRpb25EYXRlIChEOjIwMDYwMzAxMDcyODI2KQ0KPj4NCmVuZG9iag0KDQp4cmVmDQowIDExDQowMDAwMDAwMDAwIDY1NTM1IGYNCjAwMDAwMDAwMTkgMDAwMDAgbg0KMDAwMDAwMDA5MyAwMDAwMCBuDQowMDAwMDAwMTQ3IDAwMDAwIG4NCjAwMDAwMDAyMjIgMDAwMDAgbg0KMDAwMDAwMDM5MCAwMDAwMCBuDQowMDAwMDAxNTIyIDAwMDAwIG4NCjAwMDAwMDE2OTAgMDAwMDAgbg0KMDAwMDAwMjQyMyAwMDAwMCBuDQowMDAwMDAyNDU2IDAwMDAwIG4NCjAwMDAwMDI1NzQgMDAwMDAgbg0KDQp0cmFpbGVyDQo8PA0KL1NpemUgMTENCi9Sb290IDEgMCBSDQovSW5mbyAxMCAwIFINCj4+DQoNCnN0YXJ0eHJlZg0KMjcxNA0KJSVFT0YNCg==";

            Files file = new Files();
            file.codigo = 3;
            file.adjuntoBase64 = "data:application/pdf;base64," + base64;
            file.nombreAdjunto = "SOPORTE-DESISTIMIENTO-3.pdf";
            file.tipoAdjunto = "application/pdf";

            DesistNationalSealsManagement datos = new DesistNationalSealsManagement();
            datos.code = 3;
            datos.state = 17;
            datos.file = file;
            datos.observations = "BISA";

            var r = gestionPrencintosNacionales.DesistimientoSolicitud(datos);
            Assert.True(r != null);

            gestionPrencintosNacionales.ControllerContext.HttpContext.Session.SetString("token", token);

            r = gestionPrencintosNacionales.DesistimientoSolicitud(datos);
            Assert.True(r != null);
        }

        [Fact]
        public void TraerDocumentosPrecintos()
        {

            int code = 3;
            int type = _fixture.Create<int>();

            var r = gestionPrencintosNacionales.TraerDocumentosPrecintos(code, type);
            Assert.True(r != null);

            gestionPrencintosNacionales.ControllerContext.HttpContext.Session.SetString("token", token);

            r = gestionPrencintosNacionales.TraerDocumentosPrecintos(code, type);
            Assert.True(r != null);
        }

        [Fact]
        public void RadicarSolicitudSalida()
        {
            SettledNationalSealsManagement datos = new SettledNationalSealsManagement();
            datos.code = 20073;
            datos.codeSettled = "1232";
            datos.date = DateTime.Now;

            var r = gestionPrencintosNacionales.RadicarSolicitudSalida(datos);
            Assert.True(r != null);

            gestionPrencintosNacionales.ControllerContext.HttpContext.Session.SetString("token", token);

            r = gestionPrencintosNacionales.RadicarSolicitudSalida(datos);
            Assert.True(r != null);
        }

        [Fact]
        public void ConsultarCodigosPrecintos()
        {

            int code = 20073;
            var r = gestionPrencintosNacionales.ConsultarCodigosPrecintos(code);
            Assert.True(r != null);

            gestionPrencintosNacionales.ControllerContext.HttpContext.Session.SetString("token", token);

            r = gestionPrencintosNacionales.ConsultarCodigosPrecintos(code);
            Assert.True(r != null);
        }
    }
}
