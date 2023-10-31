using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Controllers;

namespace TestUnit.WEB
{
	public class ProgramTest
	{
		//Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
		private Program program;

		public ProgramTest()
		{
			program = new Program();
		}

		[Fact]
		public void GetTypeTest()
		{
			var r = program.GetType();
			Assert.True(r != null);
		}

		[Fact]
		public void GetHashCodeTest()
		{
			var r = program.GetHashCode();
			Assert.True(r != null);
		}
	}
}
