using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Models;

namespace TestUnit.WEB.Models
{
	public class ControlCuposTest
	{
        private Fixture _fixture;

        [Fact]
		public void Cupos()
		{
			Cupos datos = new Cupos();
			datos.codigoCupo = 1;
			datos.autoridadEmiteResolucion = "string";
			datos.codigoZoocriadero = "string";
			datos.numeroResolucion = "string";
			datos.fechaResolucion = DateTime.Now;
			datos.fechaRegistroResolucion = DateTime.Now;
			datos.fechaRadicado = DateTime.Now;
			datos.cuposOtorgados = 1;
			datos.cuposPorAnio = 1;
			datos.cuposTotal = 1;
			datos.fechaProduccion = DateTime.Now;
			datos.cuposAprovechamientoComercializacion = 1;
			datos.cuotaRepoblacion = "string";
			datos.cuposDisponibles = 1;
			datos.anioProduccion = 1;
			datos.observaciones = "string";
			datos.nitEmpresa = "string";
			datos.codigoEspecie = "string";
			datos.numeroInternoFinalCuotaRepoblacion = 1;
			datos.numeroInternoFinal = "string";
			datos.numeroInternoInicialCuotaRepoblacion = 1;
			datos.NumeroInternoInicial = "string";
			datos.cuposPorAnioProduccion = null;
			datos.cuposUtilizados = null;
			datos.codigoParametricaPagoCuotaDerepoblacion = null;
			datos.tipoEspecimen = null;
			datos.nombreEmnpresa = null;
			datos.tipoEmpresa = null;
			datos.soporteRepoblacion = null;

			var type = Assert.IsType<Cupos>(datos);
			Assert.NotNull(type);
		}

		[Fact]
		public void DatosEntidad()
		{
			DatosEntidad datos = new DatosEntidad();
			datos.codigoEmpresa = 1;
			datos.nombreEntidad = "string";
			datos.nombreEmpresa = "string";
			datos.nit = 1;
			datos.tipoDocumento = "string";
			datos.telefono = 1;
			datos.correo = "string";
			datos.ciudad = "string";
			datos.departamento = "string";
			datos.pais = "string";
			datos.direccion = "string";

			var type = Assert.IsType<DatosEntidad>(datos);
			Assert.NotNull(type);
		}

		[Fact]
		public void soportDocuments()
		{
			soportDocuments datos = new soportDocuments();
			datos.codigo = 1;
			datos.adjuntoBase64 = "string";
			datos.nombreAdjunto = "string";
			datos.tipoAdjunto = "string";

			var type = Assert.IsType<soportDocuments>(datos);
			Assert.NotNull(type);
		}

		[Fact]
		public void CupoPorAnios()
		{
			cupoPorAnios datos = new cupoPorAnios();
			datos.anio = 1;
			datos.cupos = 1;

			var type = Assert.IsType<cupoPorAnios>(datos);
			Assert.NotNull(type);
		}

		[Fact]
		public void InventarioCupos()
		{
			InventarioCupos datos = new InventarioCupos();
			datos.especie = 1;
			datos.codigo = 1;
			datos.numeroCartaVendeFactura = "string";
			datos.quienVende = "string";
			datos.fechaVenta = DateTime.Now;
			datos.disponibilidadInventario = 1;
			datos.fechaProduccion = DateTime.Now;
			datos.anio = 1;
			datos.inventarioDisponible = 1;

			var type = Assert.IsType<InventarioCupos>(datos);
			Assert.NotNull(type);
		}

        [Fact]
        public void ElementTypes()
        {
            ElementTypes element = new ElementTypes
            {
                code = 1,
                name = "string"
            };

            var type = Assert.IsType<ElementTypes>(element);
            Assert.NotNull(type);
        }

        [Fact]
        public void resolutionQuota()
        {
            resolutionQuota quota = new resolutionQuota
            {
                resolucionInfo = new cupoGuardar
                {
                    codigoCupo = 1,
                    autoridadEmiteResolucion = "string",
                    codigoZoocriadero = "string",
                    numeroResolucion = 1,
                    fechaResolucion = DateTime.Now,
                    fechaRegistroResolucion = DateTime.Now,
                    observaciones = "string",
                    nitEmpresa = 1
                },
                especiesExportar = new exportSpecimenss
                {
                    PkT005codigo = 1,
                    codigoCupo = 1,
                    anio = 2023,
                    cupos = 1,
                    id = 1,
                    codigoParametricaTipoMarcaje = "string",
                    codigoEspecie = "string",
                    codigoParametricaPagoCuotaDerepoblacion = "string",
                    fechaRadicado = DateTime.Now,
                    tipoEspecimen = "string",
                    añoProduccion = 2023,
                    marcaLote = "string",
                    condicionesMarcaje = "string",
                    poblacionParentalMacho = 1,
                    poblacionParentalHembra = 1,
                    poblacionParentalTotal = 1,
                    poblacionSalioDeIncubadora = 1,
                    poblacionDisponibleParaCupos = 1,
                    individuosDestinadosARepoblacion = "string",
                    cupoAprovechamientoOtorgados = 1,
                    tasaReposicion = "string",
                    numeroMortalidad = 1,
                    cuotaRepoblacionParaAprovechamiento = true,
                    cupoAprovechamientoOtorgadosPagados = "string",
                    observaciones = "string",
                    cuotaRepoblacion = "string",
                    numeroInternoInicialCuotaRepoblacion = "string",
                    numeroInternoFinalCuotaRepoblacion = "string",
                    numeroInternoInicial = 1,
                    numeroInternoFinal = 1,
                    totalCupos = 1,
                    cuposDisponibles = 1,
                    sePagoCuotaRepoblacion = true,
                    seRegistroEspecieComercializar = true
                }
            };

            var type = Assert.IsType<resolutionQuota>(quota);
            Assert.NotNull(type);
        }

