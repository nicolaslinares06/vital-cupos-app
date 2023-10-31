using AutoFixture;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUnit.WEB.Models
{
	public class CuposV003SolicitudPrecintosNacionalesTest
	{
        private Fixture _fixture;

        [Fact]
		public void CuposV003SolicitudPrecintosNacionales()
		{
			CuposV003SolicitudPrecintosNacionales datos = new CuposV003SolicitudPrecintosNacionales();
			datos.ID = 1;
			datos.CIUDAD = "string";
			datos.FECHA = DateTime.Now;
			datos.ESTABLECIMIENTO = "string";
			datos.PRIMERNOMBRE = "string";
			datos.SEGUNDONOMBRE = "string";
			datos.PRIMERAPELLIDO = "string";
			datos.SEGUNDOAPELLIDO = "string";
			datos.TIPOIDENTIFICACION = "string";
			datos.NUMEROIDENTIFICACION = 1;
			datos.DIRECCIONENTREGA = "string";
			datos.CIUDADENTREGA = "string";
			datos.TELEFONO = 1;
			//datos.FAX = "string";
			datos.CANTIDAD = 1;
			datos.ESPECIE = "string";
			datos.CODIGOINICIAL = 1;
			datos.CODIGOFINAL = 1;
			datos.LONGITUDMENOR = 1;
			datos.LONGITUDMAYOR = 1;
			datos.FECHALEGAL = DateTime.Now;
			datos.OBSERVACIONES = "string";
			datos.RESPUESTA = "string";
			//datos.CONSIGNACION = true;
			datos.CUPOSDISPONIBLES = 1;
			datos.FECHADESISTIMIENTO = DateTime.Now;
			datos.NUMERORADICACION = "string";
			datos.FECHARADICACION = DateTime.Now;
			datos.FECHAESTADO = DateTime.Now;
			datos.OBSERVACIONESDESISTIMIENTO = "string";
			datos.ANALISTA = "0";
			datos.FECHARESOLUCION = DateTime.Now;
			datos.NUMERORESOLUCION = 1;
			datos.CODIGOESPECIE = 1;
			datos.NUMERORADICACIONSALIDA = "string";
			datos.FECHARADICACIONSALIDA = DateTime.Now;

			var type = Assert.IsType<CuposV003SolicitudPrecintosNacionales>(datos);
			Assert.NotNull(type);
		}
	}
}
