using AutoFixture;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Models;

namespace TestUnit.WEB.Models
{
	public class RegistroVisitaDeCortesModelsTest
	{
        private Fixture _fixture;

        [Fact]
		public void ActaRegistro()
		{
			ActaRegistro datos = new ActaRegistro();
			datos.VisitReportId = 1;
			datos.ReportType = "string";
			datos.VisitNumber = 1;
			datos.Establishment = "string";
			datos.EstablishmentType = "string";
			datos.Date = DateTime.Now;
			datos.ReportTypeId = 1;
			datos.FechaFormat = "string";
			datos.NumerosVisitas = "string";
			datos.VisitNumberOne = true;
			datos.VisitNumberTwo = true;
			datos.RegistrationStatus = 1;
			datos.CreationDateDecimal = 1;
			datos.FechaRegistroFormat = "string";

			var type = Assert.IsType<ActaRegistro>(datos);
			Assert.NotNull(type);
		}

		[Fact]
		public void Ciudades()
		{
			Ciudades datos = new Ciudades();
			datos.PkT004codigo = 1;
			datos.A004nombre = "string";

			var type = Assert.IsType<Ciudades>(datos);
			Assert.NotNull(type);
		}

		[Fact]
		public void EditVisitaActaCortes()
		{
			EditVisitaActaCortes datos = new EditVisitaActaCortes();
			datos.ActaVisitaId = 1;
			datos.VisitaNumero = 1;
			datos.VisitaNumero1 = false;
			datos.VisitaNumero2 = false;
			datos.TipoEstablecimiento = 1;
			datos.TipoEstablecimientoNombre = "string";
			datos.NitEstablecimiento = 1;
			datos.EstablecimientoID = 1;
			datos.NombreEstablecimiento = "string";
			datos.CantidadPielACortar = 1;
			datos.PrecintoIdentificacion = 1;
			datos.EstadoPiel = "string";
			datos.FuncionarioAutoridadCites = 1;
			datos.DocumentoRepresentante = 1;
			datos.RepresentanteEstablecimiento = "string";
			datos.Departamento = 1;
			datos.Ciudad = 1;
			datos.Fecha = DateTime.Today;
			datos.FechaFormat = "string";
			datos.TipoActaVisita = 1;
			datos.EstadoPielInt = 1;
			datos.NombreCiudadDepartamento = "string";
			datos.Departamentos = null;
			datos.Ciudades = null;
			datos.FuncionarioAutoridadCitesNombre = "string";
			datos.ArchivoExcelPrecinto = null;

			var type = Assert.IsType<EditVisitaActaCortes>(datos);
			Assert.NotNull(type);
		}

		[Fact]
		public void CreateRegistroVisitaCortesIdentificablesModelView()
		{
			CreateRegistroVisitaCortesIdentificablesModelView datos = new CreateRegistroVisitaCortesIdentificablesModelView();
			datos.TiposEstablecimientos = null;
			datos.Departamentos = null;
			datos.Ciudades = null;
			datos.ActaVisitaCortes = null;

			var type = Assert.IsType<CreateRegistroVisitaCortesIdentificablesModelView>(datos);
			Assert.NotNull(type);
		}

		[Fact]
		public void CreateRegistroVisitaCortesFraccionesIrregulares()
		{
			CreateRegistroVisitaCortesFraccionesIrregulares datos = new CreateRegistroVisitaCortesFraccionesIrregulares();
			datos.TiposEstablecimientos = null;
			datos.Departamentos = null;
			datos.Ciudades = null;

			var type = Assert.IsType<CreateRegistroVisitaCortesFraccionesIrregulares>(datos);
			Assert.NotNull(type);
		}

		[Fact]
		public void EstablecimientoProps()
		{
			EstablecimientoProps datos = new EstablecimientoProps();
			datos.TipoEstablecimiento = 1;
			datos.TipoEstablecimientoNombre = "string";
			datos.EstablecimientoID = 1;
			datos.NombreEstablecimiento = "string";
			datos.NITEstablecimiento = 1;

			var type = Assert.IsType<EstablecimientoProps>(datos);
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
        public void ReqValorTecnica_Properties()
        {
            // Arrange
            var reqValorTecnica = new ReqValorTecnica();

            // Act
            reqValorTecnica.pkT007Codigo = 1;
            reqValorTecnica.a007nombre = "Nombre";
            reqValorTecnica.a007valor = "Valor";
            reqValorTecnica.a007descripcion = "Descripción";
            reqValorTecnica.a007estadoRegistro = "Activo";
            reqValorTecnica.estate = new List<SelectListItem>
        {
            new SelectListItem { Text = "Item1", Value = "1" },
            new SelectListItem { Text = "Item2", Value = "2" }
        };

            // Assert
            Assert.Equal(1, reqValorTecnica.pkT007Codigo);
            Assert.Equal("Nombre", reqValorTecnica.a007nombre);
            Assert.Equal("Valor", reqValorTecnica.a007valor);
            Assert.Equal("Descripción", reqValorTecnica.a007descripcion);
            Assert.Equal("Activo", reqValorTecnica.a007estadoRegistro);
            Assert.NotNull(reqValorTecnica.estate);
            Assert.Equal(2, reqValorTecnica.estate.Count);
            Assert.Equal("Item1", reqValorTecnica.estate[0].Text);
            Assert.Equal("Item2", reqValorTecnica.estate[1].Text);
        }
    }
}
