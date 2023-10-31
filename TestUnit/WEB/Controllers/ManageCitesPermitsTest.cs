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
	public class ManageCitesPermitsTest
	{
		//Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
		private ManageCitesPermits controller;
        private Fixture _fixture;
        private readonly string token;

        public ManageCitesPermitsTest()
		{
			controller = new ManageCitesPermits(new LoggerFactory().CreateLogger<CvController>());
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
		public void Home()
		{
			var r = controller.Home();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.Home();
            Assert.True(r != null); ;
		}
	}
}