        [Fact]
        public void exportSpecimenss()
        {
            exportSpecimenss specimens = new exportSpecimenss
            {
                PkT005codigo = 1,
                codigoCupo = 1,
                anio = 2023,
                cupos = 1,
                id = 1,
                codigoParametricaTipoMarcaje = "string",
                codigoEspecie = "string",
                codigoParametricaPagoCuotaDerepoblacion = "string",
                fechaRadicado = DateTime.Now,
                tipoEspecimen = "string",
                añoProduccion = 2023,
                marcaLote = "string",
                condicionesMarcaje = "string",
                poblacionParentalMacho = 1,
                poblacionParentalHembra = 1,
                poblacionParentalTotal = 1,
                poblacionSalioDeIncubadora = 1,
                poblacionDisponibleParaCupos = 1,
                individuosDestinadosARepoblacion = "string",
                cupoAprovechamientoOtorgados = 1,
                tasaReposicion = "string",
                numeroMortalidad = 1,
                cuotaRepoblacionParaAprovechamiento = true,
                cupoAprovechamientoOtorgadosPagados = "string",
                observaciones = "string",
                cuotaRepoblacion = "string",
                numeroInternoInicialCuotaRepoblacion = "string",
                numeroInternoFinalCuotaRepoblacion = "string",
                numeroInternoInicial = 1,
                numeroInternoFinal = 1,
                totalCupos = 1,
                cuposDisponibles = 1,
                sePagoCuotaRepoblacion = true,
                seRegistroEspecieComercializar = true
            };

            var type = Assert.IsType<exportSpecimenss>(specimens);
            Assert.NotNull(type);
        }

        [Fact]
        public void saveResolutionQuotas()
        {
            saveResolutionQuotas saveQuotas = new saveResolutionQuotas
            {
                datoGuardar = new cupoGuardar
                {
                    codigoCupo = 1,
                    autoridadEmiteResolucion = "string",
                    codigoZoocriadero = "string",
                    numeroResolucion = 1,
                    fechaResolucion = DateTime.Now,
                    fechaRegistroResolucion = DateTime.Now,
                    observaciones = "string",
                    nitEmpresa = 1
                },
                datosEspecieExportarNuevo = new List<exportSpecimenss>
                {
                    new exportSpecimenss
                    {
                        PkT005codigo = 1,
                        codigoCupo = 1,
                        anio = 2023,
                        cupos = 1,
                        id = 1,
                        codigoParametricaTipoMarcaje = "string",
                        codigoEspecie = "string",
                        codigoParametricaPagoCuotaDerepoblacion = "string",
                        fechaRadicado = DateTime.Now,
                        tipoEspecimen = "string",
                        añoProduccion = 2023,
                        marcaLote = "string",
                        condicionesMarcaje = "string",
                        poblacionParentalMacho = 1,
                        poblacionParentalHembra = 1,
                        poblacionParentalTotal = 1,
                        poblacionSalioDeIncubadora = 1,
                        poblacionDisponibleParaCupos = 1,
                        individuosDestinadosARepoblacion = "string",
                        cupoAprovechamientoOtorgados = 1,
                        tasaReposicion = "string",
                        numeroMortalidad = 1,
                        cuotaRepoblacionParaAprovechamiento = true,
                        cupoAprovechamientoOtorgadosPagados = "string",
                        observaciones = "string",
                        cuotaRepoblacion = "string",
                        numeroInternoInicialCuotaRepoblacion = "string",
                        numeroInternoFinalCuotaRepoblacion = "string",
                        numeroInternoInicial = 1,
                        numeroInternoFinal = 1,
                        totalCupos = 1,
                        cuposDisponibles = 1,
                        sePagoCuotaRepoblacion = true,
                        seRegistroEspecieComercializar = true
                    }
                }
            };

            var type = Assert.IsType<saveResolutionQuotas>(saveQuotas);
            Assert.NotNull(type);
        }

        [Fact]
        public void cupoPorAnios()
        {
            cupoPorAnios anios = new cupoPorAnios
            {
                anio = 2023,
                cupos = 1
            };

            var type = Assert.IsType<cupoPorAnios>(anios);
            Assert.NotNull(type);
        }

        [Fact]
        public void cupoGuardar()
        {
            cupoGuardar guardar = new cupoGuardar
            {
                codigoCupo = 1,
                autoridadEmiteResolucion = "string",
                codigoZoocriadero = "string",
                numeroResolucion = 1,
                fechaResolucion = DateTime.Now,
                fechaRegistroResolucion = DateTime.Now,
                observaciones = "string",
                nitEmpresa = 1
            };

            var type = Assert.IsType<cupoGuardar>(guardar);
            Assert.NotNull(type);
        }

        [Fact]
        public void cupoTotales()
        {
            cupoTotales totales = new cupoTotales
            {
                availableQuotas = 1,
                availableInventory = 1,
                pendingQuotasForProcessing = 1
            };

            var type = Assert.IsType<cupoTotales>(totales);
            Assert.NotNull(type);
        }
    }
}
