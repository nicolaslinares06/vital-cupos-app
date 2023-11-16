using DocumentFormat.OpenXml.Office2016.Drawing.Command;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;
using Repository.Helpers;
using Repository.Helpers.Models;
using System.Collections.Generic;
using System.Data;
using System.Net.Http.Headers;
using Web.Models;

namespace WebFront.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class RegistrarResolucionController : Controller
    {

        private readonly string UrlApi;
        private readonly ILogger<RegistrarResolucionController> _logger;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";

        public RegistrarResolucionController(ILogger<RegistrarResolucionController> logger)
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
        /// retorna la vista 
        /// </summary>
        /// <param name="documentType"></param>
        /// <param name="nitBussines"></param>
        /// <returns></returns>
        public IActionResult RegistrarResolucion(decimal documentType, string nitBussines)
        {
            try
            {
                var entidades = ConsultEntityDates(documentType, nitBussines, null);
                var entidadViewModel = new Entidad();

                if (entidades.Any())
                {
                    entidadViewModel = entidades[0];
                }

                var model = entidadViewModel;
                _logger.LogInformation("method called");
                ViewBag.nitBussines = nitBussines;
                ViewBag.documentType = documentType;
                return View(model);
            }
            catch (Exception ex)
            {
                return ManejarExcepcion(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Ayuda(decimal documentType, string nitBussines)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token != null)
                {
                    ViewBag.nitBussines = nitBussines;
                    ViewBag.documentType = documentType;
                    return View("Views/RegistrarResolucion/Partials/Ayuda.cshtml");
                }

                return View();
            }
            catch (Exception ex)
            {
              return ManejarExcepcion(ex);
            }
        }
        /// consultar datos de entidad
        /// </summary>
        /// <param name="documentType"></param>
        /// <param name="nitBussines"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public List<Entidad> ConsultEntityDates(decimal documentType, string nitBussines, decimal? entityType)
        {
            try
            {
                string URI = UrlApi + "/ResolutionRegister/ConsultEntityDates?documentType=" + documentType + "&nitBussines=" + nitBussines + (entityType == null ? "" : "&entityType=" + entityType);
                var resultado = ProcesarDataApiGet<List<Entidad>>(URI);   
                List<Entidad> listado =  (List<Entidad>)resultado;
                return listado ?? new List<Entidad>();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<Entidad>();
            }
        }

        /// <summary>
        /// consultar todas las resoluciones
        /// </summary>
        /// <param name="nitBussines"></param>
        /// <returns></returns>
        public object ConsultQuotas(string nitBussines)
        {
            try
            {   
                string URI = UrlApi + "/ResolutionRegister/ConsultQuotas?nitBussines=" + nitBussines;
                var resultado = ProcesarDataApiGet<List<Cupos>>(URI);
                return resultado ?? new List<Cupos>();               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<Cupos>();
            }
        }

        /// <summary>
        /// consultar cupos por numero de resolucion
        /// </summary>
        /// <param name="ResolutionNumbre"></param>
        /// <returns></returns>
        public object SearchQuotas(decimal ResolutionNumbre)
        {
            try
            {             
                string URI = UrlApi + "/ResolutionRegister/SearchQuotas?resolutionNumber=" + ResolutionNumbre;
                var resultado = ProcesarDataApiGet<List<Cupos>>(URI);
                return resultado ?? new List<Cupos>();
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<Cupos>();
            }
        }

        /// <summary>
        /// consultar invenatrio
        /// </summary>
        /// <returns></returns>
        public object ConsultInventory()
        {
            try
            {            
                string URI = UrlApi + "/ResolutionRegister/ConsultInventory";
                var resultado = ProcesarDataApiGet<List<InventarioCupos>>(URI);
                return resultado ?? new List<InventarioCupos>();               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<InventarioCupos>();
            }
        }

        /// <summary>
        /// consultar tipos de marcaje
        /// </summary>
        /// <returns></returns>
        public object ConsultMarkingType()
        {
            try
            {             
                string URI = UrlApi + "/ResolutionRegister/ConsultMarkingType";
                var resultado = ProcesarDataApiGet<List<ElementTypes>>(URI);
                return resultado ?? new List<ElementTypes>();                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<ElementTypes>(); 
            }
        }

        /// <summary>
        /// consultar tipos de marcaje
        /// </summary>
        /// <returns></returns>
        public object ConsultEntityTypes()
        {
            try
            {            
                string URI = UrlApi + "/ResolutionRegister/ConsultEntityTypes";
                var resultado = ProcesarDataApiGet<List<ElementTypes>>(URI);
                return resultado ?? new List<ElementTypes>();
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<ElementTypes>();
            }
        }

        /// <summary>
        /// consultar pagos de repoblacion
        /// </summary>
        /// <returns></returns>
        public object ConsultRepoblationPay()
        {
            try
            {              
                string URI = UrlApi + "/ResolutionRegister/ConsultRepoblationPay";
                var resultado = ProcesarDataApiGet<List<ElementTypes>>(URI);
                return resultado ?? new List<ElementTypes>();
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<ElementTypes>();
            }
        }

        /// <summary>
        /// consultar tipos de especimenes
        /// </summary>
        /// <returns></returns>
        public object ConsultEspecimensTypes()
        {
            try
            {               
                string URI = UrlApi + "/ResolutionRegister/ConsultEspecimensTypes";
                var resultado = ProcesarDataApiGet<List<ElementTypesEspecies>>(URI);
                return resultado ?? new List<ElementTypesEspecies>();                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<ElementTypesEspecies>();
            }
        }

        /// <summary>
        /// consultar una resolucion por codigo
        /// </summary>
        /// <param name="quotaCode"></param>
        /// <returns></returns>
        public object ConsultOneQuota(decimal quotaCode)
        {
            try
            {             
                string URI = UrlApi + "/ResolutionRegister/ConsultOneQuota?quotaCode=" + quotaCode;
                var resultado = ProcesarDataApiGet<resolutionQuota>(URI);
                return resultado ?? new resolutionQuota();                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new resolutionQuota();
            }
        }

        /// <summary>
        /// editar resolucion
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        public object EditaEliminarResolucionEspecieExportar(saveResolutionQuotas datos)
        {
            try
            { 
                string URI = UrlApi + "/ResolutionRegister/EditDeleteResolutionQuota";               
                var req = ObtenerDatosObject(datos);
                var resultado = ProcesarDataApiPost(URI, req);
                return resultado ?? false;

               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return false;
            }
        }

        /// <summary>
        /// gusrdar resolucion nueva
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        public object GuardarResolucionEspecieExportar(saveResolutionQuotas datos)
        {
            try
            {
                
                string URI = UrlApi + "/ResolutionRegister/saveResolutionQuota";    
                var req = ObtenerDatosObject(datos);
                var resultado = ProcesarDataApiPost(URI, req);
                return resultado ?? false;
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return false;
            }
        }

        /// <summary>
        /// deshabilitar resolucion
        /// </summary>
        /// <param name="quotaCode"></param>
        /// <returns></returns>
        public object DisableResolution(decimal quotaCode)
        {
            try
            {               
                string URI = UrlApi + "/ResolutionRegister/DisableResolution?quotaCode=" + quotaCode;
                var resultado = ProcesarDataApiGet<bool>(URI);
                return resultado ?? false;
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return false;
            }
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
                    return JsonConvert.DeserializeObject<T>(respuesta.Response.ToString() ?? "") ?? new T();
                }
            }

            return new T();
        }


        private object ProcessHttpResponsePost(HttpResponseMessage response) 
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
            var data = ProcessHttpResponsePost(response);
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

        private IActionResult ManejarExcepcion(Exception ex)
        {
            _logger.LogError(ex, "An error occurred in the method.");
            string token = HttpContext.Session.GetString("token") ?? "";
            if (!String.IsNullOrEmpty(token))
                return RedirectToAction("FlujoNegocio", "Home");
            else
                return RedirectToAction("Index", "Login");
        }

        private object ObtenerDatosObject(saveResolutionQuotas datos)
        {
            var req = new
            {
                dataToSave = new
                {
                    quotaCode = datos.datoGuardar.codigoCupo,
                    issuingAuthority = datos.datoGuardar.autoridadEmiteResolucion,
                    zoocriaderoCode = datos.datoGuardar.codigoZoocriadero,
                    resolutionNumber = datos.datoGuardar.numeroResolucion,
                    resolutionDate = datos.datoGuardar.fechaResolucion,
                    resolutionRegistrationDate = datos.datoGuardar.fechaRegistroResolucion,
                    observations = datos.datoGuardar.observaciones,
                    companyNit = datos.datoGuardar.nitEmpresa
                },
                newExportSpeciesData = datos.datosEspecieExportarNuevo?.Select(especie => new
                {
                    PkT005code = especie.PkT005codigo,
                    quotaCode = especie.codigoCupo,
                    year = especie.anio,
                    quotas = especie.cupos,
                    id = especie.id,
                    markingTypeParametricCode = especie.codigoParametricaTipoMarcaje,
                    speciesCode = especie.codigoEspecie,
                    repopulationQuotaPaymentParametricCode = especie.codigoParametricaPagoCuotaDerepoblacion,
                    filingDate = especie.fechaRadicado,
                    specimenType = especie.tipoEspecimen,
                    productionYear = especie.añoProduccion,
                    batchCode = especie.marcaLote,
                    markingConditions = especie.condicionesMarcaje,
                    parentalPopulationMale = especie.poblacionParentalMacho,
                    parentalPopulationFemale = especie.poblacionParentalHembra,
                    totalParentalPopulation = especie.poblacionParentalTotal,
                    populationFromIncubator = especie.poblacionSalioDeIncubadora,
                    populationAvailableForQuotas = especie.poblacionDisponibleParaCupos,
                    individualsForRepopulation = especie.individuosDestinadosARepoblacion,
                    grantedUtilizationQuotas = especie.cupoAprovechamientoOtorgados,
                    replacementRate = especie.tasaReposicion,
                    mortalityNumber = especie.numeroMortalidad,
                    repopulationQuotaForUtilization = especie.cuotaRepoblacionParaAprovechamiento,
                    grantedPaidUtilizationQuotas = especie.cupoAprovechamientoOtorgadosPagados,
                    observations = especie.observaciones,
                    repopulationQuota = especie.cuotaRepoblacion,
                    initialRepopulationQuotaInternalNumber = especie.numeroInternoInicialCuotaRepoblacion,
                    finalRepopulationQuotaInternalNumber = especie.numeroInternoFinalCuotaRepoblacion,
                    initialInternalNumber = especie.numeroInternoInicial,
                    finalInternalNumber = especie.numeroInternoFinal,
                    totalQuotas = especie.totalCupos,
                    availableQuotas = especie.cuposDisponibles,
                    repopulationQuotaPaid = especie.sePagoCuotaRepoblacion,
                    speciesRegisteredForCommercialization = especie.seRegistroEspecieComercializar,
                    supportingDocuments = especie.documentosSoporte?.Select(doc => new
                    {
                        code = doc.codigo,
                        base64Attachment = doc.adjuntoBase64,
                        attachmentName = doc.nombreAdjunto,
                        attachmentType = doc.tipoAdjunto
                    }).ToList(),
                    newSupportingDocuments = especie.documentosSoporteNuevos
                        ?.Select(doc => new
                        {
                            code = doc.codigo,
                            base64Attachment = doc.adjuntoBase64,
                            attachmentName = doc.nombreAdjunto,
                            attachmentType = doc.tipoAdjunto
                        }).ToList(),
                    deletedSupportingDocuments = especie.documentosSoporteEliminar?.Select(doc => new
                    {
                        code = doc.codigo,
                        base64Attachment = doc.adjuntoBase64,
                        attachmentName = doc.nombreAdjunto,
                        attachmentType = doc.tipoAdjunto
                    }).ToList()
                }).ToList()
            };

            return req;
        }


    }
}
