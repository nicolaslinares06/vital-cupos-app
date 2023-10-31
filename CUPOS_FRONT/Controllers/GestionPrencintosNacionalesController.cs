using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.Helpers;
using Repository.Helpers.Models;
using Repository.Models;
using System.Net.Http;
using System;
using System.Net.Http.Headers;
using Web.Models;
using static Web.Models.TrayForNationalSealsExternUsers;
using System.Text.RegularExpressions;
using System.Text;

namespace Web.Controllers
{
    public class GestionPrencintosNacionalesController : Controller
    {
        private readonly ILogger<GestionPrencintosNacionalesController> _logger;
        private readonly IConfiguration configuration;
        private readonly string UrlApi;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";

        public GestionPrencintosNacionalesController(ILogger<GestionPrencintosNacionalesController> logger, IConfiguration configuration)
        {
            UrlApi = string.IsNullOrEmpty(RUTAAPI) ? new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetValue<string>("Variables:RutaApi") : RUTAAPI;
            _logger = logger;
            this.configuration = configuration;
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
        public IActionResult Index()
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/GestionPrencintosNacionales/Index.cshtml");

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string? token = HttpContext.Session.GetString("token");
                if (token == null)
                    return RedirectToAction("Index", "Login");
                else
                    return RedirectToAction("FlujoNegocio", "Home");

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
                    return View("Views/GestionPrencintosNacionales/Partials/Ayuda.cshtml");

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string? token = HttpContext.Session.GetString("token");
                if (token == null)
                    return RedirectToAction("Index", "Login");
                else
                    return RedirectToAction("Index", "GestionPrencintosNacionales");

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult GenerarNumeroRadicado(int idSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token != null)
                {
                    ViewBag.idSolicitud = idSolicitud;
                    return View("Views/GestionPrencintosNacionales/Pendientes/GenerarNumeroRadicado.cshtml");
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string? token = HttpContext.Session.GetString("token");
                if (token == null)
                    return RedirectToAction("Index", "Login");
                else
                    return RedirectToAction("Index", "GestionPrencintosNacionales");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult GenerarNumeroRadicadoSalida(int code, int amount, decimal colorCode,
            decimal codeIni, decimal codeFin, string nameSpecie, decimal valueConsignment, string observations, string tipoSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token != null)
                {
                    ViewBag.code = code;
                    ViewBag.amount = amount;
                    ViewBag.colorCode = colorCode;
                    ViewBag.codeIni = codeIni;
                    ViewBag.codeFin = codeFin;
                    ViewBag.nameSpecie = nameSpecie;
                    ViewBag.valueConsignment = valueConsignment;
                    ViewBag.observations = observations;
                    ViewBag.tipoSolicitud = tipoSolicitud;
                    return View("Views/GestionPrencintosNacionales/Pendientes/GenerarNumeroRadicadoSalida.cshtml");
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string? token = HttpContext.Session.GetString("token");
                if (token == null)
                    return RedirectToAction("Index", "Login");
                else
                    return RedirectToAction("Index", "GestionPrencintosNacionales");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult SolicitudRadicadaPendiente(int idSolicitud)
        {
            try
            {
                var parametricasTipoSolicitudJson = configuration.GetSection("ParametricasTipoSolicitud").Get<List<ParametricaTipoSolicitud>>();
                if (parametricasTipoSolicitudJson is null)
                    parametricasTipoSolicitudJson = new List<ParametricaTipoSolicitud>();
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token != null)
                {
                    var model = ObtenerInfoSolicitud(idSolicitud);
                    model.ListaTiposSolicitud = parametricasTipoSolicitudJson;
                    ViewBag.idSolicitud = idSolicitud;
                    return View("Views/GestionPrencintosNacionales/Pendientes/SolicitudRadicada.cshtml", model);
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string? token = HttpContext.Session.GetString("token");
                if (token == null)
                    return RedirectToAction("Index", "Login");
                else
                    return RedirectToAction("Index", "GestionPrencintosNacionales");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult GenerarCodigoPrecintosPendiente(int idSolicitud, int cantidad, string observaciones, string tipoSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");


                if (token != null)
                {
                    string textoNormalizado = tipoSolicitud.Normalize(NormalizationForm.FormD);
                    Regex reg = new Regex("[^a-zA-Z0-9 ]");
                    var requetType = reg.Replace(textoNormalizado, "");
                    ViewBag.tipoSolicitud = requetType;
                    cuttingSafeGuar safe;

                    List<decimal> valsafe = new List<decimal>();
                    List<valCut> valCut = new List<valCut>();

                    if (requetType == "Marquillas con verificacion de corte certificada por ministerio")
                    {
                        safe = getCuttingSafe(idSolicitud);
                        valsafe = safe.salvoCon;
                        valCut = safe.Cut;
                    }

                    ViewBag.idSolicitud = idSolicitud;
                    ViewBag.cantidad = cantidad;
                    ViewBag.fractionType = valCut;
                    ViewBag.salvoCondu = valsafe;
                    ViewBag.observaciones = observaciones;
                    ViewBag.tipoSolicitud = tipoSolicitud;

                    // Imprime los valores de tipoSolicitud y observaciones en la consola
                    Console.WriteLine("tipoSolicitud: " + tipoSolicitud);
                    Console.WriteLine("observaciones: " + observaciones);

                    return View("Views/GestionPrencintosNacionales/Pendientes/GenerarCodigoPrecintos.cshtml");
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string? token = HttpContext.Session.GetString("token");
                if (token == null)
                    return RedirectToAction("Index", "Login");
                else
                    return RedirectToAction("Index", "GestionPrencintosNacionales");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult GuardarCodigoPrecintosPendiente()
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token != null)
                {
                    return View("Views/GestionPrencintosNacionales/Pendientes/GuardarCodigoPrecintos.cshtml");
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string? token = HttpContext.Session.GetString("token");
                if (token == null)
                    return RedirectToAction("Index", "Login");
                else
                    return RedirectToAction("Index", "GestionPrencintosNacionales");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult GenerarCartaSolicitantePendiente(int code, int amount, string color, decimal colorCode,
            decimal codeIni, decimal codeFin, string nameSpecie, decimal valueConsignment, string observations, string tipoSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token != null)
                {
                    ViewBag.code = code;
                    ViewBag.amount = amount;
                    ViewBag.color = color;
                    ViewBag.colorCode = colorCode;
                    ViewBag.codeIni = codeIni;
                    ViewBag.codeFin = codeFin;
                    ViewBag.nameSpecie = nameSpecie;
                    ViewBag.valueConsignment = valueConsignment;
                    ViewBag.observations = observations;
                    ViewBag.tipoSolicitud = tipoSolicitud;

                    // Imprime los valores de tipoSolicitud y observaciones en la consola
                    Console.WriteLine("tipoSolicitud: " + tipoSolicitud);
                    Console.WriteLine("observaciones: " + observations);

                    return View("Views/GestionPrencintosNacionales/Pendientes/GenerarCartaSolicitantePendiente.cshtml");
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string? token = HttpContext.Session.GetString("token");
                if (token == null)
                    return RedirectToAction("Index", "Login");
                else
                    return RedirectToAction("Index", "GestionPrencintosNacionales");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Requerimiento(int idSolicitud)
        {
            try
            {

                var parametricasTipoSolicitudJson = configuration.GetSection("ParametricasTipoSolicitud").Get<List<ParametricaTipoSolicitud>?>();
                if (parametricasTipoSolicitudJson is null)
                    parametricasTipoSolicitudJson = new List<ParametricaTipoSolicitud>();
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token != null)
                {
                    var model = ObtenerInfoSolicitud(idSolicitud);
                    model.ListaTiposSolicitud = parametricasTipoSolicitudJson;
                    ViewBag.idSolicitud = idSolicitud;
                    return View("Views/GestionPrencintosNacionales/Requerimiento/Requerimiento.cshtml", model);
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string? token = HttpContext.Session.GetString("token");
                if (token == null)
                    return RedirectToAction("Index", "Login");
                else
                    return RedirectToAction("Index", "GestionPrencintosNacionales");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult SolicitudRadicadaAprobada(int idSolicitud)
        {
            try
            {
                var parametricasTipoSolicitudJson = configuration.GetSection("ParametricasTipoSolicitud").Get<List<ParametricaTipoSolicitud>>();
                if (parametricasTipoSolicitudJson is null)
                    parametricasTipoSolicitudJson = new List<ParametricaTipoSolicitud>();
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token != null)
                {
                    var model = ObtenerInfoSolicitud(idSolicitud);
                    model.ListaTiposSolicitud = parametricasTipoSolicitudJson;
                    ViewBag.idSolicitud = idSolicitud;
                    return View("Views/GestionPrencintosNacionales/SolicitudesAprobadas/SolicitudRadicada.cshtml", model);
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string? token = HttpContext.Session.GetString("token");
                if (token == null)
                    return RedirectToAction("Index", "Login");
                else
                    return RedirectToAction("Index", "GestionPrencintosNacionales");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult SolicitudRadicadaDesistimiento(int idSolicitud)
        {
            try
            {
                var parametricasTipoSolicitudJson = configuration.GetSection("ParametricasTipoSolicitud").Get<List<ParametricaTipoSolicitud>>();
                if (parametricasTipoSolicitudJson is null)
                    parametricasTipoSolicitudJson = new List<ParametricaTipoSolicitud>();
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token != null)
                {
                    var model = ObtenerInfoSolicitud(idSolicitud);
                    model.ListaTiposSolicitud = parametricasTipoSolicitudJson;
                    ViewBag.idSolicitud = idSolicitud;
                    return View("Views/GestionPrencintosNacionales/Desistimiento/SolicitudRadicada.cshtml", model);
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string? token = HttpContext.Session.GetString("token");
                if (token == null)
                    return RedirectToAction("Index", "Login");
                else
                    return RedirectToAction("Index", "GestionPrencintosNacionales");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns> 
        public IActionResult GenerarCodigosPrecintos(int codeSpecies, int initialNumber, int finalNumber, int color, int amount, string speciesName, string colorName, int worth, int idSolicitud, string observaciones, string tipoSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");
                numbers num = new numbers();
                num.initial = 0;
                if (String.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Index", "Login");                   
                }

                string URI = UrlApi + "/NationalSealsManagement/GetNumbersSeals?requestType=" + tipoSolicitud;
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    string jsonInput = responseString;
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();
                    if(respuesta.Response != null)
                    {
                        num = JsonConvert.DeserializeObject<numbers>(respuesta.Response.ToString() ?? "") ?? new numbers();
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                  
                }

                if (num.initial != 0)
                {
                    ViewBag.initialNumber = num.initial;
                    ViewBag.finalNumber = num.initial + amount - 1;
                }
                else
                {
                    ViewBag.initialNumber = initialNumber;
                    ViewBag.finalNumber = finalNumber;
                }

                ViewBag.speciesName = speciesName;
                ViewBag.codeSpecies = codeSpecies;
                ViewBag.colorName = colorName;
                ViewBag.color = color;
                ViewBag.amount = amount;
                ViewBag.worth = worth;
                ViewBag.idSolicitud = idSolicitud;
                ViewBag.observations = observaciones;
                ViewBag.tipoSolicitud = tipoSolicitud;
                // Imprime los valores de tipoSolicitud y observaciones en la consola
                Console.WriteLine("tipoSolicitud: " + tipoSolicitud);
                Console.WriteLine("observaciones: " + observaciones);

                return View("Views/GestionPrencintosNacionales/Pendientes/GuardarCodigoPrecintos.cshtml");

                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string? token = HttpContext.Session.GetString("token");
                if (token == null)
                    return RedirectToAction("Index", "Login");
                else
                    return RedirectToAction("Index", "GestionPrencintosNacionales");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult DesistimientoSolicitud(int idSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token != null)
                {
                    ViewBag.idSolicitud = idSolicitud;
                    return View("Views/GestionPrencintosNacionales/Asignadas/DesistimientoSolicitud.cshtml");
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string? token = HttpContext.Session.GetString("token");
                if (token == null)
                    return RedirectToAction("Index", "Login");
                else
                    return RedirectToAction("Index", "GestionPrencintosNacionales");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string DesistimientoSolicitud(DesistNationalSealsManagement datos)
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
                    string URI = UrlApi + "/NationalSealsManagement/DesistSettled";
                    var httpClient = getHttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.PostAsJsonAsync(URI, datos).Result;

                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses resp = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

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
                string respuesta = "";
                respuesta = ex.Message;
                return respuesta;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<CuposV002GestionPrecintosNacionales> ConsultarPendiente()
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/NationalSealsManagement/ConsultPendingAnalyst";
                var httpClient = getHttpClient();
                string? token = HttpContext.Session.GetString("token");

                List<CuposV002GestionPrecintosNacionales> listaDatos = new List<CuposV002GestionPrecintosNacionales>();
                if (token == null)
                {
                    HttpContext.Session.Remove("token");
                    return listaDatos;
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.GetAsync(URI).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        string jsonInput = responseString;
                        Responses? respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();
                        listaDatos = JsonConvert.DeserializeObject<List<CuposV002GestionPrecintosNacionales>>(respuesta.Response.ToString() ?? "") ?? new List<CuposV002GestionPrecintosNacionales>();
                        HttpContext.Session.SetString("token", respuesta.Token);

                        return listaDatos;
                    }

                    return listaDatos;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<CuposV002GestionPrecintosNacionales> listaDatos = new List<CuposV002GestionPrecintosNacionales>();
                return listaDatos;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<CuposV002GestionPrecintosNacionales> ConsultarRequerimiento()
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/NationalSealsManagement/ConsultRequirementAnalyst";
                var httpClient = getHttpClient();
                string? token = HttpContext.Session.GetString("token");

                List<CuposV002GestionPrecintosNacionales> listaDatos = new List<CuposV002GestionPrecintosNacionales>();
                if (token == null)
                {
                    HttpContext.Session.Remove("token");
                    return listaDatos;
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.GetAsync(URI).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        string jsonInput = responseString;
                        Responses? respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();
                        listaDatos = JsonConvert.DeserializeObject<List<CuposV002GestionPrecintosNacionales>>(respuesta.Response.ToString() ?? "") ?? new List<CuposV002GestionPrecintosNacionales>();
                        HttpContext.Session.SetString("token", respuesta.Token);

                        return listaDatos;
                    }

                    return listaDatos;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<CuposV002GestionPrecintosNacionales> listaDatos = new List<CuposV002GestionPrecintosNacionales>();
                return listaDatos;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<CuposV002GestionPrecintosNacionales> ConsultarAprobada()
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/NationalSealsManagement/ConsultApprovedAnalyst";
                var httpClient = getHttpClient();
                string? token = HttpContext.Session.GetString("token");

                List<CuposV002GestionPrecintosNacionales> listaDatos = new List<CuposV002GestionPrecintosNacionales>();
                if (token == null)
                {
                    HttpContext.Session.Remove("token");
                    return listaDatos;
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.GetAsync(URI).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        string jsonInput = responseString;
                        Responses? respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();
                        listaDatos = JsonConvert.DeserializeObject<List<CuposV002GestionPrecintosNacionales>>(respuesta.Response.ToString() ?? "") ?? new List<CuposV002GestionPrecintosNacionales>();
                        HttpContext.Session.SetString("token", respuesta.Token);

                        return listaDatos;
                    }

                    return listaDatos;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<CuposV002GestionPrecintosNacionales> listaDatos = new List<CuposV002GestionPrecintosNacionales>();
                return listaDatos;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<CuposV002GestionPrecintosNacionales> ConsultarDesistimiento()
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/NationalSealsManagement/ConsultWithdrawalAnalyst";
                var httpClient = getHttpClient();
                string? token = HttpContext.Session.GetString("token");

                List<CuposV002GestionPrecintosNacionales> listaDatos = new List<CuposV002GestionPrecintosNacionales>();
                if (token == null)
                {
                    HttpContext.Session.Remove("token");
                    return listaDatos;
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.GetAsync(URI).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        string jsonInput = responseString;
                        Responses? respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();
                        listaDatos = JsonConvert.DeserializeObject<List<CuposV002GestionPrecintosNacionales>>(respuesta.Response.ToString() ?? "") ?? new List<CuposV002GestionPrecintosNacionales>();
                        HttpContext.Session.SetString("token", respuesta.Token);

                        return listaDatos;
                    }

                    return listaDatos;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<CuposV002GestionPrecintosNacionales> listaDatos = new List<CuposV002GestionPrecintosNacionales>();
                return listaDatos;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<CuposV002GestionPrecintosNacionales> ConsultarAsignada()
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/NationalSealsManagement/ConsultAssignedAnalyst";
                var httpClient = getHttpClient();
                string? token = HttpContext.Session.GetString("token");

                List<CuposV002GestionPrecintosNacionales> listaDatos = new List<CuposV002GestionPrecintosNacionales>();
                if (token == null)
                {
                    HttpContext.Session.Remove("token");
                    return listaDatos;
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.GetAsync(URI).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        string jsonInput = responseString;
                        Responses? respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();
                        listaDatos = JsonConvert.DeserializeObject<List<CuposV002GestionPrecintosNacionales>>(respuesta.Response.ToString() ?? "") ?? new List<CuposV002GestionPrecintosNacionales>();
                        HttpContext.Session.SetString("token", respuesta.Token);

                        return listaDatos;
                    }

                    return listaDatos;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<CuposV002GestionPrecintosNacionales> listaDatos = new List<CuposV002GestionPrecintosNacionales>();
                return listaDatos;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string RadicarSolicitud(SettledNationalSealsManagement datos)
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
                    string URI = UrlApi + "/NationalSealsManagement/UpdateSettled";
                    var httpClient = getHttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.PostAsJsonAsync(URI, datos).Result;

                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses resp = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

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
                string respuesta = ex.Message;
                return respuesta;

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string RadicarSolicitudSalida(SettledNationalSealsManagement datos)
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
                    string URI = UrlApi + "/NationalSealsManagement/UpdateSettledDeparture";
                    var httpClient = getHttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.PostAsJsonAsync(URI, datos).Result;

                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses resp = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

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
                string respuesta = ex.Message;
                return respuesta;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns> 
        [HttpPost]
        public List<Colores> Color()
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/SealsLabels/Colors";
                var httpClient = getHttpClient();
                string? token = HttpContext.Session.GetString("token");

                List<Colores> colores = new List<Colores>();
                if (token == null)
                {
                    HttpContext.Session.Remove("token");
                    return colores;
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.GetAsync(URI).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        string jsonInput = responseString;
                        Responses? respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();
                        colores = JsonConvert.DeserializeObject<List<Colores>>(respuesta.Response.ToString() ?? "") ?? new List<Colores>();
                        HttpContext.Session.SetString("token", respuesta.Token);

                        return colores;
                    }

                    return colores;
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
        public List<Especies> Especie()
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/NationalSealsManagement/Species";
                var httpClient = getHttpClient();
                string? token = HttpContext.Session.GetString("token");

                List<Especies> especies = new List<Especies>();
                if (token == null)
                {
                    HttpContext.Session.Remove("token");
                    return especies;
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.GetAsync(URI).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        string jsonInput = responseString;
                        Responses? respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();
                        especies = JsonConvert.DeserializeObject<List<Especies>>(respuesta.Response.ToString() ?? "") ?? new List<Especies>();
                        HttpContext.Session.SetString("token", respuesta.Token);

                        return especies;
                    }

                    return especies;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<Especies> especies = new List<Especies>();
                return especies;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<CuposV003SolicitudPrecintosNacionales> DetalleSolicitud(int id)
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/NationalSealsManagement/ConsultDetail?id=" + id;
                var httpClient = getHttpClient();
                string? token = HttpContext.Session.GetString("token");

                List<CuposV003SolicitudPrecintosNacionales> listaDatos = new List<CuposV003SolicitudPrecintosNacionales>();
                ViewBag.listaDatos = listaDatos;

                if (token == null)
                {
                    HttpContext.Session.Remove("token");
                    return listaDatos;
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.GetAsync(URI).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        string jsonInput = responseString;
                        Responses? respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();
                        listaDatos = JsonConvert.DeserializeObject<List<CuposV003SolicitudPrecintosNacionales>>(respuesta.Response.ToString() ?? "") ?? new List<CuposV003SolicitudPrecintosNacionales>();
                        HttpContext.Session.SetString("token", respuesta.Token);

                        return listaDatos;
                    }

                    return listaDatos;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<CuposV003SolicitudPrecintosNacionales> listaDatos = new List<CuposV003SolicitudPrecintosNacionales>();
                return listaDatos;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<CodigosInternos> ConsultarNumeraciones(int idSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/NationalSealsManagement/ConsultNumbers?codigoSolicitud=" + idSolicitud;
                var httpClient = getHttpClient();
                string? token = HttpContext.Session.GetString("token");

                List<CodigosInternos> listaDatos = new List<CodigosInternos>();
                ViewBag.listaDatos = listaDatos;

                if (token == null)
                {
                    HttpContext.Session.Remove("token");
                    return listaDatos;
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.GetAsync(URI).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        string jsonInput = responseString;
                        Responses? respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();
                        listaDatos = JsonConvert.DeserializeObject<List<CodigosInternos>>(respuesta.Response.ToString() ?? "") ?? new List<CodigosInternos>();
                        HttpContext.Session.SetString("token", respuesta.Token);

                        return listaDatos;
                    }

                    return listaDatos;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<CodigosInternos> listaDatos = new List<CodigosInternos>();
                return listaDatos;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string DevolverSolicitud(ReturnSettledNationalSealsManagement datos)
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
                    string URI = UrlApi + "/NationalSealsManagement/ReturnSettled";
                    var httpClient = getHttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.PostAsJsonAsync(URI, datos).Result;

                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses resp = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

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
                string respuesta = ex.Message;
                return respuesta;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns> 
        [HttpPost]
        public string GuardarCodigosPrecintos(GenerateSealCodes datos)
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
                    string URI = UrlApi + "/NationalSealsManagement/GenerateSealCodes";
                    var httpClient = getHttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.PostAsJsonAsync(URI, datos).Result;

                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses resp = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                    if (response.IsSuccessStatusCode)
                    {
                        HttpContext.Session.SetString("token", resp.Token);
                        respuesta = resp.Message;
                    }
                }
                return respuesta;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string respuesta = ex.Message;
                return respuesta;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns> 
        [HttpPost]
        public SoportsDocuments CrearCartaSolicitantePendiente(int code, int amount, string color, decimal codeIni, decimal codeFin, string tipoSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/NationalSealsManagement/GenerateDocument?code=" + code + "&amount=" + amount + "&color=" + color + "&initialCode=" + codeIni + "&finalCode=" + codeFin + "&requestType=" + tipoSolicitud;
                var httpClient = getHttpClient();
                string? token = HttpContext.Session.GetString("token");

                SoportsDocuments listaDatos = new SoportsDocuments();
                if (token == null)
                {
                    HttpContext.Session.Remove("token");
                    listaDatos.nombreAdjunto = "No se ha iniciado sesin";
                    return listaDatos;
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.GetAsync(URI).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        string jsonInput = responseString;
                        Responses? respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();
                        listaDatos = JsonConvert.DeserializeObject<SoportsDocuments>(respuesta.Response.ToString() ?? "") ?? new SoportsDocuments();
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                    else
                    {
                        listaDatos.nombreAdjunto = response.ReasonPhrase;
                    }

                    return listaDatos;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                SoportsDocuments listaDatos = new SoportsDocuments();
                return listaDatos;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns> 
        [HttpPost]
        public List<SoportsDocuments> TraerDocumentosPrecintos(int code, int type)
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/NationalSealsManagement/DocumentSeal?code=" + code + "&type=" + type;
                var httpClient = getHttpClient();
                string? token = HttpContext.Session.GetString("token");

                List<SoportsDocuments> listaDatos = new List<SoportsDocuments>();
                if (token == null)
                {
                    HttpContext.Session.Remove("token");
                    return listaDatos;
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.GetAsync(URI).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        string jsonInput = responseString;
                        Responses? respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();
                        if (respuesta.Response != null)
                        {
                            listaDatos = JsonConvert.DeserializeObject<List<SoportsDocuments>>(respuesta.Response.ToString() ?? "") ?? new List<SoportsDocuments>();
                            HttpContext.Session.SetString("token", respuesta.Token);

                            return listaDatos;
                        }
                    }

                    return listaDatos;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<SoportsDocuments> listaDatos = new List<SoportsDocuments>();
                return listaDatos;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nit"></param>
        /// <returns></returns>
        public List<ConsultCodes> ConsultarCodigosPrecintos(int code)
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/NationalSealsManagement/ConsultCodes?code=" + code;
                var httpClient = getHttpClient();
                string? token = HttpContext.Session.GetString("token");

                List<ConsultCodes> datos = new List<ConsultCodes>();


                if (token == null)
                {
                    HttpContext.Session.Remove("token");
                    return datos;
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
                        datos = JsonConvert.DeserializeObject<List<ConsultCodes>>(respuesta.Response.ToString() ?? "") ?? new List<ConsultCodes>();
                        HttpContext.Session.SetString("token", respuesta.Token);

                        return datos;
                    }

                    return datos;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<ConsultCodes> datos = new List<ConsultCodes>();
                return datos;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idSolicitud"></param>
        /// <returns></returns>
        [HttpPost]
        public List<numeracion> ListaNumeracionesSolicitud(int idSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/NationalSealsManagement/ConsultNumberings?requestType=" + idSolicitud;
                var httpClient = getHttpClient();
                string? token = HttpContext.Session.GetString("token");

                List<numeracion> numeraciones = new List<numeracion>();
                if (token == null)
                {
                    HttpContext.Session.Remove("token");
                    return numeraciones;
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.GetAsync(URI).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        string jsonInput = responseString;
                        Responses? respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();
                        numeraciones = JsonConvert.DeserializeObject<List<numeracion>>(respuesta.Response.ToString() ?? "") ?? new List<numeracion>();
                        HttpContext.Session.SetString("token", respuesta.Token);

                        return numeraciones;
                    }

                    return numeraciones;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<numeracion> numeraciones = new List<numeracion>();
                return numeraciones;
            }
        }


        [HttpPost]
        public List<cuttingSaveModel> ListaTiposPartesCortes(int idSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/NationalSealsManagement/GetTypesFractionsRequest?requestCode=" + idSolicitud;
                var httpClient = getHttpClient();
                string? token = HttpContext.Session.GetString("token");

                List<cuttingSaveModel> fracciones = new List<cuttingSaveModel>();
                if (token == null)
                {
                    HttpContext.Session.Remove("token");
                    return fracciones;
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.GetAsync(URI).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        string jsonInput = responseString;
                        Responses? respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();
                        fracciones = JsonConvert.DeserializeObject<List<cuttingSaveModel>>(respuesta.Response.ToString() ?? "") ?? new List<cuttingSaveModel>();
                        HttpContext.Session.SetString("token", respuesta.Token);

                        return fracciones;
                    }

                    return fracciones;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<cuttingSaveModel> fracciones = new List<cuttingSaveModel>();
                return fracciones;
            }
        }
        /// <summary>
		/// 
		/// </summary>
		/// <param name="idSolicitud"></param>
		/// <returns></returns>
		[HttpPost]
        public cuttingSafeGuar getCuttingSafe(int idSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/NationalSealsManagement/ConsultNumberings?requestType=" + idSolicitud;
                var httpClient = getHttpClient();
                string? token = HttpContext.Session.GetString("token");

                cuttingSafeGuar CuttingSafe = new cuttingSafeGuar();

                if (token == null)
                {
                    HttpContext.Session.Remove("token");
                    return CuttingSafe;
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.GetAsync(URI).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        string jsonInput = responseString;
                        Responses? respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();
                        CuttingSafe = JsonConvert.DeserializeObject<cuttingSafeGuar>(respuesta.Response.ToString() ?? "") ?? new cuttingSafeGuar();
                        HttpContext.Session.SetString("token", respuesta.Token);

                        return CuttingSafe;
                    }

                    return CuttingSafe;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                cuttingSafeGuar CuttingSafe = new cuttingSafeGuar();
                return CuttingSafe;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idSolicitud"></param>
        /// <returns></returns>
        [HttpPost]
        public List<NitEmpresa> ConsultarNitEmpresa(int idSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/NationalSealsManagement/ConsultNitCompany?requestType=" + idSolicitud;
                var httpClient = getHttpClient();
                string? token = HttpContext.Session.GetString("token");

                List<NitEmpresa> datos = new List<NitEmpresa>();
                if (token == null)
                {
                    HttpContext.Session.Remove("token");
                    return datos;
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.GetAsync(URI).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        string jsonInput = responseString;
                        Responses? respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();
                        datos = JsonConvert.DeserializeObject<List<NitEmpresa>>(respuesta.Response.ToString() ?? "") ?? new List<NitEmpresa>();
                        HttpContext.Session.SetString("token", respuesta.Token);

                        return datos;
                    }

                    return datos;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<NitEmpresa> datos = new List<NitEmpresa>();
                return datos;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idSolicitud"></param>
        /// <returns></returns>
        [HttpPost]
        public List<ColorPrecinto> ConsultarColorPrecinto(int idSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/NationalSealsManagement/ConsultSealColor?requestType=" + idSolicitud;
                var httpClient = getHttpClient();
                string? token = HttpContext.Session.GetString("token");

                List<ColorPrecinto> datos = new List<ColorPrecinto>();
                if (token == null)
                {
                    HttpContext.Session.Remove("token");
                    return datos;
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.GetAsync(URI).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        string jsonInput = responseString;
                        Responses? respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();
                        datos = JsonConvert.DeserializeObject<List<ColorPrecinto>>(respuesta.Response.ToString() ?? "") ?? new List<ColorPrecinto>();
                        HttpContext.Session.SetString("token", respuesta.Token);

                        return datos;
                    }

                    return datos;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<ColorPrecinto> datos = new List<ColorPrecinto>();
                return datos;
            }
        }

        private InformacionSolicitud ObtenerInfoSolicitud(decimal codigoSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                var informacionSolicitud = new InformacionSolicitud();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = $"{UrlApi}/CoordinatorAssignRequestAnalystGPN/ConsultRequerimentUnit?requestCode={codigoSolicitud}";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = httpClient.GetAsync(URI).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                    if (!String.IsNullOrEmpty(respuesta.Token))
                    {
                        HttpContext.Session.SetString("token", respuesta.Token);
                        informacionSolicitud = JsonConvert.DeserializeObject<InformacionSolicitud>(respuesta.Response.ToString() ?? "") ?? new InformacionSolicitud();
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", "");
                    }
                }

                return informacionSolicitud;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var informacionSolicitud = new InformacionSolicitud();
                return informacionSolicitud;
            }
        }
        /// <summary>
        /// obtener tipos de permisos
        /// </summary>
        /// <returns></returns>
        public object GetEspecimen(decimal code)
        {
            try
            {
                _logger.LogInformation("method called");


                List<ElementEspecimen>? unidadesMedida = new List<ElementEspecimen>();
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/NationalSealsManagement/GetEspecimen?code=" + code;
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
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                    if (respuesta != null)
                    {
                        unidadesMedida = JsonConvert.DeserializeObject<List<ElementEspecimen>>(respuesta.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }
                return unidadesMedida ?? new object { };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }
    }
}