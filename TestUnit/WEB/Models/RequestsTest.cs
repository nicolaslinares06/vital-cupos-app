using AutoFixture;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CUPOS_FRONT.Models.Requests;

namespace TestUnit.WEB.Models
{
	public class RequestsTest
	{
        private Fixture _fixture;

        [Fact]
		public void ReqLogin()
		{
			ReqLogin datos = new ReqLogin();
			datos.user = "string";
			datos.password = "string";

			var type = Assert.IsType<ReqLogin>(datos);
			Assert.NotNull(type);
		}

		[Fact]
		public void ReqChangePassword()
		{
			ReqChangePassword datos = new ReqChangePassword();
			datos.user = "string";
			datos.password = "string";
			datos.newPassword = "string";
			datos.acceptsTerms = true;
			datos.acceptsProcessingPersonalData = true;

			var type = Assert.IsType<ReqChangePassword>(datos);
			Assert.NotNull(type);
		}

		[Fact]
		public void ReqAssignment()
		{
			ReqAssignment datos = new ReqAssignment();
			datos.id = 1;
			datos.identification = 1;
			datos.firstName = "string";
			datos.secondName = "string";
			datos.firstLastname = "string";
			datos.secondLastname = "string";
			datos.rolId = "string";
			datos.estate = 1;

			var type = Assert.IsType<ReqAssignment>(datos);
			Assert.NotNull(type);
		}

		[Fact]
		public void ReqAssignmentUpdate()
		{
			ReqAssignmentUpdate datos = new ReqAssignmentUpdate();
			datos.id = 1;
			datos.estadoSolicitud = "string";

			var type = Assert.IsType<ReqAssignmentUpdate>(datos);
			Assert.NotNull(type);
		}

		[Fact]
		public void ReqAdminTecnica()
		{
			ReqAdminTecnica datos = new ReqAdminTecnica();
			datos.codigo = 1;
			datos.nombre = "string";
			datos.valor = "string";
			datos.descripcion = "string";
			datos.estadoRegistro = true;

			var type = Assert.IsType<ReqAdminTecnica>(datos);
			Assert.NotNull(type);
		}

		[Fact]
		public void ReqId()
		{
			ReqId datos = new ReqId();
			datos.id = 1;

			var type = Assert.IsType<ReqId>(datos);
			Assert.NotNull(type);
		}

        [Fact]
        public void ReqEstados_Properties()
        {
            // Arrange
            var reqEstados = new ReqEstados();

            // Act
            reqEstados.pkT008codigo = 1;
            reqEstados.a008posicion = 2;
            reqEstados.a008codigoParametricaEstado = "Estado1";
            reqEstados.a008descripcion = "Descripción";
            reqEstados.a008etapa = "Etapa1";
            reqEstados.a008estadoRegistro = "Activo";
            reqEstados.estate = new List<SelectListItem>
        {
            new SelectListItem { Text = "Item1", Value = "1" },
            new SelectListItem { Text = "Item2", Value = "2" }
        };
            reqEstados.stage = new List<SelectListItem>
        {
            new SelectListItem { Text = "Stage1", Value = "A" },
            new SelectListItem { Text = "Stage2", Value = "B" }
        };

            // Assert
            Assert.Equal(1, reqEstados.pkT008codigo);
            Assert.Equal(2, reqEstados.a008posicion);
            Assert.Equal("Estado1", reqEstados.a008codigoParametricaEstado);
            Assert.Equal("Descripción", reqEstados.a008descripcion);
            Assert.Equal("Etapa1", reqEstados.a008etapa);
            Assert.Equal("Activo", reqEstados.a008estadoRegistro);
            Assert.NotNull(reqEstados.estate);
            Assert.Equal(2, reqEstados.estate.Count);
            Assert.Equal("Item1", reqEstados.estate[0].Text);
            Assert.Equal("Item2", reqEstados.estate[1].Text);
            Assert.NotNull(reqEstados.stage);
            Assert.Equal(2, reqEstados.stage.Count);
            Assert.Equal("Stage1", reqEstados.stage[0].Text);
            Assert.Equal("Stage2", reqEstados.stage[1].Text);
        }

        [Fact]
        public void ReqRoles_Properties()
        {
            // Arrange
            var reqRoles = new ReqRoles();

            // Act
            reqRoles.id = 1;
            reqRoles.estado = "Activo";
            reqRoles.name = "Nombre";
            reqRoles.cargo = "Cargo";
            reqRoles.descripcion = "Descripción";
            reqRoles.estate = new List<SelectListItem>
        {
            new SelectListItem { Text = "Item1", Value = "1" },
            new SelectListItem { Text = "Item2", Value = "2" }
        };

            // Assert
            Assert.Equal(1, reqRoles.id);
            Assert.Equal("Activo", reqRoles.estado);
            Assert.Equal("Nombre", reqRoles.name);
            Assert.Equal("Cargo", reqRoles.cargo);
            Assert.Equal("Descripción", reqRoles.descripcion);
            Assert.NotNull(reqRoles.estate);
            Assert.Equal(2, reqRoles.estate.Count);
            Assert.Equal("Item1", reqRoles.estate[0].Text);
            Assert.Equal("Item2", reqRoles.estate[1].Text);
        }

        [Fact]
        public void ReqModulos_Properties()
        {
            // Arrange
            var reqModulos = new ReqModulos();

            // Act
            reqModulos.rolId = 1;
            reqModulos.moduleId = "Module1";
            reqModulos.consult = true;
            reqModulos.create = false;
            reqModulos.update = true;
            reqModulos.delete = false;
            reqModulos.see = true;
            reqModulos.name = "NombreModulo";

            // Assert
            Assert.Equal(1, reqModulos.rolId);
            Assert.Equal("Module1", reqModulos.moduleId);
            Assert.True(reqModulos.consult);
            Assert.False(reqModulos.create);
            Assert.True(reqModulos.update);
            Assert.False(reqModulos.delete);
            Assert.True(reqModulos.see);
            Assert.Equal("NombreModulo", reqModulos.name);
        }

        [Fact]
        public void ModulosReq_Properties()
        {
            // Arrange
            var modulosReq = new ModulosReq();

            // Act
            modulosReq.id = 1;
            modulosReq.name = "NombreModulo";

            // Assert
            Assert.Equal(1, modulosReq.id);
            Assert.Equal("NombreModulo", modulosReq.name);
        }

        [Fact]
        public void UpdateDocument_Properties()
        {
            // Arrange
            var updateDocument = new UpdateDocument();

            // Act
            updateDocument.id = 1;
            updateDocument.cambiosDoc = "Cambios en el documento";

            Assert.Equal(1, updateDocument.id);
            Assert.Equal("Cambios en el documento", updateDocument.cambiosDoc);
        }
    }
}
