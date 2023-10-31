using System;
using Web.Models;

namespace Web_Tests.Models
{
    public class GestionParametricaTest
    {
        [Fact]
        public void GestionParametrica()
        {
            GestionParametrica gestionParametrica = new GestionParametrica();
            gestionParametrica.pkT008codigo = 1;
            gestionParametrica.a008codigoUsuarioCreacion = 1;
            gestionParametrica.a008CodigoUsuarioModificacion = 1;
            gestionParametrica.a008EstadoRegistro = 1;
            gestionParametrica.a008FechaCreacion = DateTime.Now;
            gestionParametrica.a008FechaModificacion = DateTime.Now;
            gestionParametrica.a008Modulo = "string";
            gestionParametrica.a008Parametrica = "string";
            gestionParametrica.a008Valor = "string";
            gestionParametrica.a008Descripcion = "string";

            var type = Assert.IsType<GestionParametrica>(gestionParametrica);
            Assert.NotNull(type);
        }
    }
}
