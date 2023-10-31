using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Repository.Helpers.DocumentManager;

namespace TestUnit.WEB.Models
{
	public class DocumentManagerTest
	{
        private Fixture _fixture;

        [Fact]
		public void Archivo()
		{
			Archivo datos = new Archivo();
			datos.codigo = 1;
			datos.adjuntoBase64 = "string";
			datos.nombreAdjunto = "string";
			datos.tipoAdjunto = "string";
			datos.urlFTP = "string";

			var type = Assert.IsType<Archivo>(datos);
			Assert.NotNull(type);
		}
	}
}
