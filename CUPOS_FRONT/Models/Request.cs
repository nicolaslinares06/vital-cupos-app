using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Helpers.Models;
using static CUPOS_FRONT.Models.Requests;

namespace Web.Models
{
    public class LoginRequest : ReqLogin
    {
        public string user { get; set; }
        public string password { get; set; }
        public string newPassword { get; set; }
        public bool acceptsTerms { get; set; }
        public bool acceptsProcessingPersonalData { get; set; }
        public string captcha1 { get; set; }
        public string captcha2 { get; set; }
    }

    public class CambioPasswordRequest : ReqChangePassword
    {
        public string user { get; set; }
        public string password { get; set; }
        public string newPassword { get; set; }
        public bool? acceptsTerms { get; set; }
        public bool? acceptsProcessingPersonalData { get; set; }
    }

    public class UsuarioSimpleRequest : ReqSimpleUser
    {
        public string User { get; set; }
    }

    public class CreateRolRequest
    {
        public int rolId { get; set; }
        public string name { get; set; }
        public string position { get; set; }
        public string description { get; set; }
        public bool estate { get; set; }
    }

    public class AdminTecnicaReq
    {
        public decimal codigo { get; set; }
        public string nombre { get; set; }
        public string valor { get; set; }
        public string descripcion { get; set; }
        public bool estadoRegistro { get; set; }
    }

    public class ReqId
    {
        public decimal id { get; set; }
    }

    public class Technical
    {
        public List<ReqValorTecnica> technicalList { get; set; }
    }

    public class ReqValorTecnica
    {
        public decimal pkT007Codigo { get; set; }
        public string a007nombre { get; set; }
        public string a007valor { get; set; }
        public string a007descripcion { get; set; }
        public string a007estadoRegistro { get; set; }
        public List<SelectListItem> estate { get; set; }
    }

    public class AssignmentUpdateReq
    {
        public decimal id { get; set; }
        public string estadoSolicitud { get; set; }
    }

    public class ReqGesAsigRoles
    {
        public string? nombre { get; set; }
        public decimal a012CodigoParametricaTipoUsuario { get; set; }
        public string? a012segundoNombre { get; set; }
        public string? a012segundoApellido { get; set; }
        public decimal? a012identificacion { get; set; }
        public string? a012correoElectronico { get; set; }
        public decimal codigoUsuario { get; set; }
        public decimal codigoRol { get; set; }
        public string nombreRol { get; set; }
        public decimal pkT0015codigo { get; set; }
        public string? a015estadoSolicitud { get; set; }
    }

    public class ReqAdminEstados
    {
        public string etapa { get; set; }
        public string descripcion { get; set; }
        public decimal estado { get; set; }
    }

    public class ValorEstado
    {
        public string? etapa { get; set; }
    }

    public class AccessSUNL
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
