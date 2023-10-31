using AutoFixture;
using iTextSharp.text.log;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Repository.Helpers.Models;
using System.Security.Claims;
using Web.Controllers;
using Web.Models;
using static CUPOS_FRONT.Models.Requests;

namespace TestUnit.WEB
{
	public class SaleDocumentControllerTest
	{
		//Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
		private SaleDocumentController controller;
		private readonly Mock<SaleDocumentController> _logger;
        private Fixture _fixture;
        public SaleDocumentControllerTest()
		{
			_logger = new Mock<SaleDocumentController>();
			controller = new SaleDocumentController(new Microsoft.Extensions.Logging.LoggerFactory().CreateLogger<SaleDocumentController>());
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
		public void SaleDocuments()
		{
			var r = controller.SaleDocuments();

			var viewResult = Assert.IsType<ViewResult>(r);

			Assert.NotNull(viewResult);
		}

		[Fact]
		public void SaleDocumentForm()
		{
			var r = controller.SaleDocumentForm();

			var viewResult = Assert.IsType<ViewResult>(r);

			Assert.NotNull(viewResult);
		}

		[Fact]
		public void GetSaleDocumentId()
		{
			int code = _fixture.Create<int>();

			var r = controller.GetSaleDocumentId(code);
			Assert.True(r != null);
		}

		[Fact]
		public void GetSaleDocuments()
		{
			string? typeDocument = null;
			string? documentNumber = null;
			string? numberCartaVenta = null;

			var r = controller.GetSaleDocuments(typeDocument, documentNumber, numberCartaVenta);
			Assert.True(r != null);
		}

		[Fact]
		public void Quotas()
		{
			decimal company = _fixture.Create<int>(); 
			decimal typeDocument = _fixture.Create<int>(); 
			string documentNumber = _fixture.Create<string>();

			var r = controller.Quotas(company, typeDocument, documentNumber);
			Assert.True(r != null);
		}

		[Fact]
		public void GetInventory()
		{
			string documentNumber = _fixture.Create<string>(); 
			string? code = null;

			var r = controller.GetInventory(documentNumber, code);

			var viewResult = Assert.IsType<List<Inventory>>(r);

			Assert.NotNull(viewResult);
		}

		[Fact]
		public void GetSpecies()
		{
			var r = controller.GetSpecies();
			Assert.True(r != null);
		}

		[Fact]
		public void SaveSaleDocument()
		{
			SaleDocumentModel datos = new SaleDocumentModel();
			datos.Code = _fixture.Create<int>();
			datos.Numeration = _fixture.Create<int>();
			datos.CarteNumber = _fixture.Create<string>();
			datos.SaleDate = DateTime.Now;
			datos.NumberSold = _fixture.Create<int>();
			datos.BusinessSale = _fixture.Create<string>();
			datos.TypeCarte = _fixture.Create<int>();
			datos.TypeDocumentSeller = _fixture.Create<int>();
			datos.DocumentNumberSeller = _fixture.Create<string>();
			datos.ReasonSocial = _fixture.Create<string>();
			datos.InitialBalanceBusiness = _fixture.Create<int>();
			datos.FinalBalanceBusiness = _fixture.Create<int>();
			datos.Observations = _fixture.Create<string>();
			datos.BusinessShopper = _fixture.Create<string>();
			datos.InventoryAvailability = _fixture.Create<int>();
			datos.TypeDocumentShopper = _fixture.Create<int>();
			datos.DocumentNumberShopper = _fixture.Create<string>();
			datos.ReasonSocialShopper = _fixture.Create<string>();
			datos.InitialBalanceBusinessShopper = _fixture.Create<int>();
			datos.FinalBalanceBusinessShopper = _fixture.Create<int>();
			datos.ObservationsShopper = _fixture.Create<string>();
			datos.Quota = _fixture.Create<int>();
			datos.Solds = _fixture.Create<int>();
			datos.QuotasSold = _fixture.Create<int>();
			datos.NitCompanySeller = _fixture.Create<string>();
			datos.NitCompanyShopper = _fixture.Create<string>();
			datos.CompanySellerCode = _fixture.Create<int>();
			datos.CompanyShopperCode = _fixture.Create<int>();
			datos.RegistrationDateCarteSale = DateTime.Now;
			datos.SupportDocuments = null;
			datos.SupportDocumentsRemoved = null;
			datos.Quotas = null;
			datos.QuotasInventory = null;
			datos.TypeSpecimenSeller = _fixture.Create<string>();
			datos.TypeSpecimenShopper = _fixture.Create<string>();

			var r = controller.SaveSaleDocument(datos);
			Assert.True(r != null);
		}

		[Fact]
		public void ValidateCompanyForm()
		{
			var r = controller.ValidateCompanyForm();

			var viewResult = Assert.IsType<ViewResult>(r);

			Assert.NotNull(viewResult);
		}

		[Fact]
		public void ValidateCompany()
		{
			decimal company = _fixture.Create<int>();
			decimal typeDocument = _fixture.Create<int>();
			string documentNumber = "0";

			var r = controller.ValidateCompany(company, typeDocument, documentNumber);
			Assert.True(r != null);
		}

		[Fact]
		public void SearchCompany()
		{
			string number = _fixture.Create<string>(); 
			decimal typeDocument = _fixture.Create<int>();
			decimal companyCode = _fixture.Create<int>();

			var r = controller.SearchCompany(number, typeDocument, companyCode);

			var viewResult = Assert.IsType<List<Company>>(r);

			Assert.NotNull(viewResult);
		}

		[Fact]
		public void ExceptConduitMobilization()
		{
			var r = controller.ExceptConduitMobilization();

			var viewResult = Assert.IsType<ViewResult>(r);

			Assert.NotNull(viewResult);
		}

		[Fact]
		public void ConduitMobilizationForm()
		{
            string numeroIdentificacion = _fixture.Create<string>();
            string numeroSalvoConducto = _fixture.Create<string>();

            var r = controller.ConduitMobilizationForm(numeroIdentificacion, numeroSalvoConducto);

			var viewResult = Assert.IsType<ViewResult>(r);

			Assert.NotNull(viewResult);
		}

		[Fact]
		public void DeleteSaleDocument()
		{
			int id = _fixture.Create<int>();

			var r = controller.DeleteSaleDocument(id);

			var result = Assert.IsType<bool>(r);

			Assert.NotNull(result);
		}

		[Fact]
		public void UpdateSaleDocument()
		{
			SaleDocumentModel datos = new SaleDocumentModel();
			datos.Code = _fixture.Create<int>();
			datos.Numeration = _fixture.Create<int>();
			datos.CarteNumber = _fixture.Create<string>();
			datos.SaleDate = DateTime.Now;
			datos.NumberSold = _fixture.Create<int>();
			datos.BusinessSale = _fixture.Create<string>();
			datos.TypeCarte = _fixture.Create<int>();
			datos.TypeDocumentSeller = _fixture.Create<int>();
			datos.DocumentNumberSeller = _fixture.Create<string>();
			datos.ReasonSocial = _fixture.Create<string>();
			datos.InitialBalanceBusiness = _fixture.Create<int>();
			datos.FinalBalanceBusiness = _fixture.Create<int>();
			datos.Observations = _fixture.Create<string>();
			datos.BusinessShopper = _fixture.Create<string>();
			datos.InventoryAvailability = _fixture.Create<int>();
			datos.TypeDocumentShopper = _fixture.Create<int>();
			datos.DocumentNumberShopper = _fixture.Create<string>();
			datos.ReasonSocialShopper = _fixture.Create<string>();
			datos.InitialBalanceBusinessShopper = _fixture.Create<int>();
			datos.FinalBalanceBusinessShopper = _fixture.Create<int>();
			datos.ObservationsShopper = _fixture.Create<string>();
			datos.Quota = _fixture.Create<int>();
			datos.Solds = _fixture.Create<int>();
			datos.QuotasSold = _fixture.Create<int>();
			datos.NitCompanySeller = _fixture.Create<string>();
			datos.NitCompanyShopper = _fixture.Create<string>();
			datos.CompanySellerCode = _fixture.Create<int>();
			datos.CompanyShopperCode = _fixture.Create<int>();
			datos.RegistrationDateCarteSale = DateTime.Now;
			datos.SupportDocuments = null;
			datos.SupportDocumentsRemoved = null;
			datos.Quotas = null;
			datos.QuotasInventory = null;
			datos.TypeSpecimenSeller = _fixture.Create<string>();
			datos.TypeSpecimenShopper = _fixture.Create<string>();

			var r = controller.UpdateSaleDocument(datos);
			Assert.True(r != null);
		}

		[Fact]
		public void GetSupportDocument()
		{
			decimal code = _fixture.Create<int>();

			var r = controller.GetSupportDocument(code);
			Assert.True(r != null);
		}

		[Fact]
		public void GetQuotasByCode()
		{
			decimal code = 10058;
			var r = controller.GetQuotasByCode(code);
			Assert.True(r != null);
		}
	}
}
