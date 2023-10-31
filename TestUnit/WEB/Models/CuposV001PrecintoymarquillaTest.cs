using AutoFixture;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Models;

namespace TestUnit.WEB.Models
{
	public class CuposV001PrecintoymarquillaTest
	{
        private Fixture _fixture;

        [Fact]
		public void CuposV001Precintoymarquilla()
		{
			CuposV001Precintoymarquilla datos = new CuposV001Precintoymarquilla();
			datos.PKV001CODIGO = 1;
			datos.TIPODOCUMENTO = "string";
			datos.NUMERO = 1;
			datos.NOMBRE = "string";
			datos.NUMERORADICADO = "string";
			datos.NUMEROPERMISOCITES = "string";
			datos.FECHAINICIAL = DateTime.Now;
			datos.FECHAFINAL = DateTime.Now;
			datos.NUMEROINICIAL = "string";
			datos.NUMEROFINAL = "string";
			datos.NUMEROINTERNOINICIAL = 1;
			datos.NUMEROINTERNOFINAL = 1;
			datos.VIGENCIA = DateTime.Now;
			datos.CANTIDAD = 1;
			datos.COLOR = "string";
			datos.ESPECIE = "string";
			datos.CUPOSDISPONIBLES = 1;
			datos.CUPOSTOTAL = 1;
			datos.FECHACREACION = DateTime.Now;

			var type = Assert.IsType<CuposV001Precintoymarquilla>(datos);
			Assert.NotNull(type);
		}
	}
}
