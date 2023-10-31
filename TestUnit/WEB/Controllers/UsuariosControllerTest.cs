using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TestUnit.WEB;
using Web.Controllers;
using static CUPOS_FRONT.Models.Requests;
using Web.Models;
using Xunit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace testunit.web
{
    public class usuarioscontrollertest
    {
        //se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
        private UsuariosController controller;
        private Fixture _fixture;
        private readonly string token;

        public usuarioscontrollertest()
        {
            controller = new UsuariosController(new LoggerFactory().CreateLogger<UsuariosController>());
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
        public void gestionusuarios()
        {
            var r = controller.GestionUsuarios();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.GestionUsuarios();
            Assert.True(r != null);

        }

        [Fact]
        public void Ayuda()
        {
            var r = controller.Ayuda();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.Ayuda();
            Assert.True(r != null);
        }

        [Fact]
        public void cambiarcontrasena()
        {
            int id = _fixture.Create<int>();
            var r = controller.CambiarContrasena(id);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.CambiarContrasena(id);
            Assert.True(r != null);
        }

        [Fact]
        public void FiltrarUsuarios()
        {
            string nombre = _fixture.Create<string>();
            var r = controller.FiltrarUsuarios(nombre);
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.FiltrarUsuarios(nombre);
            Assert.True(r != null);
        }

        [Fact]
        public void editarusuario()
        {
            int id = _fixture.Create<int>();

            var r = controller.EditarUsuario(id);
            Assert.True(r != null);
        }

        [Fact]
        public void returnadministracion()
        {
            var r = controller.ReturnAdministracion();
            Assert.True(r != null);
        }

        [Fact]
        public void filtrar()
        {
            string nombrebus = _fixture.Create<string>();
            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            var r = controller.Filtrar(nombrebus);
            Assert.True(r != null);
        }

        [Fact]
        public void requser()
        {
            ReqUser datos = new ReqUser();
            datos.cityAddress = _fixture.Create<int>();
            datos.codeParametricDocumentType = _fixture.Create<int>();
            datos.codeParametricUserType = _fixture.Create<int>();
            datos.dependence = _fixture.Create<string>();
            datos.acceptsTerms = _fixture.Create<bool>();
            datos.acceptsProcessingPersonalData = _fixture.Create<bool>();
            datos.identification = _fixture.Create<int>();
            datos.firstName = _fixture.Create<string>();
            datos.secondName = _fixture.Create<string>();
            datos.firstLastName = _fixture.Create<string>();
            datos.SecondLastName = _fixture.Create<string>();
            datos.login = _fixture.Create<string>();
            datos.address = _fixture.Create<string>();
            datos.phone = _fixture.Create<int>();
            datos.email = _fixture.Create<string>();
            datos.password = _fixture.Create<string>();
            datos.digitalSignature = _fixture.Create<string>();
            datos.contractStartDate = DateTime.Now;
            datos.contractFinishDate = DateTime.Now;
            datos.registrationStatus = _fixture.Create<bool>();
            datos.rol = _fixture.Create<string>();
            datos.estate = _fixture.Create<string>();

            var r = controller.Crear(datos);
            Assert.True(r != null);
        }

        [Fact]
        public void cambiarpass()
        {
            actualizarRequest datos = new actualizarRequest();
            datos.code = 20072;
            datos.cityAddress = _fixture.Create<int>();
            datos.codeParametricDocumentType = _fixture.Create<int>();
            datos.codeParametricUserType = _fixture.Create<int>();
            datos.dependence = _fixture.Create<string>();
            datos.acceptsTerms = _fixture.Create<bool>();
            datos.acceptsProcessingPersonalData = _fixture.Create<bool>();
            datos.identification = _fixture.Create<int>();
            datos.firstName = _fixture.Create<string>();
            datos.secondName = _fixture.Create<string>();
            datos.firstLastName = _fixture.Create<string>();
            datos.SecondLastName = _fixture.Create<string>();
            datos.login = _fixture.Create<string>();
            datos.address = _fixture.Create<string>();
            datos.phone = _fixture.Create<int>();
            datos.email = _fixture.Create<string>();
            datos.password = _fixture.Create<string>();
            datos.digitalSignature = _fixture.Create<string>();
            datos.contractStartDate = DateTime.Now;
            datos.contractFinishDate = DateTime.Now;
            datos.registrationStatus = _fixture.Create<bool>();
            datos.rol = _fixture.Create<string>();
            datos.estate = _fixture.Create<string>();

            var r = controller.CambiarPass(datos);
            Assert.True(r != null);

            datos.password = "132456";
            datos.dependence = "132456";

            r = controller.CambiarPass(datos);
            Assert.True(r != null);
        }

        [Fact]
        public void consultardocumentos()
        {
            var r = controller.ConsultarDocumentos();
            Assert.True(r != null);
        }

        [Fact]
        public void actualizar()
        {
            ReqUser datos = new ReqUser();
            datos.code = _fixture.Create<int>();
            datos.cityAddress = _fixture.Create<int>();
            datos.codeParametricDocumentType = _fixture.Create<int>();
            datos.codeParametricUserType = _fixture.Create<int>();
            datos.dependence = _fixture.Create<string>();
            datos.acceptsTerms = _fixture.Create<bool>();
            datos.acceptsProcessingPersonalData = _fixture.Create<bool>();
            datos.identification = _fixture.Create<int>();
            datos.firstName = _fixture.Create<string>();
            datos.secondName = _fixture.Create<string>();
            datos.firstLastName = _fixture.Create<string>();
            datos.SecondLastName = _fixture.Create<string>();
            datos.login = _fixture.Create<string>();
            datos.address = _fixture.Create<string>();
            datos.phone = _fixture.Create<int>();
            datos.email = _fixture.Create<string>();
            datos.password = _fixture.Create<string>();
            datos.digitalSignature = _fixture.Create<string>();
            datos.contractStartDate = DateTime.Now;
            datos.contractFinishDate = DateTime.Now;
            datos.registrationStatus = _fixture.Create<bool>();
            datos.rol = _fixture.Create<string>();
            datos.estate = _fixture.Create<string>();

            var r = controller.Actualizar(datos);
            Assert.True(r != null);
        }

        [Fact]
        public void consultarroles()
        {
            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            var r = controller.ConsultarRoles();
            Assert.True(r != null);
        }

        [Fact]
        public void consultaredit()
        {
            controller.ControllerContext.HttpContext.Session.SetString("token", token);
            controller.ControllerContext.HttpContext.Session.SetString("idEdicionUsuario", "1");

            var r = controller.ConsultarEdit();
            Assert.True(r != null);
        }
    }
}
