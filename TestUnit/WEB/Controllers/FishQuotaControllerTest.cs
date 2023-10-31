using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Repository.Helpers;
using Repository.Helpers.Models;
using System.Security.Claims;
using Web.Controllers;
using Web.Models;
using WebFront.Controllers;
using static CUPOS_FRONT.Models.Requests;

namespace TestUnit.WEB
{
	public class FishQuotaControllerTest
	{
		//Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
		private FishQuotaController controller;
		private readonly Mock<FishQuotaController> _logger;
        private Fixture _fixture;
        private readonly string token;

        public FishQuotaControllerTest()
		{
			_logger = new Mock<FishQuotaController>();
			controller = new FishQuotaController(new LoggerFactory().CreateLogger<FishQuotaController>());
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
		public void FishQuotas()
		{
			var r = controller.FishQuotas();
			Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.FishQuotas();
            Assert.True(r != null);
        }

		[Fact]
		public void GetFishesQuotas()
		{
			DateTime? initialValidityDate = DateTime.Now;
			DateTime? finalValidityDate = DateTime.Now;
			decimal numberResolution = _fixture.Create<int>();

			var r = controller.GetFishesQuotas(initialValidityDate, finalValidityDate, numberResolution);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.GetFishesQuotas(initialValidityDate, finalValidityDate, numberResolution);
            Assert.True(r != null);
        }

		[Fact]
		public void GetFishQuotaByCode()
		{
			decimal code = _fixture.Create<int>();

			var r = controller.GetFishQuotaByCode(code);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.GetFishQuotaByCode(code);
            Assert.True(r != null);

        }

		[Fact]
		public void SaveFishQuota()
		{
			FishQuotaAmount fish = new FishQuotaAmount();
			fish.CodeFishQuotaAmount = _fixture.Create<int>();
			fish.Group = _fixture.Create<int>();
			fish.SpeciesCode = _fixture.Create<int>();
			fish.SpeciesName = _fixture.Create<string>();
			fish.speciesNameComun = _fixture.Create<string>();
			fish.Quota = _fixture.Create<int>();
			fish.Total = _fixture.Create<int>();
			fish.NameRegion = _fixture.Create<string>();
			fish.Region = _fixture.Create<int>();
			fish.ActionTemp = _fixture.Create<int>();

			List<FishQuotaAmount> listafish = new List<FishQuotaAmount>();
			listafish.Add(fish);

			string base64 = "JVBERi0xLjMNCiXi48/TDQoNCjEgMCBvYmoNCjw8DQovVHlwZSAvQ2F0YWxvZw0KL091dGxpbmVzIDIgMCBSDQovUGFnZXMgMyAwIFINCj4+DQplbmRvYmoNCg0KMiAwIG9iag0KPDwNCi9UeXBlIC9PdXRsaW5lcw0KL0NvdW50IDANCj4+DQplbmRvYmoNCg0KMyAwIG9iag0KPDwNCi9UeXBlIC9QYWdlcw0KL0NvdW50IDINCi9LaWRzIFsgNCAwIFIgNiAwIFIgXSANCj4+DQplbmRvYmoNCg0KNCAwIG9iag0KPDwNCi9UeXBlIC9QYWdlDQovUGFyZW50IDMgMCBSDQovUmVzb3VyY2VzIDw8DQovRm9udCA8PA0KL0YxIDkgMCBSIA0KPj4NCi9Qcm9jU2V0IDggMCBSDQo+Pg0KL01lZGlhQm94IFswIDAgNjEyLjAwMDAgNzkyLjAwMDBdDQovQ29udGVudHMgNSAwIFINCj4+DQplbmRvYmoNCg0KNSAwIG9iag0KPDwgL0xlbmd0aCAxMDc0ID4+DQpzdHJlYW0NCjIgSg0KQlQNCjAgMCAwIHJnDQovRjEgMDAyNyBUZg0KNTcuMzc1MCA3MjIuMjgwMCBUZA0KKCBBIFNpbXBsZSBQREYgRmlsZSApIFRqDQpFVA0KQlQNCi9GMSAwMDEwIFRmDQo2OS4yNTAwIDY4OC42MDgwIFRkDQooIFRoaXMgaXMgYSBzbWFsbCBkZW1vbnN0cmF0aW9uIC5wZGYgZmlsZSAtICkgVGoNCkVUDQpCVA0KL0YxIDAwMTAgVGYNCjY5LjI1MDAgNjY0LjcwNDAgVGQNCigganVzdCBmb3IgdXNlIGluIHRoZSBWaXJ0dWFsIE1lY2hhbmljcyB0dXRvcmlhbHMuIE1vcmUgdGV4dC4gQW5kIG1vcmUgKSBUag0KRVQNCkJUDQovRjEgMDAxMCBUZg0KNjkuMjUwMCA2NTIuNzUyMCBUZA0KKCB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiApIFRqDQpFVA0KQlQNCi9GMSAwMDEwIFRmDQo2OS4yNTAwIDYyOC44NDgwIFRkDQooIEFuZCBtb3JlIHRleHQuIEFuZCBtb3JlIHRleHQuIEFuZCBtb3JlIHRleHQuIEFuZCBtb3JlIHRleHQuIEFuZCBtb3JlICkgVGoNCkVUDQpCVA0KL0YxIDAwMTAgVGYNCjY5LjI1MDAgNjE2Ljg5NjAgVGQNCiggdGV4dC4gQW5kIG1vcmUgdGV4dC4gQm9yaW5nLCB6enp6ei4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kICkgVGoNCkVUDQpCVA0KL0YxIDAwMTAgVGYNCjY5LjI1MDAgNjA0Ljk0NDAgVGQNCiggbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiApIFRqDQpFVA0KQlQNCi9GMSAwMDEwIFRmDQo2OS4yNTAwIDU5Mi45OTIwIFRkDQooIEFuZCBtb3JlIHRleHQuIEFuZCBtb3JlIHRleHQuICkgVGoNCkVUDQpCVA0KL0YxIDAwMTAgVGYNCjY5LjI1MDAgNTY5LjA4ODAgVGQNCiggQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgKSBUag0KRVQNCkJUDQovRjEgMDAxMCBUZg0KNjkuMjUwMCA1NTcuMTM2MCBUZA0KKCB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBFdmVuIG1vcmUuIENvbnRpbnVlZCBvbiBwYWdlIDIgLi4uKSBUag0KRVQNCmVuZHN0cmVhbQ0KZW5kb2JqDQoNCjYgMCBvYmoNCjw8DQovVHlwZSAvUGFnZQ0KL1BhcmVudCAzIDAgUg0KL1Jlc291cmNlcyA8PA0KL0ZvbnQgPDwNCi9GMSA5IDAgUiANCj4+DQovUHJvY1NldCA4IDAgUg0KPj4NCi9NZWRpYUJveCBbMCAwIDYxMi4wMDAwIDc5Mi4wMDAwXQ0KL0NvbnRlbnRzIDcgMCBSDQo+Pg0KZW5kb2JqDQoNCjcgMCBvYmoNCjw8IC9MZW5ndGggNjc2ID4+DQpzdHJlYW0NCjIgSg0KQlQNCjAgMCAwIHJnDQovRjEgMDAyNyBUZg0KNTcuMzc1MCA3MjIuMjgwMCBUZA0KKCBTaW1wbGUgUERGIEZpbGUgMiApIFRqDQpFVA0KQlQNCi9GMSAwMDEwIFRmDQo2OS4yNTAwIDY4OC42MDgwIFRkDQooIC4uLmNvbnRpbnVlZCBmcm9tIHBhZ2UgMS4gWWV0IG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gKSBUag0KRVQNCkJUDQovRjEgMDAxMCBUZg0KNjkuMjUwMCA2NzYuNjU2MCBUZA0KKCBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSB0ZXh0LiBBbmQgbW9yZSApIFRqDQpFVA0KQlQNCi9GMSAwMDEwIFRmDQo2OS4yNTAwIDY2NC43MDQwIFRkDQooIHRleHQuIE9oLCBob3cgYm9yaW5nIHR5cGluZyB0aGlzIHN0dWZmLiBCdXQgbm90IGFzIGJvcmluZyBhcyB3YXRjaGluZyApIFRqDQpFVA0KQlQNCi9GMSAwMDEwIFRmDQo2OS4yNTAwIDY1Mi43NTIwIFRkDQooIHBhaW50IGRyeS4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gQW5kIG1vcmUgdGV4dC4gKSBUag0KRVQNCkJUDQovRjEgMDAxMCBUZg0KNjkuMjUwMCA2NDAuODAwMCBUZA0KKCBCb3JpbmcuICBNb3JlLCBhIGxpdHRsZSBtb3JlIHRleHQuIFRoZSBlbmQsIGFuZCBqdXN0IGFzIHdlbGwuICkgVGoNCkVUDQplbmRzdHJlYW0NCmVuZG9iag0KDQo4IDAgb2JqDQpbL1BERiAvVGV4dF0NCmVuZG9iag0KDQo5IDAgb2JqDQo8PA0KL1R5cGUgL0ZvbnQNCi9TdWJ0eXBlIC9UeXBlMQ0KL05hbWUgL0YxDQovQmFzZUZvbnQgL0hlbHZldGljYQ0KL0VuY29kaW5nIC9XaW5BbnNpRW5jb2RpbmcNCj4+DQplbmRvYmoNCg0KMTAgMCBvYmoNCjw8DQovQ3JlYXRvciAoUmF2ZSBcKGh0dHA6Ly93d3cubmV2cm9uYS5jb20vcmF2ZVwpKQ0KL1Byb2R1Y2VyIChOZXZyb25hIERlc2lnbnMpDQovQ3JlYXRpb25EYXRlIChEOjIwMDYwMzAxMDcyODI2KQ0KPj4NCmVuZG9iag0KDQp4cmVmDQowIDExDQowMDAwMDAwMDAwIDY1NTM1IGYNCjAwMDAwMDAwMTkgMDAwMDAgbg0KMDAwMDAwMDA5MyAwMDAwMCBuDQowMDAwMDAwMTQ3IDAwMDAwIG4NCjAwMDAwMDAyMjIgMDAwMDAgbg0KMDAwMDAwMDM5MCAwMDAwMCBuDQowMDAwMDAxNTIyIDAwMDAwIG4NCjAwMDAwMDE2OTAgMDAwMDAgbg0KMDAwMDAwMjQyMyAwMDAwMCBuDQowMDAwMDAyNDU2IDAwMDAwIG4NCjAwMDAwMDI1NzQgMDAwMDAgbg0KDQp0cmFpbGVyDQo8PA0KL1NpemUgMTENCi9Sb290IDEgMCBSDQovSW5mbyAxMCAwIFINCj4+DQoNCnN0YXJ0eHJlZg0KMjcxNA0KJSVFT0YNCg==";

			SupportDocuments adjunto = new SupportDocuments();
			adjunto.codigo = _fixture.Create<int>();
			adjunto.adjuntoBase64 = "data:application/pdf;base64," + base64;
			adjunto.nombreAdjunto = "abc.pdf";
			adjunto.tipoAdjunto = "application/pdf";
			adjunto.ActionTemp = _fixture.Create<string>();

			List<SupportDocuments> listadjunto = new List<SupportDocuments>();
			listadjunto.Add(adjunto);

			FishQuota datos = new FishQuota();
			datos.Code = _fixture.Create<int>();
			datos.Type = _fixture.Create<string>();
			datos.NumberResolution = _fixture.Create<int>();
			datos.ResolutionDate = DateTime.Now;
			datos.ValidityDate = DateTime.Now;
			datos.ValidityYear = _fixture.Create<int>();
			datos.ResolutionObject = _fixture.Create<string>();
			datos.Document = _fixture.Create<int>();
			datos.InitialValidityDate = DateTime.Now;
			datos.FinalValidityDate = DateTime.Now;
			datos.CodeFishQuotaAmount = _fixture.Create<int>();
			datos.Group = _fixture.Create<int>();
			datos.GroupName = _fixture.Create<string>();
			datos.speciesNameComun = _fixture.Create<string>();
			datos.SpeciesCode = _fixture.Create<int>();
			datos.Quota = _fixture.Create<int>();
			datos.Total = _fixture.Create<int>();
			datos.Region = _fixture.Create<int>();
			datos.SpeciesName = _fixture.Create<string>();
			datos.FishQuotaAmounts = listafish;
			datos.FishQuotaAmountsRemoved = listafish;
			datos.SupportDocuments = listadjunto;
			datos.SupportDocumentsRemoved = listadjunto;

			var r = controller.SaveFishQuota(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.SaveFishQuota(datos);
            Assert.True(r != null);
        }

		[Fact]
		public void DeleteFishQuota()
		{
			int code = _fixture.Create<int>();

			var r = controller.DeleteFishQuota(code);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.DeleteFishQuota(code);
            Assert.True(r != null);
        }

		[Fact]
		public void UpdateFishQuota()
		{
			FishQuota datos = new FishQuota();
			datos.Code = _fixture.Create<int>();
			datos.Type = _fixture.Create<string>();
			datos.NumberResolution = _fixture.Create<int>();
			datos.ResolutionDate = DateTime.Now;
			datos.ValidityDate = DateTime.Now;
			datos.ValidityYear = _fixture.Create<int>();
			datos.ResolutionObject = _fixture.Create<string>();
			datos.Document = _fixture.Create<int>();
			datos.InitialValidityDate = DateTime.Now;
			datos.FinalValidityDate = DateTime.Now;
			datos.CodeFishQuotaAmount = _fixture.Create<int>();
			datos.Group = _fixture.Create<int>();
			datos.GroupName = _fixture.Create<string>();
			datos.speciesNameComun = _fixture.Create<string>();
			datos.SpeciesCode = _fixture.Create<int>();
			datos.Quota = _fixture.Create<int>();
			datos.Total = _fixture.Create<int>();
			datos.Region = _fixture.Create<int>();
			datos.SpeciesName = _fixture.Create<string>();
			datos.FishQuotaAmounts = null;
			datos.FishQuotaAmountsRemoved = null;
			datos.SupportDocuments = null;
			datos.SupportDocumentsRemoved = null;

			var r = controller.UpdateFishQuota(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.UpdateFishQuota(datos);
            Assert.True(r != null);
        }

		[Fact]
		public void GetSpecies()
		{
			var r = controller.GetSpecies();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.GetSpecies();
            Assert.True(r != null);
        }

		[Fact]
		public void GetSupportDocument()
		{
			decimal code = _fixture.Create<int>();

			var r = controller.GetSupportDocument(code);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.GetSupportDocument(code);
            Assert.True(r != null);
        }
	}
}
