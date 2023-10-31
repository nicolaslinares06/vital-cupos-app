using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Models;
using static Repository.Helpers.DocumentManager;

namespace Repository.Helpers.Models
{
    public class Company
    {
        public decimal Code { get; set; }
        public string ReasonSocial { get; set; }
        public decimal InitialBalanceBusiness { get; set; }
        public decimal FinalBalanceBusiness { get; set; }
    }

    public class Entidad
    {
        public decimal? codigoEmpresa { get; set; }
        public decimal? idtipoDocumento { get; set; }
        public string? tipoDocumento { get; set; }
        public decimal idtipoEntidad { get; set; }
        public string nombreEntidad { get; set; }
        public string nombreEmpresa { get; set; }
        public decimal nit { get; set; }
        public decimal telefono { get; set; }
        public string correo { get; set; }
        public decimal idciudad { get; set; }
        public string ciudad { get; set; }
        public decimal iddepartamento { get; set; }
        public string departamento { get; set; }
        public decimal idpais { get; set; }
        public string pais { get; set; }
        public string direccion { get; set; }
        public string? matriculaMercantil { get; set; }
        public decimal idestado { get; set; }
        public string estado { get; set; }
    }

    public class Novedad
    {
        public decimal codigo { get; set; }
        public decimal codigoEmpresa { get; set; }
        public decimal idTipoNovedad { get; set; }
        public string nombreTipoNovedad { get; set; }
        public DateTime fechaRegistroNovedad { get; set; }
        public decimal idEstado { get; set; }
        public string estado { get; set; }
        public decimal idEstadoEmpresa { get; set; }
        public string estadoEmpresa { get; set; }
        public decimal? idEstadoEmisionCITES { get; set; }
        public string? estadoEmisionCITES { get; set; }
        public string? observaciones { get; set; }
        public decimal? saldoProduccionDisponible { get; set; }
        public decimal? cuposDisponibles { get; set; }
        public decimal? inventarioDisponible { get; set; }
        public decimal? numeroCupospendientesportramitar { get; set; }
        public decimal? idDisposicionEspecimen { get; set; }
        public decimal? idEmpresaZoo { get; set; }
        public decimal? NitEmpresaZoo { get; set; }
        public string? otroCual { get; set; }
        public string? observacionesDetalle { get; set; }
        public List<Archivo>? documentos { get; set; }
    }
}
