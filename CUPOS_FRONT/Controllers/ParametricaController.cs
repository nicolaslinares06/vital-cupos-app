using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.Helpers;
using Repository.Helpers.Models;
using System;
using System.Net.Http.Headers;
using Web.Models;
using static CUPOS_FRONT.Models.Requests;

namespace Web.Controllers
{
    public class ParametricaController : Controller
    {

        private readonly ILogger<ParametricaController  > _logger;
        private readonly string UrlApi;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI");

        public ParametricaController(ILogger<ParametricaController> logger)
        {
            UrlApi = string.IsNullOrEmpty(RUTAAPI) ? new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetValue<string>("Variables:RutaApi") : RUTAAPI;
            _logger = logger;
        }

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
        public IActionResult Registrar()
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

                return View("Views/Parametrica/RegistrarTabla.cshtml");
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
            string? token = HttpContext.Session.GetString("token");

            if (token == null)
                return View("Views/Login/Index.cshtml");

            return View();
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
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

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
        [HttpPost]
        public List<GestionParametrica> Filtrar(string nombreBus)
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/Parametric/Consult?searchString=" + nombreBus + "&page=";
                var httpClient = getHttpClient();
                string? token = HttpContext.Session.GetString("token");

                List<GestionParametrica> parametricas = new List<GestionParametrica>();

                if (token == null)
                {
                    HttpContext.Session.Remove("token");
                    return parametricas;
                }
                else
                {

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.GetAsync(URI).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        string jsonInput = responseString;
                        Responses respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput);
                        parametricas = JsonConvert.DeserializeObject<List<GestionParametrica>>(respuesta.Response.ToString());
                        HttpContext.Session.SetString("token", respuesta.Token);

                        return parametricas;
                    }

                    return parametricas;
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
        /// <returns></returns>
        [HttpPost]
        public string Crear(ReqParametric datosPar)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");
                string respuesta = "";

                if (token == null)
                {
                    HttpContext.Session.Remove("token");
                    respuesta = "sin token";
                    return respuesta;
                }
                else
                {
                    string URI = UrlApi + "/Parametric/Create";
                    var httpClient = getHttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.PostAsJsonAsync(URI, datosPar).Result;

                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses resp = JsonConvert.DeserializeObject<Responses>(responseString);

                    if (response.IsSuccessStatusCode)
                    {
                        HttpContext.Session.SetString("token", resp.Token);
                        respuesta = "exito";
                    }
                }
                return respuesta;
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
        [HttpPost]
        public string Actualizar(ReqParametric datosPar)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");
                string respuesta = "";

                if (token == null)
                {
                    HttpContext.Session.Remove("token");
                    respuesta = "sin token";
                    return respuesta;
                }
                else
                {
                    string URI = UrlApi + "/Parametric/Update";
                    var httpClient = getHttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.PostAsJsonAsync(URI, datosPar).Result;

                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses resp = JsonConvert.DeserializeObject<Responses>(responseString);

                    if (response.IsSuccessStatusCode)
                    {
                        HttpContext.Session.SetString("token", resp.Token);
                        respuesta = "exito";
                    }
                }
                return respuesta;
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
        [HttpPost]
        public string Eliminar(ReqIdParametric datosPar)
        {
            try
            {
                _logger.LogInformation("method called");
                string r = "";
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                {
                    HttpContext.Session.Remove("token");
                    ViewBag.AlertaPass = "false";
                    ViewBag.RespuestaCambioPass = "Sesion caducada";
                }
                else
                {
                    string URI = UrlApi + "/Parametric/Delete";
                    var httpClient = getHttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.PostAsJsonAsync(URI, datosPar).Result;

                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses resp = JsonConvert.DeserializeObject<Responses>(responseString);

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
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string ActivarInactivar(ReqParametric datosPar)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");
                string respuesta = "";

                if (token == null)
                {
                    HttpContext.Session.Remove("token");
                    respuesta = "sin token";
                    return respuesta;
                }
                else
                {
                    string URI = UrlApi + "/Parametric/ActivateInactivate";
                    var httpClient = getHttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.PostAsJsonAsync(URI, datosPar).Result;

                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses resp = JsonConvert.DeserializeObject<Responses>(responseString);

                    if (response.IsSuccessStatusCode)
                    {
                        HttpContext.Session.SetString("token", resp.Token);
                        respuesta = "exito";
                    }
                }
                return respuesta;
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
        public List<ReqDocumentType> ConsultarParametrica(string parametrica)
        {
            try
            {
                _logger.LogInformation("method called");
                List<ReqDocumentType> docs = new List<ReqDocumentType>();

                string? token = HttpContext.Session.GetString("token");

                string URI = UrlApi + "/Parametric/ConsultParametric?parametric=" + parametrica;
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;

                string responseString = response.Content.ReadAsStringAsync().Result;
                Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                docs = JsonConvert.DeserializeObject<List<ReqDocumentType>>(respuesta.Response.ToString());

                return docs;
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
        public List<ReqAdminTecnica> ConsultarPaises()
        {
            try
            {
                _logger.LogInformation("method called");
                List<ReqAdminTecnica> docs = new List<ReqAdminTecnica>();

                string? token = HttpContext.Session.GetString("token");

                string URI = UrlApi + "/Parametric/ConsultCountries";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;

                string responseString = response.Content.ReadAsStringAsync().Result;
                Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                docs = JsonConvert.DeserializeObject<List<ReqAdminTecnica>>(respuesta.Response.ToString());

                return docs;
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
        public List<ReqAdminTecnica> ConsultarDepartamentos(int idpais)
        {
            try
            {
                _logger.LogInformation("method called");
                List<ReqAdminTecnica>? docs = new List<ReqAdminTecnica>();

                string? token = HttpContext.Session.GetString("token");

                string URI = UrlApi + "/Parametric/ConsultDepartments?idCountry=" + idpais;
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;

                string responseString = response.Content.ReadAsStringAsync().Result;
                Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                docs = JsonConvert.DeserializeObject<List<ReqAdminTecnica>>(respuesta.Response.ToString());

                return docs;
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
        public List<ReqAdminTecnica> ConsultarCiudades(int iddepartamento)
        {
            try
            {
                _logger.LogInformation("method called");
                List<ReqAdminTecnica>? docs = new List<ReqAdminTecnica>();

                string? token = HttpContext.Session.GetString("token");

                string URI = UrlApi + "/Parametric/ConsultCities?idDepartament=" + iddepartamento;
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;

                string responseString = response.Content.ReadAsStringAsync().Result;
                Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                docs = JsonConvert.DeserializeObject<List<ReqAdminTecnica>>(respuesta.Response.ToString());

                return docs;
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
        [Produces("application/json")]
        [HttpGet]
        public List<string> BuscarParametro()
        {
            try
            {
                _logger.LogInformation("method called");
                List<string> valores = new List<string>();

                string term = HttpContext.Request.Query["term"].ToString();
                List<ValorParametro>? datos = new List<ValorParametro>();

                string? token = HttpContext.Session.GetString("token");

                string URI = UrlApi + "/Parametric/ConsultParameters?parameter=" + term;
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;

                string responseString = response.Content.ReadAsStringAsync().Result;
                Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                datos = JsonConvert.DeserializeObject<List<ValorParametro>>(respuesta.Response.ToString());

                foreach (var Parametros in datos)
                {
                    valores.Add(Convert.ToString(Parametros.a008Parametrica));
                }

                return valores;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }
    }
}