using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Models;

namespace TestUnit.WEB.Models
{
	public class SpeciesTest
	{
        private Fixture _fixture;

        [Fact]
		public void Species()
		{
			Species datos = new Species();
			datos.Code = 1;
			datos.CommonName = "string";
			datos.Name = "string";
			datos.NameFamily = "string";
			datos.ScientificName = "string";

			var type = Assert.IsType<Species>(datos);
			Assert.NotNull(type);
		}


	}
}
