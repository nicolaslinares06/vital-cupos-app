using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Repository.Helpers;
using System.Net.Http.Headers;
using Web.Models;

namespace Web.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class AdministracionTecnicaController : Controller
    {
        private readonly ILogger<AdministracionTecnicaController> _logger;
        private readonly string UrlApi;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public AdministracionTecnicaController(ILogger<AdministracionTecnicaController> logger)
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

                Technical technical = filtradoInicial();
                return View(technical);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return StatusCode(500, "An error occurred.");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult AgregarValor()
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
                return StatusCode(500, "An error occurred.");
            }
        }

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
                return StatusCode(500, "An error occurred.");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public Technical filtradoInicial()
        {
            try
            {
                _logger.LogInformation("method called");
                List<ReqValorTecnica> r = new List<ReqValorTecnica>();

                string token = HttpContext.Session.GetString("token") ?? "";

                string URI = UrlApi + "/TechnicalAdmin/Consult";
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
                        r = JsonConvert.DeserializeObject<List<ReqValorTecnica>>(respuesta.Response.ToString() ?? "") ?? new List<ReqValorTecnica>();
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }

                foreach (var tecnico in r)
                {
                    List<SelectListItem> resultadoListado = new List<SelectListItem>();

                    if (tecnico.a007estadoRegistro.ToUpper() == "ACTIVO")
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

                    tecnico.estate = resultadoListado;
                }

                Technical listado = new Technical();

                listado.technicalList = r;

                return listado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        public List<ReqValorTecnica> filtroValor(string nombreValor)
        {
            try
            {
                _logger.LogInformation("method called");
                List<ReqValorTecnica> r = new List<ReqValorTecnica>();

                string token = HttpContext.Session.GetString("token") ?? "";

                string URI = UrlApi + "/TechnicalAdmin/Consult?value=" + nombreValor;
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
                        r = JsonConvert.DeserializeObject<List<ReqValorTecnica>>(respuesta.Response.ToString() ?? "") ?? new List<ReqValorTecnica>();
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
        public string Actualizar(AdminTecnicaReq _datosAct)
        {
            try
            {
                _logger.LogInformation("method called");
                if (HttpContext.Session.GetString("ActualizarTecnica") != "True")
                {
                    return "El usuario no cuenta con los permisos para Actualizar la información";
                }

                string r = "";
                string token = HttpContext.Session.GetString("token") ?? "";

                string URI = UrlApi + "/TechnicalAdmin/Update";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var req = new
                {
                    code = _datosAct.codigo,
                    name = _datosAct.nombre,
                    value = _datosAct.valor,
                    description = _datosAct.descripcion,
                    registrationStatus = _datosAct.estadoRegistro
                };

                var response = httpClient.PutAsJsonAsync(URI, req).Result;

                string responseString = response.Content.ReadAsStringAsync().Result;
                Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                HttpContext.Session.SetString("token", respuesta.Token);

                r = respuesta.Message;
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
        /// <param name="idEliminar"></param>
        /// <returns></returns>
        [HttpPost]
        public string Eliminar(ReqId idEliminar)
        {
            try
            {
                _logger.LogInformation("method called");
                if (HttpContext.Session.GetString("EliminarTecnica") != "True")
                {
                    return "El usuario no cuenta con los permisos para Eliminar la información";
                }

                string r = "";
                string token = HttpContext.Session.GetString("token") ?? "";

                string URI = UrlApi + "/TechnicalAdmin/Delete";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.PostAsJsonAsync(URI, idEliminar).Result;

                string responseString = response.Content.ReadAsStringAsync().Result;
                Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                HttpContext.Session.SetString("token", respuesta.Token);

                r = respuesta.Message;
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
        public IActionResult Crear(AdminTecnicaReq _datosCreacion)
        {
            try
            {
                _logger.LogInformation("method called");
                if (HttpContext.Session.GetString("ActualizarTecnica") != "True")
                {
                    ViewBag.ErrorCreacion = "false";
                    ViewBag.RespuestaCreacionTabla = "El usuario no cuenta con los permisos para Actualizar la información";
                    return View("AgregarValor");
                }

                string token = HttpContext.Session.GetString("token") ?? "";

                string URI = UrlApi + "/TechnicalAdmin/Create";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var req = new
                {
                    code = _datosCreacion.codigo,
                    name = _datosCreacion.nombre,
                    value = _datosCreacion.valor,
                    description = _datosCreacion.descripcion,
                    registrationStatus = _datosCreacion.estadoRegistro
                };

                var response = httpClient.PostAsJsonAsync(URI, req).Result;

                string responseString = response.Content.ReadAsStringAsync().Result;
                Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                HttpContext.Session.SetString("token", respuesta.Token);

                if (!respuesta.Error)
                {
                    ViewBag.ErrorCreacion = "false";
                    ViewBag.RespuestaCreacionTabla = respuesta.Message;
                    return View("AgregarValor");
                }
                else
                {
                    ViewBag.RespuestaCreacionTabla = respuesta.Message;
                    return View("AgregarValor");
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
        [Produces("application/json")]
        [HttpGet]
        public List<string> BuscarValoresTecnicos()
        {
            try
            {
                _logger.LogInformation("method called");
                List<string> valores = new List<string>();

                string term = HttpContext.Request.Query["term"].ToString();

                string token = HttpContext.Session.GetString("token") ?? "";

                string URI = UrlApi + "/TechnicalAdmin/ConsultTechnicalValues?parameter=" + term;
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
