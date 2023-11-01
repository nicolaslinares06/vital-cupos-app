using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
                _logger.LogInformation("method called");
                List<DatosEntidad>? datos = new List<DatosEntidad>();
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/ControlFishRequest/ConsultEntityDates?nitBussines=" + DocumentNumber + "&documentType=" + documentType;
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
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                    if (respuesta.Response != null)
                    {
                        datos = JsonConvert.DeserializeObject<List<DatosEntidad>>(respuesta.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }
                return datos ?? new object { };
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
                _logger.LogInformation("method called");
                List<ResolucionPermisos>? datos = new List<ResolucionPermisos>();
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/ControlFishRequest/ConsultPermitsReslution?codeBussines=" + codeBussines;
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
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                    if (respuesta.Response != null)
                    {
                        datos = JsonConvert.DeserializeObject<List<ResolucionPermisos>>(respuesta.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }
                return datos ?? new object { };
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
                _logger.LogInformation("method called");
                ResolucionPermisos? datos = new ResolucionPermisos();
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/ControlFishRequest/ConsultOnePermitResolution?codeReslution=" + codeReslution;
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
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                    if (respuesta.Response != null)
                    {
                        datos = JsonConvert.DeserializeObject<ResolucionPermisos>(respuesta.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }
                return datos ?? new object { };
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

                if(resolution is null)
                    resolution = new ResolucionPermisos();
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/ControlFishRequest/SaveResolution";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                    if (respuesta != null)
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
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/ControlFishRequest/EditResolution";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/ControlFishRequest/DeleteResolution";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = httpClient.PostAsJsonAsync(URI, codeResolution).Result;
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }
    }
}
