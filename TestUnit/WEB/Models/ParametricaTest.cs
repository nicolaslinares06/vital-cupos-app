using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Models;

namespace TestUnit.WEB.Models
{
	public class ParametricaTest
	{
        private Fixture _fixture;

        [Fact]
		public void ValorParametro()
		{
			ValorParametro datos = new ValorParametro();
			datos.a008Parametrica = "string";

			var type = Assert.IsType<ValorParametro>(datos);
			Assert.NotNull(type);
		}
	}
}
