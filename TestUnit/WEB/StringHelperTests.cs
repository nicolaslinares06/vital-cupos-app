using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Helpers;

namespace Test.Repository.Helpers
{
    public class StringHelperTests
    {
        [Fact]
        public void esEmailValido_DevuelveTrue_CuandoElEmailEsValido()
        {
            // Arrange
            string email = "correo@dominio.com";

            // Act
            bool esValido = StringHelper.esEmailValido(email);

            // Assert
            Assert.True(esValido);
        }

        [Fact]
        public void esEmailValido_DevuelveFalse_CuandoElEmailNoEsValido()
        {
            // Arrange
            string email = "correo.com";

            // Act
            bool esValido = StringHelper.esEmailValido(email);

            // Assert
            Assert.False(esValido);
        }

        [Fact]
        public void generaContrasenaAleatoria_DevuelveUnaContrasenaDe6Caracteres()
        {
            // Arrange
            Random rdn = new Random();
            string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890%$#@";

            // Act
            string contrasena = StringHelper.generaContrasenaAleatoria();

            // Assert
            Assert.Equal(6, contrasena.Length);
        }
    }
}
