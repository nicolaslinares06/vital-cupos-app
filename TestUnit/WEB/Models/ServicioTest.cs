using System;
using CUPOS_FRONT.Models;

namespace CUPOS_FRONT_Tests.Models
{
    public class ServicioTest
    {
        [Fact]
        public void Servicio()
        {
            Servicio servicio = new Servicio();
            servicio.Nombre = "string";
            servicio.Url = "string";
            servicio.Estado = "string";

            var type = Assert.IsType<Servicio>(servicio);
            Assert.NotNull(type);
        }
    }
}
