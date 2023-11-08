using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Repository.Helpers;
using Repository.Helpers.Models;
using System.Net.Http.Headers;
using Web.Models;
using static CUPOS_FRONT.Models.Requests;

namespace Web.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class AdministrarEstadosController : Controller
    {
        private readonly ILogger<AdministrarEstadosController> _logger;
        private readonly string UrlApi;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public AdministrarEstadosController(ILogger<AdministrarEstadosController> logger)
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
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

                Estados estados = actividadesSinFiltro();
                return View(estados);
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
        public IActionResult AgregarEstado()
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
        /// <param name="nombreEstado"></param>
        /// <param name="estadoRegistro"></param>
        /// <returns></returns
        public Estados actividadesSinFiltro()
        {
            List<ReqEstados> r = new List<ReqEstados>();

            string token = HttpContext.Session.GetString("token") ?? "";

            string URI = UrlApi + "/Estate/Consult";
            var httpClient = getHttpClient();

            if (token == "")
            {
                HttpContext.Session.Remove("token");
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
                    r = JsonConvert.DeserializeObject<List<ReqEstados>>(respuesta.Response.ToString() ?? "") ?? new List<ReqEstados>();
                    HttpContext.Session.SetString("token", respuesta.Token);
                }
            }

            var estadosCertificado = ConsultarEstadosCertificado();

            foreach (var actividad in r)
            {
                List<SelectListItem> resultadoListado = new List<SelectListItem>();
                List<SelectListItem> resultadoEstadosCert = new List<SelectListItem>();
                int contador = 0;

                if (actividad.a008estadoRegistro.ToUpper() == "ACTIVO")
                {
                    var opcionSelected = new SelectListItem("ACTIVO", "ACTIVO", true);
                    var opcion2 = new SelectListItem("INACTIVO", "INACTIVO", false);
                    resultadoListado.Insert(0, opcionSelected);
                    resultadoListado.Insert(1, opcion2);
                }
                else
                {
                    var opcionSelected = new SelectListItem("INACTIVO", "INACTIVO", true);
                    var opcion2 = new SelectListItem("ACTIVO", "ACTIVO", false);
                    resultadoListado.Insert(0, opcionSelected);
                    resultadoListado.Insert(1, opcion2);
                }

                foreach (var estadoCert in estadosCertificado.Select(x => x.nombre))
                {
                    if (estadoCert == actividad.a008codigoParametricaEstado)
                    {
                        var optionSelected = new SelectListItem(estadoCert, estadoCert, true);
                        resultadoEstadosCert.Insert(contador, optionSelected);
                    }
                    else
                    {
                        var option = new SelectListItem(estadoCert, estadoCert, false);
                        resultadoEstadosCert.Insert(contador, option);
                    }
                    contador++;
                }

                actividad.estate = resultadoListado;
                actividad.stage = resultadoEstadosCert;
            }

            Estados listado = new Estados();

            listado.estateList = r;

            return listado;
        }

        public List<ReqEstados> filtroEstados(string nombreEstado, string estadoRegistro)
        {
            try
            {
                _logger.LogInformation("method called");
                List<ReqEstados> r = new List<ReqEstados>();

                string token = HttpContext.Session.GetString("token") ?? "";

                string URI = UrlApi + "/Estate/Consult?name=" + nombreEstado + "&registrationStatus=" + estadoRegistro;
                var httpClient = getHttpClient();

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
                        r = JsonConvert.DeserializeObject<List<ReqEstados>>(respuesta.Response.ToString() ?? "") ?? new List<ReqEstados>();
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
        /// <param name="_datosAct"></param>
        /// <returns></returns>
        [HttpPost]
        public string ActualizarEstado(ReqEstado _datosAct)
        {
            try
            {
                _logger.LogInformation("method called");
                if (HttpContext.Session.GetString("ActualizarEstados") != "True")
                {
                    return "El usuario no cuenta con los permisos para Actualizar estados";
                }

                string r = "";
                string token = HttpContext.Session.GetString("token") ?? "";

                if (_datosAct.description == null)
                {
                    _datosAct.description = "";
                }

                if (token == "")
                {
                    HttpContext.Session.Remove("token");
                }
                else
                {
                    string URI = UrlApi + "/Estate/Update";
                    var httpClient = getHttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.PutAsJsonAsync(URI, _datosAct).Result;

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
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_datosCreacion"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Crear(ReqAdminEstados _datosCreacion)
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";

                string URI = UrlApi + "/Estate/Create";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var req = new
                {
                    stage = _datosCreacion.etapa,
                    description = _datosCreacion.descripcion,
                    state = _datosCreacion.estado
                };

                var response = httpClient.PostAsJsonAsync(URI, req).Result;

                string responseString = response.Content.ReadAsStringAsync().Result;
                Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                HttpContext.Session.SetString("token", respuesta.Token);

                ViewBag.ErrorCreacionEstado = "false";
                ViewBag.RespuestaCreacionEstado = respuesta.Message;
                return View("AgregarEstado");
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
        public List<ReqEstadoCertificado> ConsultarEstadosCertificado()
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";

                string URI = UrlApi + "/Parametric/ConsultEstateCertificate";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;

                string responseString = response.Content.ReadAsStringAsync().Result;
                Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                List<ReqEstadoCertificado> docs = JsonConvert.DeserializeObject<List<ReqEstadoCertificado>>(respuesta.Response.ToString() ?? "") ?? new List<ReqEstadoCertificado>();

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
        public List<string> BuscarEstados()
        {
            try
            {
                _logger.LogInformation("method called");
                List<string> valores = new List<string>();

                string term = HttpContext.Request.Query["term"].ToString();

                string token = HttpContext.Session.GetString("token") ?? "";

                string URI = UrlApi + "/Estate/ConsultEstates?parameter=" + term;
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;

                string responseString = response.Content.ReadAsStringAsync().Result;
                Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                List<ValorEstado> datos = JsonConvert.DeserializeObject<List<ValorEstado>>(respuesta.Response.ToString() ?? "") ?? new List<ValorEstado>();

                foreach (var etapa in datos)
                {
                    valores.Add(Convert.ToString(etapa.etapa) ?? "");
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
