using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Models;

namespace TestUnit.WEB.Models
{
	public class RolesTest
	{
        private Fixture _fixture;

        [Fact]
		public void Roles()
		{
			Roles datos = new Roles();
			datos.rolId = 1;
			datos.name = "string";
			datos.position = "string";
			datos.description = "string";
			datos.estate = true;

			var type = Assert.IsType<Roles>(datos);
			Assert.NotNull(type);
		}
	}
}
