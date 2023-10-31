using AutoFixture;
using Repository.Helpers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUnit.WEB.Models
{
	public class InventoryTest
	{
        private Fixture _fixture;

        [Fact]
		public void Inventory()
		{
			Inventory datos = new Inventory();
			datos.quotaCode = 1;
			datos.Code = 1;
			datos.NumberSaleCarte = "string";
			datos.ReasonSocial = "string";
			datos.SaleDate = DateTime.Now;
			datos.AvailabilityInventory = 1;
			datos.Year = "string";
			datos.AvailableInventory = 1;
			datos.InitialNumeration = 1;
			datos.FinalNumeration = 1;
			datos.InitialNumerationRePoblation = 1;
			datos.FinalNumerationRePoblation = 1;
			datos.InitialNumerationSeal = 1;
			datos.FinalNumerationSeal = 1;
			datos.SpeciesCode = 1;
			datos.SpeciesName = "string";
			datos.InventorySold = 1;

			var type = Assert.IsType<Inventory>(datos);
			Assert.NotNull(type);
		}
	}
}
