using AutoFixture;
using Repository.Helpers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUnit.WEB.Models
{
	public class NationalSealsManagementTest
	{
        private Fixture _fixture;

        [Fact]
		public void SoportsDocuments()
		{
			SoportsDocuments datos = new SoportsDocuments();
			datos.codigo = 1;
			datos.adjuntoBase64 = "string";
			datos.nombreAdjunto = "string";
			datos.tipoAdjunto = "string";

			var type = Assert.IsType<SoportsDocuments>(datos);
			Assert.NotNull(type);
		}

		[Fact]
		public void DocumentApplication()
		{
			DocumentApplication datos = new DocumentApplication();
			datos.Codigo = 1;
			datos.NombreArchivo = "string";
			datos.Base64string = "string";
			datos.TipoArchivo = "string";

			var type = Assert.IsType<DocumentApplication>(datos);
			Assert.NotNull(type);
		}

		[Fact]
		public void DocumentInformation()
		{
			DocumentInformation datos = new DocumentInformation();
			datos.codigo = 1;
			datos.nombreArchivo = "string";
			datos.url = "string";

			var type = Assert.IsType<DocumentInformation>(datos);
			Assert.NotNull(type);
		}

		[Fact]
		public void ConsultCodes()
		{
			ConsultCodes datos = new ConsultCodes();
			datos.A019Cantidad = 1;

			var type = Assert.IsType<ConsultCodes>(datos);
			Assert.NotNull(type);
		}

        [Fact]
        public void Especies()
        {
            Especies especies = new Especies();
            especies.pkT005Codigo = 1;
            especies.a005Nombre = "string";

            var type = Assert.IsType<Especies>(especies);
            Assert.NotNull(type);
        }

        [Fact]
        public void SignApplicationDocument()
        {
            SignApplicationDocument document = new SignApplicationDocument();
            document.code = 1;
            document.initialNumbering = 1001.0M;
            document.finalNumbering = 1010.0M;
            document.amount = 10;

            var type = Assert.IsType<SignApplicationDocument>(document);
            Assert.NotNull(type);
        }

        [Fact]
        public void GenerateSealCodes()
        {
            GenerateSealCodes codes = new GenerateSealCodes();
            codes.code = "ABC123";
            codes.codeSpecies = 1.0M;
            codes.initialNumber = "1001";
            codes.finalNumber = "1010";
            codes.color = 2.5M;
            codes.amount = 10.0M;
            codes.worth = 100.0M;
            codes.observations = "Some Observations";
            codes.tipoSolicitud = "Some TipoSolicitud";

            var type = Assert.IsType<GenerateSealCodes>(codes);
            Assert.NotNull(type);
        }

        [Fact]
        public void Consult_Codes()
        {
            ConsultCodes codes = new ConsultCodes
            {
                A006numeroInicial = "1001",
                A006numeroFinal = "1010",
                A019Cantidad = 10
            };

            var type = Assert.IsType<ConsultCodes>(codes);
            Assert.NotNull(type);
        }

        [Fact]
        public void valCutMail()
        {
            valCutMail mail = new valCutMail
            {
                cuttingCode = 1.0M,
                cantCut = 10.0M,
                areaCut = 100.0M,
                tipoPart = "Some TipoPart",
                safeGuard = "Some SafeGuard"
            };

            var type = Assert.IsType<valCutMail>(mail);
            Assert.NotNull(type);
        }

        [Fact]
        public void NitEmpresa()
        {
            NitEmpresa empresa = new NitEmpresa
            {
                a001nit = 1234567890
            };

            var type = Assert.IsType<NitEmpresa>(empresa);
            Assert.NotNull(type);
        }

        [Fact]
        public void ColorPrecinto()
        {
            ColorPrecinto colorPrecinto = new ColorPrecinto
            {
                a008valor = "Some Valor"
            };

            var type = Assert.IsType<ColorPrecinto>(colorPrecinto);
            Assert.NotNull(type);
        }


        [Fact]
        public void MailApproval()
        {
            MailApproval approval = new MailApproval
            {
                code = 1.0M,
                numberradication = "12345",
                filingDate = DateTime.Now,
                establishment = "Some Establishment",
                nit = "1234567890",
                city = "Some City",
                address = "Some Address",
                phone = "123-456-7890",
                amount = 10,
                color = "Some Color",
                initialNumbering = "1001",
                finalNumbering = "1010",
                cantCut = "10.0M",
                areaCut = "100.0M",
                tipoPart = "Some TipoPart",
                safeGuard = "Some SafeGuard",
                initialInternalCoding = "Initial Coding",
                finalInternalCoding = "Final Coding",
                subtotal = 100,
                consignmentValue = 1000,
                sendDate = DateTime.Now,
                analyst = "Some Analyst"
            };

            var type = Assert.IsType<MailApproval>(approval);
            Assert.NotNull(type);
        }

        [Fact]
        public void RequestCutting()
        {
            RequestCutting cutting = new RequestCutting
            {
                ID = 1,
                CIUDAD = "Some City",
                CIUDADENTREGA = "Some Delivery City",
                FECHA = DateTime.Now,
                ESTABLECIMIENTO = "Some Establishment",
                TELEFONO = 1234567890,
                CANTIDAD = 10.0M,
                DIRECCIONENTREGA = "Some Delivery Address",
                NUMERORADICACION = "12345",
                FECHARADICACION = DateTime.Now,
                ANALISTA = "Some Analyst",
                VALORCONSIGNACION = 1000.0M,
                TIPOSOLICITUD = "Some TipoSolicitud",
                TYPEREQUESTCODE = 1,
                CODIGOCORTEPIELSOLICITUD = 2,
                NITEMPRESA = 1234567890,
                cuttingCode = 3.0M,
                cantCut = 10.0M,
                areaCut = 100.0M,
                tipoPart = "Some TipoPart",
                safeGuard = "Some SafeGuard"
            };

            var type = Assert.IsType<RequestCutting>(cutting);
            Assert.NotNull(type);
        }

        [Fact]
        public void ElementEspecimen()
        {
            ElementEspecimen espcimen = new ElementEspecimen
            {
                code = 1.0M,
                name = "Some Name",
                nameComun = "Some Common Name"
            };

            var type = Assert.IsType<ElementEspecimen>(espcimen);
            Assert.NotNull(type);
        }
    }
}
