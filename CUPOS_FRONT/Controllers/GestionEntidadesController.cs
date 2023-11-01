using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.Helpers;
using Repository.Helpers.Models;
using System.Net.Http.Headers;
using Web.Models;
using static CUPOS_FRONT.Models.Requests;

namespace Web.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class GestionEntidadesController : Controller
    {
        private readonly ILogger<GestionEntidadesController> _logger;
        private readonly string UrlApi;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public GestionEntidadesController(ILogger<GestionEntidadesController> logger)
        {
            UrlApi = string.IsNullOrEmpty(RUTAAPI) ? new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetValue<string>("Variables:RutaApi") : RUTAAPI;
            _logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public HttpClient getHttpClient()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            return new HttpClient(clientHandler);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            try
            {
                _logger.LogInformation("method called");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (!String.IsNullOrEmpty(token))
                    return RedirectToAction("FlujoNegocio", "Home");
                else
                    return RedirectToAction("Index", "Login");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult HistorialCupos(int nit)
        {
            try
            {
                ViewBag.nitHistorial = nit;
                _logger.LogInformation("method called");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (!String.IsNullOrEmpty(token))
                    return RedirectToAction("FlujoNegocio", "Home");
                else
                    return RedirectToAction("Index", "Login");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult returnAdministracion()
        {
            try
            {
                _logger.LogInformation("method called");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (!String.IsNullOrEmpty(token))
                    return RedirectToAction("FlujoNegocio", "Home");
                else
                    return RedirectToAction("Index", "Login");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_emp"></param>
        /// <returns></returns>
        [HttpPost]
        public object Actualizar(ReqEntidad _emp)
        {
            try
            {
                _logger.LogInformation("method called");
                string r = "";
                string token = HttpContext.Session.GetString("token") ?? "";

                if (String.IsNullOrEmpty(token))
                {
                    HttpContext.Session.Remove("token");
                    ViewBag.AlertaPass = "false";
                    ViewBag.RespuestaCambioPass = "Sesion caducada";
                }
                else
                {
                    string URI = UrlApi + "/Company/Update";
                    var httpClient = getHttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var req = new
                    {
                        CompanyCode = _emp.codigoEmpresa,
                        DocumentTypeId = _emp.idtipoDocumento,
                        EntityTypeId = _emp.idtipoEntidad,
                        CompanyName = _emp.nombreEmpresa,
                        NIT = _emp.nit,
                        CityId = _emp.idciudad,
                        Address = _emp.direccion,
                        Phone = _emp.telefono,
                        Email = _emp.correo,
                        BusinessRegistration = _emp.matriculaMercantil
                    };

                    var response = httpClient.PutAsJsonAsync(URI, req).Result;
                   
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return new
                        {
                            volverInicio = true
                        };
                    }

                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses resp = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                    r = resp.Message;
                    if (!resp.Error)
                    {
                        HttpContext.Session.SetString("token", resp.Token);
                    }
                }
                return r;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return "";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigoEmpresa"></param>
        /// <param name="idNovedad"></param>
        /// <returns></returns>
        public object ConsultNovedades(decimal codigoEmpresa, decimal? idNovedad)
        {
            try
            {
                _logger.LogInformation("method called");
                List<Novedad> datos = new List<Novedad>();
                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = UrlApi + "/Company/ConsultNovedades?companyCode=" + codigoEmpresa + (idNovedad != null ? "&noveltyId=" + idNovedad : "");
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;
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
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                    if (respuesta.Response != null)
                    {
                        datos = JsonConvert.DeserializeObject<List<Novedad>>(respuesta.Response.ToString() ?? "") ?? new List<Novedad>();
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }

                return datos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<Novedad>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        public object GuardarNovedad(ReqNovedad datos)
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = UrlApi + "/Company/Save";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var req = new
                {
                    code = datos.codigo,
                    companyCode = datos.codigoEmpresa,
                    typeOfNoveltyId = datos.idTipoNovedad,
                    companyStatusId = datos.idEstadoEmpresa,
                    CITESPermitIssuanceStatusId = datos.idEstadoEmisionCITES,
                    noveltyRegistrationDate = datos.fechaRegistroNovedad,
                    observations = datos.observaciones,
                    availableProductionBalance = datos.saldoProduccionDisponible,
                    availableQuotas = datos.cuposDisponibles,
                    availableInventory = datos.inventarioDisponible,
                    pendingQuotasToProcess = datos.numeroCupospendientesportramitar,
                    specimenDispositionId = datos.idDisposicionEspecimen,
                    zooCompanyId = datos.idEmpresaZoo,
                    otherDescription = datos.otroCual,
                    detailedObservations = datos.observacionesDetalle,
                    documents = datos.documentos,
                    documentsToDelete = datos.documentosAeliminar
                };

                var response = httpClient.PostAsJsonAsync(URI, req).Result;

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
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                    if (respuesta.Response != null)
                    {
                        HttpContext.Session.SetString("token", respuesta.Token);
                        return true;
                    }

                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public object ConsultCupos(decimal idEmpresa)
        {
            try
            {
                _logger.LogInformation("method called");
                cupoTotales datos = new cupoTotales();
                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = UrlApi + "/Company/ConsultQuotas?companyId=" + idEmpresa;
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;
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
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                    if (respuesta.Response != null)
                    {
                        datos = JsonConvert.DeserializeObject<cupoTotales>(respuesta.Response.ToString() ?? "") ?? new cupoTotales();
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }

                return datos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new cupoTotales();
            }
        }
    }
}
