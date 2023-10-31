using System;
using System.ComponentModel.DataAnnotations;
using CUPOS_FRONT.Models;
using static CUPOS_FRONT.Models.ReportesMarquillasModels;

namespace CUPOS_FRONT_Tests.Models
{
    public class DatosMarquillaModelTest
    {
        [Fact]
        public void DatosMarquillaModel()
        {
            DatosMarquillaModel datos = new DatosMarquillaModel();
            datos.RadicationNumber = "string";
            datos.RadicationDate = DateTime.Now;
            datos.CompanyName = "string";
            datos.NIT = 1;
            datos.City = "string";
            datos.Address = "string";
            datos.Phone = 1;
            datos.Species = "string";
            datos.Type = "string";
            datos.SpeciesTags = "string";
            datos.Amount = 1;
            datos.Color = "string";
            datos.InitialNumber = "string";
            datos.FinalNumber = "string";
            datos.ConsignmentValue = 1;
            datos.Evaluator = "string";
            datos.AnswerDate = DateTime.Now;
            datos.InitialNumberTags = "string";
            datos.FinalNumberTags = "string";

            var type = Assert.IsType<DatosMarquillaModel>(datos);
            Assert.NotNull(type);
        }
    }
}
