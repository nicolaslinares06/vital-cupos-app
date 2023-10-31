using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Web.Controllers;
using Web.Models;
using Microsoft.Extensions.Logging;
using CUPOS_FRONT.Controllers;

namespace TestUnit.WEB.Controllers
{
    public class ValidateApiControllerTest
    {
        //Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
        private ValidateApiController controller;
        private Fixture _fixture;

        public ValidateApiControllerTest()
        {
            controller = new ValidateApiController();
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
        public void ValidateApi()
        {
            var r = controller.ValidateApi();
            Assert.True(r != null);
        }
    }
}
