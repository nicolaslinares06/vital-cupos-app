using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Models;

namespace TestUnit.WEB.Models
{
    public class AuditoriaTest
	{
		private Fixture _fixture;

		[Fact]
		public void Auditoria()
		{
			Auditoria datos = new Auditoria();
			datos.nombre = "string";
			datos.accion = "string";
			datos.fecha = DateTime.Now;
			datos.subModulo = "string";
			datos.ip = "string";
			datos.rol = "string";
			datos.a013camposModificados = "string";
			datos.a013estadoAnterior = "string";
			datos.a013estadoActual = "string";

			var type = Assert.IsType<Auditoria>(datos);
			Assert.NotNull(type);
		}
	}
}
