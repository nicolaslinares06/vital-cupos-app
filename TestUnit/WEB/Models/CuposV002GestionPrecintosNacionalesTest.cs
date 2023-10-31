using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUnit.WEB.Models;

namespace Web.Models
{
    public class CuposV002GestionPrecintosNacionalesTest
    {
        [Fact]
        public void CuposV002GestionPrecintosNacionales()
        {
            CuposV002GestionPrecintosNacionales datos = new CuposV002GestionPrecintosNacionales();
            datos.ID = 1;
            datos.NUMERORADICADO = "string";
            datos.FECHARADICADO = DateTime.Now;
            datos.PRECINTOSNACIONALES = "string";
            datos.ENTIDAD = "string";
            datos.FECHASOLICITUD = DateTime.Now;
            datos.ESTADO = "string";
            datos.ANALISTA = 1;
            datos.NUMERORADICADOSALIDA = "string";
            datos.FECHARADICADOSALIDA = DateTime.Now;
            datos.TIPOSOLICITUD = "string";

            var type = Assert.IsType<CuposV002GestionPrecintosNacionales>(datos);
            Assert.NotNull(type);
        }
    }
}
