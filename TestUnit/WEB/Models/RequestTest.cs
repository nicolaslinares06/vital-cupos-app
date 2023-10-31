using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Models;

namespace TestUnit.WEB.Models
{
	public class RequestTest
	{
        private Fixture _fixture;

        [Fact]
		public void CambioPasswordRequest()
		{
			CambioPasswordRequest datos = new CambioPasswordRequest();
			datos.user = "string";
			datos.password = "string";
			datos.newPassword = "string";
			datos.acceptsTerms = true;
			datos.acceptsProcessingPersonalData = true;

			var type = Assert.IsType<CambioPasswordRequest>(datos);
			Assert.NotNull(type);
		}

		[Fact]
		public void UsuarioSimpleRequest()
		{
			UsuarioSimpleRequest datos = new UsuarioSimpleRequest();
			datos.User = "string";

			var type = Assert.IsType<UsuarioSimpleRequest>(datos);
			Assert.NotNull(type);
		}

		[Fact]
		public void ReqValorTecnica()
		{
			ReqValorTecnica datos = new ReqValorTecnica();
			datos.pkT007Codigo = 1;
			datos.a007nombre = "string";
			datos.a007valor = "string";
			datos.a007descripcion = "string";
			datos.a007estadoRegistro = "string";

			var type = Assert.IsType<ReqValorTecnica>(datos);
			Assert.NotNull(type);
		}

        [Fact]
        public void Technical_Properties()
        {
            // Arrange
            var technical = new Technical();

            // Act
            technical.technicalList = new List<ReqValorTecnica>
        {
            new ReqValorTecnica { a007nombre = "Nombre1" },
            new ReqValorTecnica { a007nombre = "Nombre2" }
        };

            // Assert
            Assert.NotNull(technical.technicalList);
            Assert.Equal(2, technical.technicalList.Count);
            Assert.Equal("Nombre1", technical.technicalList[0].a007nombre);
            Assert.Equal("Nombre2", technical.technicalList[1].a007nombre);
        }

        [Fact]
        public void ReqGesAsigRoles_Properties()
        {
            // Arrange
            var reqGesAsigRoles = new ReqGesAsigRoles();

            // Act
            reqGesAsigRoles.nombre = "Nombre";
            reqGesAsigRoles.a012CodigoParametricaTipoUsuario = 1;
            reqGesAsigRoles.a012segundoNombre = "SegundoNombre";
            reqGesAsigRoles.a012segundoApellido = "SegundoApellido";
            reqGesAsigRoles.a012identificacion = 12345;
            reqGesAsigRoles.a012correoElectronico = "correo@ejemplo.com";
            reqGesAsigRoles.codigoUsuario = 10;
            reqGesAsigRoles.codigoRol = 20;
            reqGesAsigRoles.nombreRol = "NombreRol";
            reqGesAsigRoles.pkT0015codigo = 30;
            reqGesAsigRoles.a015estadoSolicitud = "EstadoSolicitud";

            // Assert
            Assert.Equal("Nombre", reqGesAsigRoles.nombre);
            Assert.Equal(1, reqGesAsigRoles.a012CodigoParametricaTipoUsuario);
            Assert.Equal("SegundoNombre", reqGesAsigRoles.a012segundoNombre);
            Assert.Equal("SegundoApellido", reqGesAsigRoles.a012segundoApellido);
            Assert.Equal(12345, reqGesAsigRoles.a012identificacion);
            Assert.Equal("correo@ejemplo.com", reqGesAsigRoles.a012correoElectronico);
            Assert.Equal(10, reqGesAsigRoles.codigoUsuario);
            Assert.Equal(20, reqGesAsigRoles.codigoRol);
            Assert.Equal("NombreRol", reqGesAsigRoles.nombreRol);
            Assert.Equal(30, reqGesAsigRoles.pkT0015codigo);
            Assert.Equal("EstadoSolicitud", reqGesAsigRoles.a015estadoSolicitud);
        }
    }
}
