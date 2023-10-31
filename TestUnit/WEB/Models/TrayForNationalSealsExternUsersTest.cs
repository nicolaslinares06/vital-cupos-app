using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Web.Models.TrayForNationalSealsExternUsers;

namespace TestUnit.WEB.Models
{
	public class TrayForNationalSealsExternUsersTest
	{
        private Fixture _fixture;

        [Fact]
		public void solicitudes()
		{
			solicitudes datos = new solicitudes();
			datos.codigo = 1;
			datos.numeroRadicado = "string";
			datos.solicitudPrecintoMarquilla = "string";
			datos.nombreEntidadSolicitante = "string";
			datos.fechaSolicitud = DateTime.Now;
			datos.fechaRadicacion = DateTime.Now;
			datos.estado = "string";
			datos.observaciones = "string";

			var type = Assert.IsType<solicitudes>(datos);
			Assert.NotNull(type);
		}


		[Fact]
		public void registerPending()
		{
			registerPending datos = new registerPending();
			datos.codigoSolicitud = 1;
			datos.fechaRadicado = DateTime.Now;
			datos.numeroRadicado = 1;

			var type = Assert.IsType<registerPending>(datos);
			Assert.NotNull(type);
		}


		[Fact]
		public void EspecimenQuota()
		{
			EspecimenQuota datos = new EspecimenQuota();
			datos.codigoCupo = 1;
			datos.codigoEspecie = "string";
			datos.cuposDisponibles = 1;
			datos.validacion = true;

			var type = Assert.IsType<EspecimenQuota>(datos);
			Assert.NotNull(type);
		}

        [Fact]
        public void SafeGuardNumbersModel()
        {
            safeGuardNumbersModel model = new safeGuardNumbersModel
            {
                id = 1,
                idCutting = 2,
                number = "12345"
            };

            var type = Assert.IsType<safeGuardNumbersModel>(model);
            Assert.NotNull(type);
        }

        [Fact]
        public void CuttingSaveModel()
        {
            cuttingSaveModel model = new cuttingSaveModel
            {
                id = 1,
                idCutting = 2,
                fractionType = "Half",
                amountSelected = 10,
                totalAreaSelected = 100
            };

            var type = Assert.IsType<cuttingSaveModel>(model);
            Assert.NotNull(type);
        }

        [Fact]
        public void CuttingSafeGuar()
        {
            cuttingSafeGuar model = new cuttingSafeGuar
            {
                salvoCon = new List<decimal> { 1, 2, 3 },
                Cut = new List<valCut>
                {
                    new valCut
                    {
                        cantCut = 0,
                        areaCut = 5,
                        tipoPart = "TypeA"
                    },
                    new valCut
                    {
                        cantCut = 1,
                        areaCut = 1,
                        tipoPart = "TypeB"
                    }
                }
            };

            var type = Assert.IsType<cuttingSafeGuar>(model);
            Assert.NotNull(type);
        }

        [Fact]
        public void ValCut()
        {
            valCut model = new valCut
            {
                cantCut = 5,
                areaCut = 5,
                tipoPart = "TypeA"
            };

            var type = Assert.IsType<valCut>(model);
            Assert.NotNull(type);
        }

        [Fact]
        public void RepresentaeLegalEmpresa()
        {
            RepresentaeLegalEmpresa model = new RepresentaeLegalEmpresa
            {
                codigoEmpresa = 1,
                ciudad = 12345,
                primerNombre = "John",
                segundoNombre = "Doe",
                primerApellido = "Smith",
                segundoApellido = "Johnson",
                tipoIdentificacion = 1,
                numeroIdentificacion = "ID12345",
                telefono = "123456789",
                fax = "987654321"
            };

            var type = Assert.IsType<RepresentaeLegalEmpresa>(model);
            Assert.NotNull(type);
        }

        [Fact]
        public void ConsultUnableNumbersModel()
        {
            ConsultUnableNumbersModel model = new ConsultUnableNumbersModel
            {
                code = 1,
                nitEmpresa = 12345,
                cupo = true,
                inventario = false
            };

            var type = Assert.IsType<ConsultUnableNumbersModel>(model);
            Assert.NotNull(type);
        }

        [Fact]
        public void ValidateNumbersModel()
        {
            validateNumbersModel model = new validateNumbersModel
            {
                codeCompany = 1,
                numeros = new numbers
                {
                    initial = 1,
                    final = 10
                },
                origen = 1
            };

            var type = Assert.IsType<validateNumbersModel>(model);
            Assert.NotNull(type);
        }

        [Fact]
        public void Numbers()
        {
            numbers model = new numbers
            {
                initial = 1,
                final = 10
            };

            var type = Assert.IsType<numbers>(model);
            Assert.NotNull(type);
        }

        [Fact]
        public void CuttingReport()
        {
            cuttingReport model = new cuttingReport
            {
                code = 1,
                dateVisit = DateTime.Now,
                dateRegister = DateTime.Now,
                visitNumber = "Visit123"
            };

            var type = Assert.IsType<cuttingReport>(model);
            Assert.NotNull(type);
        }

        [Fact]
        public void Cutting()
        {
            cutting model = new cutting
            {
                code = 1,
                fractionsType = "Half",
                amount = 10,
                totalArea = 100
            };

            var type = Assert.IsType<cutting>(model);
            Assert.NotNull(type);
        }

        [Fact]
        public void Safeguard()
        {
            Safeguard model = new Safeguard
            {
                code = 1,
                codSafeguard = 12345
            };

            var type = Assert.IsType<Safeguard>(model);
            Assert.NotNull(type);
        }
    }
}
