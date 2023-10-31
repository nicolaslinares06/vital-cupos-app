using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Models;

namespace TestUnit.WEB.Models
{
    public class CertificadosFloraNoMaderableTest
    {
        [Fact]
        public void CertificadosFloraNoMaderable_Properties()
        {
            // Arrange
            var certificado = new CertificadosFloraNoMaderable();

            // Act
            certificado.codigoCertificado = 1;
            certificado.numeroCertificacion = "Cert-001";
            certificado.fechaCertificacion = DateTime.Now;
            certificado.fechaRegistroCertificacion = DateTime.Now;
            certificado.nit = 1234567890;
            certificado.vigenciaCertificacion = DateTime.Now.AddYears(1);

            // Assert
            Assert.Equal(1, certificado.codigoCertificado);
            Assert.Equal("Cert-001", certificado.numeroCertificacion);
            Assert.NotNull(certificado.fechaCertificacion);
            Assert.NotNull(certificado.fechaRegistroCertificacion);
            Assert.Equal(1234567890, certificado.nit);
            Assert.NotNull(certificado.vigenciaCertificacion);
        }

        [Fact]
        public void ElementTypesEspecies_Properties()
        {
            // Arrange
            var especie = new ElementTypesEspecies();

            // Act
            especie.id = 1;
            especie.text = "Especie 1";
            especie.NameFamily = "Familia 1";
            especie.CommonName = "Nombre Común";

            // Assert
            Assert.Equal(1, especie.id);
            Assert.Equal("Especie 1", especie.text);
            Assert.Equal("Familia 1", especie.NameFamily);
            Assert.Equal("Nombre Común", especie.CommonName);
        }
    }
}
