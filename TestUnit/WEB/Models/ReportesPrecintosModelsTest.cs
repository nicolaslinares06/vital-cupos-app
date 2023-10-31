using AutoFixture;
using CUPOS_FRONT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CUPOS_FRONT.Models.ReportesPrecintosModels;

namespace TestUnit.WEB.Models
{
	public class ReportesPrecintosModelsTest
	{
        private Fixture _fixture;

        [Fact]
		public void DatosPrecintosModel()
		{
			DatosPrecintosModel datos = new DatosPrecintosModel();
            datos.RadicationNumber = "string";
            datos.RadicationDate = "string";
            datos.CompanyName = "string";
            datos.NIT = 1;
            datos.City = "string";
            datos.DeliveryAddress = "string";
            datos.Telephone = "string";
            datos.Species = "string";
            datos.LesserLength = 1;
            datos.GreaterLength = 1;
            datos.Quantity = 1;
            datos.Color = "string";
            datos.ProductionYear = 1;
            datos.InitialInternalNumber = 1;
            datos.FinalInternalNumber = 1;
            datos.InitialNumber = "0";
            datos.FinalNumber = "0";
            datos.CompanyCode = 1;
            datos.DepositValue = "string";
            datos.Analyst = "string";

            var type = Assert.IsType<DatosPrecintosModel>(datos);
			Assert.NotNull(type);
		}

	}
}
