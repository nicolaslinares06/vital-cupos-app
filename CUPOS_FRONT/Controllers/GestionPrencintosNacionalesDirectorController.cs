using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.Helpers;
using Repository.Helpers.Models;
using Repository.Models;
using System.Net.Http.Headers;
using Web.Models;

namespace Web.Controllers
{
    public class GestionPrencintosNacionalesDirectorController : Controller
    {
        private readonly ILogger<GestionPrencintosNacionalesDirectorController> _logger;
        private readonly IConfiguration configuration;
        private readonly string UrlApi;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";

        public GestionPrencintosNacionalesDirectorController(ILogger<GestionPrencintosNacionalesDirectorController> logger, IConfiguration configuration)
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
        [HttpPost]
        public List<CuposV002GestionPrecintosNacionales> ConsultarPendiente()
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/NationalSealsManagement/ConsultPendingDirector";
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
                        Responses respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();
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
        public List<CuposV002GestionPrecintosNacionales> ConsultarFirmada()
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/NationalSealsManagement/ConsulSignedDirector";
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
                        Responses respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();
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
                string URI = UrlApi + "/NationalSealsManagement/ConsultApprovedDirector";
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
                        Responses respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();
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
                string URI = UrlApi + "/NationalSealsManagement/ConsultWithdrawalDirector";
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
                        Responses respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();
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
        public IActionResult Index()
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/GestionPrencintosNacionalesDirector/Index.cshtml");

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return View("Views/GestionPrencintosNacionalesDirector/Index.cshtml");
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
                    return View("Views/GestionPrencintosNacionalesDirector/Partials/Ayuda.cshtml");

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return View("Views/GestionPrencintosNacionalesDirector/Index.cshtml");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult VisualizarCartaSolicitud(int idSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token != null)
                {
                    ViewBag.idSolicitud = idSolicitud;
                    return View("Views/GestionPrencintosNacionalesDirector/Pendientes/CartaSolicitantePendiente.cshtml");
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return View("Views/GestionPrencintosNacionalesDirector/Index.cshtml");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult VisualizarCartaFirmadaSolicitud(int idSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token != null)
                {
                    ViewBag.idSolicitud = idSolicitud;
                    return View("Views/GestionPrencintosNacionalesDirector/Firmadas/CartaFirmadaSolicitud.cshtml");
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return View("Views/GestionPrencintosNacionalesDirector/Index.cshtml");
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
                    return View("Views/GestionPrencintosNacionalesDirector/SolicitudesAprobadas/SolicitudRadicada.cshtml", model);
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return View("Views/GestionPrencintosNacionalesDirector/Index.cshtml");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult FirmarCartaSolicitud(int code)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                {
                    return RedirectToAction("Index", "Login");
                }

                var requestDetail = DetalleSolicitud(code);
                List<RequestCutting> RequestCutting = new List<RequestCutting>();

                var consulta = ConsultarColorPrecinto(code).FirstOrDefault();
                if (consulta != null)
                {
                    var color = consulta.a008valor;
                    ViewBag.color = color;
                }

                var codigos = ConsultarCodigosPrecintos(code).FirstOrDefault();

                var typeRequest = requestDetail.FirstOrDefault();
                if (typeRequest != null && typeRequest.TYPEREQUESTCODE == 20207)
                {
                    List<valCutMail> valCuting = ConsultarCodigosMarquillas(code);

                    foreach (var item in requestDetail)
                    {
                        RequestCutting d = new RequestCutting();
                        d.ID = item.ID;
                        d.ANALISTA = item.ANALISTA;
                        d.FECHA = item.FECHA;
                        d.FECHARADICACION = item.FECHARADICACION;
                        d.NUMERORADICACION = item.NUMERORADICACION;
                        d.VALORCONSIGNACION = item.VALORCONSIGNACION;
                        d.TYPEREQUESTCODE = item.TYPEREQUESTCODE;
                        d.CANTIDAD = item.CANTIDAD;
                        d.CODIGOCORTEPIELSOLICITUD = item.CODIGOCORTEPIELSOLICITUD;
                        d.ESTABLECIMIENTO = item.ESTABLECIMIENTO;
                        d.TELEFONO = item.TELEFONO;
                        d.TIPOSOLICITUD = item.TIPOSOLICITUD;
                        d.CIUDAD = item.CIUDAD;
                        d.CIUDADENTREGA = item.CIUDADENTREGA;
                        d.DIRECCIONENTREGA = item.DIRECCIONENTREGA;
                        Console.WriteLine("Ciudad" + item.CIUDADENTREGA);
                        d.NITEMPRESA = item.NITEMPRESA;

                        var ValCuting = (valCuting.Where(p => p.cuttingCode == item.CODIGOCORTEPIELSOLICITUD)).FirstOrDefault();
                        if (ValCuting != null)
                        {
                            d.areaCut = ValCuting.areaCut;
                            d.cantCut = ValCuting.cantCut;
                            d.tipoPart = ValCuting.tipoPart;
                            d.safeGuard = ValCuting.safeGuard;
                        }
                        else
                        {
                            d.areaCut = 0;
                            d.cantCut = 0;
                            d.tipoPart = "";
                            d.safeGuard = "";
                        }

                        RequestCutting.Add(d);
                    }
                }
                else if (typeRequest != null && typeRequest.TYPEREQUESTCODE == 60240)
                {

                    foreach (var item in requestDetail)
                    {
                        RequestCutting d = new RequestCutting();
                        d.ID = item.ID;
                        d.ANALISTA = item.ANALISTA;
                        d.FECHA = item.FECHA;
                        d.FECHARADICACION = item.FECHARADICACION;
                        d.NUMERORADICACION = item.NUMERORADICACION;
                        d.VALORCONSIGNACION = item.VALORCONSIGNACION;
                        d.TYPEREQUESTCODE = item.TYPEREQUESTCODE;
                        d.CANTIDAD = item.CANTIDAD;
                        d.CODIGOCORTEPIELSOLICITUD = item.CODIGOCORTEPIELSOLICITUD;
                        d.ESTABLECIMIENTO = item.ESTABLECIMIENTO;
                        d.TELEFONO = item.TELEFONO;
                        d.TIPOSOLICITUD = item.TIPOSOLICITUD;
                        d.CIUDAD = item.CIUDAD;
                        d.CIUDADENTREGA = item.CIUDADENTREGA;
                        Console.WriteLine("Ciudad" + item.CIUDADENTREGA);
                        d.DIRECCIONENTREGA = item.DIRECCIONENTREGA;
                        d.NITEMPRESA = item.NITEMPRESA;
                        RequestCutting.Add(d);
                    }
                }

                if (codigos != null)
                {
                    ViewBag.numeracionInicialPrecintos = codigos.A006numeroInicial;
                    ViewBag.numeracionFinalPrecintos = codigos.A006numeroFinal;
                    ViewBag.numeracionCantidaPrecintos = codigos.A019Cantidad;
                }

                if (typeRequest != null)
                    ViewBag.requestType = typeRequest.TYPEREQUESTCODE;
                ViewBag.RequestCutting = RequestCutting;
                ViewBag.idSolicitud = code;
                return View("Views/GestionPrencintosNacionalesDirector/Pendientes/FirmarSolicitantePendiente.cshtml");


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return View("Views/GestionPrencintosNacionalesDirector/Index.cshtml");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string FirmarCartaSolicitud(SignApplicationDocument datos)
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

                string URI = UrlApi + "/NationalSealsManagement/SignApplicationDocument";
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
        public List<RequestCutting> FirmarCartaSolicitudMin(int code)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                List<RequestCutting> RequestCutting = new List<RequestCutting>();
                if (token == null)
                {
                    HttpContext.Session.Remove("token");
                    return RequestCutting;
                }

                var requestDetail = DetalleSolicitud(code);
                var request = requestDetail.FirstOrDefault();

                if (request != null && request.TYPEREQUESTCODE == 20207)
                {
                    List<valCutMail> valCuting = ConsultarCodigosMarquillas(code);

                    foreach (var item in requestDetail)
                    {
                        RequestCutting d = new RequestCutting();
                        d.ID = item.ID;
                        d.ANALISTA = item.ANALISTA;
                        d.FECHA = item.FECHA;
                        d.FECHARADICACION = item.FECHARADICACION;
                        d.NUMERORADICACION = item.NUMERORADICACION;
                        d.VALORCONSIGNACION = item.VALORCONSIGNACION;
                        d.TYPEREQUESTCODE = item.TYPEREQUESTCODE;
                        d.CANTIDAD = item.CANTIDAD;
                        d.CODIGOCORTEPIELSOLICITUD = item.CODIGOCORTEPIELSOLICITUD;
                        d.ESTABLECIMIENTO = item.ESTABLECIMIENTO;
                        d.TELEFONO = item.TELEFONO;
                        d.TIPOSOLICITUD = item.TIPOSOLICITUD;
                        d.CIUDAD = item.CIUDAD;
                        d.DIRECCIONENTREGA = item.DIRECCIONENTREGA;
                        d.NITEMPRESA = item.NITEMPRESA;

                        var ValCuting = (valCuting.Where(p => p.cuttingCode == item.CODIGOCORTEPIELSOLICITUD)).FirstOrDefault();
                        if (ValCuting != null)
                        {
                            d.areaCut = ValCuting.areaCut;
                            d.cantCut = ValCuting.cantCut;
                            d.tipoPart = ValCuting.tipoPart;
                            d.safeGuard = ValCuting.safeGuard;
                        }
                        else
                        {
                            d.areaCut = 0;
                            d.cantCut = 0;
                            d.tipoPart = "";
                            d.safeGuard = "";
                        }

                        RequestCutting.Add(d);
                    }
                }
                else if (request != null && request.TYPEREQUESTCODE == 60240)
                {

                    foreach (var item in requestDetail)
                    {
                        RequestCutting d = new RequestCutting();
                        d.ID = item.ID;
                        d.ANALISTA = item.ANALISTA;
                        d.FECHA = item.FECHA;
                        d.FECHARADICACION = item.FECHARADICACION;
                        d.NUMERORADICACION = item.NUMERORADICACION;
                        d.VALORCONSIGNACION = item.VALORCONSIGNACION;
                        d.TYPEREQUESTCODE = item.TYPEREQUESTCODE;
                        d.CANTIDAD = item.CANTIDAD;
                        d.CODIGOCORTEPIELSOLICITUD = item.CODIGOCORTEPIELSOLICITUD;
                        d.ESTABLECIMIENTO = item.ESTABLECIMIENTO;
                        d.TELEFONO = item.TELEFONO;
                        d.TIPOSOLICITUD = item.TIPOSOLICITUD;
                        d.CIUDAD = item.CIUDAD;
                        d.DIRECCIONENTREGA = item.DIRECCIONENTREGA;
                        d.NITEMPRESA = item.NITEMPRESA;
                        RequestCutting.Add(d);
                    }
                }

                return RequestCutting;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<RequestCutting> RequestCutting = new List<RequestCutting>();
                return RequestCutting;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nit"></param>
        /// <returns></returns>
        public List<valCutMail> ConsultarCodigosMarquillas(int code)
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/NationalSealsManagement/CheckCodesSealsMin?code=" + code;
                var httpClient = getHttpClient();
                string? token = HttpContext.Session.GetString("token");

                List<valCutMail> datos = new List<valCutMail>();

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
                        datos = JsonConvert.DeserializeObject<List<valCutMail>>(respuesta.Response.ToString() ?? "") ?? new List<valCutMail>();
                        HttpContext.Session.SetString("token", respuesta.Token);

                        return datos;
                    }

                    return datos;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<valCutMail> datos = new List<valCutMail>();
                return datos;
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
                    return View("Views/GestionPrencintosNacionalesDirector/Desistimiento/SolicitudRadicada.cshtml", model);
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return RedirectToAction("FlujoNegocio", "Home");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult CartaFirmadaSolicitud(int code)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token != null)
                {
                    ViewBag.idSolicitud = code;
                    return View("Views/GestionPrencintosNacionalesDirector/Pendientes/CartaFirmadaSolicitud.cshtml");
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return RedirectToAction("FlujoNegocio", "Home");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GuardarSolicitudPrecintos(SaveRequestSeals datos)
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
                    string URI = UrlApi + "/NationalSealsManagement/SaveRequestSeals";
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
        public async Task<JsonResult> EnviarCorreoAprobacionSolicitud(List<MailApproval> datos)
        {
            try
            {
                _logger.LogInformation("method called");
                var mensaje = "El correo para generar precintos ha sido enviado";
                var error = false;

                var correoEnviado = await EnviarCorreoaAprobado(datos);
                if (correoEnviado != "exito")
                {
                    error = true;
                    mensaje = "Ha ocurrido un error al enviar el correo.";

                }
                return new JsonResult(new { Error = error, Mensaje = mensaje });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new JsonResult(new { Error = true, Mensaje = ex.Message });

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task<string> EnviarCorreoaAprobado(List<MailApproval> datos)
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
                    string URI = UrlApi + "/NationalSealsManagement/SendEmailtoApproved";
                    var httpClient = getHttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await httpClient.PostAsJsonAsync(URI, datos);

                    string responseString = await response.Content.ReadAsStringAsync();
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
    }

}
