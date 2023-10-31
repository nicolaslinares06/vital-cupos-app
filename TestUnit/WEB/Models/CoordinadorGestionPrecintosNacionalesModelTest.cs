using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Models;

namespace TestUnit.WEB.Models
{
	public class CoordinadorGestionPrecintosNacionalesModelTest
	{
        private Fixture _fixture;

        [Fact]
		public void CoordinadorSolicitudAprobada()
		{
			ArchivoSolicitud adjunto = new ArchivoSolicitud();
			adjunto.adjuntoBase64 = "string";
			adjunto.tipoAdjunto = "string";
			adjunto.nombreAdjunto = "string";

			CoordinadorSolicitudAprobada datos = new CoordinadorSolicitudAprobada();
			datos.InfoSolicitud = null;
			datos.ArchivoAprobacionAnalista = adjunto;

			var type = Assert.IsType<CoordinadorSolicitudAprobada>(datos);
			Assert.NotNull(type);
		}

		[Fact]
		public void InformacionSolicitud()
		{
			InformacionSolicitud datos = new InformacionSolicitud();
			datos.IdSolicitud = 1;
			datos.Ciudad = "string";
			datos.Establecimiento = "string";
			datos.Fecha = DateTime.Now;
			datos.PrimerNombre = "string";
			datos.SegundoNombre = "string";
			datos.PrimerApellido = "string";
			datos.SegundoApellido = "string";
			datos.TipoIdentificacion = "string";
			datos.NumeroIdentificacion = "string";
			datos.DireccionEntrega = "string";
			datos.CiudadEntrega = "string";
			datos.Telefonos = "string";
			datos.Fax = "string";
			datos.Cantidad = 1;
			datos.EspeciesSubEspecies = "string";
			datos.CodigoInicialPieles = 1;
			datos.CodigoFinalPieles = 1;
			datos.LongitudMenor = 1;
			datos.LongitudMayor = 1;
			datos.SoporteConsignacionBase64 = null;
			datos.FechaAsignacion = DateTime.Now;
			datos.EsSoporteConsignacion = true;
			datos.Observaciones = "string";
			datos.Analista = 1;
			datos.NumeroRadicado = "string";
			datos.Nit = 1;
			datos.ValorConsignacion = 1;
			var type = Assert.IsType<InformacionSolicitud>(datos);
			Assert.NotNull(type);
		}

		[Fact]
		public void PropsEnvioCorreoCoordinador()
		{
			PropsEnvioCorreoCoordinador datos = new PropsEnvioCorreoCoordinador();
			datos.IdSolicitud = 1;
			datos.Asunto = "string";
			datos.Correo = "string";
			datos.Body = "string";

			var type = Assert.IsType<PropsEnvioCorreoCoordinador>(datos);
			Assert.NotNull(type);
		}

        [Fact]
        public void CoordinadorGestionPrecintosNacionalesModel_Properties()
        {
            // Arrange
            var datos = new CoordinadorGestionPrecintosNacionalesModel();

            // Act
            datos.CodigoSolicitud = 1;
            datos.NumeroRadicado = "string";
            datos.SolicitudPrecintoNacional = "string";
            datos.NombreEntidadSolicitante = "string";
            datos.FechaSolicitud = "string";
            datos.FechaRadicacion = "string";
            datos.Estado = "string";
            datos.AccionVisual = "string";
            datos.NumeroRadicacionSalida = "string";
            datos.FechaRadicacionSalida = "string";

            // Assert
            Assert.Equal(1, datos.CodigoSolicitud);
            Assert.Equal("string", datos.NumeroRadicado);
            Assert.Equal("string", datos.SolicitudPrecintoNacional);
            Assert.Equal("string", datos.NombreEntidadSolicitante);
            Assert.Equal("string", datos.FechaSolicitud);
            Assert.Equal("string", datos.FechaRadicacion);
            Assert.Equal("string", datos.Estado);
            Assert.Equal("string", datos.AccionVisual);
            Assert.Equal("string", datos.NumeroRadicacionSalida);
            Assert.Equal("string", datos.FechaRadicacionSalida);
        }

        [Fact]
        public void ActualizacionAprobacionViewModel_Properties()
        {
            // Arrange
            var viewModel = new ActualizacionAprobacionViewModel();

            // Act
            viewModel.SolicitudAprobada = true;
            viewModel.IdSolicitud = 1;
            viewModel.ArchivoAprobacionAnalista = new ArchivoSolicitud();
            viewModel.AnlistaAsignado = new AnalistaSolicitud();
            viewModel.InfoSolicitud = new InformacionSolicitud();

            // Assert
            Assert.True(viewModel.SolicitudAprobada);
            Assert.Equal(1, viewModel.IdSolicitud);
            Assert.NotNull(viewModel.ArchivoAprobacionAnalista);
            Assert.NotNull(viewModel.AnlistaAsignado);
            Assert.NotNull(viewModel.InfoSolicitud);
        }

        [Fact]
        public void CoordinadorAsignacionViewModel_Properties()
        {
            // Arrange
            var viewModel = new CoordinadorAsignacionViewModel();

            // Act
            viewModel.ListadoAnalistas = new List<AnalistaAsignacion>();
            viewModel.InfoSolicitud = new InformacionSolicitud();

            // Assert
            Assert.NotNull(viewModel.ListadoAnalistas);
            Assert.NotNull(viewModel.InfoSolicitud);
        }

        [Fact]
        public void NumeracionesSolicitudCoordinador_Properties()
        {
            // Arrange
            var numeraciones = new NumeracionesSolicitudCoordinador();

            // Act
            numeraciones.Especie = "Especie 1";
            numeraciones.NumeracionInicial = 10;
            numeraciones.NumeracionFinal = 20;

            // Assert
            Assert.Equal("Especie 1", numeraciones.Especie);
            Assert.Equal(10, numeraciones.NumeracionInicial);
            Assert.Equal(20, numeraciones.NumeracionFinal);
        }

        [Fact]
        public void ParametricaTipoSolicitud_Properties()
        {
            // Arrange
            var tipoSolicitud = new ParametricaTipoSolicitud();

            // Act
            tipoSolicitud.IdTipoSolicitud = 1;
            tipoSolicitud.VistaParcial = "Vista Parcial";
            tipoSolicitud.TipoSolicitud = "Tipo de Solicitud";

            // Assert
            Assert.Equal(1, tipoSolicitud.IdTipoSolicitud);
            Assert.Equal("Vista Parcial", tipoSolicitud.VistaParcial);
            Assert.Equal("Tipo de Solicitud", tipoSolicitud.TipoSolicitud);
        }
    }
}
