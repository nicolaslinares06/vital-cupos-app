using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Helpers.Models;
using static CUPOS_FRONT.Models.Requests;

namespace Web.Models
{
    public class ListaUsuarios
    {
        public List<GestionUsuario> Usuarios { get; set; }
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
        public string a012contrasena { get; set; }

    }

    public class CreacionUsu
    {
        public string firstName { get; set; }
        public string secondName { get; set; }
        public string firstLastName { get; set; }
        public string SecondLastName { get; set; }
        public decimal codeParametricDocumentType { get; set; }
        public decimal identification { get; set; }
        public string address { get; set; }
        public string dependence { get; set; }
        public decimal phone { get; set; }
        public string email { get; set; }
        public string celular { get; set; }
        public DateTime contractStartDate { get; set; }
        public DateTime contractFinishDate { get; set; }
        public bool acceptsTerms { get; set; }
        public bool acceptsProcessingPersonalData { get; set; }
        public string rol { get; set; }
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
        public string email { get; set; }
        public string name { get; set; }
        public decimal identification { get; set; }
        public decimal rol { get; set; }
        public List<SelectListItem> rols { get; set; }
    }

    public class VitalReq
    {
        public decimal code { get; set; }
        public string status { get; set; }
        public string permissions { get; set; }
        public string message { get; set; }
        public decimal ID { get; set; }
        public string User { get; set; }
        public string Name { get; set; }
        public decimal Document { get; set; }
        public string EMail { get; set; }
        public DateTime LastLogin { get; set; }
        public string Active { get; set; }
        public string Enabled { get; set; }
        public string Module { get; set; }
        public string Url { get; set; }
        public string Token { get; set; }
        public string UrlError { get; set; }
        public decimal rol { get; set; }
        public List<SelectListItem> rols { get; set; }
    }

    public class AssignRol
    {
        public decimal code { get; set; }
        public string status { get; set; }
        public string permissions { get; set; }
        public string message { get; set; }
        public decimal id { get; set; }
        public string user { get; set; }
        public string name { get; set; }
        public decimal document { get; set; }
        public string eMail { get; set; }
        public DateTime lastLogin { get; set; }
        public string active { get; set; }
        public string enabled { get; set; }
        public string module { get; set; }
        public string url { get; set; }
        public string token { get; set; }
        public string urlError { get; set; }
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
