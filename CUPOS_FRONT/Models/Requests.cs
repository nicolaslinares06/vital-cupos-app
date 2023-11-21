using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Helpers;
using static Repository.Helpers.DocumentManager;

namespace CUPOS_FRONT.Models
{
    public class Requests
    {
        public class ReqLogin
        {
            public string user { get; set; } = string.Empty;
            public string password { get; set; } = string.Empty;
        }

        public class ReqSimpleUser
        {
            public string user { get; set; } = string.Empty;
        }

        public class ReqUser
        {
            public decimal code { get; set; }
            public decimal? cityAddress { get; set; }
            public decimal? codeParametricDocumentType { get; set; }
            public decimal? codeParametricUserType { get; set; }
            public string? dependence { get; set; }
            public bool? acceptsTerms { get; set; }
            public bool? acceptsProcessingPersonalData { get; set; }
            public decimal identification { get; set; }
            public string? firstName { get; set; }
            public string? secondName { get; set; }
            public string? firstLastName { get; set; }
            public string? SecondLastName { get; set; }
            public string? login { get; set; }
            public string? address { get; set; }
            public decimal? phone { get; set; }
            public string? email { get; set; }
            public string? celular { get; set; }
            public string? password { get; set; }
            public string? digitalSignature { get; set; }
            public DateTime? contractStartDate { get; set; }
            public DateTime? contractFinishDate { get; set; }
            public bool? registrationStatus { get; set; }
            public string? rol { get; set; }
            public string? estate { get; set; }
            public List<SelectListItem>? documentType { get; set; }
            public List<SelectListItem>? dependenceType { get; set; }
        }

        public class ReqDocumentType
        {
            public decimal id { get; set; }
            public string type { get; set; } = string.Empty;
        }

        public class ReqChangePassword
        {
            public string user { get; set; } = string.Empty;
            public string password { get; set; } = string.Empty;
            public string newPassword { get; set; } = string.Empty;
            public bool? acceptsTerms { get; set; }
            public bool? acceptsProcessingPersonalData { get; set; }
        }

        public class ReqId
        {
            public decimal id { get; set; }
        }

        public class Estados
        {
            public List<ReqEstados> estateList { get; set; } = new List<ReqEstados>();
        }

        public class ReqEstados
        {
            public decimal pkT008codigo { get; set; }
            public decimal a008posicion { get; set; }
            public string a008codigoParametricaEstado { get; set; } = string.Empty;
            public string a008descripcion { get; set; } = string.Empty;
            public string a008etapa { get; set; } = string.Empty;
            public string a008estadoRegistro { get; set; } = string.Empty;
            public List<SelectListItem> estate { get; set; } = new List<SelectListItem>();
            public List<SelectListItem> stage { get; set; } = new List<SelectListItem>();
        }

        public class ReqEstado
        {
            public decimal id { get; set; }
            public decimal position { get; set; }
            public string idEstate { get; set; } = string.Empty;
            public string description { get; set; } = string.Empty;
            public string stage { get; set; } = string.Empty;
            public bool estate { get; set; }
        }

        public class ReqRoles
        {
            public decimal id { get; set; }
            public string estado { get; set; } = string.Empty;
            public string name { get; set; } = string.Empty;
            public string cargo { get; set; } = string.Empty;
            public string descripcion { get; set; } = string.Empty;
            public List<SelectListItem> estate { get; set; } = new List<SelectListItem>();
        }

        public class ReqRol
        {
            public int rolId { get; set; }
            public string name { get; set; } = string.Empty;
            public string position { get; set; } = string.Empty;
            public string description { get; set; } = string.Empty;
            public bool estate { get; set; }
        }

        public class RolModPermition
        {
            public int rolId { get; set; }
            public int moduleId { get; set; }
            public bool consult { get; set; }
            public bool create { get; set; }
            public bool update { get; set; }
            public bool delete { get; set; }
            public bool see { get; set; }
            public string name { get; set; } = string.Empty;
        }

        public class ReqAssignment
        {
            public decimal id { get; set; }
            public decimal? identification { get; set; }
            public string? firstName { get; set; }
            public string? secondName { get; set; }
            public string? firstLastname { get; set; }
            public string? secondLastname { get; set; }
            public string? rolId { get; set; }
            public decimal estate { get; set; }
        }

        public class ReqAssignmentUpdate
        {
            public decimal id { get; set; }
            public string estadoSolicitud { get; set; } = string.Empty;
        }

        public class ReqAdminTecnica
        {
            public decimal codigo { get; set; }
            public string nombre { get; set; } = string.Empty;
            public string valor { get; set; } = string.Empty;  
            public string descripcion { get; set; } = string.Empty;
            public bool estadoRegistro { get; set; }
        }
        public class ReqEntidad
        {
            public decimal? codigoEmpresa { get; set; }
            public decimal? idtipoDocumento { get; set; }
            public decimal idtipoEntidad { get; set; }
            public string nombreEmpresa { get; set; } = string.Empty;
            public decimal nit { get; set; }
            public decimal idciudad { get; set; }
            public string direccion { get; set; } = string.Empty;
            public decimal telefono { get; set; }
            public string correo { get; set; } = string.Empty;
            public string? matriculaMercantil { get; set; }
        }
        public class ReqModulos
        {
            public decimal rolId { get; set; }
            public string moduleId { get; set; } = string.Empty;
            public bool consult { get; set; }
            public bool create { get; set; }
            public bool update { get; set; }
            public bool delete { get; set; }
            public bool see { get; set; }
            public string name { get; set; } = string.Empty;
        }

        public class ModulosReq
        {
            public decimal id { get; set; }
            public string name { get; set; } = string.Empty;
        }

        public class ReqNovedad
        {
            public decimal codigo { get; set; }
            public decimal codigoEmpresa { get; set; }
            public decimal idTipoNovedad { get; set; }
            public decimal idEstadoEmpresa { get; set; }
            public decimal idEstadoEmisionCITES { get; set; }
            public DateTime fechaRegistroNovedad { get; set; }
            public string? observaciones { get; set; }
            public decimal? saldoProduccionDisponible { get; set; }
            public decimal? cuposDisponibles { get; set; }
            public decimal? inventarioDisponible { get; set; }
            public decimal? numeroCupospendientesportramitar { get; set; }
            public decimal? idDisposicionEspecimen { get; set; }
            public decimal? idEmpresaZoo { get; set; }
            public string? otroCual { get; set; }
            public string? observacionesDetalle { get; set; }
            public List<Archivo>? documentos { get; set; }
            public List<Archivo>? documentosAeliminar { get; set; } = new List<Archivo>();
        }

        public class ReqEstadoCertificado
        {
            public decimal codigo { get; set; }
            public string nombre { get; set; } = string.Empty;
        }

        public class UpdateDocument
        {
            public decimal id { get; set; }
            public string cambiosDoc { get; set; } = string.Empty;
        }
    }
}
