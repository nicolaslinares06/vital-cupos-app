using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Web.Controllers;
using Web.Models;
using Microsoft.Extensions.Logging;
using static CUPOS_FRONT.Models.Requests;
using Microsoft.Extensions.Configuration;
using iText.Kernel.Pdf.Canvas.Wmf;

namespace TestUnit.WEB
{
	public class AdministrarRolesFuncionalidadesControllerTest
	{
		//Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
		private AdministrarRolesFuncionalidadesController controller;
        private Fixture _fixture;
        private readonly string token;

        public AdministrarRolesFuncionalidadesControllerTest()
        {
            controller = new AdministrarRolesFuncionalidadesController(new LoggerFactory().CreateLogger<AdministrarRolesFuncionalidadesController>());
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
			string rol = _fixture.Create<string>(); 
			string cargo = _fixture.Create<string>(); 
			string estado = _fixture.Create<string>(); 
			decimal idRol = _fixture.Create<int>();

			var r = controller.Index(rol, cargo, estado, idRol);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.Index(rol, cargo, estado, idRol);
            Assert.True(r != null);
        }

		[Fact]
		public void ConsultarModulos()
		{
			decimal codRol = 9; 
			string nomCargo = "Analista";

			var r = controller.consultarModulos(codRol, nomCargo);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.consultarModulos(codRol, nomCargo);
            Assert.True(r != null);

        }

		[Fact]
		public void Consultar()
		{
			decimal codRol = _fixture.Create<int>();
			string nomCargo = _fixture.Create<string>();

			var r = controller.consultar(codRol, nomCargo);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.consultar(codRol, nomCargo);
            Assert.True(r != null);

        }

		[Fact]
		public void Actualizar()
		{
			RolModPermition datos = new RolModPermition();
			datos.rolId = 10023;
			datos.moduleId = 15;
			datos.consult = _fixture.Create<bool>();
			datos.create = _fixture.Create<bool>();
			datos.update = _fixture.Create<bool>();
			datos.delete = _fixture.Create<bool>();
			datos.see = _fixture.Create<bool>();
			datos.name = _fixture.Create<string>();

			var r = controller.actualizar(datos);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.actualizar(datos);
            Assert.True(r != null);
        }
	}
}
