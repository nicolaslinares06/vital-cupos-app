using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Tsp;
using Repository.Helpers;
using System.Data;
using System.Net.Http.Headers;
using System.Xml.Linq;
using Web.Models;
using static Web.Models.ResolucionPeces;


namespace WebFront.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class ControlFishRequestController : Controller
    {

        private readonly string UrlApi;
        private readonly ILogger<ControlFishRequestController> _logger;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";

        public ControlFishRequestController(ILogger<ControlFishRequestController> logger)
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
        /// retorna vista
        /// </summary>
        /// <returns></returns>
        public IActionResult ControlFishRequest()
        {
            try
            {
                _logger.LogInformation("method called");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
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
                    return View("Views/ControlFishRequest/Partials/Ayuda.cshtml");

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }
        /// <summary>
        /// consultar datos entidad
        /// </summary>
        /// <param name="documentType"></param>
        /// <param name="DocumentNumber"></param>
        /// <returns></returns>
        public object ConsultEntityDates(decimal documentType, decimal DocumentNumber)
        {
            try
            {                
                string URI = UrlApi + "/ControlFishRequest/ConsultEntityDates?nitBussines=" + DocumentNumber + "&documentType=" + documentType;
                var respuesta = ProcesarDataApiGet<List<DatosEntidad>>(URI);
                return respuesta;
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// consultar resluciones por empresa
        /// </summary>
        /// <param name="codeBussines"></param>
        /// <returns></returns>
        public object ConsultPermitsReslution(decimal codeBussines)
        {
            try
            {   
                string URI = UrlApi + "/ControlFishRequest/ConsultPermitsReslution?codeBussines=" + codeBussines;
                var respuesta = ProcesarDataApiGet<List<ResolucionPermisos>>(URI);
                return respuesta;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// consultar una resolucion por codugo
        /// </summary>
        /// <param name="codeReslution"></param>
        /// <returns></returns>
        public object ConsultOnePermitResolution(decimal codeReslution)
        {
            try
            {               
                string URI = UrlApi + "/ControlFishRequest/ConsultOnePermitResolution?codeReslution=" + codeReslution;
                var respuesta = ProcesarDataApiGet<ResolucionPermisos>(URI);
                return respuesta;               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// guadar resolucion nueva
        /// </summary>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public object SaveResolution(ResolucionPermisos resolution)
        {
            try
            {
               
                string URI = UrlApi + "/ControlFishRequest/SaveResolution";
                var req = ObtenerObjectReq(resolution);
                var respuesta = ProcesarDataApiPost(URI, req);
                return respuesta;               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }
        /// <summary>
        /// editar resolucion
        /// </summary>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public object EditResolution(ResolucionPermisos resolution)
        {
            try
            {                
                string URI = UrlApi + "/ControlFishRequest/EditResolution";
                var req = ObtenerObjectReq(resolution);
                var respuesta = ProcesarDataApiPost(URI, req);
                return respuesta;
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// deshabilitar resolucion
        /// </summary>
        /// <param name="codeResolution"></param>
        /// <returns></returns>
        public object DeleteResolution(decimal codeResolution)
        {
            try
            {             
                string URI = UrlApi + "/ControlFishRequest/DeleteResolution";               
                var respuesta = ProcesarDataApiPost(URI, codeResolution);
                return respuesta;               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
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
            var data = ProcesarPeticion(response);
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

        private object ObtenerObjectReq(ResolucionPermisos resolution)
        {
            if (resolution is null)
                resolution = new ResolucionPermisos();

            var req = new
            {
                resolutionCode = resolution.codigoResolucion,
                companyCode = resolution.codigoEmpresa,
                resolutionNumber = resolution.numeroResolucion,
                resolutionDate = resolution.fechaResolucion,
                startDate = resolution.fechaInicio,
                endDate = resolution.fechaFin,
                attachment = new
                {
                    code = resolution.adjunto.codigo,
                    base64Attachment = resolution.adjunto.adjuntoBase64,
                    attachmentName = resolution.adjunto.nombreAdjunto,
                    attachmentType = resolution.adjunto.tipoAdjunto
                },
                resolutionObject = resolution.objetoResolucion
            };

            return req;
        }

    }
}
