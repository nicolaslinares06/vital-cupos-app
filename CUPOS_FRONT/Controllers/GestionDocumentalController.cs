using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.Helpers;
using System.Net.Http.Headers;
using Web.Models;
using static CUPOS_FRONT.Models.Requests;

namespace Web.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class GestionDocumentalController : Controller
    {
        private readonly ILogger<GestionDocumentalController> _logger;
        private readonly string UrlApi;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public GestionDocumentalController(ILogger<GestionDocumentalController> logger)
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
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult EditarArchivo(decimal id)
        {
            try
            {
                _logger.LogInformation("method called");
                ViewBag.idArchivo = id;
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
        /// <param name="id"></param>
        /// <param name="nombre"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public IActionResult AdjuntarArchivo(decimal id, string nombre, string url)
        {
            try
            {
                _logger.LogInformation("method called");
                ViewBag.idArchivo = id;
                ViewBag.nombreArchivo = nombre;
                ViewBag.urlArchivo = url;
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
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult VisualizarArchivo(decimal id)
        {
            try
            {
                _logger.LogInformation("method called");
                ViewBag.idArch = id;
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
        /// <param name="cadBusqueda"></param>
        /// <returns></returns>
        [HttpPost]
        public List<ConsultDocument> BusquedaDocumentos(string cadBusqueda)
        {
            try
            {
                _logger.LogInformation("method called");
                List<ConsultDocument> r = new();

                string URI = UrlApi + "/DocumentManagement/Consult?searchString=" + cadBusqueda;
                var httpClient = getHttpClient();
                string token = HttpContext.Session.GetString("token") ?? "";

                if (token == "")
                {
                    HttpContext.Session.Remove("token");
                    return r;
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.GetAsync(URI).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        string jsonInput = responseString;
                        Responses respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();
                        r = JsonConvert.DeserializeObject<List<ConsultDocument>>(respuesta.Response.ToString() ?? "") ?? new List<ConsultDocument>();
                        HttpContext.Session.SetString("token", respuesta.Token);

                        return r;
                    }

                    return r;
                }
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
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public soportsDocuments ObtenerDocumentos(decimal id)
        {
            try
            {
                _logger.LogInformation("method called");
                soportsDocuments r = new();

                string URI = UrlApi + "/DocumentManagement/GetDocument?id=" + id;
                var httpClient = getHttpClient();
                string token = HttpContext.Session.GetString("token") ?? "";

                if (token == "")
                {
                    HttpContext.Session.Remove("token");
                    return r;
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.GetAsync(URI).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        string jsonInput = responseString;
                        Responses respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();

                        if (respuesta.Error)
                        {
                            return r;
                        }
                        r = JsonConvert.DeserializeObject<soportsDocuments>(respuesta.Response.ToString() ?? "") ?? new soportsDocuments();

                        return r;
                    }

                    return r;
                }
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
        /// <param name="documento"></param>
        /// <returns></returns>
        public string SaveDocument(ReqGuardarDoc documento)
        {
            try
            {
                _logger.LogInformation("method called");
                string r = "";
                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = UrlApi + "/DocumentManagement/SaveDocument";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var req = new
                {
                    documento.id,
                    document = new
                    {
                        code = documento.archivo.codigo,
                        base64Attachment = documento.archivo.adjuntoBase64,
                        attachmentName = documento.archivo.nombreAdjunto,
                        attachmentType = documento.archivo.tipoAdjunto
                    }
                };

                var response = httpClient.PostAsJsonAsync(URI, req).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                    if(respuesta.Response != null)
                    {
                        HttpContext.Session.SetString("token", respuesta.Token);
                        r = respuesta.Message;
                        return r;
                    }
                }

                return r;
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
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public string LeerDocumento(decimal id)
        {
            try
            {
                _logger.LogInformation("method called");
                string r = "";

                string URI = UrlApi + "/DocumentManagement/ReadDocument?id=" + id;
                var httpClient = getHttpClient();
                string token = HttpContext.Session.GetString("token") ?? "";

                if (token == "")
                {
                    HttpContext.Session.Remove("token");
                    return r;
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.GetAsync(URI).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        string jsonInput = responseString;
                        Responses respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();

                        if (respuesta.Error)
                        {
                            return r;
                        }
                        r = respuesta.Response.ToString() ?? "";

                        return r;
                    }

                    return r;
                }
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
        /// <param name="datosDoc"></param>
        /// <returns></returns>
        [HttpPost]
        public string ActualizarDocumento (UpdateDocument datosDoc)
        {
            try
            {
                _logger.LogInformation("method called");
                string r = "";


                string URI = UrlApi + "/DocumentManagement/UpdateDocument";
                var httpClient = getHttpClient();
                string token = HttpContext.Session.GetString("token") ?? "";

                if (token == "")
                {
                    HttpContext.Session.Remove("token");
                    return r;
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var req = new
                    {
                        datosDoc.id,
                        documentChanges = datosDoc.cambiosDoc
                    };

                    var response = httpClient.PostAsJsonAsync(URI, req).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        string jsonInput = responseString;
                        Responses respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();

                        if (respuesta.Error)
                        {
                            return r;
                        }
                        r = respuesta.Message;

                        return r;
                    }

                    return r;
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
