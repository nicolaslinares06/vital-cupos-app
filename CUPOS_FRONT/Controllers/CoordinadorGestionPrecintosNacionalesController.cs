using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.Helpers;
using System.Net.Http.Headers;
using Web.Models;
using System.Text.Json;
using static Web.Models.TrayForNationalSealsExternUsers;

namespace Web.Controllers
{
    public class CoordinadorGestionPrecintosNacionalesController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<CoordinadorGestionPrecintosNacionalesController> _logger;
        private readonly string rutaAPI;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public CoordinadorGestionPrecintosNacionalesController(IConfiguration configuration, ILogger<CoordinadorGestionPrecintosNacionalesController> logger)
        {
            rutaAPI = string.IsNullOrEmpty(RUTAAPI) ? new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetValue<string>("Variables:RutaApi") : RUTAAPI;
            this.configuration = configuration;
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
                return RedirectToAction("Index", "Login");
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

                string? token = HttpContext.Session.GetString("token");

                if (token != null)
                    return View("Views/CoordinadorGestionPrecintosNacionales/Partials/Ayuda.cshtml");

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return RedirectToAction("Index", "Login");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigoSolicitud"></param>
        /// <returns></returns>
        public async Task<IActionResult> CoordinadorGPNAsignacion(decimal codigoSolicitud)
        {
            try
            {
                string? token = HttpContext.Session.GetString("token") ?? "";
                if (String.IsNullOrEmpty(token))
                    return RedirectToAction("Index", "Login");

                var parametricasTipoSolicitudJson = configuration.GetSection("ParametricasTipoSolicitud").Get<List<ParametricaTipoSolicitud>>();
                if (parametricasTipoSolicitudJson is null)
                    parametricasTipoSolicitudJson = new List<ParametricaTipoSolicitud>();

                _logger.LogInformation("method called");
                var model = new CoordinadorAsignacionViewModel();
                model.InfoSolicitud = ObtenerInfoSolicitud(codigoSolicitud);
                model.ListadoAnalistas = await ObtenerAnalistas(codigoSolicitud);
                model.InfoSolicitud.IdSolicitud = codigoSolicitud;
                model.InfoSolicitud.archivoConsignacion = ObtenerArchivoSolicitud(codigoSolicitud, 10109);
                model.InfoSolicitud.ArchivoCartaVenta = ObtenerAnexosSolicitud(codigoSolicitud, 10110);
                model.InfoSolicitud.ArchivosRespuesta = ObtenerAnexosSolicitud(codigoSolicitud, 10169);
                model.InfoSolicitud.ListaTiposSolicitud = parametricasTipoSolicitudJson;
                model.InfoSolicitud.FraccionesSolicitud = obtenerTiposFraccionesSolicitud(codigoSolicitud);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string token = HttpContext.Session.GetString("token") ?? "";

                if (String.IsNullOrEmpty(token))
                    return RedirectToAction("Index", "CoordinadorGestionPrecintosNacionales");
                else
                    return RedirectToAction("Index", "Login");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigoSolicitud"></param>
        /// <returns></returns>
        public async Task<IActionResult> CoordinadorGPNAprobacion(decimal codigoSolicitud)
        {
            try
            {
                var parametricasTipoSolicitudJson = configuration.GetSection("ParametricasTipoSolicitud").Get<List<ParametricaTipoSolicitud>>();
                if (parametricasTipoSolicitudJson is null)
                    parametricasTipoSolicitudJson = new List<ParametricaTipoSolicitud>();

                _logger.LogInformation("method called");
                var model = new ActualizacionAprobacionViewModel();
                model.IdSolicitud = codigoSolicitud;
                model.ArchivoAprobacionAnalista = ObtenerArchivoSolicitud(codigoSolicitud, 10170);
                model.AnlistaAsignado = await ObtenerAnalistaSolicitud(codigoSolicitud);
                model.InfoSolicitud = ObtenerInfoSolicitud(codigoSolicitud);
                model.InfoSolicitud.archivoConsignacion = ObtenerArchivoSolicitud(codigoSolicitud, 10109);
                model.InfoSolicitud.ArchivoCartaVenta = ObtenerAnexosSolicitud(codigoSolicitud, 10110);
                model.InfoSolicitud.ArchivosRespuesta = ObtenerAnexosSolicitud(codigoSolicitud, 10169);
                model.InfoSolicitud.ListaTiposSolicitud = parametricasTipoSolicitudJson;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return RedirectToAction("Index", "CoordinadorGestionPrecintosNacionales");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigoSolicitud"></param>
        /// <returns></returns>
        public IActionResult CoordinadorGPNSolicitudAprobada(decimal codigoSolicitud)
        {
            try
            {
                var parametricasTipoSolicitudJson = configuration.GetSection("ParametricasTipoSolicitud").Get<List<ParametricaTipoSolicitud>>();
                if (parametricasTipoSolicitudJson is null)
                    parametricasTipoSolicitudJson = new List<ParametricaTipoSolicitud>();
                _logger.LogInformation("method called");
                var model = new CoordinadorSolicitudAprobada();
                model.InfoSolicitud = ObtenerInfoSolicitud(codigoSolicitud);
                model.InfoSolicitud.IdSolicitud = codigoSolicitud;
                model.ArchivoAprobacionAnalista = ObtenerArchivoSolicitud(codigoSolicitud, 10170);
                model.InfoSolicitud.archivoConsignacion = ObtenerArchivoSolicitud(codigoSolicitud, 10109);
                model.InfoSolicitud.ArchivoCartaVenta = ObtenerAnexosSolicitud(codigoSolicitud, 10110);
                model.InfoSolicitud.ArchivosRespuesta = ObtenerAnexosSolicitud(codigoSolicitud, 10169);
                model.InfoSolicitud.ListaTiposSolicitud = parametricasTipoSolicitudJson;
                model.InfoSolicitud.FraccionesSolicitud = obtenerTiposFraccionesSolicitud(codigoSolicitud);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return RedirectToAction("Index", "CoordinadorGestionPrecintosNacionales");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigoSolicitud"></param>
        /// <returns></returns>
        public async Task<IActionResult> CoordinadorGPNSolicitudDesisistido(decimal codigoSolicitud)
        {
            try
            {


                var parametricasTipoSolicitudJson = configuration.GetSection("ParametricasTipoSolicitud").Get<List<ParametricaTipoSolicitud>>();
                if (parametricasTipoSolicitudJson is null)
                    parametricasTipoSolicitudJson = new List<ParametricaTipoSolicitud>();
                _logger.LogInformation("method called");
                var model = new CoordinadorGPNDesistimientoModel();
                model.InfoSolicitud = ObtenerInfoSolicitud(codigoSolicitud);
                model.InfoSolicitud.IdSolicitud = codigoSolicitud;
                model.DatosDesistimiento = await ObtenerSolicitudDesistido(codigoSolicitud);
                model.InfoSolicitud.archivoConsignacion = ObtenerArchivoSolicitud(codigoSolicitud, 10109);
                model.InfoSolicitud.ArchivoCartaVenta = ObtenerAnexosSolicitud(codigoSolicitud, 10110);
                model.InfoSolicitud.ArchivosRespuesta = ObtenerAnexosSolicitud(codigoSolicitud, 10169);
                model.InfoSolicitud.ListaTiposSolicitud = parametricasTipoSolicitudJson;
                model.InfoSolicitud.FraccionesSolicitud = obtenerTiposFraccionesSolicitud(codigoSolicitud);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return RedirectToAction("Index", "CoordinadorGestionPrecintosNacionales");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoConsultaTabla"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ObtenerListado(int tipoConsultaTabla)
        {
            try
            {
                _logger.LogInformation("method called");
                string[] arregloTiposSolicitudes = { "ENVIADA", "PRE-APROBADO", "APROBADO", "DESISTIDO" };
                string[] accionVisualArreglo = { "CoordinadorGPNAsignacion", "CoordinadorGPNAprobacion", "CoordinadorGPNSolicitudAprobada", "CoordinadorGPNSolicitudDesisistido" };

                var listaPrecintosNacionales = await ObtenerListaSolicitudes(arregloTiposSolicitudes[tipoConsultaTabla]);
                if (listaPrecintosNacionales.Any())
                {


                    foreach (var item in listaPrecintosNacionales)
                    {
                        item.AccionVisual = accionVisualArreglo[tipoConsultaTabla];
                    }
                }

                return new JsonResult(new { listado = listaPrecintosNacionales });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var listaPrecintosNacionales = new List<CoordinadorGestionPrecintosNacionalesModel>();
                return new JsonResult(new { listado = listaPrecintosNacionales });
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigoSolicitud"></param>
        /// <param name="tipoDocumento"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDocumentoSolicitud(decimal codigoSolicitud, decimal tipoDocumento)
        {
            try
            {
                _logger.LogInformation("method called");
                var documentoSolicitud = ObtenerArchivoSolicitud(codigoSolicitud, tipoDocumento);
                return new JsonResult(new { documentoSolicitud });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var documentoSolicitud = new ArchivoSolicitud();
                return new JsonResult(new { documentoSolicitud });
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigoSolicitud"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerListaNumeracionesSolicitud(decimal codigoSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                var listaNumeraciones = ObtenerNumeracionesSolicitud(codigoSolicitud);
                return new JsonResult(new { listaNumeraciones });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var listaNumeraciones = new List<NumeracionesSolicitudCoordinador>();
                return new JsonResult(new { listaNumeraciones });
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datosActualizacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EnviarDatosAsignacionAnalista(ActualizacionAsignacionAnalista datosActualizacion)
        {
            try
            {
                _logger.LogInformation("method called");
                var error = false;

                string token = HttpContext.Session.GetString("token") ?? "";

                string URI = rutaAPI + "/CoordinatorAssignRequestAnalystGPN/UpdateIdAnalyst";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var req = new
                {
                    AnalystId = datosActualizacion.AnalistaId,
                    RequestCode = datosActualizacion.CodigoSolicitud
                };

                var response = httpClient.PutAsJsonAsync(URI, req).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                    if (String.IsNullOrEmpty(respuesta.Token))
                    {
                        HttpContext.Session.SetString("token", "");
                        return new JsonResult(new { Error = true, mensaje = "La sesion ha caducado" });

                    }

                    HttpContext.Session.SetString("token", respuesta.Token);

                    if (respuesta.Error)
                        return new JsonResult(new { Error = true, mensaje = respuesta.Message });


                }

                return new JsonResult(new { Error = error, mensaje = "La solicitud de asignacion ha sido enviada" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new JsonResult(new { Error = true, mensaje = ex.Message });
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datosActualizacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EnviarDatosAprobacionSolicitud(ActualizacionAprobacionAnalista datosActualizacion)
        {
            try
            {
                _logger.LogInformation("method called");
                var estadoSolicitud = "RADICADA";
                if (datosActualizacion.EstadoSolicitud == 2)
                    estadoSolicitud = "APROBADO PARA FIRMA";

                var error = ActualizarEstadoSolicitud(datosActualizacion.IdSolicitud, estadoSolicitud);
                var mensaje = "La solicitud ha sido aprobada con éxito";
                var esSolicitudAprobada = true;

                if (datosActualizacion.EstadoSolicitud == 1)
                {
                    mensaje = "La solicitud ha sido devuelto al estado radicado";
                    esSolicitudAprobada = false;
                }


                if (error)
                {
                    mensaje = "Ha ocurrido un error en la actualizacion del registro";
                    esSolicitudAprobada = false;
                }




                return new JsonResult(new { Error = error, Mensaje = mensaje, solicitudAprobada = esSolicitudAprobada });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new JsonResult(new { Error = true, Mensaje = ex.Message, solicitudAprobada = false });

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoConsultaTabla"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerListaPagination(decimal codeRquest)
        {
            try
            {
                var listadoPagination = ObtenerListaSolicitudesConPagination(codeRquest);
                return new JsonResult(new { listado = listadoPagination });
            }
            catch (Exception ex)
            {
                var listadoPagination = new List<CoordinadorGestionPrecintosNacionalesModel>();
                _logger.LogError(ex, "An error occurred in the method.");
                return new JsonResult(new { listado = listadoPagination });
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoSolicitud"></param>
        /// <returns></returns>
        private async Task<List<CoordinadorGestionPrecintosNacionalesModel>> ObtenerListaSolicitudes(string tipoSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                var listaSolicitudes = new List<CoordinadorGestionPrecintosNacionalesModel>();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = $"{rutaAPI}/CoordinatorAssignRequestAnalystGPN/ConsultRequerimentsSeals?requestStatusType={tipoSolicitud}";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.GetAsync(URI);

                if (!response.IsSuccessStatusCode)
                {
                    HttpContext.Session.SetString("token", "");
                    return listaSolicitudes;
                }

                string responseString = await response.Content.ReadAsStringAsync();
                Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                if (!String.IsNullOrEmpty(respuesta.Token))
                {
                    HttpContext.Session.SetString("token", respuesta.Token);
                    listaSolicitudes = JsonConvert.DeserializeObject<List<CoordinadorGestionPrecintosNacionalesModel>?>(respuesta.Response.ToString() ?? "") ?? new List<CoordinadorGestionPrecintosNacionalesModel>();

                    if (listaSolicitudes.Any())
                    {
                        ModificarFechas(listaSolicitudes);
                    }
                }

                return listaSolicitudes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var listaSolicitudes = new List<CoordinadorGestionPrecintosNacionalesModel>();
                return listaSolicitudes;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoSolicitud"></param>
        /// <returns></returns>
        private List<CoordinadorGestionPrecintosNacionalesModel> ObtenerListaSolicitudesConPagination(decimal tipoSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                var listaSolicitudes = new List<CoordinadorGestionPrecintosNacionalesModel>();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = $"{rutaAPI}/CoordinatorAssignRequestAnalystGPN/ConsultRequerimentsSeals?requestStatusType={tipoSolicitud}";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = httpClient.GetAsync(URI).Result;

                if (!response.IsSuccessStatusCode)
                {
                    return listaSolicitudes;
                }

                string responseString = response.Content.ReadAsStringAsync().Result;
                Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                if (!String.IsNullOrEmpty(respuesta.Token))
                {
                    HttpContext.Session.SetString("token", respuesta.Token);
                    listaSolicitudes = JsonConvert.DeserializeObject<List<CoordinadorGestionPrecintosNacionalesModel>?>(respuesta.Response.ToString() ?? "") ?? new List<CoordinadorGestionPrecintosNacionalesModel>();

                    if (listaSolicitudes.Any())
                    {
                        ModificarFechas(listaSolicitudes);
                    }
                }

                return listaSolicitudes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var listaSolicitudes = new List<CoordinadorGestionPrecintosNacionalesModel>();
                return listaSolicitudes;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigoSolicitud"></param>
        /// <returns></returns>
        private async Task<List<AnalistaAsignacion>> ObtenerAnalistas(decimal codigoSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                var listaAnalistas = new List<AnalistaAsignacion>();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = $"{rutaAPI}/CoordinatorAssignRequestAnalystGPN/ConsultUsersAnalists?requestCode={codigoSolicitud}";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.GetAsync(URI);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                    if (!String.IsNullOrEmpty(respuesta.Token))
                    {
                        HttpContext.Session.SetString("token", respuesta.Token);
                        listaAnalistas = JsonConvert.DeserializeObject<List<AnalistaAsignacion>>(respuesta.Response.ToString() ?? "");
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", "");
                    }
                }

                if (listaAnalistas is null)
                    listaAnalistas = new List<AnalistaAsignacion>();

                return listaAnalistas;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var listaAnalistas = new List<AnalistaAsignacion>();
                return listaAnalistas;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigoSolicitud"></param>
        /// <returns></returns>
        private async Task<AnalistaSolicitud> ObtenerAnalistaSolicitud(decimal codigoSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                var analistaSolicitud = new AnalistaSolicitud();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = $"{rutaAPI}/CoordinatorAssignRequestAnalystGPN/ConsultUserAnalistRequeriment?requestCode={codigoSolicitud}";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.GetAsync(URI);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                    if (!String.IsNullOrEmpty(respuesta.Token))
                    {
                        HttpContext.Session.SetString("token", respuesta.Token);
                        analistaSolicitud = JsonConvert.DeserializeObject<AnalistaSolicitud>(respuesta.Response.ToString() ?? "");
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", "");
                    }
                }

                if (analistaSolicitud is null)
                    analistaSolicitud = new AnalistaSolicitud();

                return analistaSolicitud;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var analistaSolicitud = new AnalistaSolicitud();
                return analistaSolicitud;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigoSolicitud"></param>
        /// <returns></returns>
        private async Task<CoordinadorSolicitudDesistida> ObtenerSolicitudDesistido(decimal codigoSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                var solicitudDesistido = new CoordinadorSolicitudDesistida();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = $"{rutaAPI}/CoordinatorAssignRequestAnalystGPN/ConsultRequerimenWithdrawal?requestCode={codigoSolicitud}";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.GetAsync(URI);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                    if (!String.IsNullOrEmpty(respuesta.Token))
                    {
                        HttpContext.Session.SetString("token", respuesta.Token);
                        if (respuesta.Response != null)
                            solicitudDesistido = JsonConvert.DeserializeObject<CoordinadorSolicitudDesistida>(respuesta.Response.ToString() ?? "");
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", "");
                    }
                }
                if (solicitudDesistido is null)
                    solicitudDesistido = new CoordinadorSolicitudDesistida();

                return solicitudDesistido;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var solicitudDesistido = new CoordinadorSolicitudDesistida();
                return solicitudDesistido;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigoSolicitud"></param>
        /// <returns></returns>
        private InformacionSolicitud ObtenerInfoSolicitud(decimal codigoSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                var informacionSolicitud = new InformacionSolicitud();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = $"{rutaAPI}/CoordinatorAssignRequestAnalystGPN/ConsultRequerimentUnit?requestCode={codigoSolicitud}";
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
                        if (respuesta.Response != null)
                            informacionSolicitud = JsonConvert.DeserializeObject<InformacionSolicitud>(respuesta.Response.ToString() ?? "");

                    }
                    else
                    {
                        HttpContext.Session.SetString("token", "");
                    }
                }
                if (informacionSolicitud is null)
                    informacionSolicitud = new InformacionSolicitud();

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
        /// 
        /// </summary>
        /// <param name="codigoSolicitud"></param>
        /// <param name="tipoDocumento"></param>
        /// <returns></returns>
        private ArchivoSolicitud ObtenerArchivoSolicitud(decimal codigoSolicitud, decimal tipoDocumento)
        {
            try
            {
                _logger.LogInformation("method called");
                var archivo = new ArchivoSolicitud();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = $"{rutaAPI}/CoordinatorAssignRequestAnalystGPN/ConsultRequerimentFile?requestCode={codigoSolicitud}&documentType={tipoDocumento}";
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
                        archivo = JsonConvert.DeserializeObject<ArchivoSolicitud>(respuesta.Response.ToString() ?? "");
                    }
                }

                if (archivo is null)
                    archivo = new ArchivoSolicitud();

                return archivo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var archivo = new ArchivoSolicitud();
                return archivo;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigoSolicitud"></param>
        /// <param name="tipoDocumento"></param>
        /// <returns></returns>
        private List<ArchivoSolicitud> ObtenerAnexosSolicitud(decimal codigoSolicitud, decimal tipoDocumento)
        {
            try
            {
                _logger.LogInformation("method called");
                var archivos = new List<ArchivoSolicitud>();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = $"{rutaAPI}/CoordinatorAssignRequestAnalystGPN/ConsultFiles?requestCode={codigoSolicitud}&documentType={tipoDocumento}";
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
                        archivos = JsonConvert.DeserializeObject<List<ArchivoSolicitud>>(respuesta.Response.ToString() ?? "");
                    }
                }

                if (archivos is null)
                    archivos = new List<ArchivoSolicitud>();

                return archivos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var archivos = new List<ArchivoSolicitud>();
                return archivos;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigoSolicitud"></param>
        /// <returns></returns>
        private List<NumeracionesSolicitudCoordinador> ObtenerNumeracionesSolicitud(decimal codigoSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                var listaNumeraciones = new List<NumeracionesSolicitudCoordinador>();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = $"{rutaAPI}/CoordinatorAssignRequestAnalystGPN/ConsultSealNumbers?requestCode={codigoSolicitud}";
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
                        if (respuesta.Response != null)
                            listaNumeraciones = JsonConvert.DeserializeObject<List<NumeracionesSolicitudCoordinador>>(respuesta.Response.ToString() ?? "");
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", "");
                    }
                }

                if (listaNumeraciones is null)
                    listaNumeraciones = new List<NumeracionesSolicitudCoordinador>();

                return listaNumeraciones;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var listaNumeraciones = new List<NumeracionesSolicitudCoordinador>();
                return listaNumeraciones;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigoSolicitud"></param>
        /// <returns></returns>
        private List<cuttingSaveModel> obtenerTiposFraccionesSolicitud(decimal codigoSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                var listaFracciones = new List<cuttingSaveModel>();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = $"{rutaAPI}/CoordinatorAssignRequestAnalystGPN/ConsultTypesFractions?requestCode={codigoSolicitud}";
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
                        listaFracciones = JsonConvert.DeserializeObject<List<cuttingSaveModel>>(respuesta.Response.ToString() ?? "");
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", "");
                    }
                }

                if (listaFracciones is null)
                    listaFracciones = new List<cuttingSaveModel>();

                return listaFracciones;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var listaFracciones = new List<cuttingSaveModel>();
                return listaFracciones;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigoSolicitud"></param>
        /// <param name="estadoSolicitud"></param>
        /// <returns></returns>
        private bool ActualizarEstadoSolicitud(decimal codigoSolicitud, string estadoSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                var solicitudEstadoAPI = new EstadoSolicitudCoordinador();
                solicitudEstadoAPI.RequestId = codigoSolicitud;
                solicitudEstadoAPI.RequestStatus = estadoSolicitud;
                var error = false;


                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = $"{rutaAPI}/CoordinatorAssignRequestAnalystGPN/UpdateRequerimentState";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = httpClient.PutAsJsonAsync(URI, solicitudEstadoAPI).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                    if (!String.IsNullOrEmpty(respuesta.Token))
                    {
                        HttpContext.Session.SetString("token", respuesta.Token);
                        error = respuesta.Error;
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", "");
                    }
                }

                return error;
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
        /// <param name="listaSolicitudes"></param>       
        /// <returns></returns>
        private void ModificarFechas(List<CoordinadorGestionPrecintosNacionalesModel> listaSolicitudes)
        {
            foreach (var item in listaSolicitudes)
            {
                if (!String.IsNullOrEmpty(item.FechaRadicacion))
                {
                    var fecha = item.FechaRadicacion.Split('T');
                    item.FechaRadicacion = fecha[0];
                }

                if (!String.IsNullOrEmpty(item.FechaRadicacionSalida))
                {
                    var fecha = item.FechaRadicacionSalida.Split('T');
                    item.FechaRadicacionSalida = fecha[0];
                }

            }
        }
    }

}
