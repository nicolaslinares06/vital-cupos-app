using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CUPOS_FRONT.Models.ReportesCuposEmpresasMarcajeModels;

namespace TestUnit.WEB.Models
{
	public class ReportesCuposEmpresasMarcajeModelsTest
	{
        private Fixture _fixture;

        [Fact]
		public void DatosEmpresasModel()
		{
			DatosEmpresasModel datos = new DatosEmpresasModel();
			datos.TipoEmpresa = "string";
			datos.NombreEmpresa = "string";
			datos.NIT = 1;
			datos.Estado = "string";
			datos.EstadoEmisionCITES = "string";
			datos.NumeroResolucion = "string";
			datos.FechaEmisionResolucion = DateTime.Now.ToString();
			datos.Especies = "string";
			datos.Machos = 1;
			datos.Hembras = 1;
			datos.PoblacionTotalParental = 1;
			datos.AnioProduccion = 1;
			datos.CuposComercializacion = 1;
			datos.CuotaRepoblacion = "string";
			datos.CuposAsignadosTotal = 1;
			datos.SoportesRepoblacion = true;
			datos.CupoUtilizado = 1;
			datos.CupoDisponible = 1;

			var type = Assert.IsType<DatosEmpresasModel>(datos);
			Assert.NotNull(type);
		}
	}
}
