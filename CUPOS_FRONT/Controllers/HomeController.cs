using CUPOS_FRONT.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.Helpers;
using Repository.Helpers.Models;
using System;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using WebFront.Models;
using static CUPOS_FRONT.Models.Requests;

namespace WebFront.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string UrlApi;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public HttpClient GetHttpClient()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            return new HttpClient(clientHandler);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public HomeController(ILogger<HomeController> logger)
        {
            UrlApi = string.IsNullOrEmpty(RUTAAPI) ? new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetValue<string>("Variables:RutaApi") : RUTAAPI;
            _logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public IActionResult Index(string usuario)
        {
            try
            {
                _logger.LogInformation("method called");
                if (usuario != null)
                {
                    HttpContext.Session.SetString("User", usuario);
                    ViewBag.usuario = usuario;
                }
                else
                {
                    ViewBag.usuario = HttpContext.Session.GetString("User");
                }
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
        public IActionResult Administracion()
        {
            try
            {
                _logger.LogInformation("method called");
                ViewBag.usuario = HttpContext.Session.GetString("User");
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
        /// <param name="validar"></param>
        /// <param name="cv"></param>
        /// <returns></returns>
        public IActionResult FlujoNegocio(int validar=0, int cv=0)
        {
            try
            {
                _logger.LogInformation("method called");
                ViewBag.validar = validar;
                ViewBag.cv = cv;
                ViewBag.usuario = HttpContext.Session.GetString("User");
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
        public IActionResult Reportes()
        {
            try
            {
                _logger.LogInformation("method called");
                ViewBag.user = HttpContext.Session.GetString("User");
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
        [AllowAnonymous]
        public IActionResult Privacy()
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
        [AllowAnonymous]
        public IActionResult MapaSitio()
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
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            try
            {
                _logger.LogInformation("method called");
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

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
        public List<string> ConsultarRol()
        {
            try
            {
                _logger.LogInformation("method called");
                List<ModulosReq> r = new();
                List<string> u = new();

                string URI = UrlApi + "/Module/ConsultRols";
                var httpClient = GetHttpClient();
                string token = HttpContext.Session.GetString("token") ?? "";

                if (token == "")
                {
                    HttpContext.Session.Remove("token");
                    return u;
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
                        r = JsonConvert.DeserializeObject<List<ModulosReq>>(respuesta.Response.ToString() ?? "") ?? new List<ModulosReq>();
                        HttpContext.Session.SetString("token", respuesta.Token);

                    }
                }

                List<string> html = new();

                //Reiniciar Etiquetas
                //Administrador
                HttpContext.Session.SetString("ConsultarRoles", "False");
                HttpContext.Session.SetString("ConsultarAsignacionRol", "False");
                HttpContext.Session.SetString("ConsultarEstados", "False");
                HttpContext.Session.SetString("ConsultarParametrica", "False");
                HttpContext.Session.SetString("ConsultarAuditoria", "False");
                HttpContext.Session.SetString("ConsultarDocumental", "False");
                HttpContext.Session.SetString("ConsultarTecnica", "False");
                HttpContext.Session.SetString("ConsultarUsuario", "False");
                HttpContext.Session.SetString("ConsultarFuncionalidades", "False");
                HttpContext.Session.SetString("ConsultarServicio", "False");
                //Flujo de Negocio
                HttpContext.Session.SetString("ResAsignacionCUPOS", "False");
                HttpContext.Session.SetString("DocumentosVenta", "False");
                HttpContext.Session.SetString("GestionarEntidad", "False");
                HttpContext.Session.SetString("HojaVidaEntidad", "False");
                HttpContext.Session.SetString("FloraNoMaderable", "False");
                HttpContext.Session.SetString("PresolucionesPecesNivelNacional", "False");
                HttpContext.Session.SetString("ResolucionPecesEntidad", "False");
                HttpContext.Session.SetString("PrecintosMarquillas", "False");
                HttpContext.Session.SetString("PrecintosNacionales", "False");
                HttpContext.Session.SetString("GestionPrecintosNacionalesAnalista", "False");
                HttpContext.Session.SetString("GestionPrecintosNacionalesDirector", "False");
                HttpContext.Session.SetString("ActaVisitaCortes", "False");
                HttpContext.Session.SetString("SolicitudesPermisosCITES", "False");
                HttpContext.Session.SetString("CoordinadorGPNSolicitudes", "False");
                //Reportes
                HttpContext.Session.SetString("consultReportes", "False");
                HttpContext.Session.SetString("consultReporteEmpresa", "False");
                HttpContext.Session.SetString("consultReportePrecintos", "False");
                HttpContext.Session.SetString("consultReportesMarquillas", "False");
                HttpContext.Session.SetString("consultReportesCupos", "False");

                int contAdmin = 0;
                int contFlujoNegocio = 0;
                int contReportes = 0;
                StringBuilder htmlAdmin = new("<li class='barnav-cell Administracion' id='BarraNavegacionAdmin'><a id='padre' onclick='Administracion()'>Administración</a><ul class='submenu'>");
                StringBuilder htmlFlujoNegocio = new("<li class='barnav-cell FlujoNegocio' id='BarraNavegacionFlujoNegocio'><a id='padre' onclick='FlujoNegocio()'>Flujo de negocio</a><ul class='submenu'>");
                StringBuilder htmlReportes = new("<li class='barnav-cell Reportes' id='BarraNavegacionReportes'><a id='padre' onclick='Reportes()'> Reportes </a><ul class='submenu'>");

                foreach (ModulosReq s in r)
                {
                    string token2 = HttpContext.Session.GetString("token") ?? "";

                    string URI2 = UrlApi + "/Rol/ConsultByRol?rol=" + (int)s.id + "&charge=" + s.name;
                    var httpClient2 = GetHttpClient();
                    httpClient2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);
                    var response = httpClient2.GetAsync(URI2).Result;

                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                    List<ReqModulos> modulos = JsonConvert.DeserializeObject<List<ReqModulos>>(respuesta.Response.ToString() ?? "") ?? new List<ReqModulos>();

                    foreach (ReqModulos a in modulos)
                    {
                        //Modulos de Administracion
                        contAdmin = ObtenerModulosAdministracion(contAdmin, htmlAdmin, a);

                        //Modulos de FLujo de Negocio
                        contFlujoNegocio = ObtenerModulosFlujoNegocio(contFlujoNegocio, htmlFlujoNegocio, a);

                        //Modulos de Reportes
                        contReportes = ObtenerModulosReportes(contReportes, htmlReportes, a);
                    }
                }

                htmlAdmin.Append("<ul></li>");
                htmlFlujoNegocio.Append("<ul></li>");
                htmlReportes.Append("<ul></li>");

                if (contAdmin == 0)
                {
                    htmlAdmin.Clear();
                }

                if (contFlujoNegocio == 0)
                {
                    htmlFlujoNegocio.Clear();
                }

                if (contReportes == 0)
                {
                    htmlReportes.Clear();
                }

                HttpContext.Session.SetInt32("contAdmin", contAdmin);
                HttpContext.Session.SetInt32("contFlujoNegocio", contFlujoNegocio);

                html.Add(htmlAdmin.ToString());
                html.Add(htmlFlujoNegocio.ToString());
                html.Add(htmlReportes.ToString());
                html.Add(contAdmin.ToString());
                html.Add(contFlujoNegocio.ToString());
                html.Add(contReportes.ToString());

                return html;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        private int ObtenerModulosReportes(int contReportes, StringBuilder htmlReportes, ReqModulos a)
        {
            if (a.moduleId == StringHelper.CodGenerarReportesInformes.ToString() && a.consult)
            {
                contReportes = ValidacionReportes(contReportes, htmlReportes, a, "<li id='ReporteAnual'><a asp-controller='' asp-action=''>Reporte anual</a></li><li id='ReporteCITES'><a asp-controller='' asp-action=''>Reporte CITES</a></li><li id='ReporteInspecciones'><a asp-controller='' asp-action=''>Reporte inspecciones</a></li>");
                HttpContext.Session.SetString("consultReportes", a.consult.ToString());
                HttpContext.Session.SetString("NombreconsultReportes", a.name);
            }

            if (a.moduleId == StringHelper.codReportesEmpresasMarcajes.ToString() && a.consult)
            {
                contReportes = ValidacionReportes(contReportes, htmlReportes, a, "<li id='liReporteEmpresasMarcaje'><a onclick='ReportesEmpresasMarcaje()'>Reporte Establecimientos</a></li>");
                HttpContext.Session.SetString("consultReporteEmpresa", a.consult.ToString());
                HttpContext.Session.SetString("NombreconsultReporteEmpresa", a.name);
            }

            if (a.moduleId == StringHelper.codReportesPrecintos.ToString() && a.consult)
            {
                contReportes = ValidacionReportes(contReportes, htmlReportes, a, "<li id='liReportePrecintos'><a onclick='ReportesPrecintos()'>Reporte Precintos</a></li>");
                HttpContext.Session.SetString("consultReportePrecintos", a.consult.ToString());
                HttpContext.Session.SetString("NombreconsultReportePrecintos", a.name);
            }

            if (a.moduleId == StringHelper.codReportesMarquillas.ToString() && a.consult)
            {
                contReportes = ValidacionReportes(contReportes, htmlReportes, a, "<li id='liReportesMarquillas'><a onclick='ReportesMarquillas()'>Reporte Marquillas</a></li>");
                HttpContext.Session.SetString("consultReportesMarquillas", a.consult.ToString());
                HttpContext.Session.SetString("NombreconsultReportesMarquillas", a.name);
            }

            if (a.moduleId == StringHelper.codReportesCupos.ToString() && a.consult)
            {
                contReportes = ValidacionReportes(contReportes, htmlReportes, a, "<li id='liReportesCupos'><a onclick='ReportesCupos()'>Reporte Cupos</a></li>");
                HttpContext.Session.SetString("consultReportesCupos", a.consult.ToString());
                HttpContext.Session.SetString("NombreconsultReportesCupos", a.name);
            }

            return contReportes;
        }

        private int ObtenerModulosFlujoNegocio(int contFlujoNegocio, StringBuilder htmlFlujoNegocio, ReqModulos a)
        {
            if (a.moduleId == StringHelper.CodResolucionAsignacionCUPOS.ToString() && a.consult)
            {
                contFlujoNegocio = ValidacionFN(contFlujoNegocio, htmlFlujoNegocio, a, "<li id=''><a onclick='resolucionCupos()'>" + a.name + "</a></li>");
                HttpContext.Session.SetString("ResAsignacionCUPOS", a.consult.ToString());
                HttpContext.Session.SetString("NombreResAsignacionCUPOS", a.name);
            }
            if (a.moduleId == StringHelper.CodRegistroDocumentosVenta.ToString() && a.consult)
            {
                contFlujoNegocio = ValidacionFN(contFlujoNegocio, htmlFlujoNegocio, a, "<li id=''><a onclick='RegistroDocumentosVenta()'>" + a.name + "</a></li>");
                HttpContext.Session.SetString("DocumentosVenta", a.consult.ToString());
                HttpContext.Session.SetString("NombreDocumentosVenta", a.name);
            }
            if (a.moduleId == StringHelper.CodGestionarEntidad.ToString() && a.consult)
            {
                contFlujoNegocio = ValidacionFN(contFlujoNegocio, htmlFlujoNegocio, a, "<li id=''><a onclick='GestionarEntidad()'>" + a.name + "</a></li>");
                HttpContext.Session.SetString("GestionarEntidad", a.consult.ToString());
                HttpContext.Session.SetString("NombreGestionarEntidad", a.name);
            }
            if (a.moduleId == StringHelper.CodHojaVidaEntidad.ToString() && a.consult)
            {
                contFlujoNegocio = ValidacionFN(contFlujoNegocio, htmlFlujoNegocio, a, "<li id=''><a onclick='hojadevida()'>" + a.name + "</a></li>");
                HttpContext.Session.SetString("HojaVidaEntidad", a.consult.ToString());
                HttpContext.Session.SetString("NombreHojaVidaEntidad", a.name);
            }
            if (a.moduleId == StringHelper.CodFloraNoMaderable.ToString() && a.consult)
            {
                contFlujoNegocio = ValidacionFN(contFlujoNegocio, htmlFlujoNegocio, a, "<li id=''><a onclick='FloraNoMaderable()'>" + a.name + "</a></li>");
                HttpContext.Session.SetString("FloraNoMaderable", a.consult.ToString());
                HttpContext.Session.SetString("NombreFloraNoMaderable", a.name);
            }
            if (a.moduleId == StringHelper.CodPresolucionPecesNivelNacional.ToString() && a.consult)
            {
                contFlujoNegocio = ValidacionFN(contFlujoNegocio, htmlFlujoNegocio, a, "<li id=''><a onclick='PresolucionesPecesNivelNacional()'>" + a.name + "</a></li>");
                HttpContext.Session.SetString("PresolucionesPecesNivelNacional", a.consult.ToString());
                HttpContext.Session.SetString("NombrePresolucionesPecesNivelNacional", a.name);
            }
            if (a.moduleId == StringHelper.CodResolucionPecesEntidad.ToString() && a.consult)
            {
                contFlujoNegocio = ValidacionFN(contFlujoNegocio, htmlFlujoNegocio, a, "<li id=''><a onclick='ResolucionPecesEntidad()'>" + a.name + "</a></li>");
                HttpContext.Session.SetString("ResolucionPecesEntidad", a.consult.ToString());
                HttpContext.Session.SetString("NombreResolucionPecesEntidad", a.name);
            }

            contFlujoNegocio = ObtenerModulosFlujoNegocio2(contFlujoNegocio, htmlFlujoNegocio, a);
            contFlujoNegocio = ObtenerModulosFlujoNegocio3(contFlujoNegocio, htmlFlujoNegocio, a);

            return contFlujoNegocio;
        }

        private int ObtenerModulosFlujoNegocio2(int contFlujoNegocio, StringBuilder htmlFlujoNegocio, ReqModulos a)
        {
            if (a.moduleId == StringHelper.CodPrecintosMarquillas.ToString() && a.consult)
            {
                contFlujoNegocio = ValidacionFN(contFlujoNegocio, htmlFlujoNegocio, a, "<li id=''><a onclick='PrecintosMarquillas()'>" + a.name + "</a></li>");
                HttpContext.Session.SetString("PrecintosMarquillas", a.consult.ToString());
                HttpContext.Session.SetString("NombrePrecintosMarquillas", a.name);
            }
            if (a.moduleId == StringHelper.CodBandejaPrecintosNacional.ToString() && a.consult)
            {
                contFlujoNegocio = ValidacionFN(contFlujoNegocio, htmlFlujoNegocio, a, "<li id=''><a onclick='BandejaSolicitudPrecintosNacionales()'>Bandeja de solicitud de precintos nacionales y marquillas - Usuario externo</a></li>");
                HttpContext.Session.SetString("PrecintosNacionales", a.consult.ToString());
                HttpContext.Session.SetString("NombrePrecintosNacionales", a.name);
            }
            if (a.moduleId == StringHelper.CodBandejaTrabajoUsuarioInterno.ToString() && a.consult)
            {
                contFlujoNegocio = ValidacionFN(contFlujoNegocio, htmlFlujoNegocio, a, "<li id=''><a asp-controller='' asp-action=''>" + a.name + "</a></li>");
                HttpContext.Session.SetString("UsuarioInterno", a.consult.ToString());
                HttpContext.Session.SetString("NombreBandejaTrabajoInterno", a.name);
            }
            if (a.moduleId == StringHelper.CodBandejaTrabajoValidacionSolicitud.ToString() && a.consult)
            {
                contFlujoNegocio = ValidacionFN(contFlujoNegocio, htmlFlujoNegocio, a, "<li id=''><a onclick='GestionPrencintosNacionalesAnalista()'>" + a.name + "</a></li>");
                HttpContext.Session.SetString("GestionPrecintosNacionalesAnalista", a.consult.ToString());
                HttpContext.Session.SetString("NombreGestionPrecintosNacionalesAnalista", a.name);
            }
            if (a.moduleId == StringHelper.CodBandejaTrabajoFirmaRespuestaSolicitante.ToString() && a.consult)
            {
                contFlujoNegocio = ValidacionFN(contFlujoNegocio, htmlFlujoNegocio, a, "<li id=''><a onclick='GestionPrencintosNacionalesDirector()'>" + a.name + "</a></li>");
                HttpContext.Session.SetString("GestionPrecintosNacionalesDirector", a.consult.ToString());
                HttpContext.Session.SetString("NombreGestionPrecintosNacionalesDirector", a.name);
            }
            if (a.moduleId == StringHelper.CodRegistroActaVisitaCortes.ToString() && a.consult)
            {
                contFlujoNegocio = ValidacionFN(contFlujoNegocio, htmlFlujoNegocio, a, "<li id=''><a onclick='RegistroActaVisitaCortes()'>" + a.name + "</a></li>");
                HttpContext.Session.SetString("ActaVisitaCortes", a.consult.ToString());
                HttpContext.Session.SetString("NombreActaVisitaCortes", a.name);
            }
            if (a.moduleId == StringHelper.CodGestionarSolicitudesPermisosCITES.ToString() && a.consult)
            {
                contFlujoNegocio = ValidacionFN(contFlujoNegocio, htmlFlujoNegocio, a, "<li id=''><a onclick='GestionarPermisosCITES()'>" + a.name + "</a></li>");
                HttpContext.Session.SetString("SolicitudesPermisosCITES", a.consult.ToString());
                HttpContext.Session.SetString("NombreSolicitudesPermisosCITES", a.name);
            }

            return contFlujoNegocio;
        }

        private int ObtenerModulosFlujoNegocio3(int contFlujoNegocio, StringBuilder htmlFlujoNegocio, ReqModulos a)
        {
            if (a.moduleId == StringHelper.CodCoordinadorGestionarPrecintos.ToString() && a.consult)
            {
                contFlujoNegocio = ValidacionFN(contFlujoNegocio, htmlFlujoNegocio, a, "<li id=''><a onclick='CoordinadorGestionPrecintosNacionales()'>" + a.name + "</a></li>");
                HttpContext.Session.SetString("CoordinadorGPNSolicitudes", a.consult.ToString());
                HttpContext.Session.SetString("NombreCoordinadorGPNSolicitudes", a.name);
            }

            return contFlujoNegocio;
        }

        private int ObtenerModulosAdministracion(int contAdmin, StringBuilder htmlAdmin, ReqModulos a)
        {
            if (a.moduleId == StringHelper.CodAdminRoles.ToString() && a.consult)
            {
                contAdmin = ValidacionAdmin(contAdmin, htmlAdmin, a, "<li id='AdministrarRoles'><a onclick='AdministrarRoles()'>" + a.name + "</a></li>");
                HttpContext.Session.SetString("ConsultarRoles", a.consult.ToString());
                HttpContext.Session.SetString("CrearRoles", a.create.ToString());
                HttpContext.Session.SetString("ActualizarRoles", a.update.ToString());
                HttpContext.Session.SetString("NombreRoles", a.name);
            }
            if (a.moduleId == StringHelper.CodGestionSolicitudesAsigRol.ToString() && a.consult)
            {
                contAdmin = ValidacionAdmin(contAdmin, htmlAdmin, a, "<li id='GestionAsignacionRoles'><a onclick='GestionAsignacionRoles()'>" + a.name + "</a></li>");
                HttpContext.Session.SetString("ConsultarAsignacionRol", a.consult.ToString());
                HttpContext.Session.SetString("ActualizarAsignacionRol", a.update.ToString());
                HttpContext.Session.SetString("NombreAsignacionRol", a.name);
            }
            if (a.moduleId == StringHelper.CodAdminEstados.ToString() && a.consult)
            {
                contAdmin = ValidacionAdmin(contAdmin, htmlAdmin, a, "<li id='AdministrarEstados'><a onclick='AdministrarEstados()'>" + a.name + "</a></li>");
                HttpContext.Session.SetString("ConsultarEstados", a.consult.ToString());
                HttpContext.Session.SetString("ActualizarEstados", a.update.ToString());
                HttpContext.Session.SetString("NombreEstados", a.name);
            }
            if (a.moduleId == StringHelper.CodAdminTablasParametricas.ToString() && a.consult)
            {
                contAdmin = ValidacionAdmin(contAdmin, htmlAdmin, a, "<li id='Parametrica'><a onclick='AdministrarTablasParametricas()'>" + a.name + "</a></li>");
                HttpContext.Session.SetString("ConsultarParametrica", a.consult.ToString());
                HttpContext.Session.SetString("NombreParametrica", a.name);
            }
            if (a.moduleId == StringHelper.CodGestionAuditoria.ToString() && a.consult)
            {
                contAdmin = ValidacionAdmin(contAdmin, htmlAdmin, a, "<li id='Auditoria'><a onclick='GestionAuditoria()'>" + a.name + "</a></li>");
                HttpContext.Session.SetString("ConsultarAuditoria", a.consult.ToString());
                HttpContext.Session.SetString("DetallesAuditoria", a.see.ToString());
                HttpContext.Session.SetString("NombreAuditoria", a.name);
            }
            if (a.moduleId == StringHelper.CodGestionDocumental.ToString() && a.consult)
            {
                contAdmin = ValidacionAdmin(contAdmin, htmlAdmin, a, "<li id='GestionDocumental'><a onclick='GestionDocumental()'>" + a.name + "</a></li>");
                HttpContext.Session.SetString("ConsultarDocumental", a.consult.ToString());
                HttpContext.Session.SetString("NombreDocumental", a.name);
            }
            if (a.moduleId == StringHelper.CodAdminTecnica.ToString() && a.consult)
            {
                contAdmin = ValidacionAdmin(contAdmin, htmlAdmin, a, "<li id='AdministracionTecnica'><a onclick='AdministracionTecnica()'>" + a.name + "</a></li>");
                HttpContext.Session.SetString("ConsultarTecnica", a.consult.ToString());
                HttpContext.Session.SetString("ActualizarTecnica", a.update.ToString());
                HttpContext.Session.SetString("EliminarTecnica", a.delete.ToString());
                HttpContext.Session.SetString("CrearTecnica", a.create.ToString());
                HttpContext.Session.SetString("NombreTecnica", a.name);
            }
            contAdmin = ObtenerModulosAdministracion2(contAdmin, htmlAdmin, a);

            return contAdmin;
        }

        private int ObtenerModulosAdministracion2(int contAdmin, StringBuilder htmlAdmin, ReqModulos a)
        {
            if (a.moduleId == StringHelper.CodGestionUsuario.ToString() && a.consult)
            {
                contAdmin = ValidacionAdmin(contAdmin, htmlAdmin, a, "<li id='GestionUsuario'><a onclick='GestionUsuarios()'>" + a.name + "</a></li>");
                HttpContext.Session.SetString("ConsultarUsuario", a.consult.ToString());
                HttpContext.Session.SetString("ActualizarUsuario", a.update.ToString());
                HttpContext.Session.SetString("CrearUsuario", a.create.ToString());
                HttpContext.Session.SetString("NombreUsuario", a.name);
            }
            if (a.moduleId == StringHelper.CodAdminRolesFuncionalidades.ToString())
            {
                HttpContext.Session.SetString("ActualizarFuncionalidades", a.update.ToString());
                HttpContext.Session.SetString("CrearFuncionalidades", a.create.ToString());
                HttpContext.Session.SetString("ConsultarFuncionalidades", a.consult.ToString());
                HttpContext.Session.SetString("NombreFuncionalidades", a.name);
            }
            if (a.moduleId == StringHelper.CodAdminTablasServicios.ToString() && a.consult)
            {
                contAdmin = ValidacionAdmin(contAdmin, htmlAdmin, a, "<li id='Servicios'><a onclick='AdministrarServicios()'>" + a.name + "</a></li>");
                HttpContext.Session.SetString("ConsultarServicio", a.consult.ToString());
                HttpContext.Session.SetString("NombreServicio", a.name);
            }

            return contAdmin;
        }

        private static int ValidacionAdmin(int contAdmin, StringBuilder htmlAdmin, ReqModulos a, string cadenaAñadir)
        {
            if (!htmlAdmin.ToString().Contains(a.name))
            {
                htmlAdmin.Append(cadenaAñadir);
                contAdmin++;
            }

            return contAdmin;
        }

        private static int ValidacionFN(int contFlujoNegocio, StringBuilder htmlFlujoNegocio, ReqModulos a, string cadenaAñadir)
        {
            if (!htmlFlujoNegocio.ToString().Contains(a.name))
            {
                htmlFlujoNegocio.Append(cadenaAñadir);
                contFlujoNegocio++;
            }

            return contFlujoNegocio;
        }

        static int ValidacionReportes(int contReportes, StringBuilder htmlReportes, ReqModulos a, string cadenaAñadir)
        {
            if (!htmlReportes.ToString().Contains(a.name))
            {
                htmlReportes.Append(cadenaAñadir);
                contReportes++;
            }

            return contReportes;
        }
    }
}