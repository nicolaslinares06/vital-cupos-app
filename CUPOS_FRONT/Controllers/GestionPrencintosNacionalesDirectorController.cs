using iTextSharp.text.pdf;
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
        public async Task<List<CuposV002GestionPrecintosNacionales>> ConsultarPendiente()
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/NationalSealsManagement/ConsultPendingDirector";
                return await ObtenerDataDeApi<List<CuposV002GestionPrecintosNacionales>>(URI);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<CuposV002GestionPrecintosNacionales>();               
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async  Task<List<CuposV002GestionPrecintosNacionales>> ConsultarFirmada()
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/NationalSealsManagement/ConsulSignedDirector";
                return await ObtenerDataDeApi<List<CuposV002GestionPrecintosNacionales>>(URI);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<CuposV002GestionPrecintosNacionales>(); 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<CuposV002GestionPrecintosNacionales>> ConsultarAprobada()
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/NationalSealsManagement/ConsultApprovedDirector";
                return await ObtenerDataDeApi<List<CuposV002GestionPrecintosNacionales>>(URI);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<CuposV002GestionPrecintosNacionales>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<CuposV002GestionPrecintosNacionales>> ConsultarDesistimiento()
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/NationalSealsManagement/ConsultWithdrawalDirector";
                return await ObtenerDataDeApi<List<CuposV002GestionPrecintosNacionales>>(URI);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<CuposV002GestionPrecintosNacionales>();
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

                return ProcesarView("Views/GestionPrencintosNacionalesDirector/Index.cshtml");
            }
            catch (Exception ex)
            {
                return ManejarErrorEnVista(ex);
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
                return ProcesarView("Views/GestionPrencintosNacionalesDirector/Partials/Ayuda.cshtml");
            }
            catch (Exception ex)
            {
               
                return ManejarErrorEnVista(ex);
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
                ViewBag.idSolicitud = idSolicitud;    
                return ProcesarView("Views/GestionPrencintosNacionalesDirector/Pendientes/CartaSolicitantePendiente.cshtml");
            }
            catch (Exception ex)
            {
                return ManejarErrorEnVista(ex);
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
                ViewBag.idSolicitud = idSolicitud;
                return ProcesarView("Views/GestionPrencintosNacionalesDirector/Firmadas/CartaFirmadaSolicitud.cshtml");
            }
            catch (Exception ex)
            {
                return ManejarErrorEnVista(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> SolicitudRadicadaAprobada(int idSolicitud)
        {
            try
            {

                ViewBag.idSolicitud = idSolicitud;
                return await ProcesarViewInformacion(idSolicitud, "Views/GestionPrencintosNacionalesDirector/SolicitudesAprobadas/SolicitudRadicada.cshtml");
        
            }
            catch (Exception ex)
            {
                return ManejarErrorEnVista(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> FirmarCartaSolicitud(int code)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                {
                    return RedirectToAction("Index", "Login");
                }

                var requestDetail = await DetalleSolicitud(code);
                List<RequestCutting> RequestCutting = new List<RequestCutting>();

                var consulta =  await ConsultarColorPrecinto(code);
                if (consulta != null)
                {
                    var consultaUnidad = consulta.FirstOrDefault();
                    var color = consultaUnidad.a008valor;
                    ViewBag.color = color;
                }

                var codigos = await ConsultarCodigosPrecintos(code);

                var typeRequest = requestDetail.FirstOrDefault();
                if (typeRequest != null && typeRequest.TYPEREQUESTCODE == 20207)
                {
                    List<valCutMail> valCuting = await ConsultarCodigosMarquillas(code);

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
                    var codigo = codigos.FirstOrDefault();
                    ViewBag.numeracionInicialPrecintos = codigo.A006numeroInicial;
                    ViewBag.numeracionFinalPrecintos = codigo.A006numeroFinal;
                    ViewBag.numeracionCantidaPrecintos = codigo.A019Cantidad;
                }

                if (typeRequest != null)
                    ViewBag.requestType = typeRequest.TYPEREQUESTCODE;
                ViewBag.RequestCutting = RequestCutting;
                ViewBag.idSolicitud = code;
                return View("Views/GestionPrencintosNacionalesDirector/Pendientes/FirmarSolicitantePendiente.cshtml");


            }
            catch (Exception ex)
            {
                return ManejarErrorEnVista(ex);
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
        public async Task<List<RequestCutting>> FirmarCartaSolicitudMin(int code)
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

                var requestDetail = await DetalleSolicitud(code);
                var request = requestDetail.FirstOrDefault();

                if (request != null && request.TYPEREQUESTCODE == 20207)
                {
                    List<valCutMail> valCuting = await ConsultarCodigosMarquillas(code);

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
                return new List<RequestCutting>();
            }
        }

        /// <summary>
		/// 
		/// </summary>
		/// <param name="nit"></param>
		/// <returns></returns>
		public async Task <List<ConsultCodes>> ConsultarCodigosPrecintos(int code)
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/NationalSealsManagement/ConsultCodes?code=" + code;
                return await ObtenerDataDeApi<List<ConsultCodes>>(URI);
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");               
                return new List<ConsultCodes>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idSolicitud"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<ColorPrecinto>> ConsultarColorPrecinto(int idSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/NationalSealsManagement/ConsultSealColor?requestType=" + idSolicitud;
                return await ObtenerDataDeApi<List<ColorPrecinto>>(URI);
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");              
                return new List<ColorPrecinto>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nit"></param>
        /// <returns></returns>
        public async Task<List<valCutMail>> ConsultarCodigosMarquillas(int code)
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/NationalSealsManagement/CheckCodesSealsMin?code=" + code;
                return await ObtenerDataDeApi<List<valCutMail>>(URI);
          
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");           
                return new List<valCutMail>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<CuposV003SolicitudPrecintosNacionales>> DetalleSolicitud(int id)
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/NationalSealsManagement/ConsultDetail?id=" + id;
                return await ObtenerDataDeApi<List<CuposV003SolicitudPrecintosNacionales>>(URI);
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");         
                return new List<CuposV003SolicitudPrecintosNacionales>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> SolicitudRadicadaDesistimiento(int idSolicitud)
        {
            try
            {
                ViewBag.idSolicitud = idSolicitud;
                return await ProcesarViewInformacion(idSolicitud, "Views/GestionPrencintosNacionalesDirector/Desistimiento/SolicitudRadicada.cshtml");               
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
                return ProcesarView("Views/GestionPrencintosNacionalesDirector/Pendientes/CartaFirmadaSolicitud.cshtml");
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

        private async Task<InformacionSolicitud> ObtenerInfoSolicitud(decimal codigoSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");  
                string URI = $"{UrlApi}/CoordinatorAssignRequestAnalystGPN/ConsultRequerimentUnit?requestCode={codigoSolicitud}";
                return await ObtenerDataDeApi<InformacionSolicitud>(URI);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");                
                return new InformacionSolicitud();
            }
        }


        private async Task<T> ObtenerDataDeApi<T>(string URI) where T : new()
        {
            string? token = HttpContext.Session.GetString("token");
            if (token == null)
            {
                HttpContext.Session.Remove("token");
                return new T();
            }

            var httpClient = getHttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync(URI);
            if (!response.IsSuccessStatusCode)
            {
                return new T();
            }

            string responseString = await response.Content.ReadAsStringAsync();
            var respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
            HttpContext.Session.SetString("token", respuesta.Token);

            return JsonConvert.DeserializeObject<T>(respuesta.Response.ToString() ?? "") ?? new T();
        }

        private IActionResult ProcesarView(string rutaView)
        {
            _logger.LogInformation("method called");
            string? token = HttpContext.Session.GetString("token");

            if (token != null)
                return View(rutaView);

            return View();
        }
        private IActionResult ManejarErrorEnVista(Exception ex)
        {
            _logger.LogError(ex, "An error occurred in the method.");
            return View("Views/GestionPrencintosNacionalesDirector/Index.cshtml");
        }

        private async  Task<IActionResult> ProcesarViewInformacion(decimal idSolicitud, string rutaView)
        {

            var parametricasTipoSolicitudJson = configuration.GetSection("ParametricasTipoSolicitud").Get<List<ParametricaTipoSolicitud>>();
            if (parametricasTipoSolicitudJson is null)
                parametricasTipoSolicitudJson = new List<ParametricaTipoSolicitud>();
            _logger.LogInformation("method called");
            string? token = HttpContext.Session.GetString("token");

            if (token != null)
            {
                var model = await ObtenerInfoSolicitud(idSolicitud);
                model.ListaTiposSolicitud = parametricasTipoSolicitudJson;
                return View(rutaView , model);
            }

            return View();

        }
    }

}
