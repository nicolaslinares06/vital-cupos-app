using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Models;

namespace TestUnit.WEB.Models
{
	public class PrecintosMarquillasTest
	{
        private Fixture _fixture;

        [Fact]
		public void PrecintosMarquillas()
		{
			PrecintosMarquillas datos = new PrecintosMarquillas();
			datos.pkT006Codigo = 1;
			datos.a006CodigoPrecintoymarquilla = 1;
			datos.a006CodigoEspecieExportar = 1;
			datos.a006CodigoUsuarioCreacion = 1;
			datos.a006CodigoUsuarioModificacion = 1;
			datos.a006CodigoParametricaTipoPrecintomarquilla = 1;
			datos.a006codigoParametricaColorPrecintosymarquillas = 1;
			datos.a006EstadoRegistro = "string";
			datos.a006FechaCreacion = DateTime.Now;
			datos.a006FechaModificacion = DateTime.Now;
			datos.a006Observacion = "string";

			var type = Assert.IsType<PrecintosMarquillas>(datos);
			Assert.NotNull(type);
		}

        [Fact]
        public void TiposDocumentosEmpresas()
        {
            TiposDocumentosEmpresas tipoDocumento = new TiposDocumentosEmpresas
            {
                pkT008Codigo = 1,
                a008Parametrica = "Some Parametrica",
                a008Valor = "Some Valor"
            };

            var type = Assert.IsType<TiposDocumentosEmpresas>(tipoDocumento);
            Assert.NotNull(type);
        }

        [Fact]
        public void Colores()
        {
            Colores color = new Colores
            {
                pkT008Codigo = 1,
                a008Parametrica = "Some Parametrica",
                a008Valor = "Some Valor"
            };

            var type = Assert.IsType<Colores>(color);
            Assert.NotNull(type);
        }

        [Fact]
        public void FiltrosPrecintosMarquillas()
        {
            FiltrosPrecintosMarquillas filtro = new FiltrosPrecintosMarquillas
            {
                documentType = "Some Document Type",
                initialDate = DateTime.Now,
                number = "123",
                documentNumber = "456",
                finalDate = DateTime.Now,
                color = "Some Color",
                companyName = "Some Company Name",
                validity = "Some Validity"
            };

            var type = Assert.IsType<FiltrosPrecintosMarquillas>(filtro);
            Assert.NotNull(type);
        }
    }
}
