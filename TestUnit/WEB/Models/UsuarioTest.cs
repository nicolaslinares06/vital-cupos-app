using AutoFixture;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Models;
using WebFront.Models;

namespace TestUnit.WEB.Models
{
	public class UsuarioTest
	{
        private Fixture _fixture;

        [Fact]
		public void GestionUsuario()
		{
			GestionUsuario datos = new GestionUsuario();
			datos.pkT012codigo = 1;
			datos.roles = "string";
			datos.a012identificacion = 1;
			datos.A012direccion = "string";
			datos.a012telefono = 1;
			datos.A012correoElectronico = "string";
			//datos.A012fax = "string";
			datos.a008valor = "string";
			datos.A012fechaCreacion = DateTime.Now;
			datos.A012fechaModificacion = DateTime.Now;
			datos.a012fechaExpiracontraseña = DateTime.Now;
			datos.A012fechaFinContrato = DateTime.Now;
			datos.A012fechaInicioContrato = DateTime.Now;
			datos.A012primerNombre = "string";
			datos.A012segundoNombre = "string";
			datos.A012segundoApellido = "string";
			datos.A012primerApellido = "string";
			datos.a012login = "string";

			var type = Assert.IsType<GestionUsuario>(datos);
			Assert.NotNull(type);
		}

		[Fact]
		public void ReqDocs()
		{
			ReqDocs datos = new ReqDocs();
			datos.archivo = null;

			var type = Assert.IsType<ReqDocs>(datos);
			Assert.NotNull(type);
		}

		[Fact]
		public void ReqAceptarCondiciones()
		{
			ReqAceptarCondiciones datos = new ReqAceptarCondiciones();
			datos.A012aceptaTerminos = true;
			datos.A012aceptaTerminos = true;

			var type = Assert.IsType<ReqAceptarCondiciones>(datos);
			Assert.NotNull(type);
		}

		[Fact]
		public void ReqUsers()
		{
			ReqUsers datos = new ReqUsers();
			datos.id = 1;
			datos.primerNombre = "string";
			datos.segundoNombre = "string";
			datos.segundoApellido = "string";
			datos.primerApellido = "string";
			datos.tipoDocumento = "string";
			datos.identificacion = 1;
			datos.direccion = "string";
			datos.dependencia = "string";
			datos.telefono = 1;
			//datos.fax = "string";
			datos.inicioContrato = DateTime.Now;
			datos.finContrato = DateTime.Now;
			datos.email = "string";
			datos.rol = "string";
			datos.estado = "string";
			datos.login = "string";
			datos.estate = "string";

			var type = Assert.IsType<ReqUsers>(datos);
			Assert.NotNull(type);
		}

        [Fact]
        public void ReqAssignRols()
        {
            ReqAssignRols model = new ReqAssignRols
            {
                email = "user@example.com",
                name = "John Doe",
                identification = 12345,
                rol = 1,
                rols = new List<SelectListItem>()
            };

            var type = Assert.IsType<ReqAssignRols>(model);
            Assert.NotNull(type);
            Assert.Equal("user@example.com", model.email);
            Assert.Equal("John Doe", model.name);
            Assert.Equal(12345, model.identification);
            Assert.Equal(1, model.rol);
            Assert.NotNull(model.rols);
        }

        [Fact]
        public void VitalReq()
        {
            VitalReq model = new VitalReq
            {
                code = 1,
                status = "Active",
                permissions = "Read, Write",
                message = "Success",
                ID = 123,
                User = "johndoe",
                Name = "John Doe",
                Document = 54321,
                EMail = "user@example.com",
                LastLogin = DateTime.Now,
                Active = "Yes",
                Enabled = "Yes",
                Module = "Admin",
                Url = "/admin",
                Token = "token123",
                UrlError = "/error",
                rol = 2,
                rols = new List<SelectListItem>()
            };

            var type = Assert.IsType<VitalReq>(model);
            Assert.NotNull(type);
            Assert.Equal(1, model.code);
            Assert.Equal("Active", model.status);
            Assert.Equal("Read, Write", model.permissions);
            Assert.Equal("Success", model.message);
            Assert.Equal(123, model.ID);
            Assert.Equal("johndoe", model.User);
            Assert.Equal("John Doe", model.Name);
            Assert.Equal(54321, model.Document);
            Assert.Equal("user@example.com", model.EMail);
            Assert.Equal("Yes", model.Active);
            Assert.Equal("Yes", model.Enabled);
            Assert.Equal("Admin", model.Module);
            Assert.Equal("/admin", model.Url);
            Assert.Equal("token123", model.Token);
            Assert.Equal("/error", model.UrlError);
            Assert.Equal(2, model.rol);
            Assert.NotNull(model.rols);
        }

        [Fact]
        public void AssignRol()
        {
            AssignRol model = new AssignRol
            {
                code = 1,
                status = "Active",
                permissions = "Read, Write",
                message = "Success",
                id = 123,
                user = "johndoe",
                name = "John Doe",
                document = 54321,
                eMail = "user@example.com",
                lastLogin = DateTime.Now,
                active = "Yes",
                enabled = "Yes",
                module = "Admin",
                url = "/admin",
                token = "token123",
                urlError = "/error",
                rol = 2
            };

            var type = Assert.IsType<AssignRol>(model);
            Assert.NotNull(type);
            Assert.Equal(1, model.code);
            Assert.Equal("Active", model.status);
            Assert.Equal("Read, Write", model.permissions);
            Assert.Equal("Success", model.message);
            Assert.Equal(123, model.id);
            Assert.Equal("johndoe", model.user);
            Assert.Equal("John Doe", model.name);
            Assert.Equal(54321, model.document);
            Assert.Equal("user@example.com", model.eMail);
            Assert.Equal("Yes", model.active);
            Assert.Equal("Yes", model.enabled);
            Assert.Equal("Admin", model.module);
            Assert.Equal("/admin", model.url);
            Assert.Equal("token123", model.token);
            Assert.Equal("/error", model.urlError);
            Assert.Equal(2, model.rol);
        }
    }
}
