using CUPOS_FRONT.Models;
using DocumentFormat.OpenXml.Math;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.Helpers;
using Repository.Helpers.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Xml.Linq;
using Web.Models;
using static Web.Models.TrayForNationalSealsExternUsers;

namespace Web.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]


    public class TrayForNationalSealsExternUsersController : Controller
    {
        private readonly string UrlApi;
        private readonly ILogger<TrayForNationalSealsExternUsersController> _logger;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";


        public TrayForNationalSealsExternUsersController(ILogger<TrayForNationalSealsExternUsersController> logger)
        {
            UrlApi = string.IsNullOrEmpty(RUTAAPI) ? new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetValue<string>("Variables:RutaApi") : RUTAAPI;
            _logger = logger;
        }

        /// <summary>
        /// htttp client
        /// </summary>
        /// <returns></returns>
        public HttpClient getHttpClient()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            return new HttpClient(clientHandler);
        }

        /// <summary>
        /// retornar vista
        /// </summary>
        /// <returns></returns>
        public IActionResult TrayForNationalSealsExternUsers(decimal codigoEmpresa, decimal nitEmpresa, decimal tipoEmpresa)
        {
            try
            {
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

                _logger.LogInformation("method called");
                ViewBag.nitEmpresa = nitEmpresa;
                ViewBag.codigoEmpresa = codigoEmpresa;
                ViewBag.tipoEmpresa = tipoEmpresa;
                ViewBag.ConsultDepartment = ConsultDepartments();
                ViewBag.tipoDato = ConsultRequestTypes();
                ViewBag.tipoDocument = ConsultDocumentsTypes();
                ViewBag.consulRepresen = ConsultBussinesAndLegalRepresentant(codigoEmpresa);
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string? token = HttpContext.Session.GetString("token") ?? "";

                if (String.IsNullOrEmpty(token))
                    return View("Views/Login/Index.cshtml");
                else
                    return View("Views/Home/FlujoNegocio.cshtml");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Ayuda()
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token != null)
                    return View("Views/TrayForNationalSealsExternUsers/Partials/Ayuda.cshtml");

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }
        public object ConsultDocumentsTypes()
        {
            try
            {  
                string URI = UrlApi + "/NonTimberFloraCertificate/ConsultDocumentsTypes";
                var respuesta = ProcesarDataApiGet<List<ElementTypes>>(URI);
                return respuesta;                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");                
                return new List<ElementTypes>();
            }
        }

        /// <summary>   
        /// consultar tipos solicitud
        /// </summary>
        /// <returns></returns>
        public object ConsultRequestTypes()
        {
            try
            {               
                string URI = UrlApi + "/TrayForNationalSealsExternUsers/ConsultRequestTypes";
                var respuesta = ProcesarDataApiGet<List<ElementTypes>>(URI);
                return respuesta;               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");                
                return new List<ElementTypes>(); 
            }
        }

        /// <summary>
        /// consultar empresas
        /// </summary>
        /// <returns></returns>
        public object ConsultBussiness()
        {
            try
            {              
                string URI = UrlApi + "/TrayForNationalSealsExternUsers/ConsultBussiness";
                var respuesta = ProcesarDataApiGet<List<ElementTypes>>(URI);
                return respuesta;               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");              
                return new List<ElementTypes>(); 
            }
        }

        /// <summary>
        /// consultar empresa y representante legal
        /// </summary>
        /// <returns></returns>
        public object ConsultBussinesAndLegalRepresentant(decimal codigoEmpresa)
        {
            try
            {    
                string URI = UrlApi + "/TrayForNationalSealsExternUsers/ConsultBussinesAndLegalRepresentant?codeBussines=" + codigoEmpresa;
                var respuesta = ProcesarDataApiGet<RepresentaeLegalEmpresa>(URI);
                return respuesta;               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");               
                return new RepresentaeLegalEmpresa(); 
            }
        }

        /// <summary>
        /// consultar ciudades
        /// </summary>
        /// <returns></returns>
        public object ConsultCities()
        {
            try
            {               
                string URI = UrlApi + "/TrayForNationalSealsExternUsers/ConsultCities";
                var respuesta = ProcesarDataApiGet<List<ElementTypesEspecies>>(URI);
                return respuesta;                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<ElementTypesEspecies>();

            }
        }

        /// <summary>
        /// gusardar solicitud
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public object RegisterRequest(Request request)
        {
            try
            {  
                string URI = UrlApi + "/TrayForNationalSealsExternUsers/RegisterRequest";    
                var req = ObtenerDataObjeto(request);
                var respuesta = ProcesarDataApiPost(URI, req);
                return respuesta;                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return false;
            }
        }


        /// <summary>
        /// consultar solicitudes radicadas
        /// </summary>
        /// <param name="codigoEmpresa"></param>
        /// <returns></returns>
        public object ConsultRegisteredRecuest(decimal codigoEmpresa)
        {
            try
            {             
                string URI = UrlApi + "/TrayForNationalSealsExternUsers/ConsultRegisteredRecuest?companyCode=" + codigoEmpresa;
                var respuesta = ProcesarDataApiGet<List<solicitudes>>(URI);
                return respuesta;               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");               
                return new List<solicitudes>();
            }
        }

        /// <summary>
        /// consultar solicitudes en requerieminto
        /// </summary>
        /// <param name="codigoEmpresa"></param>
        /// <returns></returns>
        public object ConsultRequirements(decimal codigoEmpresa)
        {
            try
            {               
                string URI = UrlApi + "/TrayForNationalSealsExternUsers/ConsultRequirements?companyCode=" + codigoEmpresa;
                var respuesta = ProcesarDataApiGet<List<solicitudes>>(URI);
                return respuesta;               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");               
                return new List<solicitudes>();
            }
        }

        /// <summary>
        /// consultar solicitudes aprobadas
        /// </summary>
        /// <param name="codigoEmpresa"></param>
        /// <returns></returns>
        public object ConsultApproved(decimal codigoEmpresa)
        {
            try
            {              
                string URI = UrlApi + "/TrayForNationalSealsExternUsers/ConsultApproved?companyCode=" + codigoEmpresa;
                var respuesta = ProcesarDataApiGet<List<solicitudes>>(URI);
                return respuesta;               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// cponsultar solicitudes desistidas
        /// </summary>
        /// <param name="codigoEmpresa"></param>
        /// <returns></returns>
        public object ConsultDesisted(decimal codigoEmpresa)
        {
            try
            {               
                string URI = UrlApi + "/TrayForNationalSealsExternUsers/ConsultDesisted?companyCode=" + codigoEmpresa;
                var respuesta = ProcesarDataApiGet<List<solicitudes>>(URI);
                return respuesta;                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<solicitudes>();
            }
        }

        /// <summary>
        /// consultar una solicitud
        /// </summary>
        /// <param name="codigoSolicitud"></param>
        /// <returns></returns>
        public object ConsultOnePendientRegister(decimal codigoSolicitud)
        {
            try
            {               
                string URI = UrlApi + "/TrayForNationalSealsExternUsers/ConsultOnePendientRegister?codeRequest=" + codigoSolicitud;
                var respuesta = ProcesarDataApiGet<Request> (URI);
                return respuesta;               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");               
                return new Request(); 
            }
        }

        /// <summary>
        /// editar solicitud
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public object EditRequest(Request request)
        {
            try
            { 
                string URI = UrlApi + "/TrayForNationalSealsExternUsers/EditRequest";
                var req = ObtenerDataObjeto(request);
                var respuesta = ProcesarDataApiPost(URI, req);
                return respuesta;                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return false;
            }
        }

        /// <summary>
        /// Obtiene los cupos
        /// </summary>
        /// <returns></returns>
        public object GetQuotas(string documentNumber, string especie)
        {
            try
            {   
                string uri = String.Format("{0}/TrayForNationalSealsExternUsers/GetQuotas?documentNumber={1}&species={2}", UrlApi, documentNumber, especie);
                var respuesta = ProcesarDataApiGet<List<Quota>>(uri);
                return respuesta;                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// Obtiene los cupos
        /// </summary>
        /// <returns></returns>
        public object GetInventory(string documentNumber, string especie)
        {
            try
            { 
                string uri = String.Format("{0}/TrayForNationalSealsExternUsers/GetInventory?documentNumber={1}&species={2}", UrlApi, documentNumber, especie);
                var respuesta = ProcesarDataApiGet<List<Inventory>>(uri);
                return respuesta;                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// consultar departamentos
        /// </summary>
        /// <returns></returns>
        public object ConsultDepartments()
        {
            try
            {   
                string URI = UrlApi + "/TrayForNationalSealsExternUsers/ConsultDepartments";
                var respuesta = ProcesarDataApiGet<List<ElementTypesEspecies>>(URI);
                return respuesta;                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");               
                return new List<ElementTypesEspecies>(); 
            }
        }

        /// <summary>
        /// Consultar ciudades por id de departamento
        /// 
        /// /// </summary>
        /// <returns></returns>
        public object ConsultCitiesByDepartmentId(decimal departamentoId)
        {
            try
            {  
                string URI = UrlApi + "/TrayForNationalSealsExternUsers/ConsultCitiesByIdDepartment?idDepartment=" + departamentoId;
                var respuesta = ProcesarDataApiGet<List<ElementTypesEspecies>>(URI);
                return respuesta;                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");               
                return new List<ElementTypesEspecies>(); 
            }
        }

        /// <summary>
        /// Consultar numeraciones solicitudes no disponibles
        /// <returns></returns>
        public object getNumbersRequest(ConsultUnableNumbersModel data)
        {
            try
            {   
                var req = new
                {
                    code = data.code,
                    companyNit = data.nitEmpresa,
                    quota = data.cupo,
                    inventory = data.inventario
                };

                string URI = UrlApi + "/TrayForNationalSealsExternUsers/getNumbersRequest";
                var respuesta = ObtenerDataApiPost<List<numbers>>(URI, req);
                return respuesta;
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<numbers>(); 
            }
        }

        /// <summary>
        /// Valida si estan disponibvles las numeraciones
        /// </summary>
        /// <returns></returns>
        public object ValidateNumbers(validateNumbersModel numbers)
        {
            try
            { 
                var req = new
                {
                    codeCompany = numbers.codeCompany,
                    numbers = numbers.numeros != null ? new
                    {
                        initial = numbers.numeros.initial,
                        final = numbers.numeros.final
                    } : null,
                    origin = numbers.origen
                };
                string uri = String.Format("{0}/TrayForNationalSealsExternUsers/ValidateNumbers", UrlApi);
                var respuesta = ObtenerDataApiPost<object>(uri, req);
                return respuesta;                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// obtener actas de corte
        /// </summary>
        /// <param name="documentNumber"></param>
        /// <returns></returns>
        public object getActaData(string documentNumber)
        {
            string URI = UrlApi + "/TrayForNationalSealsExternUsers/getActaData?documentNumber=" + documentNumber;
            var respuesta = ProcesarDataApiGet<List<cuttingReport>>(URI);
            return respuesta;           
        }

        /// <summary>
        /// obtener fracciones
        /// </summary>
        /// <param name="cuttingCode"></param>
        /// <returns></returns>
        public object getFractions(int cuttingCode)
        {           
            string URI = UrlApi + "/TrayForNationalSealsExternUsers/getFractions?cuttingCode=" + cuttingCode;
            var respuesta = ProcesarDataApiGet<List<cutting>>(URI);
            return respuesta;           
        }

        /// <summary>
        /// obtener salvoconducto
        /// </summary>
        /// <param name="cuttingCode"></param>
        /// <returns></returns>
        public object getSafeguard(int reportCode)
        {  
            string URI = UrlApi + "/TrayForNationalSealsExternUsers/getSafeguard?reportCode=" + reportCode;
            var respuesta = ProcesarDataApiGet<List<Safeguard>>(URI);
            return respuesta;            
        }

        private object ProcesarDataApiGet<T>(string URI) where T : new()
        {

            var httpClient = ConfigurarHttpClient();
            var response = httpClient.GetAsync(URI).Result;
            var data = ProcessHttpResponse<T>(response);
            return data ?? new object { };
        }

        private object ProcesarDataApiPost<T>(string URI, T req)
        {

            var httpClient = ConfigurarHttpClient();
            var response = httpClient.PostAsJsonAsync(URI, req).Result;
            var data = ProcesarPeticion(response);
            return data ?? new object { };
        }

        private object ObtenerDataApiPost<T>(string URI, object req) where T : new()
        {

            var httpClient = ConfigurarHttpClient();
            var response = httpClient.PostAsJsonAsync(URI, req).Result;
            var data = ProcessHttpResponse<T>(response);
            return data ?? new object { };
        }

        private HttpClient ConfigurarHttpClient()
        {
            string? token = HttpContext.Session.GetString("token");
            var httpClient = getHttpClient();

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return httpClient;
        }


        private object ProcessHttpResponse<T>(HttpResponseMessage response) where T : new()
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return new { volverInicio = true };
            }

            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;
                Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                if (respuesta.Response != null)
                {
                    HttpContext.Session.SetString("token", respuesta.Token);
                    if(respuesta.Response is bool)
                        return respuesta.Response;
                    return JsonConvert.DeserializeObject<T>(respuesta.Response.ToString() ?? "") ?? new T();
                }
            }

            return new T();
        }

        private object ProcesarPeticion(HttpResponseMessage response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return new
                {
                    volverInicio = true
                };
            }

            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;
                Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                if (respuesta.Response != null)
                {
                    HttpContext.Session.SetString("token", respuesta.Token);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private object ObtenerDataObjeto(Request request)
        {
            var req = new
            {
                companyCode = request.codigoEmpresa,
                requestCode = request.codigoSolicitud,
                date = request.fecha,
                representativeCity = request.ciudadRepresentante,
                deliveryAddress = request.direccionEntrega,
                quantity = request.cantidad,
                specimens = request.especimenes,
                initialSkinCode = request.codigoInicialPieles,
                finalSkinCode = request.codigoFinalPieles,
                minorLength = request.longitudMenor,
                majorLength = request.longitudMayor,
                generateSealsForConsignation = false,
                representativeDate = request.fechaRepresentante,
                observations = request.observaciones,
                response = request.respuesta,
                requestStatus = request.estadoSolicitud,
                registrationDate = request.fechaRadicado,
                requestType = request.tipoSolicitud,
                statusChangeDate = request.fechaCambioEstado,
                withdrawalObservations = request.observacionesDesistimiento,
                numerations = request.numeraciones?.Select(n => new
                {
                    code = n.codigo,
                    initial = n.inicial,
                    final = n.final,
                    origen = n.origen
                }).ToList(),
                invoiceAttachment = new
                {
                    code = request.facturaAdjunto.codigo,
                    base64Attachment = request.facturaAdjunto.adjuntoBase64,
                    attachmentName = request.facturaAdjunto.nombreAdjunto,
                    attachmentType = request.facturaAdjunto.tipoAdjunto
                },
                letterAttachment = request.adjuntoCarta != null ? new
                {
                    code = request.adjuntoCarta.codigo,
                    base64Attachment = request.adjuntoCarta.adjuntoBase64,
                    attachmentName = request.adjuntoCarta.nombreAdjunto,
                    attachmentType = request.adjuntoCarta.tipoAdjunto
                } : null,
                attachedAnnexes = request.AnexosAdjuntos?.Select(a => new
                {
                    code = a.codigo,
                    base64Attachment = a.adjuntoBase64,
                    attachmentName = a.nombreAdjunto,
                    attachmentType = a.tipoAdjunto
                }).ToList(),
                attachedAnnexesToDelete = request.anexosAdjuntosEliminar?.Select(a => new
                {
                    code = a.codigo,
                    base64Attachment = a.adjuntoBase64,
                    attachmentName = a.nombreAdjunto,
                    attachmentType = a.tipoAdjunto
                }).ToList(),
                responseAttachments = request.adjuntosRespuesta?.Select(a => new
                {
                    code = a.codigo,
                    base64Attachment = a.adjuntoBase64,
                    attachmentName = a.nombreAdjunto,
                    attachmentType = a.tipoAdjunto
                }).ToList(),
                responseAttachmentsToDelete = request.adjuntosRespuestaEliminar?.Select(a => new
                {
                    code = a.codigo,
                    base64Attachment = a.adjuntoBase64,
                    attachmentName = a.nombreAdjunto,
                    attachmentType = a.tipoAdjunto
                }).ToList(),
                representativeDepartment = request.departamentoRepresentante,
                safeGuardNumbers = request.safeGuardNumbers,
                cuttingSave = request.cuttingSave
            };

            return req;

        }

    }
}
