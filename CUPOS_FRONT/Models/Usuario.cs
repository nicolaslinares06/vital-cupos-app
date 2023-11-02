using iText.Commons.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Helpers.Models;
using static CUPOS_FRONT.Models.Requests;

namespace Web.Models
{
    public class ListaUsuarios
    {
        public List<GestionUsuario> Usuarios { get; set; } = new List<GestionUsuario>();
    }
    public class GestionUsuario
    {
        public decimal pkT012codigo { get; set; }
        public string? roles { get; set; }
        public decimal a012identificacion { get; set; }
        public string A012direccion { get; set; } = null!;
        public decimal a012telefono { get; set; }
        public string A012correoElectronico { get; set; } = null!;
        public string? A012celular { get; set; }
        public string a008valor { get; set; } = null!;
        public DateTime A012fechaCreacion { get; set; }
        public DateTime? A012fechaModificacion { get; set; }
        public DateTime? a012fechaExpiracontraseña { get; set; }
        public DateTime? A012fechaFinContrato { get; set; }
        public DateTime? A012fechaInicioContrato { get; set; }
        public string A012primerNombre { get; set; } = null!;
        public string A012segundoNombre { get; set; } = null!;
        public string A012segundoApellido { get; set; } = null!;
        public string A012primerApellido { get; set; } = null!;
        public string a012login { get; set; } = null!;
        public string a012contrasena { get; set; } = string.Empty;

    }

    public class CreacionUsu
    {
        public string firstName { get; set; } = string.Empty;
        public string secondName { get; set; } = string.Empty;
        public string firstLastName { get; set; } = string.Empty;
        public string SecondLastName { get; set; } = string.Empty;
        public decimal codeParametricDocumentType { get; set; }
        public decimal identification { get; set; }
        public string address { get; set; } = string.Empty;
        public string dependence { get; set; } = string.Empty;
        public decimal phone { get; set; }  
        public string email { get; set; } = string.Empty;
        public string celular { get; set; } = string.Empty;
        public DateTime contractStartDate { get; set; }
        public DateTime contractFinishDate { get; set; }
        public bool acceptsTerms { get; set; }
        public bool acceptsProcessingPersonalData { get; set; }
        public string rol { get; set; } = string.Empty;
    }

    public class actualizarRequest : ReqUser
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
    }

    public class ConsultDocument
    {
        public decimal id { get; set; }
        public string? name { get; set; }
        public DateTime date { get; set; }
        public decimal estate { get; set; }
        public string? url { get; set; }
    }

    public class ReqDocs
    {
        public soportsDocuments? archivo { get; set; }
    }

    public class ReqAceptarCondiciones
    {
        public bool A012aceptaTerminos { get; set; } = false;
        public bool A012aceptaTratamientoDatosPersonales { get; set; } = false;
    }

    public class ReqAssignRols
    {
        public string email { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public decimal identification { get; set; }
        public decimal rol { get; set; }
        public List<SelectListItem> rols { get; set; } = new List<SelectListItem>();
    }

    public class VitalReq
    {
        public decimal code { get; set; }
        public string status { get; set; } = string.Empty;
        public string permissions { get; set; } = string.Empty;
        public string message { get; set; } = string.Empty;
        public decimal ID { get; set; }
        public string User { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Document { get; set; }
        public string EMail { get; set; } = string.Empty;
        public DateTime LastLogin { get; set; }
        public string Active { get; set; } = string.Empty;
        public string Enabled { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string UrlError { get; set; } = string.Empty;
        public decimal rol { get; set; }
        public List<SelectListItem> rols { get; set; } = new List<SelectListItem>();
    }

    public class AssignRol
    {
        public decimal code { get; set; }
        public string status { get; set; } = string.Empty;
        public string permissions { get; set; } = string.Empty;
        public string message { get; set; } = string.Empty;
        public decimal id { get; set; }
        public string user { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public decimal document { get; set; }
        public string eMail { get; set; } = string.Empty;
        public DateTime lastLogin { get; set; }
        public string active { get; set; } = string.Empty;
        public string enabled { get; set; } = string.Empty;
        public string module { get; set; } = string.Empty;
        public string url { get; set; } = string.Empty;
        public string token { get; set; } = string.Empty;
        public string urlError { get; set; } = string.Empty;
        public decimal rol { get; set; }
    }

    public class ReqUsers
    {
        public decimal id { get; set; }
        public string? primerNombre { get; set; }
        public string segundoNombre { get; set; } = null!;
        public string segundoApellido { get; set; } = null!;
        public string primerApellido { get; set; } = null!;
        public string? tipoDocumento { get; set; }
        public decimal identificacion { get; set; }
        public string? direccion { get; set; }
        public string? dependencia { get; set; }
        public decimal telefono { get; set; }
        public string? celular { get; set; }
        public DateTime? inicioContrato { get; set; }
        public DateTime? finContrato { get; set; }
        public string? email { get; set; }
        public string? rol { get; set; }
        public string? estado { get; set; }
        public string? login { get; set; }
        public string? estate { get; set; }
    }
}
