using AutoFixture;
using CUPOS_FRONT.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repository.Helpers.Models;
using System.Reflection;
using System.Security.Claims;
using Web.Controllers;
using Web.Models;

namespace TestUnit.WEB
{
	public class RegistroVisitaDeCortesControllerTest
	{
		//Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
		private RegistroVisitaDeCortesController controller;
        private Fixture _fixture;
        private readonly string token;
        private readonly string base64;

        public RegistroVisitaDeCortesControllerTest()
		{
			controller = new RegistroVisitaDeCortesController(null, null, new LoggerFactory().CreateLogger<RegistroVisitaDeCortesController>());
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
            base64 = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Development.json").Build().GetValue<string>("Variables:Base64");

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
		public void ConsultarActaVisita()
		{
			decimal idActaVisita = _fixture.Create<int>();

			var r = controller.ConsultarActaVisita(idActaVisita);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultarActaVisita(idActaVisita);
            Assert.True(r != null);
        }

		[Fact]
		public void CreateTipoActaCorte()
		{
			var r = controller.CreateTipoActaCorte();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.CreateTipoActaCorte();
            Assert.True(r != null);
        }

		[Fact]
		public void CreateActaCortePartesIdentificables()
		{
			var r = controller.CreateActaCortePartesIdentificables();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.CreateActaCortePartesIdentificables();
            Assert.True(r != null);
        }

		[Fact]
		public void EditActaCortePartesIdentificables()
		{
			int idActaVisita = _fixture.Create<int>();

			var r = controller.EditActaCortePartesIdentificables(idActaVisita);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.EditActaCortePartesIdentificables(idActaVisita);
            Assert.True(r != null);
        }

		[Fact]
		public void EditActaCortesFraccionesIrregulares()
		{
			int idActaVisita = _fixture.Create<int>();

			var r = controller.EditActaCortesFraccionesIrregulares(idActaVisita);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.EditActaCortesFraccionesIrregulares(idActaVisita);
            Assert.True(r != null);
        }

		[Fact]
		public void IndexRegistro()
		{
			RegistroActasBusqueda datos = new RegistroActasBusqueda();
			datos.IdEstablecimiento = _fixture.Create<int>();
			datos.IdTipoEstablecimiento = _fixture.Create<int>();
			datos.FechaVisita = DateTime.Now;
			datos.TipoBusqueda = _fixture.Create<int>();

			var r = controller.Index(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.Index(datos);
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultarActas()
		{
			RegistroActasBusqueda datos = new RegistroActasBusqueda();
			datos.IdEstablecimiento = _fixture.Create<int>();
			datos.IdTipoEstablecimiento = _fixture.Create<int>();
			datos.FechaVisita = DateTime.Now;
			datos.TipoBusqueda = _fixture.Create<int>();

			var r = controller.ConsultarActas(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultarActas(datos);
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultarCiudades()
		{
			decimal departamentoId = _fixture.Create<int>();

			var r = controller.ConsultarCiudades(departamentoId);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultarCiudades(departamentoId);
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultarEstablecimientosPorTipo()
		{
			decimal tipoEstablecimientoId = _fixture.Create<int>();

			var r = controller.ConsultarEstablecimientosPorTipo(tipoEstablecimientoId);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultarEstablecimientosPorTipo(tipoEstablecimientoId);
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultarDatosEmpresaPorNit()
		{
			decimal NitEmpresa = _fixture.Create<int>();

			var r = controller.ConsultarDatosEmpresaPorNit(NitEmpresa);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ConsultarDatosEmpresaPorNit(NitEmpresa);
            Assert.True(r != null);
        }

		[Fact]
		public void ValidarDatoslArchivoPrecintos()
		{
			string base64ArchivoExcelPrecintos = _fixture.Create<string>();

			var r = controller.ValidarDatoslArchivoPrecintos(base64ArchivoExcelPrecintos, _fixture.Create<int>());
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ValidarDatoslArchivoPrecintos(base64ArchivoExcelPrecintos, _fixture.Create<int>());
            Assert.True(r != null);
        }

		[Fact]
		public void CreateActaCortePartesIdentificablesResgistro()
		{
			TipoCortesPielIdentificables tcpi = new TipoCortesPielIdentificables();
			tcpi.TipoPiel = "1";
			tcpi.Cantidad = _fixture.Create<int>();
			tcpi.CodigoActaVisita = _fixture.Create<int>();

			List<TipoCortesPielIdentificables> listtcpi = new List<TipoCortesPielIdentificables>();

			listtcpi.Add(tcpi);

			TipoPartesPielIdentificables tppi = new TipoPartesPielIdentificables();
			tppi.TipoParte = "1";
			tppi.Cantidad = _fixture.Create<int>();
			tppi.CodigoActaVisita = _fixture.Create<int>();

			List<TipoPartesPielIdentificables> listtppi = new List<TipoPartesPielIdentificables>();

			listtppi.Add(tppi);

			string base64 = "JVBERi0xLjMNCiXi48/TDQoNCjEgMCBvYmoNCjw8DQovVHlwZSAvQ2F0YWxvZw0KL091dGxpbmVzIDIgMCBSDQovUGFnZXMgMyAwIFINCj4+DQplbmRvYmoNCg0KMiAwIG9iag0KPDwNCi9UeXBlIC9PdXRsaW5lcw0KL0NvdW50IDANCj4+DQplbmRvYmoNCg0KMyAwIG9iag0KPDwNCi9UeXBlIC9QYWdlcw0KL0NvdW50IDINCi9LaWRzIFsgNCAwIFIgNiAwIFIgXSANCj4+DQplbmRvYmoNCg0KNCAwIG9iag0KPDwNCi9UeXBlIC9QYWdlDQovUGFyZW50IDMgMCBSDQovUmVzb3VyY2VzIDw8DQovRm9udCA8PA0KL0YxIDkgMCBSIA0KPj4NCi9Qcm9jU2V0IDggMCBSDQo+Pg0KL01lZGlhQm94IFswIDAgNjEyLjAwMDAgNzkyLjAwMDBdDQovQ29udGVudHMgNSAwIFINCj4+DQplbmRvYmoNCg0KNSAwIG9iag0KPDwgL0xlbmd0aCAxMDc0ID4+DQpzdHJlYW0NCjIgSg0KQlQNCjAgMCAwIHJnDQovRjEgMDAyNyBUZg0KNTcuMzc1MCA3MjIuMjgwMCBUZA0KKCBBIFNpbXBsZSBQREYgRmlsZSApIFRqDQpFVA0KQlQNCi9GMSAwMDEwIFRmDQo2OS4yNTAwIDY4OC42MDgwIFRkDQooIFRoaXMgaXMgYSBzbWFsbCBkZW1vbnN0cmF0aW9uIC5wZGYgZmlsZSAtICkgVGoNCkVUDQpCVA0KL0YxIDAwMTAgVGYNCjY5LjI1MDAgNjY0LjcwNDAgVGQNCigganVzdCBmb3IgdXNlIGluIHRoZSBWaXJ0dWFsIE1lY2hhbmljcyB0dXRvcmlhbHMuIE1vcmUgdGV4dC4gQW5kIG1vcmUgKSBUag0KRVQNCkJUDQovRjEgMDAxMCBUZg0KNjkuMjUwMCA2NTIuNzUyMCBUZA0KKCB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiApIFRqDQpFVA0KQlQNCi9GMSAwMDEwIFRmDQo2OS4yNTAwIDYyOC44NDgwIFRkDQooIEFuZCBtb3JlIHRleHQuIEFuZCBtb3JlIHRleHQuIEFuZCBtb3JlIHRleHQuIEFuZCBtb3JlIHRleHQuIEFuZCBtb3JlICkgVGoNCkVUDQpCVA0KL0YxIDAwMTAgVGYNCjY5LjI1MDAgNjE2Ljg5NjAgVGQNCiggdGV4dC4gQW5kIG1vcmUgdGV4dC4gQm9yaW5nLCB6enp6ei4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kICkgVGoNCkVUDQpCVA0KL0YxIDAwMTAgVGYNCjY5LjI1MDAgNjA0Ljk0NDAgVGQNCiggbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiApIFRqDQpFVA0KQlQNCi9GMSAwMDEwIFRmDQo2OS4yNTAwIDU5Mi45OTIwIFRkDQooIEFuZCBtb3JlIHRleHQuIEFuZCBtb3JlIHRleHQuICkgVGoNCkVUDQpCVA0KL0YxIDAwMTAgVGYNCjY5LjI1MDAgNTY5LjA4ODAgVGQNCiggQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgKSBUag0KRVQNCkJUDQovRjEgMDAxMCBUZg0KNjkuMjUwMCA1NTcuMTM2MCBUZA0KKCB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBFdmVuIG1vcmUuIENvbnRpbnVlZCBvbiBwYWdlIDIgLi4uKSBUag0KRVQNCmVuZHN0cmVhbQ0KZW5kb2JqDQoNCjYgMCBvYmoNCjw8DQovVHlwZSAvUGFnZQ0KL1BhcmVudCAzIDAgUg0KL1Jlc291cmNlcyA8PA0KL0ZvbnQgPDwNCi9GMSA5IDAgUiANCj4+DQovUHJvY1NldCA4IDAgUg0KPj4NCi9NZWRpYUJveCBbMCAwIDYxMi4wMDAwIDc5Mi4wMDAwXQ0KL0NvbnRlbnRzIDcgMCBSDQo+Pg0KZW5kb2JqDQoNCjcgMCBvYmoNCjw8IC9MZW5ndGggNjc2ID4+DQpzdHJlYW0NCjIgSg0KQlQNCjAgMCAwIHJnDQovRjEgMDAyNyBUZg0KNTcuMzc1MCA3MjIuMjgwMCBUZA0KKCBTaW1wbGUgUERGIEZpbGUgMiApIFRqDQpFVA0KQlQNCi9GMSAwMDEwIFRmDQo2OS4yNTAwIDY4OC42MDgwIFRkDQooIC4uLmNvbnRpbnVlZCBmcm9tIHBhZ2UgMS4gWWV0IG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gKSBUag0KRVQNCkJUDQovRjEgMDAxMCBUZg0KNjkuMjUwMCA2NzYuNjU2MCBUZA0KKCBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSApIFRqDQpFVA0KQlQNCi9GMSAwMDEwIFRmDQo2OS4yNTAwIDY2NC43MDQwIFRkDQooIHRleHQuIE9oLCBob3cgYm9yaW5nIHR5cGluZyB0aGlzIHN0dWZmLiBCdXQgbm90IGFzIGJvcmluZyBhcyB3YXRjaGluZyApIFRqDQpFVA0KQlQNCi9GMSAwMDEwIFRmDQo2OS4yNTAwIDY1Mi43NTIwIFRkDQooIHBhaW50IGRyeS4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gKSBUag0KRVQNCkJUDQovRjEgMDAxMCBUZg0KNjkuMjUwMCA2NDAuODAwMCBUZA0KKCBCb3JpbmcuICBNb3JlLCBhIGxpdHRsZSBtb3JlIHRleHQuIFRoZSBlbmQsIGFuZCBqdXN0IGFzIHdlbGwuICkgVGoNCkVUDQplbmRzdHJlYW0NCmVuZG9iag0KDQo4IDAgb2JqDQpbL1BERiAvVGV4dF0NCmVuZG9iag0KDQo5IDAgb2JqDQo8PA0KL1R5cGUgL0ZvbnQNCi9TdWJ0eXBlIC9UeXBlMQ0KL05hbWUgL0YxDQovQmFzZUZvbnQgL0hlbHZldGljYQ0KL0VuY29kaW5nIC9XaW5BbnNpRW5jb2RpbmcNCj4+DQplbmRvYmoNCg0KMTAgMCBvYmoNCjw8DQovQ3JlYXRvciAoUmF2ZSBcKGh0dHA6Ly93d3cubmV2cm9uYS5jb20vcmF2ZVwpKQ0KL1Byb2R1Y2VyIChOZXZyb25hIERlc2lnbnMpDQovQ3JlYXRpb25EYXRlIChEOjIwMDYwMzAxMDcyODI2KQ0KPj4NCmVuZG9iag0KDQp4cmVmDQowIDExDQowMDAwMDAwMDAwIDY1NTM1IGYNCjAwMDAwMDAwMTkgMDAwMDAgbg0KMDAwMDAwMDA5MyAwMDAwMCBuDQowMDAwMDAwMTQ3IDAwMDAwIG4NCjAwMDAwMDAyMjIgMDAwMDAgbg0KMDAwMDAwMDM5MCAwMDAwMCBuDQowMDAwMDAxNTIyIDAwMDAwIG4NCjAwMDAwMDE2OTAgMDAwMDAgbg0KMDAwMDAwMjQyMyAwMDAwMCBuDQowMDAwMDAyNDU2IDAwMDAwIG4NCjAwMDAwMDI1NzQgMDAwMDAgbg0KDQp0cmFpbGVyDQo8PA0KL1NpemUgMTENCi9Sb290IDEgMCBSDQovSW5mbyAxMCAwIFINCj4+DQoNCnN0YXJ0eHJlZg0KMjcxNA0KJSVFT0YNCg==";

			ActaVisitasPropArchivos file = new ActaVisitasPropArchivos();
			file.Code = 3;
			file.Base64String = "data:application/pdf;base64," + base64;
			file.FileName = "SOPORTE-DESISTIMIENTO-3.pdf";
			file.FileType = "application/pdf";

			List<ActaVisitasPropArchivos> avpa = new List<ActaVisitasPropArchivos>();
			avpa.Add(file);

			IdRegistrosDocumentos ird = new IdRegistrosDocumentos();
			ird.NumeroDocumento = _fixture.Create<int>();

			List<IdRegistrosDocumentos> listird = new List<IdRegistrosDocumentos>();
			listird.Add(ird);	

			RegistroVisitaCortesIdentificables datos = new RegistroVisitaCortesIdentificables();
			datos.TipoCortesPiel = listtcpi;
			datos.TipoPartes = listtppi;
			datos.Archivos = avpa;
			datos.ArchivoExcelPrecinto = file;
			datos.Archivos = avpa;
			datos.CantidadPielACortar = _fixture.Create<int>();
			datos.Ciudad = _fixture.Create<int>();
			datos.DocumentoOrigenPiel = null;
			datos.DocumentoRepresentante = _fixture.Create<int>();
			datos.EstablecimientoID = _fixture.Create<int>();
			datos.EstadoPiel = "1";
			datos.Fecha = DateTime.Now;
			datos.FuncionarioAutoridadCites = _fixture.Create<int>();
			datos.PrecintoIdentificacion = _fixture.Create<int>();
			datos.RepresentanteEstablecimiento = "bisa";
			datos.ResolucionorigenPiel = listird;
			datos.SalvoCondcutoNumeroOrigenPiel = listird;
			datos.TipoEstablecimiento = _fixture.Create<int>();
			datos.VisitaNumero = _fixture.Create<int>();
			datos.VisitaNumero1 = _fixture.Create<bool>();
			datos.VisitaNumero2 = _fixture.Create<bool>();

			var r = controller.CreateActaCortePartesIdentificables(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.CreateActaCortePartesIdentificables(datos);
            Assert.True(r != null);
        }

		[Fact]
		public void CreateActaVisitaCorteIrregular()
		{
			TiposPielIrregulares tcpi = new TiposPielIrregulares();
			tcpi.TipoPielIrregular = "1";
			tcpi.AreaPromedioTipoPiel = "1";
			tcpi.CantidadTipoPiel = _fixture.Create<int>();
			tcpi.AreaTotalTipoPiel = _fixture.Create<int>();
			tcpi.CodigoActaVisita = _fixture.Create<int>();

			List<TiposPielIrregulares> listtcpi = new List<TiposPielIrregulares>();

			listtcpi.Add(tcpi);

			TiposParteIrregulares tppi = new TiposParteIrregulares();
			tppi.TipoParte = "1";
			tppi.CantidadTipoParte = _fixture.Create<int>();
			tppi.AreaTotalTipoParte = _fixture.Create<int>();
			tppi.CodigoActaVisita = _fixture.Create<int>();

			List<TiposParteIrregulares> listtppi = new List<TiposParteIrregulares>();

			listtppi.Add(tppi);

			string base64 = "JVBERi0xLjMNCiXi48/TDQoNCjEgMCBvYmoNCjw8DQovVHlwZSAvQ2F0YWxvZw0KL091dGxpbmVzIDIgMCBSDQovUGFnZXMgMyAwIFINCj4+DQplbmRvYmoNCg0KMiAwIG9iag0KPDwNCi9UeXBlIC9PdXRsaW5lcw0KL0NvdW50IDANCj4+DQplbmRvYmoNCg0KMyAwIG9iag0KPDwNCi9UeXBlIC9QYWdlcw0KL0NvdW50IDINCi9LaWRzIFsgNCAwIFIgNiAwIFIgXSANCj4+DQplbmRvYmoNCg0KNCAwIG9iag0KPDwNCi9UeXBlIC9QYWdlDQovUGFyZW50IDMgMCBSDQovUmVzb3VyY2VzIDw8DQovRm9udCA8PA0KL0YxIDkgMCBSIA0KPj4NCi9Qcm9jU2V0IDggMCBSDQo+Pg0KL01lZGlhQm94IFswIDAgNjEyLjAwMDAgNzkyLjAwMDBdDQovQ29udGVudHMgNSAwIFINCj4+DQplbmRvYmoNCg0KNSAwIG9iag0KPDwgL0xlbmd0aCAxMDc0ID4+DQpzdHJlYW0NCjIgSg0KQlQNCjAgMCAwIHJnDQovRjEgMDAyNyBUZg0KNTcuMzc1MCA3MjIuMjgwMCBUZA0KKCBBIFNpbXBsZSBQREYgRmlsZSApIFRqDQpFVA0KQlQNCi9GMSAwMDEwIFRmDQo2OS4yNTAwIDY4OC42MDgwIFRkDQooIFRoaXMgaXMgYSBzbWFsbCBkZW1vbnN0cmF0aW9uIC5wZGYgZmlsZSAtICkgVGoNCkVUDQpCVA0KL0YxIDAwMTAgVGYNCjY5LjI1MDAgNjY0LjcwNDAgVGQNCigganVzdCBmb3IgdXNlIGluIHRoZSBWaXJ0dWFsIE1lY2hhbmljcyB0dXRvcmlhbHMuIE1vcmUgdGV4dC4gQW5kIG1vcmUgKSBUag0KRVQNCkJUDQovRjEgMDAxMCBUZg0KNjkuMjUwMCA2NTIuNzUyMCBUZA0KKCB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiApIFRqDQpFVA0KQlQNCi9GMSAwMDEwIFRmDQo2OS4yNTAwIDYyOC44NDgwIFRkDQooIEFuZCBtb3JlIHRleHQuIEFuZCBtb3JlIHRleHQuIEFuZCBtb3JlIHRleHQuIEFuZCBtb3JlIHRleHQuIEFuZCBtb3JlICkgVGoNCkVUDQpCVA0KL0YxIDAwMTAgVGYNCjY5LjI1MDAgNjE2Ljg5NjAgVGQNCiggdGV4dC4gQW5kIG1vcmUgdGV4dC4gQm9yaW5nLCB6enp6ei4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kICkgVGoNCkVUDQpCVA0KL0YxIDAwMTAgVGYNCjY5LjI1MDAgNjA0Ljk0NDAgVGQNCiggbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiApIFRqDQpFVA0KQlQNCi9GMSAwMDEwIFRmDQo2OS4yNTAwIDU5Mi45OTIwIFRkDQooIEFuZCBtb3JlIHRleHQuIEFuZCBtb3JlIHRleHQuICkgVGoNCkVUDQpCVA0KL0YxIDAwMTAgVGYNCjY5LjI1MDAgNTY5LjA4ODAgVGQNCiggQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgKSBUag0KRVQNCkJUDQovRjEgMDAxMCBUZg0KNjkuMjUwMCA1NTcuMTM2MCBUZA0KKCB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBFdmVuIG1vcmUuIENvbnRpbnVlZCBvbiBwYWdlIDIgLi4uKSBUag0KRVQNCmVuZHN0cmVhbQ0KZW5kb2JqDQoNCjYgMCBvYmoNCjw8DQovVHlwZSAvUGFnZQ0KL1BhcmVudCAzIDAgUg0KL1Jlc291cmNlcyA8PA0KL0ZvbnQgPDwNCi9GMSA5IDAgUiANCj4+DQovUHJvY1NldCA4IDAgUg0KPj4NCi9NZWRpYUJveCBbMCAwIDYxMi4wMDAwIDc5Mi4wMDAwXQ0KL0NvbnRlbnRzIDcgMCBSDQo+Pg0KZW5kb2JqDQoNCjcgMCBvYmoNCjw8IC9MZW5ndGggNjc2ID4+DQpzdHJlYW0NCjIgSg0KQlQNCjAgMCAwIHJnDQovRjEgMDAyNyBUZg0KNTcuMzc1MCA3MjIuMjgwMCBUZA0KKCBTaW1wbGUgUERGIEZpbGUgMiApIFRqDQpFVA0KQlQNCi9GMSAwMDEwIFRmDQo2OS4yNTAwIDY4OC42MDgwIFRkDQooIC4uLmNvbnRpbnVlZCBmcm9tIHBhZ2UgMS4gWWV0IG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gKSBUag0KRVQNCkJUDQovRjEgMDAxMCBUZg0KNjkuMjUwMCA2NzYuNjU2MCBUZA0KKCBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSApIFRqDQpFVA0KQlQNCi9GMSAwMDEwIFRmDQo2OS4yNTAwIDY2NC43MDQwIFRkDQooIHRleHQuIE9oLCBob3cgYm9yaW5nIHR5cGluZyB0aGlzIHN0dWZmLiBCdXQgbm90IGFzIGJvcmluZyBhcyB3YXRjaGluZyApIFRqDQpFVA0KQlQNCi9GMSAwMDEwIFRmDQo2OS4yNTAwIDY1Mi43NTIwIFRkDQooIHBhaW50IGRyeS4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gKSBUag0KRVQNCkJUDQovRjEgMDAxMCBUZg0KNjkuMjUwMCA2NDAuODAwMCBUZA0KKCBCb3JpbmcuICBNb3JlLCBhIGxpdHRsZSBtb3JlIHRleHQuIFRoZSBlbmQsIGFuZCBqdXN0IGFzIHdlbGwuICkgVGoNCkVUDQplbmRzdHJlYW0NCmVuZG9iag0KDQo4IDAgb2JqDQpbL1BERiAvVGV4dF0NCmVuZG9iag0KDQo5IDAgb2JqDQo8PA0KL1R5cGUgL0ZvbnQNCi9TdWJ0eXBlIC9UeXBlMQ0KL05hbWUgL0YxDQovQmFzZUZvbnQgL0hlbHZldGljYQ0KL0VuY29kaW5nIC9XaW5BbnNpRW5jb2RpbmcNCj4+DQplbmRvYmoNCg0KMTAgMCBvYmoNCjw8DQovQ3JlYXRvciAoUmF2ZSBcKGh0dHA6Ly93d3cubmV2cm9uYS5jb20vcmF2ZVwpKQ0KL1Byb2R1Y2VyIChOZXZyb25hIERlc2lnbnMpDQovQ3JlYXRpb25EYXRlIChEOjIwMDYwMzAxMDcyODI2KQ0KPj4NCmVuZG9iag0KDQp4cmVmDQowIDExDQowMDAwMDAwMDAwIDY1NTM1IGYNCjAwMDAwMDAwMTkgMDAwMDAgbg0KMDAwMDAwMDA5MyAwMDAwMCBuDQowMDAwMDAwMTQ3IDAwMDAwIG4NCjAwMDAwMDAyMjIgMDAwMDAgbg0KMDAwMDAwMDM5MCAwMDAwMCBuDQowMDAwMDAxNTIyIDAwMDAwIG4NCjAwMDAwMDE2OTAgMDAwMDAgbg0KMDAwMDAwMjQyMyAwMDAwMCBuDQowMDAwMDAyNDU2IDAwMDAwIG4NCjAwMDAwMDI1NzQgMDAwMDAgbg0KDQp0cmFpbGVyDQo8PA0KL1NpemUgMTENCi9Sb290IDEgMCBSDQovSW5mbyAxMCAwIFINCj4+DQoNCnN0YXJ0eHJlZg0KMjcxNA0KJSVFT0YNCg==";

			ActaVisitasPropArchivos file = new ActaVisitasPropArchivos();
			file.Code = 3;
			file.Base64String = "data:application/pdf;base64," + base64;
			file.FileName = "SOPORTE-DESISTIMIENTO-3.pdf";
			file.FileType = "application/pdf";

			List<ActaVisitasPropArchivos> avpa = new List<ActaVisitasPropArchivos>();
			avpa.Add(file);

			IdRegistrosDocumentos ird = new IdRegistrosDocumentos();
			ird.NumeroDocumento = _fixture.Create<int>();

			List<IdRegistrosDocumentos> listird = new List<IdRegistrosDocumentos>();
			listird.Add(ird);

			RegistroVisitaCortesIrregulares datos = new RegistroVisitaCortesIrregulares();
			datos.TipoCortesPiel = listtcpi;
			datos.TipoPartes = listtppi;
			datos.Archivos = avpa;
			datos.ArchivoExcelPrecinto = file;
			datos.Archivos = avpa;
			datos.CantidadPielACortar = _fixture.Create<int>();
			datos.Ciudad = _fixture.Create<int>();
			datos.DocumentoOrigenPiel = null;
			datos.DocumentoRepresentante = _fixture.Create<int>();
			datos.EstablecimientoID = _fixture.Create<int>();
			datos.EstadoPiel = "1";
			datos.Fecha = DateTime.Now;
			datos.FuncionarioAutoridadCites = _fixture.Create<int>();
			datos.PrecintoIdentificacion = _fixture.Create<int>();
			datos.RepresentanteEstablecimiento = "bisa";
			datos.ResolucionorigenPiel = listird;
			datos.SalvoCondcutoNumeroOrigenPiel = listird;
			datos.TipoEstablecimiento = _fixture.Create<int>();
			datos.VisitaNumero = _fixture.Create<int>();
			datos.VisitaNumero1 = _fixture.Create<bool>();
			datos.VisitaNumero2 = _fixture.Create<bool>();


			var r = controller.CreateActaVisitaCorteIrregular(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.CreateActaVisitaCorteIrregular(datos);
            Assert.True(r != null);
        }

		[Fact]
		public void EditActaCortePartesIdentificablesRegistro()
		{
			EditActaVisitaIdentificableModelView datos = new EditActaVisitaIdentificableModelView();
			datos.ActaVisitaCortes = null;
			datos.DocumentoOrigenPiel = null;
			datos.ResolucionorigenPiel = null;
			datos.SalvoCondcutoNumeroOrigenPiel = null;
			datos.TiposEstablecimientos = null;
			datos.TipoCortesPiel = null;
			datos.TipoPartes = null;
			datos.Archivos = null;

			var r = controller.EditActaCortePartesIdentificables(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.EditActaCortePartesIdentificables(datos);
            Assert.True(r != null);
        }

		[Fact]
		public void EditActaVisitaCortesIrregulares()
		{
			EditActaVisitaIrregularesModelView datos = new EditActaVisitaIrregularesModelView();
			datos.ActaVisitaCortes = null;
			datos.DocumentoOrigenPiel = null;
			datos.ResolucionorigenPiel = null;
			datos.SalvoCondcutoNumeroOrigenPiel = null;
			datos.TiposEstablecimientos = null;
			datos.TipoCortesPiel = null;
			datos.TipoPartes = null;
			datos.Archivos = null;

			var r = controller.EditActaVisitaCortesIrregulares(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.EditActaVisitaCortesIrregulares(datos);
            Assert.True(r != null);
        }

		[Fact]
		public void InhabilitarActaVisita()
		{
			decimal actaVisitaId = _fixture.Create<int>();

			var r = controller.InhabilitarActaVisita(actaVisitaId);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.InhabilitarActaVisita(actaVisitaId);
            Assert.True(r != null);
        }

		[Fact]
		public void ObtenerArchivosPost()
		{
			int actaVisitaId = _fixture.Create<int>();

			var r = controller.ObtenerArchivosPost(actaVisitaId);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ObtenerArchivosPost(actaVisitaId);
            Assert.True(r != null);
        }

		[Fact]
		public void ObtenerExcelPrecintoActaVisita()
		{
			int actaVisitaId = _fixture.Create<int>();

			var r = controller.ObtenerExcelPrecintoActaVisita(actaVisitaId);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.ObtenerExcelPrecintoActaVisita(actaVisitaId);
            Assert.True(r != null);
        }

		[Fact]
		public void DescargarPlantillaPrecintoExcel()
		{
			ActaVisitasPropArchivos file = new ActaVisitasPropArchivos();
			file.Code = _fixture.Create<int>();
			file.FileName = _fixture.Create<string>();
            file.Base64String = base64;
			file.FileType = _fixture.Create<string>();

			var r = controller.DescargarPlantillaPrecintoExcel(file);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.DescargarPlantillaPrecintoExcel(file);
            Assert.True(r != null);
        }

        [Fact]
        public void IngresarTipoPiel()
        {
            int codigoActa = _fixture.Create<int>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("IngresarTipoPiel", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { null, codigoActa });
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = methodInfo.Invoke(controller, new object[] { null, codigoActa });
            Assert.True(r != null);
        }


        [Fact]
        public void IngresarTipoParteIdentificable()
        {
            int codigoActa = _fixture.Create<int>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("IngresarTipoParteIdentificable", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { null, codigoActa });

            Assert.True(r != null);
        }

        [Fact]
        public void IngresarTipoPielIrregular()
        {
            int codigoActa = _fixture.Create<int>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("IngresarTipoPielIrregular", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { null, codigoActa });

            Assert.True(r != null);
        }

        [Fact]
        public void IngresarTipoParteIrregular()
        {
            int codigoActa = _fixture.Create<int>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("IngresarTipoParteIrregular", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { null, codigoActa });

            Assert.True(r != null);
        }

        [Fact]
        public void IngresarActaVisitaOrigenDocumento()
        {
            int codigoActa = _fixture.Create<int>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("IngresarActaVisitaOrigenDocumento", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { null, codigoActa });

            Assert.True(r != null);
        }

        [Fact]
        public void IngresarActaVisitaResolucionNumero()
        {
            int codigoActa = _fixture.Create<int>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("IngresarActaVisitaResolucionNumero", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { null, codigoActa });

            Assert.True(r != null);
        }

        [Fact]
        public void IngresarActaVisitaActaSalvoconducto()
        {
            int codigoActa = _fixture.Create<int>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("IngresarActaVisitaActaSalvoconducto", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { null, codigoActa });

            Assert.True(r != null);
        }

        [Fact]
        public void ObtenerTiposEstablecimientosSelect()
        {
            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ObtenerTiposEstablecimientosSelect", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, null);

            Assert.True(r != null);
        }

        [Fact]
        public void ObtenerDepartamentos()
        {
            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ObtenerDepartamentos", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, null);

            Assert.True(r != null);
        }

        [Fact]
        public void ObtenerCiudades()
        {
            decimal departamentoId = _fixture.Create<int>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ObtenerCiudades", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { departamentoId });

            Assert.True(r != null);
        }

        [Fact]
        public void ObtenerEstablecimientosPorTipo()
        {
            decimal tipoEstablecimiento = _fixture.Create<int>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ObtenerEstablecimientosPorTipo", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { tipoEstablecimiento });

            Assert.True(r != null);
        }

        [Fact]
        public void ObtenerEmpresaPorNit()
        {
            decimal Nit = _fixture.Create<int>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ObtenerEmpresaPorNit", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { Nit });

            Assert.True(r != null);
        }

        [Fact]
        public void ObtenerEstablecimientosSelectList()
        {
            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ObtenerEstablecimientosSelectList", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, null);

            Assert.True(r != null);
        }

        [Fact]
        public void ObtenerActasTipoEstablecimiento()
        {
            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ObtenerActasTipoEstablecimiento", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { null });

            Assert.True(r != null);
        }

        [Fact]
        public void ObtenerActaVisita()
        {
            decimal idActaVisita = _fixture.Create<int>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ObtenerActaVisita", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { idActaVisita });

            Assert.True(r != null);
        }

        [Fact]
        public void ObtenerActaVisitaIrregular()
        {
            decimal idActaVisita = _fixture.Create<int>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ObtenerActaVisitaIrregular", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { idActaVisita });

            Assert.True(r != null);
        }

        [Fact]
        public void ObtenerDatosDocumentos()
        {
            string ruta = _fixture.Create<string>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ObtenerDatosDocumentos", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { ruta });

            Assert.True(r != null);
        }

        [Fact]
        public void BorrarDatos()
        {
            string ruta = "https://localhost:7075/api/ActaVisitasCortes/BorrarDatos";

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("BorrarDatos", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { ruta });

            Assert.True(r != null);
        }

        [Fact]
        public void ObtenerTipoPielIdentificable()
        {
            decimal idActaVisita = _fixture.Create<int>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ObtenerTipoPielIdentificable", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { idActaVisita });

            Assert.True(r != null);
        }

        [Fact]
        public void ObtenerTipoParteIdentificable()
        {
            decimal idActaVisita = _fixture.Create<int>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ObtenerTipoParteIdentificable", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { idActaVisita });

            Assert.True(r != null);
        }

        [Fact]
        public void ObtenerTipoPielIrregulares()
        {
            decimal idActaVisita = _fixture.Create<int>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ObtenerTipoPielIrregulares", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { idActaVisita });

            Assert.True(r != null);
        }

        [Fact]
        public void ObtenerTiposPartesIrregulares()
        {
            decimal idActaVisita = _fixture.Create<int>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ObtenerTiposPartesIrregulares", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { idActaVisita });

            Assert.True(r != null);
        }

        [Fact]
        public void ObtenerArchivosActaVisita()
        {
            decimal idActaVisita = _fixture.Create<int>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ObtenerArchivosActaVisita", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { idActaVisita });

            Assert.True(r != null);
        }

        [Fact]
        public void ObtenerExcelPrecinto()
        {
            decimal idActaVisita = _fixture.Create<int>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ObtenerExcelPrecinto", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { idActaVisita });

            Assert.True(r != null);
        }

        [Fact]
        public void ValidarDatosExcelPrecintos()
        {
            string base64Excel = _fixture.Create<string>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ValidarDatosExcelPrecintos", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { base64Excel });

            Assert.True(r != null);
        }

        [Fact]
        public void ObtenerValorEstadoPiel()
        {
            int valorIndice = _fixture.Create<int>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ObtenerValorEstadoPiel", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { valorIndice });

            Assert.True(r != null);
        }

        [Fact]
        public void ObtenerIdEstadoPiel()
        {
            string estadoPiel = _fixture.Create<string>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ObtenerIdEstadoPiel", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { estadoPiel });

            Assert.True(r != null);
        }

        [Fact]
        public void ObtenerListaEstadosPiel()
        {
            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ObtenerListaEstadosPiel", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, null);

            Assert.True(r != null);
        }

        [Fact]
        public void IngresarArchivosActaVisita()
        {
            int codigoActa = _fixture.Create<int>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("IngresarArchivosActaVisita", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { null, codigoActa });

            Assert.True(r != null);
        }

        [Fact]
        public void ObtenerNombreCiudadDepartamento()
        {
            decimal departamento = _fixture.Create<int>();
            decimal ciudad = _fixture.Create<int>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ObtenerNombreCiudadDepartamento", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { departamento, ciudad });

            Assert.True(r != null);
        }

        [Fact]
        public void ObtenerVisitasString()
        {
            bool primeraVisita = _fixture.Create<bool>();
            bool SegundaVisita = _fixture.Create<bool>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("ObtenerVisitasString", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { primeraVisita, SegundaVisita });

            Assert.True(r != null);
        }

        [Fact]
        public void DefinirTipoBusquedaActas()
        {
            RegistroActasBusqueda datos = new RegistroActasBusqueda();
            datos.IdEstablecimiento = _fixture.Create<int>();
            datos.IdTipoEstablecimiento = _fixture.Create<int>();
            datos.FechaVisita = DateTime.Now;
            datos.TipoBusqueda = _fixture.Create<int>();

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("DefinirTipoBusquedaActas", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { datos });

            Assert.True(r != null);
        }

        [Fact]
        public void FormatearFechaDecimal()
        {
            decimal fecha = 20230129;

            var methodInfo = typeof(RegistroVisitaDeCortesController).GetMethod("FormatearFechaDecimal", BindingFlags.NonPublic | BindingFlags.Instance);
            var r = methodInfo.Invoke(controller, new object[] { fecha });

            Assert.True(r != null);
        }

        [Fact]
		public void CreateActaCortesPartesIrregulares()
		{
			var r = controller.CreateActaCortesPartesIrregulares();
			Assert.True(r != null);
		}
	}
}
