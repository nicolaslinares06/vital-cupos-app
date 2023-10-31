using AutoFixture;
using Repository.Helpers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUnit.WEB.Models
{
	public class SupportDocumentsTest
	{
        private Fixture _fixture;

        [Fact]
		public void SupportDocuments()
		{
			SupportDocuments datos = new SupportDocuments();
			datos.codigo = 1;
			datos.adjuntoBase64 = "string";
			datos.nombreAdjunto = "string";
			datos.tipoAdjunto = "string";
			datos.ActionTemp = "string";

			var type = Assert.IsType<SupportDocuments>(datos);
			Assert.NotNull(type);
		}
	}
}
