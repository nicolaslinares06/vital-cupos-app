using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Repository.Helpers;
using System.Globalization;
using System.Net.Http.Headers;
using Web.Models;

namespace Web.Controllers
{
    public class RegistroVisitaDeCortesController : Controller
    {
        private readonly ILogger<RegistroVisitaDeCortesController> _logger;
        private readonly string rutaAPI;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="logger"></param>
        public RegistroVisitaDeCortesController(IConfiguration configuration, ILogger<RegistroVisitaDeCortesController> logger)
        {
            rutaAPI = string.IsNullOrEmpty(RUTAAPI) ? new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetValue<string>("Variables:RutaApi") : RUTAAPI;
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
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (String.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Index", "Login");
                }
                var modelo = new RegistroVisitaDeCortesModels();
                modelo.Establecimientos = await ObtenerEstablecimientosSelectList();
                modelo.TipoEstablecimientos = await ObtenerTiposEstablecimientosSelect();
                modelo.ActasRegistros = new List<ActaRegistro>();
                return View(modelo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (!String.IsNullOrEmpty(token))
                    return RedirectToAction("Index", "RegistroVisitaDeCortes");
                else
                    return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteriosBusquedaActas"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Index(RegistroActasBusqueda criteriosBusquedaActas)
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (String.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Index", "Login");
                }
                var modelo = new RegistroVisitaDeCortesModels();

                modelo.TipoEstablecimientos = await ObtenerTiposEstablecimientosSelect();
                modelo.ActasRegistros = new List<ActaRegistro>();
                modelo.ActasRegistros = await ObtenerActasTipoEstablecimiento(criteriosBusquedaActas);

                return View(modelo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (!String.IsNullOrEmpty(token))
                    return RedirectToAction("Index", "RegistroVisitaDeCortes");
                else
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
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token != null)
                    return View("Views/RegistroVisitaDeCortes/Partials/Ayuda.cshtml");

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (!String.IsNullOrEmpty(token))
                    return RedirectToAction("Index", "RegistroVisitaDeCortes");
                else
                    return RedirectToAction("Index", "Login");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idActaVisita"></param>
        /// <returns></returns>
        public async Task<IActionResult> ConsultarActaVisita(decimal idActaVisita)
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (String.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Index", "Login");
                }
                var model = new ConsultActaVisitaModelView();
                model.DatosActaVisita = await ObtenerActaVisita(idActaVisita);
                model.DocumentoOrigenPiel = await ObtenerDatosDocumentosString($"{rutaAPI}/VisitRecords/ConsultNumberSkinoOrigin?idActaVisit={idActaVisita}");
                model.ResolucionorigenPiel = await ObtenerDatosDocumentos($"{rutaAPI}/VisitRecords/ConsultNumberResolutionActVisit?idActaVisit={idActaVisita}");
                model.SalvoCondcutoNumeroOrigenPiel = await ObtenerDatosDocumentos($"{rutaAPI}/VisitRecords/ConsultNumberExceptDuctActVisit?idActaVisit={idActaVisita}");
                model.TipoCortesPielIdentificable = await ObtenerTipoPielIdentificable(idActaVisita);
                model.TipoPartesIdentificables = await ObtenerTipoParteIdentificable(idActaVisita);
                model.TipoCortesPielIrregulares = await ObtenerTipoPielIrregulares(idActaVisita);
                model.TipoPartesIrregulares = await ObtenerTiposPartesIrregulares(idActaVisita);
                model.Archivos = await ObtenerArchivosActaVisita(idActaVisita);
                model.DatosActaVisita.NombreCiudadDepartamento = await ObtenerNombreCiudadDepartamento(model.DatosActaVisita.Departamento, model.DatosActaVisita.Ciudad);
                model.DatosActaVisita.ArchivoExcelPrecinto = await ObtenerExcelPrecinto(idActaVisita);

                var tiposPartes = await ObtenerTiposPartesLista();


                if (model.TipoPartesIdentificables.Any())
                {
                    foreach (var item in model.TipoPartesIdentificables)
                    {
                        item.TipoParte = tiposPartes.Where(t => t.Value == item.TipoParte)
                                                    .Select(t => t.Text).FirstOrDefault();
                    }

                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (!String.IsNullOrEmpty(token))
                    return RedirectToAction("Index", "RegistroVisitaDeCortes");
                else
                    return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult CreateTipoActaCorte()
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (String.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Index", "Login");
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (!String.IsNullOrEmpty(token))
                    return RedirectToAction("Index", "RegistroVisitaDeCortes");
                else
                    return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> CreateActaCortePartesIdentificables()
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (String.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Index", "Login");
                }

                var model = new CreateRegistroVisitaCortesIdentificablesModelView();
                model.Departamentos = await ObtenerDepartamentos();
                model.Ciudades = await ObtenerCiudades(0);
                model.TiposPartesPiel = await ObtenerTiposPartesLista();
                ViewBag.usuario = HttpContext.Session.GetString("User");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (!String.IsNullOrEmpty(token))
                    return RedirectToAction("Index", "RegistroVisitaDeCortes");
                else
                    return RedirectToAction("Index", "Login");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registroCortesIdentificables"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> CreateActaCortePartesIdentificables(RegistroVisitaCortesIdentificables registroCortesIdentificables)
        {
            try
            {
                _logger.LogInformation("method called");
                registroCortesIdentificables.EstadoPiel = ObtenerValorEstadoPiel(Int32.Parse(registroCortesIdentificables.EstadoPiel ?? ""));

                string token = HttpContext.Session.GetString("token") ?? "";

                string URI = rutaAPI + "/VisitRecords/CreateActVisitCutsIdentfiable";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var req = new
                {
                    VisitNumber = registroCortesIdentificables.VisitaNumero,
                    VisitNumber1 = registroCortesIdentificables.VisitaNumero1,
                    VisitNumber2 = registroCortesIdentificables.VisitaNumero2,
                    EstablishmentType = registroCortesIdentificables.TipoEstablecimiento,
                    EstablishmentID = registroCortesIdentificables.EstablecimientoID,
                    QuantityOfSkinToCut = registroCortesIdentificables.CantidadPielACortar,
                    IdentificationSeal = registroCortesIdentificables.PrecintoIdentificacion,
                    SkinStatus = registroCortesIdentificables.EstadoPiel,
                    CitesAuthorityOfficer = registroCortesIdentificables.FuncionarioAutoridadCites,
                    RepresentativeDocument = registroCortesIdentificables.DocumentoRepresentante,
                    EstablishmentRepresentative = registroCortesIdentificables.RepresentanteEstablecimiento,
                    City = registroCortesIdentificables.Ciudad,
                    Date = registroCortesIdentificables.Fecha,
                    ExcelSealFile = new
                    {
                        Code = registroCortesIdentificables.ArchivoExcelPrecinto.Code,
                        FileName = registroCortesIdentificables.ArchivoExcelPrecinto.FileName,
                        Base64String = registroCortesIdentificables.ArchivoExcelPrecinto.Base64String,
                        FileType = registroCortesIdentificables.ArchivoExcelPrecinto.FileType
                    },
                    IdentifiableSkinCutsType = registroCortesIdentificables.TipoCortesPiel?.Select(n => new
                    {
                        SkinType = n.TipoPiel,
                        Quantity = n.Cantidad,
                        VisitReportCode = n.CodigoActaVisita
                    }).ToList(),
                    IdentifiableSkinPartsType = registroCortesIdentificables.TipoPartes?.Select(n => new
                    {
                        SkinPartType = n.TipoParte,
                        Quantity = n.Cantidad,
                        VisitReportCode = n.CodigoActaVisita
                    }).ToList()
                };

                var response = await httpClient.PostAsJsonAsync(URI, req);

                if (!response.IsSuccessStatusCode)
                {
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar datos en el servidor" });
                }

                string responseString = await response.Content.ReadAsStringAsync();
                Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString ?? "") ?? new Responses();
                if (String.IsNullOrEmpty(respuesta.Token))
                {
                    HttpContext.Session.SetString("token", "");
                    return new JsonResult(new { Error = true, mensaje = "La sesion ha caducado" });

                }
                HttpContext.Session.SetString("token", respuesta.Token);
                var codigoActa = Int32.Parse(respuesta.Response.ToString() ?? "");
                if (codigoActa <= 0)
                    return new JsonResult(new { Error = true, mensaje = respuesta.Message });

                var ingresosTiposPiel = await IngresarTipoPiel(registroCortesIdentificables.TipoCortesPiel ?? new List<TipoCortesPielIdentificables>(), codigoActa);
                if (!ingresosTiposPiel)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar Tipos  de Piel" });


                var ingresosTiposParte = await IngresarTipoParteIdentificable(registroCortesIdentificables.TipoPartes ?? new List<TipoPartesPielIdentificables>(), codigoActa);
                if (!ingresosTiposParte)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar Tipos  de Partes" });

                var ingresosDocumentosOrigenPiel = await IngresarActaVisitaOrigenDocumento(registroCortesIdentificables.DocumentoOrigenPiel.ToList(), codigoActa);

                if (!ingresosDocumentosOrigenPiel)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar Documentos de Origen de piel en el servidor" });


                var ingresosDocumentoResolucion = await IngresarActaVisitaResolucionNumero(registroCortesIdentificables.ResolucionorigenPiel.ToList(), codigoActa);
                if (!ingresosDocumentoResolucion)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar Documentos de Resolucion en el servidor" });


                var ingresosDocumentosSalvoConductos = await IngresarActaVisitaActaSalvoconducto(registroCortesIdentificables.SalvoCondcutoNumeroOrigenPiel.ToList(), codigoActa);
                if (!ingresosDocumentosSalvoConductos)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar Documentos de SalvoConductos en el servidor" });

                var ingresosArchivos = await IngresarArchivosActaVisita(registroCortesIdentificables.Archivos.ToList(), codigoActa);
                if (!ingresosArchivos)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar la actualizacion de archivos" });

                return new JsonResult(new { Error = false, mensaje = "Su información ha sido guardada con éxito" });
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
        /// <returns></returns>
        public async Task<IActionResult> CreateActaCortesPartesIrregulares()
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (String.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Index", "Login");
                }
                var modelo = new CreateRegistroVisitaCortesFraccionesIrregulares();
                modelo.TiposEstablecimientos = await ObtenerTiposEstablecimientosSelect();
                modelo.Departamentos = await ObtenerDepartamentos();
                modelo.Ciudades = await ObtenerCiudades(0);
                modelo.TiposPartesPiel = await ObtenerTiposPartesLista();
                ViewBag.usuario = HttpContext.Session.GetString("User");
                return View(modelo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (!String.IsNullOrEmpty(token))
                    return RedirectToAction("Index", "RegistroVisitaDeCortes");
                else
                    return RedirectToAction("Index", "Login");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idActaVisita"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditActaCortePartesIdentificables(decimal idActaVisita)
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (String.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Index", "Login");
                }

                var model = new EditActaVisitaIdentificableModelView();
                model.ActaVisitaCortes = await ObtenerActaVisita(idActaVisita);
                model.TiposEstablecimientos = await ObtenerTiposEstablecimientosSelect();
                model.DocumentoOrigenPiel = await ObtenerDatosDocumentosString($"{rutaAPI}/VisitRecords/ConsultNumberSkinoOrigin?idActaVisit={idActaVisita}");
                model.ResolucionorigenPiel = await ObtenerDatosDocumentos($"{rutaAPI}/VisitRecords/ConsultNumberResolutionActVisit?idActaVisit={idActaVisita}");
                model.SalvoCondcutoNumeroOrigenPiel = await ObtenerDatosDocumentos($"{rutaAPI}/VisitRecords/ConsultNumberExceptDuctActVisit?idActaVisit={idActaVisita}");
                model.TipoCortesPiel = await ObtenerTipoPielIdentificable(idActaVisita);
                model.TipoPartes = await ObtenerTipoParteIdentificable(idActaVisita);
                model.ActaVisitaCortes.Departamentos = await ObtenerDepartamentos();
                model.ActaVisitaCortes.Ciudades = await ObtenerCiudades(model.ActaVisitaCortes.Departamento);
                model.TiposPartesPiel = await ObtenerTiposPartesLista();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (!String.IsNullOrEmpty(token))
                    return RedirectToAction("Index", "RegistroVisitaDeCortes");
                else
                    return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registroCortesIdentificables"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> EditActaCortePartesIdentificables(EditActaVisitaIdentificableModelView registroCortesIdentificables)
        {
            try
            {
                _logger.LogInformation("method called");
                registroCortesIdentificables.ActaVisitaCortes.EstadoPiel = ObtenerValorEstadoPiel(Int32.Parse(registroCortesIdentificables.ActaVisitaCortes.EstadoPiel ?? ""));

                string token = HttpContext.Session.GetString("token") ?? "";

                string URI = rutaAPI + "/VisitRecords/EditActVisitIdentfiable";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var datosActaVisita = new EditVisitaActaCortes()
                {
                    ActaVisitaId = registroCortesIdentificables.ActaVisitaCortes.ActaVisitaId,
                    VisitaNumero = registroCortesIdentificables.ActaVisitaCortes.VisitaNumero,
                    TipoEstablecimiento = registroCortesIdentificables.ActaVisitaCortes.TipoEstablecimiento,
                    TipoEstablecimientoNombre = registroCortesIdentificables.ActaVisitaCortes.TipoEstablecimientoNombre,
                    EstablecimientoID = registroCortesIdentificables.ActaVisitaCortes.EstablecimientoID,
                    NombreEstablecimiento = registroCortesIdentificables.ActaVisitaCortes.NombreEstablecimiento,
                    CantidadPielACortar = registroCortesIdentificables.ActaVisitaCortes.CantidadPielACortar,
                    PrecintoIdentificacion = registroCortesIdentificables.ActaVisitaCortes.PrecintoIdentificacion,
                    EstadoPiel = registroCortesIdentificables.ActaVisitaCortes.EstadoPiel,
                    RepresentanteEstablecimiento = registroCortesIdentificables.ActaVisitaCortes.RepresentanteEstablecimiento,
                    DocumentoRepresentante = registroCortesIdentificables.ActaVisitaCortes.DocumentoRepresentante,
                    Ciudad = registroCortesIdentificables.ActaVisitaCortes.Ciudad,
                    Fecha = registroCortesIdentificables.ActaVisitaCortes.Fecha,
                    FechaFormat = "",
                    VisitaNumero1 = registroCortesIdentificables.ActaVisitaCortes.VisitaNumero1,
                    VisitaNumero2 = registroCortesIdentificables.ActaVisitaCortes.VisitaNumero2,
                    ArchivoExcelPrecinto = registroCortesIdentificables.ActaVisitaCortes.ArchivoExcelPrecinto
                };

                var req = new
                {
                    VisitReportId = datosActaVisita.ActaVisitaId,
                    VisitNumber = datosActaVisita.VisitaNumero,
                    EstablishmentType = datosActaVisita.TipoEstablecimiento,
                    EstablishmentTypeName = datosActaVisita.TipoEstablecimientoNombre,
                    EstablishmentID = datosActaVisita.EstablecimientoID,
                    EstablishmentName = datosActaVisita.NombreEstablecimiento,
                    AmountOfSkinToCut = datosActaVisita.CantidadPielACortar,
                    SealIdentification = datosActaVisita.PrecintoIdentificacion,
                    SkinStatus = datosActaVisita.EstadoPiel,
                    RepresentativeDocument = datosActaVisita.DocumentoRepresentante,
                    EstablishmentRepresentative = datosActaVisita.RepresentanteEstablecimiento,
                    City = datosActaVisita.Ciudad,
                    Date = datosActaVisita.Fecha,
                    DateFormat = datosActaVisita.FechaFormat,
                    VisitNumber1 = datosActaVisita.VisitaNumero1,
                    VisitNumber2 = datosActaVisita.VisitaNumero2,
                    ExcelSealFile = new
                    {
                        Code = datosActaVisita.ArchivoExcelPrecinto.Code,
                        FileName = datosActaVisita.ArchivoExcelPrecinto.FileName,
                        Base64String = datosActaVisita.ArchivoExcelPrecinto.Base64String,
                        FileType = datosActaVisita.ArchivoExcelPrecinto.FileType
                    }
                };

                var response = await httpClient.PutAsJsonAsync(URI, req);

                if (!response.IsSuccessStatusCode)
                {
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar datos en el servidor" });
                }

                string responseString = await response.Content.ReadAsStringAsync();
                Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                if (String.IsNullOrEmpty(respuesta.Token))
                {
                    HttpContext.Session.SetString("token", "");
                    return new JsonResult(new { Error = true, mensaje = "La sesion ha caducado" });

                }

                HttpContext.Session.SetString("token", respuesta.Token);

                if (respuesta.Error)
                    return new JsonResult(new { Error = true, mensaje = respuesta.Message });

                int codigoActa = (int)registroCortesIdentificables.ActaVisitaCortes.ActaVisitaId;

                var borrarTipoPiel = await BorrarDatos($"{rutaAPI}/VisitRecords/DeletePartSkinsActVisit?idActaVisit={codigoActa}");
                if (!borrarTipoPiel)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar la actualizacion de Tipos Piel" });

                var ingresosTiposPiel = await IngresarTipoPiel(registroCortesIdentificables.TipoCortesPiel.ToList(), codigoActa);
                if (!ingresosTiposPiel)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar la actualizacion de Tipos Piel" });


                var borrarTipoParte = await BorrarDatos($"{rutaAPI}/VisitRecords/DeletePartsTypeActVisit?idActaVisit={codigoActa}");
                if (!borrarTipoParte)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar la actualizacion de Tipos Partes" });

                var ingresosTiposParte = await IngresarTipoParteIdentificable(registroCortesIdentificables.TipoPartes.ToList(), codigoActa);
                if (!ingresosTiposParte)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar la actualizacion de Tipos Partes" });

                var borrarDocumentosorigen = await BorrarDatos($"{rutaAPI}/VisitRecords/DeleteNumberSkinOriginActVisit?idActaVisit={codigoActa}");
                if (!borrarDocumentosorigen)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar la actualizacion de Documentos de Origen" });

                var ingresosDocumentosOrigenPiel = await IngresarActaVisitaOrigenDocumento(registroCortesIdentificables.DocumentoOrigenPiel.ToList(), codigoActa);
                if (!ingresosDocumentosOrigenPiel)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar la actualizacion de Documentos de Origen" });

                var borrarDocumentoResolucion = await BorrarDatos($"{rutaAPI}/VisitRecords/DeleteResolutionsNumberActVisit?idActaVisit={codigoActa}");
                if (!borrarDocumentoResolucion)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar la actualizacion de Documentos de Resolucion" });

                var ingresosDocumentoResolucion = await IngresarActaVisitaResolucionNumero(registroCortesIdentificables.ResolucionorigenPiel.ToList(), codigoActa);
                if (!ingresosDocumentoResolucion)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar la actualizacion de Documentos de Resolucion" });

                var ingresosDocumentosSalvoConductos = await IngresarActaVisitaActaSalvoconducto(registroCortesIdentificables.SalvoCondcutoNumeroOrigenPiel.ToList(), codigoActa);
                if (!ingresosDocumentosSalvoConductos)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar la actualizacion de Documentos de salvoConductos" });

                var borrarArchivos = await BorrarDatos($"{rutaAPI}/VisitRecords/DeleteFileActVisit?idActaVisit={codigoActa}");
                if (!borrarArchivos)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar la actualizacion de archivos" });

                var ingresosArchivos = await IngresarArchivosActaVisita(registroCortesIdentificables.Archivos.ToList(), codigoActa);
                if (!ingresosArchivos)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar la actualizacion de archivos" });

                return new JsonResult(new { Error = false, mensaje = "Su información ha sido guardada con éxito" });
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
        /// <param name="idActaVisita"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditActaCortesFraccionesIrregulares(decimal idActaVisita)
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (String.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Index", "Login");
                }

                var model = new EditActaVisitaIrregularesModelView();
                model.ActaVisitaCortes = await ObtenerActaVisitaIrregular(idActaVisita);
                model.TiposEstablecimientos = await ObtenerTiposEstablecimientosSelect();
                model.DocumentoOrigenPiel = await ObtenerDatosDocumentosString($"{rutaAPI}/VisitRecords/ConsultNumberSkinoOrigin?idActaVisit={idActaVisita}");
                model.ResolucionorigenPiel = await ObtenerDatosDocumentos($"{rutaAPI}/VisitRecords/ConsultNumberResolutionActVisit?idActaVisit={idActaVisita}");
                model.SalvoCondcutoNumeroOrigenPiel = await ObtenerDatosDocumentos($"{rutaAPI}/VisitRecords/ConsultNumberExceptDuctActVisit?idActaVisit={idActaVisita}");
                model.TipoCortesPiel = await ObtenerTipoPielIrregulares(idActaVisita);
                model.TipoPartes = await ObtenerTiposPartesIrregulares(idActaVisita);
                model.ActaVisitaCortes.Departamentos = await ObtenerDepartamentos();
                model.ActaVisitaCortes.Ciudades = await ObtenerCiudades(model.ActaVisitaCortes.Departamento);
                model.TiposPartesPiel = await ObtenerTiposPartesLista();

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (!String.IsNullOrEmpty(token))
                    return RedirectToAction("Index", "RegistroVisitaDeCortes");
                else
                    return RedirectToAction("Index", "Login");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteriosBusquedaActas"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ConsultarActas(RegistroActasBusqueda criteriosBusquedaActas)
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (String.IsNullOrEmpty(token))
                {
                    RedirectToAction("Index", "Login");
                }

                criteriosBusquedaActas.TipoBusqueda = DefinirTipoBusquedaActas(criteriosBusquedaActas);

                var ActasRegistros = await ObtenerActasTipoEstablecimiento(criteriosBusquedaActas);

                return new JsonResult(ActasRegistros);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var actasRegistros = new List<ActaRegistro>();
                return new JsonResult(actasRegistros);

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="departamentoId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ConsultarCiudades(decimal departamentoId)
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (String.IsNullOrEmpty(token))
                {
                    RedirectToAction("Index", "Login");
                }

                var ciudades = await ObtenerCiudades(departamentoId);
                return new JsonResult(ciudades);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var ciudades = new List<SelectListItem>();
                return new JsonResult(ciudades);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoEstablecimientoId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ConsultarEstablecimientosPorTipo(decimal tipoEstablecimientoId)
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (String.IsNullOrEmpty(token))
                {
                    RedirectToAction("Index", "Login");
                }

                var establecimientos = await ObtenerEstablecimientosPorTipo(tipoEstablecimientoId);
                return new JsonResult(establecimientos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var establecimientos = new List<SelectListItem>();
                return new JsonResult(establecimientos);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="NitEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ConsultarDatosEmpresaPorNit(decimal NitEmpresa)
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (String.IsNullOrEmpty(token))
                {
                    RedirectToAction("Index", "Login");
                }
                var empresa = await ObtenerEmpresaPorNit(NitEmpresa);
                return new JsonResult(empresa);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var empresa = new EstablecimientoProps();
                return new JsonResult(empresa);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="base64ArchivoExcelPrecintos"></param>
        /// <param name="nit"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ValidarDatoslArchivoPrecintos(string base64ArchivoExcelPrecintos, decimal nit)
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (String.IsNullOrEmpty(token))
                {
                    RedirectToAction("Index", "Login");
                }

                var errorValidacionDatosExcel = await ValidarDatosExcelPrecintos(base64ArchivoExcelPrecintos, nit);

                return new JsonResult(errorValidacionDatosExcel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var validacion = new Responses();
                validacion.Error = true;
                validacion.Message = ex.Message;
                return new JsonResult(validacion);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registroCortesIrregulares"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> CreateActaVisitaCorteIrregular(RegistroVisitaCortesIrregulares registroCortesIrregulares)
        {
            try
            {
                _logger.LogInformation("method called");
                registroCortesIrregulares.EstadoPiel = ObtenerValorEstadoPiel(Int32.Parse(registroCortesIrregulares.EstadoPiel ?? ""));

                string token = HttpContext.Session.GetString("token") ?? "";

                string URI = rutaAPI + "/VisitRecords/CreateActVisitCutsIrregulars";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var req = new
                {
                    VisitNumber = registroCortesIrregulares.VisitaNumero,
                    VisitNumber1 = registroCortesIrregulares.VisitaNumero1,
                    VisitNumber2 = registroCortesIrregulares.VisitaNumero2,
                    EstablishmentType = registroCortesIrregulares.TipoEstablecimiento,
                    EstablishmentID = registroCortesIrregulares.EstablecimientoID,
                    QuantityOfSkinToCut = registroCortesIrregulares.CantidadPielACortar,
                    IdentificationSeal = registroCortesIrregulares.PrecintoIdentificacion,
                    SkinStatus = registroCortesIrregulares.EstadoPiel,
                    CitesAuthorityOfficer = registroCortesIrregulares.FuncionarioAutoridadCites,
                    RepresentativeDocument = registroCortesIrregulares.DocumentoRepresentante,
                    EstablishmentRepresentative = registroCortesIrregulares.RepresentanteEstablecimiento,
                    City = registroCortesIrregulares.Ciudad,
                    Date = registroCortesIrregulares.Fecha,
                    ExcelSealFile = new
                    {
                        Code = registroCortesIrregulares.ArchivoExcelPrecinto.Code,
                        FileName = registroCortesIrregulares.ArchivoExcelPrecinto.FileName,
                        Base64String = registroCortesIrregulares.ArchivoExcelPrecinto.Base64String,
                        FileType = registroCortesIrregulares.ArchivoExcelPrecinto.FileType
                    },
                    IdentifiableSkinCutsType = registroCortesIrregulares.TipoCortesPiel?.Select(n => new
                    {
                        IrregularSkinType = n.TipoPielIrregular,
                        AverageAreaForSkinType = n.AreaPromedioTipoPiel,
                        SkinTypeQuantity = n.CantidadTipoPiel,
                        TotalAreaForSkinType = n.AreaTotalTipoPiel,
                        VisitReportCode = n.CodigoActaVisita
                    }).ToList(),
                    IdentifiableSkinPartsType = registroCortesIrregulares.TipoPartes?.Select(n => new
                    {
                        PartType = n.TipoParte,
                        PartTypeQuantity = n.CantidadTipoParte,
                        TotalAreaForPartType = n.AreaTotalTipoParte,
                        VisitReportCode = n.CodigoActaVisita
                    }).ToList()
                };

                var response = await httpClient.PostAsJsonAsync(URI, req);

                if (!response.IsSuccessStatusCode)
                {
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar datos en el servidor" });

                }

                string responseString = await response.Content.ReadAsStringAsync();
                Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                if (String.IsNullOrEmpty(respuesta.Token))
                {
                    HttpContext.Session.SetString("token", "");
                    return new JsonResult(new { Error = true, mensaje = "La sesion ha caducado" });

                }

                HttpContext.Session.SetString("token", respuesta.Token);
                var codigoActa = Int32.Parse(respuesta.Response.ToString() ?? "");
                if (codigoActa <= 0)
                    return new JsonResult(new { Error = true, mensaje = respuesta.Message });

                var ingresosTiposPiel = await IngresarTipoPielIrregular(registroCortesIrregulares.TipoCortesPiel ?? new List<TiposPielIrregulares>(), codigoActa);
                if (!ingresosTiposPiel)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar Tipos  de Piel" });

                var ingresosTiposParte = await IngresarTipoParteIrregular(registroCortesIrregulares.TipoPartes ?? new List<TiposParteIrregulares>(), codigoActa);
                if (!ingresosTiposParte)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar Tipos  de Partes" });


                var ingresosDocumentosOrigenPiel = await IngresarActaVisitaOrigenDocumento(registroCortesIrregulares.DocumentoOrigenPiel.ToList(), codigoActa);
                if (!ingresosDocumentosOrigenPiel)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar Documentos de Origen de piel en el servidor" });


                var ingresosDocumentoResolucion = await IngresarActaVisitaResolucionNumero(registroCortesIrregulares.ResolucionorigenPiel.ToList(), codigoActa);
                if (!ingresosDocumentoResolucion)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar Documentos de Resolucion en el servidor" });


                var ingresosDocumentosSalvoConductos = await IngresarActaVisitaActaSalvoconducto(registroCortesIrregulares.SalvoCondcutoNumeroOrigenPiel.ToList(), codigoActa);

                if (!ingresosDocumentosSalvoConductos)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar Documentos de SalvoConductos en el servidor" });


                var ingresosArchivos = await IngresarArchivosActaVisita(registroCortesIrregulares.Archivos.ToList(), codigoActa);
                if (!ingresosArchivos)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar la actualizacion de archivos" });

                return new JsonResult(new { Error = false, mensaje = "Su información ha sido guardada con éxito" });
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
        /// <param name="registroCortesIrregulares"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> EditActaVisitaCortesIrregulares(EditActaVisitaIrregularesModelView registroCortesIrregulares)
        {
            try
            {
                _logger.LogInformation("method called");
                registroCortesIrregulares.ActaVisitaCortes.EstadoPiel = ObtenerValorEstadoPiel(Int32.Parse(registroCortesIrregulares.ActaVisitaCortes.EstadoPiel ?? ""));

                string token = HttpContext.Session.GetString("token") ?? "";

                if (String.IsNullOrEmpty(token))
                {
                    RedirectToAction("Index", "Login");
                }

                string URI = rutaAPI + "/VisitRecords/EditActVisitIdentfiable";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var datosActaVisita = new EditVisitaActaCortes()
                {
                    ActaVisitaId = registroCortesIrregulares.ActaVisitaCortes.ActaVisitaId,
                    VisitaNumero = registroCortesIrregulares.ActaVisitaCortes.VisitaNumero,
                    TipoEstablecimiento = registroCortesIrregulares.ActaVisitaCortes.TipoEstablecimiento,
                    TipoEstablecimientoNombre = registroCortesIrregulares.ActaVisitaCortes.TipoEstablecimientoNombre,
                    EstablecimientoID = registroCortesIrregulares.ActaVisitaCortes.EstablecimientoID,
                    NombreEstablecimiento = registroCortesIrregulares.ActaVisitaCortes.NombreEstablecimiento,
                    CantidadPielACortar = registroCortesIrregulares.ActaVisitaCortes.CantidadPielACortar,
                    PrecintoIdentificacion = registroCortesIrregulares.ActaVisitaCortes.PrecintoIdentificacion,
                    EstadoPiel = registroCortesIrregulares.ActaVisitaCortes.EstadoPiel,
                    FuncionarioAutoridadCites = 1,
                    RepresentanteEstablecimiento = registroCortesIrregulares.ActaVisitaCortes.RepresentanteEstablecimiento,
                    DocumentoRepresentante = registroCortesIrregulares.ActaVisitaCortes.DocumentoRepresentante,
                    Ciudad = registroCortesIrregulares.ActaVisitaCortes.Ciudad,
                    Fecha = registroCortesIrregulares.ActaVisitaCortes.Fecha,
                    VisitaNumero1 = registroCortesIrregulares.ActaVisitaCortes.VisitaNumero1,
                    VisitaNumero2 = registroCortesIrregulares.ActaVisitaCortes.VisitaNumero2,
                    ArchivoExcelPrecinto = registroCortesIrregulares.ActaVisitaCortes.ArchivoExcelPrecinto,
                    FechaFormat = ""
                };

                var response = await httpClient.PutAsJsonAsync(URI, datosActaVisita);

                if (!response.IsSuccessStatusCode)
                {
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar datos en el servidor" });
                }

                string responseString = await response.Content.ReadAsStringAsync();
                Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                if (String.IsNullOrEmpty(respuesta.Token))
                {
                    HttpContext.Session.SetString("token", "");
                    return new JsonResult(new { Error = true, mensaje = "La sesion ha caducado" });

                }

                HttpContext.Session.SetString("token", respuesta.Token);

                if (respuesta.Error)
                    return new JsonResult(new { Error = true, mensaje = respuesta.Message });

                int codigoActa = (int)registroCortesIrregulares.ActaVisitaCortes.ActaVisitaId;

                var borrarTipoPiel = await BorrarDatos($"{rutaAPI}/VisitRecords/DeletePartSkinsActVisit?idActaVisit={codigoActa}");
                if (!borrarTipoPiel)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar la actualizacion de Tipos Piel" });


                var ingresosTiposPiel = await IngresarTipoPielIrregular(registroCortesIrregulares.TipoCortesPiel, codigoActa);
                if (!ingresosTiposPiel)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar la actualizacion de Tipos Piel" });


                var borrarTipoParte = await BorrarDatos($"{rutaAPI}/VisitRecords/DeletePartsTypeActVisit?idActaVisit={codigoActa}");
                if (!borrarTipoParte)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar la actualizacion de Tipos Partes" });

                var ingresosTiposParte = await IngresarTipoParteIrregular(registroCortesIrregulares.TipoPartes, codigoActa);
                if (!ingresosTiposParte)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar la actualizacion de Tipos Partes" });

                var borrarDocumentosorigen = await BorrarDatos($"{rutaAPI}/VisitRecords/DeleteNumberSkinOriginActVisit?idActaVisit={codigoActa}");
                if (!borrarDocumentosorigen)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar la actualizacion de Documentos de Origen" });


                var ingresosDocumentosOrigenPiel = await IngresarActaVisitaOrigenDocumento(registroCortesIrregulares.DocumentoOrigenPiel.ToList(), codigoActa);
                if (!ingresosDocumentosOrigenPiel)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar la actualizacion de Documentos de Origen" });

                var borrarDocumentoResolucion = await BorrarDatos($"{rutaAPI}/VisitRecords/DeleteResolutionsNumberActVisit?idActaVisit={codigoActa}");
                if (!borrarDocumentoResolucion)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar la actualizacion de Documentos de Resolucion" });

                var ingresosDocumentoResolucion = await IngresarActaVisitaResolucionNumero(registroCortesIrregulares.ResolucionorigenPiel.ToList(), codigoActa);
                if (!ingresosDocumentoResolucion)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar la actualizacion de Documentos de Resolucion" });

                var ingresosDocumentosSalvoConductos = await IngresarActaVisitaActaSalvoconducto(registroCortesIrregulares.SalvoCondcutoNumeroOrigenPiel.ToList(), codigoActa);
                if (!ingresosDocumentosSalvoConductos)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar la actualizacion de Documentos de salvoConductos" });


                var borrarArchivos = await BorrarDatos($"{rutaAPI}/VisitRecords/DeleteFileActVisit?idActaVisit={codigoActa}");
                if (!borrarArchivos)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar la actualizacion de archivos" });

                var ingresosArchivos = await IngresarArchivosActaVisita(registroCortesIrregulares.Archivos.ToList(), codigoActa);
                if (!ingresosArchivos)
                    return new JsonResult(new { Error = true, mensaje = "Error al procesar la actualizacion de archivos" });

                return new JsonResult(new { Error = false, mensaje = "Su información ha sido guardada con éxito" });
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
        /// <param name="actaVisitaId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> InhabilitarActaVisita(decimal actaVisitaId)
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";

                string URI = rutaAPI + "/VisitRecords/DisableActVisit?idActaVisit=" + actaVisitaId;
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.PutAsJsonAsync(URI, actaVisitaId);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                    if (String.IsNullOrEmpty(respuesta.Token))
                    {
                        HttpContext.Session.SetString("token", "");
                        return false;
                    }

                    HttpContext.Session.SetString("token", respuesta.Token);

                    if (respuesta.Error)
                        return false;

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
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actaVisitaId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ObtenerArchivosPost(int actaVisitaId)
        {
            try
            {
                _logger.LogInformation("method called");
                var listaArchivos = await ObtenerArchivosActaVisita(actaVisitaId);
                return new JsonResult(new { listaArchivos });
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
        /// <param name="actaVisitaId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ObtenerExcelPrecintoActaVisita(int actaVisitaId)
        {
            try
            {
                _logger.LogInformation("method called");
                var archivoExcel = await ObtenerExcelPrecinto(actaVisitaId);
                return new JsonResult(new { archivoExcel });
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
        /// <param name="documentoExcel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DescargarPlantillaPrecintoExcel(ActaVisitasPropArchivos documentoExcel)
        {
            try
            {
                _logger.LogInformation("method called");

                if (String.IsNullOrEmpty(documentoExcel.Base64String))
                    documentoExcel.Base64String = "";

                var nombreArchivo = documentoExcel.FileName;
                string eliminar = "data:" + documentoExcel.FileType + ";base64,";
                string SinData = documentoExcel.Base64String.Replace(eliminar, String.Empty);
                byte[] bytes = Convert.FromBase64String(SinData);
                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return BadRequest("An error occurred while processing your request.");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoPielListado"></param>
        /// <param name="codigoActa"></param>
        /// <returns></returns>
        private async Task<bool> IngresarTipoPiel(List<TipoCortesPielIdentificables> tipoPielListado, int codigoActa)
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = rutaAPI + "/VisitRecords/InsertTypeSkinIdentfiable";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                foreach (var item in tipoPielListado)
                {
                    item.CodigoActaVisita = codigoActa;

                    var req = new
                    {
                        SkinType = item.TipoPiel,
                        Quantity = item.Cantidad,
                        VisitReportCode = item.CodigoActaVisita
                    };

                    var response = await httpClient.PostAsJsonAsync(URI, req);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                        if (String.IsNullOrEmpty(respuesta.Token))
                        {
                            HttpContext.Session.SetString("token", "");
                            return false;
                        }

                        HttpContext.Session.SetString("token", respuesta.Token);

                        if (respuesta.Error)
                            return false;

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoParteListado"></param>
        /// <param name="codigoActa"></param>
        /// <returns></returns>
        private async Task<bool> IngresarTipoParteIdentificable(List<TipoPartesPielIdentificables> tipoParteListado, int codigoActa)
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = rutaAPI + "/VisitRecords/InsertPartTypeIdentfiable";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                foreach (var item in tipoParteListado)
                {
                    item.CodigoActaVisita = codigoActa;

                    var req = new
                    {
                        SkinPartType = item.TipoParte,
                        Quantity = item.Cantidad,
                        VisitReportCode = item.CodigoActaVisita
                    };

                    var response = await httpClient.PostAsJsonAsync(URI, req);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                        if (String.IsNullOrEmpty(respuesta.Token))
                        {
                            HttpContext.Session.SetString("token", "");
                            return false;
                        }


                        HttpContext.Session.SetString("token", respuesta.Token);

                        if (respuesta.Error)
                            return false;

                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoPielListado"></param>
        /// <param name="codigoActa"></param>
        /// <returns></returns>
        private async Task<bool> IngresarTipoPielIrregular(List<TiposPielIrregulares> tipoPielListado, int codigoActa)
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = rutaAPI + "/VisitRecords/InsertPartSkinIrregular";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                foreach (var item in tipoPielListado)
                {
                    item.CodigoActaVisita = codigoActa;

                    var req = new
                    {
                        IrregularSkinType = item.TipoPielIrregular,
                        AverageAreaForSkinType = item.AreaPromedioTipoPiel,
                        SkinTypeQuantity = item.CantidadTipoPiel,
                        TotalAreaForSkinType = item.AreaTotalTipoPiel,
                        VisitReportCode = item.CodigoActaVisita
                    };

                    var response = await httpClient.PostAsJsonAsync(URI, req);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                        if (String.IsNullOrEmpty(respuesta.Token))
                        {
                            HttpContext.Session.SetString("token", "");
                            return false;
                        }



                        HttpContext.Session.SetString("token", respuesta.Token);

                        if (respuesta.Error)
                            return false;

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoParteIrregularListado"></param>
        /// <param name="codigoActa"></param>
        /// <returns></returns>
        private async Task<bool> IngresarTipoParteIrregular(List<TiposParteIrregulares> tipoParteIrregularListado, int codigoActa)
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = rutaAPI + "/VisitRecords/InsertIrregularPartType";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                foreach (var item in tipoParteIrregularListado)
                {
                    item.CodigoActaVisita = codigoActa;

                    var req = new
                    {
                        PartType = item.TipoParte,
                        PartTypeQuantity = item.CantidadTipoParte,
                        TotalAreaForPartType = item.AreaTotalTipoParte,
                        VisitReportCode = item.CodigoActaVisita
                    };

                    var response = await httpClient.PostAsJsonAsync(URI, req);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                        if (String.IsNullOrEmpty(respuesta.Token))
                        {
                            HttpContext.Session.SetString("token", "");
                            return false;
                        }

                        HttpContext.Session.SetString("token", respuesta.Token);

                        if (respuesta.Error)
                            return false;

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentosListado"></param>
        /// <param name="codigoActa"></param>
        /// <returns></returns>
        private async Task<bool> IngresarActaVisitaOrigenDocumento(List<IdRegistrosDocumentosString> documentosListado, int codigoActa)
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = rutaAPI + "/VisitRecords/InsertRecordVisitSkinOrigin";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                foreach (var item in documentosListado)
                {
                    var documento = new ActaVisitaDocumentoOrigenPiel()
                    {
                        A015CodigoActaVisita = codigoActa,
                        A015DocumentoOrigenPielNumero = item.NumeroDocumento
                    };

                    var req = new
                    {
                        VisitReportCode = documento.A015CodigoActaVisita,
                        SkinOriginDocumentNumber = documento.A015DocumentoOrigenPielNumero
                    };

                    var response = await httpClient.PostAsJsonAsync(URI, req);

                    if (response.IsSuccessStatusCode)
                    {

                        string responseString = await response.Content.ReadAsStringAsync();
                        Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                        if (String.IsNullOrEmpty(respuesta.Token))
                        {
                            HttpContext.Session.SetString("token", "");
                            return false;
                        }

                        HttpContext.Session.SetString("token", respuesta.Token);

                        if (respuesta.Error)
                            return false;

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentosResolucionListado"></param>
        /// <param name="codigoActa"></param>
        /// <returns></returns>
        private async Task<bool> IngresarActaVisitaResolucionNumero(List<IdRegistrosDocumentos> documentosResolucionListado, int codigoActa)
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = rutaAPI + "/VisitRecords/InsertActVisitResolutionNumber";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                foreach (var item in documentosResolucionListado)
                {
                    var documento = new ActaVisitaResolucionNumero()
                    {
                        A016CodigoActaVisita = codigoActa,
                        A016ResolucionNumero = item.NumeroDocumento
                    };

                    var req = new
                    {
                        VisitReportCode = documento.A016CodigoActaVisita,
                        ResolutionNumber = documento.A016ResolucionNumero
                    };

                    var response = await httpClient.PostAsJsonAsync(URI, req);

                    if (response.IsSuccessStatusCode)
                    {

                        string responseString = await response.Content.ReadAsStringAsync();
                        Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                        if (String.IsNullOrEmpty(respuesta.Token))
                        {
                            HttpContext.Session.SetString("token", "");
                            return false;
                        }


                        HttpContext.Session.SetString("token", respuesta.Token);

                        if (respuesta.Error)
                            return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentosSalvoConductoListado"></param>
        /// <param name="codigoActa"></param>
        /// <returns></returns>
        private async Task<bool> IngresarActaVisitaActaSalvoconducto(List<IdRegistrosDocumentos> documentosSalvoConductoListado, int codigoActa)
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = rutaAPI + "/VisitRecords/InsertActVisitExceptDuct";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                foreach (var item in documentosSalvoConductoListado)
                {
                    var documento = new ActaVisitaSalvoConducto()
                    {
                        A017CodigoActaVisita = codigoActa,
                        A017SalvoConductoNumero = item.NumeroDocumento
                    };

                    var req = new
                    {
                        VisitReportCode = documento.A017CodigoActaVisita,
                        SafeConductNumber = documento.A017SalvoConductoNumero
                    };

                    var response = await httpClient.PostAsJsonAsync(URI, req);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                        if (String.IsNullOrEmpty(respuesta.Token))
                        {
                            HttpContext.Session.SetString("token", "");
                            return false;
                        }

                        HttpContext.Session.SetString("token", respuesta.Token);

                        if (respuesta.Error)
                            return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<SelectListItem>> ObtenerTiposEstablecimientosSelect()
        {
            try
            {
                _logger.LogInformation("method called");
                List<GestionParametrica> tiposEstablecimientos = new List<GestionParametrica>();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = rutaAPI + "/VisitRecords/ConsultTypesOfEstablishments";
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
                        tiposEstablecimientos = JsonConvert.DeserializeObject<List<GestionParametrica>>(respuesta.Response.ToString() ?? "") ?? new List<GestionParametrica>();
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", "");
                    }
                }

                var resultadoListado = tiposEstablecimientos.Select(x => new SelectListItem(x.a008Valor, x.pkT008codigo.ToString())).ToList();

                var opcionPorDefecto = new SelectListItem("Seleccione un Tipo Establecimiento", "0", true);
                resultadoListado.Insert(0, opcionPorDefecto);

                return resultadoListado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var resultadoListado = new List<SelectListItem>();
                var opcionPorDefecto = new SelectListItem("Seleccione un Tipo Establecimiento", "0", true);
                resultadoListado.Insert(0, opcionPorDefecto);
                return resultadoListado;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task<List<SelectListItem>> ObtenerDepartamentos()
        {
            try
            {
                _logger.LogInformation("method called");
                List<Departamentos> departamentos = new List<Departamentos>();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = rutaAPI + "/VisitRecords/ConsultDepartments";
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
                        departamentos = JsonConvert.DeserializeObject<List<Departamentos>>(respuesta.Response.ToString() ?? "") ?? new List<Departamentos>();
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", "");
                    }
                }

                var resultadoListado = departamentos.Select(x => new SelectListItem(x.A003nombre, x.PkT003codigo.ToString())).ToList();
                var opcionPorDefecto = new SelectListItem("Seleccione un Departamento", "0", true);
                resultadoListado.Insert(0, opcionPorDefecto);

                return resultadoListado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var resultadoListado = new List<SelectListItem>();
                var opcionPorDefecto = new SelectListItem("Seleccione un Departamento", "0", true);
                resultadoListado.Insert(0, opcionPorDefecto);
                return resultadoListado;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="departamentoId"></param>
        /// <returns></returns>
        private async Task<List<SelectListItem>> ObtenerCiudades(decimal departamentoId = 0)
        {
            try
            {
                _logger.LogInformation("method called");
                List<Ciudades> ciudades = new List<Ciudades>();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = rutaAPI + $"/VisitRecords/ConsultCitiesByDepartment?departmentId={departamentoId}";
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
                        ciudades = JsonConvert.DeserializeObject<List<Ciudades>>(respuesta.Response.ToString() ?? "") ?? new List<Ciudades>();
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", "");
                    }
                }

                var resultadoListado = ciudades.Select(x => new SelectListItem(x.A004nombre, x.PkT004codigo.ToString())).ToList();

                var opcionPorDefecto = new SelectListItem("Seleccione una Ciudad", "", true);
                resultadoListado.Insert(0, opcionPorDefecto);

                return resultadoListado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var resultadoListado = new List<SelectListItem>();
                var opcionPorDefecto = new SelectListItem("Seleccione una Ciudad", "", true);
                resultadoListado.Insert(0, opcionPorDefecto);
                return resultadoListado;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoEstablecimiento"></param>
        /// <returns></returns>
        private async Task<List<SelectListItem>> ObtenerEstablecimientosPorTipo(decimal tipoEstablecimiento = 0)
        {
            try
            {
                _logger.LogInformation("method called");
                List<EstablecimientoProps> establecimientos = new List<EstablecimientoProps>();
                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = rutaAPI + $"/VisitRecords/ConsultBussinesByType?typeEstablishment={tipoEstablecimiento}";
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
                        establecimientos = JsonConvert.DeserializeObject<List<EstablecimientoProps>>(respuesta.Response.ToString() ?? "") ?? new List<EstablecimientoProps>();
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", "");
                    }
                }

                var resultadoListado = establecimientos.Select(x => new SelectListItem(x.NombreEstablecimiento, x.EstablecimientoID.ToString())).ToList();

                var opcionPorDefecto = new SelectListItem("Seleccione un Establecimiento", "0", true);
                resultadoListado.Insert(0, opcionPorDefecto);

                return resultadoListado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var resultadoListado = new List<SelectListItem>();
                var opcionPorDefecto = new SelectListItem("Seleccione un Establecimiento", "0", true);
                resultadoListado.Insert(0, opcionPorDefecto);
                return resultadoListado;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Nit"></param>
        /// <returns></returns>
        private async Task<EstablecimientoProps> ObtenerEmpresaPorNit(decimal Nit = 0)
        {
            try
            {
                _logger.LogInformation("method called");
                EstablecimientoProps establecimiento = new EstablecimientoProps();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = rutaAPI + $"/VisitRecords/ConsultBussinesByNIT?nit={Nit}";
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
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", "");
                    }

                    if (!respuesta.Error)
                        establecimiento = JsonConvert.DeserializeObject<EstablecimientoProps>(respuesta.Response.ToString() ?? "") ?? new EstablecimientoProps();

                }
                return establecimiento;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                EstablecimientoProps establecimiento = new EstablecimientoProps();
                return establecimiento;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<SelectListItem>> ObtenerEstablecimientosSelectList()
        {
            try
            {
                _logger.LogInformation("method called");
                List<EmpresasEstablecimientos> empresasEstablecimientos = new List<EmpresasEstablecimientos>();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = rutaAPI + "/VisitRecords/ConsultBusiness";
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
                        empresasEstablecimientos = JsonConvert.DeserializeObject<List<EmpresasEstablecimientos>>(respuesta.Response.ToString() ?? "") ?? new List<EmpresasEstablecimientos>();
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", "");
                    }

                }
                var resultadoListado = empresasEstablecimientos.Select(x => new SelectListItem(x.A001nombre, x.PkT001codigo.ToString())).ToList();
                var opcionPorDefecto = new SelectListItem("Seleccione Establecimiento", "0", true);
                resultadoListado.Insert(0, opcionPorDefecto);
                return resultadoListado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var resultadoListado = new List<SelectListItem>();
                var opcionPorDefecto = new SelectListItem("Seleccione Establecimiento", "0", true);
                resultadoListado.Insert(0, opcionPorDefecto);
                return resultadoListado;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="criterios"></param>
        /// <returns></returns>
        private async Task<List<ActaRegistro>> ObtenerActasTipoEstablecimiento(RegistroActasBusqueda criterios)
        {
            try
            {
                _logger.LogInformation("method called");
                var listadoRegistros = new List<ActaRegistro>();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = $"{rutaAPI}/VisitRecords/ConsultActsEstablishmentsByType";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var req = new
                {
                    establishmentId = criterios.IdEstablecimiento,
                    establishmentTypeId = criterios.IdTipoEstablecimiento,
                    visitDate = criterios.FechaVisita,
                    searchType = criterios.TipoBusqueda
                };

                var response = await httpClient.PostAsJsonAsync(URI, req);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                    if (!String.IsNullOrEmpty(respuesta.Token))
                    {
                        HttpContext.Session.SetString("token", respuesta.Token);
                        listadoRegistros = JsonConvert.DeserializeObject<List<ActaRegistro>>(respuesta.Response.ToString() ?? "") ?? new List<ActaRegistro>();
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", "");
                    }
                }

                if (listadoRegistros.Any())
                {
                    foreach (var item in listadoRegistros)
                    {
                        item.ReportType = ValidarTipoActa(item.ReportTypeId);
                        item.FechaFormat = item.Date.ToString("dd/MM/yyyy");
                        item.NumerosVisitas = ObtenerVisitasString(item.VisitNumberOne, item.VisitNumberTwo);
                        item.FechaRegistroFormat = FormatearFechaDecimal(item.CreationDateDecimal);
                    }
                }
                return listadoRegistros;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var listadoRegistros = new List<ActaRegistro>();
                return listadoRegistros;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idActaVisita"></param>
        /// <returns></returns>
        private async Task<EditVisitaActaCortes> ObtenerActaVisita(decimal idActaVisita)
        {
            try
            {
                _logger.LogInformation("method called");
                var registroEdit = new EditVisitaActaCortes();
                var listRegistroEdit = new List<EditVisitaActaCortes>();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = $"{rutaAPI}/VisitRecords/ConsultActVisitById?idActaVisit={idActaVisita}";
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
                        listRegistroEdit = JsonConvert.DeserializeObject<List<EditVisitaActaCortes>>(respuesta.Response.ToString() ?? "");
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", "");
                    }
                }

                if (listRegistroEdit != null && listRegistroEdit.Any())
                {
                    registroEdit.ActaVisitaId = listRegistroEdit[0].ActaVisitaId;
                    registroEdit.VisitaNumero = listRegistroEdit[0].VisitaNumero;
                    registroEdit.TipoEstablecimiento = listRegistroEdit[0].TipoEstablecimiento;
                    registroEdit.TipoEstablecimientoNombre = listRegistroEdit[0].TipoEstablecimientoNombre;
                    registroEdit.CantidadPielACortar = listRegistroEdit[0].CantidadPielACortar;
                    registroEdit.PrecintoIdentificacion = listRegistroEdit[0].PrecintoIdentificacion;
                    registroEdit.EstadoPiel = listRegistroEdit[0].EstadoPiel;
                    registroEdit.FuncionarioAutoridadCites = listRegistroEdit[0].FuncionarioAutoridadCites;
                    registroEdit.DocumentoRepresentante = listRegistroEdit[0].DocumentoRepresentante;
                    registroEdit.RepresentanteEstablecimiento = listRegistroEdit[0].RepresentanteEstablecimiento;
                    registroEdit.Fecha = listRegistroEdit[0].Fecha;
                    registroEdit.FechaFormat = listRegistroEdit[0].Fecha.ToString("d");
                    registroEdit.EstablecimientoID = listRegistroEdit[0].EstablecimientoID;
                    registroEdit.NombreEstablecimiento = listRegistroEdit[0].NombreEstablecimiento;
                    registroEdit.TipoActaVisita = listRegistroEdit[0].TipoActaVisita;
                    registroEdit.Ciudad = listRegistroEdit[0].Ciudad;
                    registroEdit.Departamento = listRegistroEdit[0].Departamento;
                    registroEdit.EstadoPielInt = ObtenerIdEstadoPiel(listRegistroEdit[0].EstadoPiel ?? "");
                    registroEdit.VisitaNumero1 = listRegistroEdit[0].VisitaNumero1;
                    registroEdit.VisitaNumero2 = listRegistroEdit[0].VisitaNumero2;
                    registroEdit.FuncionarioAutoridadCitesNombre = HttpContext.Session.GetString("User") ?? "";
                    registroEdit.NitEstablecimiento = listRegistroEdit[0].NitEstablecimiento;
                }

                return registroEdit;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var registroEdit = new EditVisitaActaCortes();
                return registroEdit;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idActaVisita"></param>
        /// <returns></returns>
        private async Task<EditVisitaActaCortes> ObtenerActaVisitaIrregular(decimal idActaVisita)
        {
            try
            {
                _logger.LogInformation("method called");
                var registroEdit = new EditVisitaActaCortes();
                var listRegistroEdit = new List<EditVisitaActaCortes>();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = $"{rutaAPI}/VisitRecords/ConsultActVisitById?idActaVisit={idActaVisita}";
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
                        listRegistroEdit = JsonConvert.DeserializeObject<List<EditVisitaActaCortes>>(respuesta.Response.ToString() ?? "");
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", "");
                    }
                }

                if (listRegistroEdit != null && listRegistroEdit.Any())
                {
                    registroEdit.ActaVisitaId = listRegistroEdit[0].ActaVisitaId;
                    registroEdit.VisitaNumero = listRegistroEdit[0].VisitaNumero;
                    registroEdit.TipoEstablecimiento = listRegistroEdit[0].TipoEstablecimiento;
                    registroEdit.TipoEstablecimientoNombre = listRegistroEdit[0].TipoEstablecimientoNombre;
                    registroEdit.CantidadPielACortar = listRegistroEdit[0].CantidadPielACortar;
                    registroEdit.PrecintoIdentificacion = listRegistroEdit[0].PrecintoIdentificacion;
                    registroEdit.EstadoPiel = listRegistroEdit[0].EstadoPiel;
                    registroEdit.FuncionarioAutoridadCites = listRegistroEdit[0].FuncionarioAutoridadCites;
                    registroEdit.RepresentanteEstablecimiento = listRegistroEdit[0].RepresentanteEstablecimiento;
                    registroEdit.DocumentoRepresentante = listRegistroEdit[0].DocumentoRepresentante;
                    registroEdit.FechaFormat = listRegistroEdit[0].Fecha.ToString("d");
                    registroEdit.Fecha = listRegistroEdit[0].Fecha;
                    registroEdit.EstablecimientoID = listRegistroEdit[0].EstablecimientoID;
                    registroEdit.NombreEstablecimiento = listRegistroEdit[0].NombreEstablecimiento;
                    registroEdit.Ciudad = listRegistroEdit[0].Ciudad;
                    registroEdit.Departamento = listRegistroEdit[0].Departamento;
                    registroEdit.EstadoPielInt = ObtenerIdEstadoPiel(listRegistroEdit[0].EstadoPiel ?? "");
                    registroEdit.VisitaNumero1 = listRegistroEdit[0].VisitaNumero1;
                    registroEdit.VisitaNumero2 = listRegistroEdit[0].VisitaNumero2;
                    registroEdit.FuncionarioAutoridadCitesNombre = HttpContext.Session.GetString("User") ?? "";
                    registroEdit.NitEstablecimiento = listRegistroEdit[0].NitEstablecimiento;
                }

                return registroEdit;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var registroEdit = new EditVisitaActaCortes();
                return registroEdit;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoActa"></param>
        /// <returns></returns>
        private string ValidarTipoActa(decimal tipoActa)
        {
            try
            {
                _logger.LogInformation("method called");
                var stringTipoActa = "";
                if (tipoActa == 1)
                    stringTipoActa = "Acta de control corte pieles partes identificables";
                if (tipoActa == 2)
                    stringTipoActa = "Acta de control corte pieles fracciones irregulares";

                return stringTipoActa;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return "";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruta"></param>
        /// <returns></returns>
        private async Task<List<IdRegistrosDocumentos>> ObtenerDatosDocumentos(string ruta)
        {
            try
            {
                _logger.LogInformation("method called");
                var listNumerosDoc = new List<IdRegistrosDocumentos>();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = ruta;
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
                        listNumerosDoc = JsonConvert.DeserializeObject<List<IdRegistrosDocumentos>>(respuesta.Response.ToString() ?? "") ?? new List<IdRegistrosDocumentos>();
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", "");
                    }
                }
                return listNumerosDoc;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var listNumerosDoc = new List<IdRegistrosDocumentos>();
                return listNumerosDoc;

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruta"></param>
        /// <returns></returns>
        private async Task<List<IdRegistrosDocumentosString>> ObtenerDatosDocumentosString(string ruta)
        {
            try
            {
                _logger.LogInformation("method called");
                var listNumerosDoc = new List<IdRegistrosDocumentosString>();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = ruta;
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
                        listNumerosDoc = JsonConvert.DeserializeObject<List<IdRegistrosDocumentosString>>(respuesta.Response.ToString() ?? "") ?? new List<IdRegistrosDocumentosString>();
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", "");
                    }
                }
                return listNumerosDoc;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var listNumerosDoc = new List<IdRegistrosDocumentosString>();
                return listNumerosDoc;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruta"></param>
        /// <returns></returns>
        private async Task<bool> BorrarDatos(string ruta)
        {
            try
            {
                _logger.LogInformation("method called");
                var listNumerosDoc = true;

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = ruta;
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.DeleteAsync(URI);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                    if (!String.IsNullOrEmpty(respuesta.Token))
                        HttpContext.Session.SetString("token", respuesta.Token);
                    else
                        HttpContext.Session.SetString("token", "");


                    if (respuesta.Error)
                        listNumerosDoc = false;
                }
                return listNumerosDoc;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idActaVisita"></param>
        /// <returns></returns>
        private async Task<List<TipoCortesPielIdentificables>> ObtenerTipoPielIdentificable(decimal idActaVisita)
        {
            try
            {
                _logger.LogInformation("method called");
                var listaTipoPiel = new List<TipoCortesPielIdentificables>();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = $"{rutaAPI}/VisitRecords/ConsultSkinTypeIdentfiableActVisit?idActaVisit={idActaVisita}";
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
                        listaTipoPiel = JsonConvert.DeserializeObject<List<TipoCortesPielIdentificables>>(respuesta.Response.ToString() ?? "") ?? new List<TipoCortesPielIdentificables>();
                    }
                    else
                        HttpContext.Session.SetString("token", "");

                }

                return listaTipoPiel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var listaTipoPiel = new List<TipoCortesPielIdentificables>();
                return listaTipoPiel;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idActaVisita"></param>
        /// <returns></returns>
        private async Task<List<TipoPartesPielIdentificables>> ObtenerTipoParteIdentificable(decimal idActaVisita)
        {
            try
            {
                _logger.LogInformation("method called");
                var listaTipoParteIdentificable = new List<TipoPartesPielIdentificables>();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = $"{rutaAPI}/VisitRecords/ConsultIdentfiablePartTypeActVisit?idActaVisit={idActaVisita}";
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
                        listaTipoParteIdentificable = JsonConvert.DeserializeObject<List<TipoPartesPielIdentificables>>(respuesta.Response.ToString() ?? "") ?? new List<TipoPartesPielIdentificables>();
                    }
                    else
                        HttpContext.Session.SetString("token", "");
                }

                return listaTipoParteIdentificable;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var listaTipoParteIdentificable = new List<TipoPartesPielIdentificables>();
                return listaTipoParteIdentificable;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idActaVisita"></param>
        /// <returns></returns>
        private async Task<List<TiposPielIrregulares>> ObtenerTipoPielIrregulares(decimal idActaVisita)
        {
            try
            {
                _logger.LogInformation("method called");
                var listaTipoPiel = new List<TiposPielIrregulares>();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = $"{rutaAPI}/VisitRecords/ConsultIrregularSkinTypeActVisit?idActaVisit={idActaVisita}";
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
                        listaTipoPiel = JsonConvert.DeserializeObject<List<TiposPielIrregulares>>(respuesta.Response.ToString() ?? "") ?? new List<TiposPielIrregulares>();
                    }
                    else
                        HttpContext.Session.SetString("token", "");

                }
                return listaTipoPiel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var listaTipoPiel = new List<TiposPielIrregulares>();
                return listaTipoPiel;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idActaVisita"></param>
        /// <returns></returns>
        private async Task<List<TiposParteIrregulares>> ObtenerTiposPartesIrregulares(decimal idActaVisita)
        {
            try
            {
                _logger.LogInformation("method called");
                var listaTiposParteIrregulares = new List<TiposParteIrregulares>();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = $"{rutaAPI}/VisitRecords/ConsultIrregularPartTypeActVisit?idActaVisit={idActaVisita}";
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
                        listaTiposParteIrregulares = JsonConvert.DeserializeObject<List<TiposParteIrregulares>>(respuesta.Response.ToString() ?? "") ?? new List<TiposParteIrregulares>();
                    }
                    else
                        HttpContext.Session.SetString("token", "");

                }
                return listaTiposParteIrregulares;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var listaTiposParteIrregulares = new List<TiposParteIrregulares>();
                return listaTiposParteIrregulares;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idActaVisita"></param>
        /// <returns></returns>
        private async Task<List<ActaVisitasPropArchivos>> ObtenerArchivosActaVisita(decimal idActaVisita)
        {
            try
            {
                _logger.LogInformation("method called");
                var listaArchivosActaVisita = new List<ActaVisitasPropArchivos>();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = $"{rutaAPI}/VisitRecords/ConsultFilePDFActVisit?idActaVisit={idActaVisita}";
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
                        listaArchivosActaVisita = JsonConvert.DeserializeObject<List<ActaVisitasPropArchivos>>(respuesta.Response.ToString() ?? "") ?? new List<ActaVisitasPropArchivos>();
                    }
                    else
                        HttpContext.Session.SetString("token", "");

                }
                return listaArchivosActaVisita;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var listaArchivosActaVisita = new List<ActaVisitasPropArchivos>();
                return listaArchivosActaVisita;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idActaVisita"></param>
        /// <returns></returns>
        private async Task<ActaVisitasPropArchivos> ObtenerExcelPrecinto(decimal idActaVisita)
        {
            try
            {
                _logger.LogInformation("method called");
                var archivoExcel = new ActaVisitasPropArchivos();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = $"{rutaAPI}/VisitRecords/ConsultFileExcelSeals?idActaVisit={idActaVisita}";
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
                        archivoExcel = JsonConvert.DeserializeObject<ActaVisitasPropArchivos>(respuesta.Response.ToString() ?? "") ?? new ActaVisitasPropArchivos();
                    }
                    else
                        HttpContext.Session.SetString("token", "");
                }

                return archivoExcel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var archivoExcel = new ActaVisitasPropArchivos();
                return archivoExcel;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="base64Excel"></param>
        /// <param name="nitEmpresa"></param>
        /// <returns></returns>
        private async Task<Responses> ValidarDatosExcelPrecintos(string base64Excel, decimal nitEmpresa)
        {
            try
            {
                _logger.LogInformation("method called");
                var archivoExcelPrecintos = new ArchivoExcelPrecintos();
                archivoExcelPrecintos.base64Excel = base64Excel;
                archivoExcelPrecintos.NIT = nitEmpresa;

                Responses respuesta = new Responses();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = $"{rutaAPI}/VisitRecords/ValidateDataExcelSeals";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.PostAsJsonAsync(URI, archivoExcelPrecintos);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                    if (!String.IsNullOrEmpty(respuesta.Token))
                    {
                        HttpContext.Session.SetString("token", respuesta.Token);

                    }
                    else
                        HttpContext.Session.SetString("token", "");
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                Responses respuesta = new Responses();
                respuesta.Error = true;
                respuesta.Message = ex.Message;
                return respuesta;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SelectListItem>> ObtenerTiposPartesLista()
        {
            try
            {
                _logger.LogInformation("method called");
                var listaTiposPartes = new List<GestionParametrica>();
                var listaSelectTiposPartes = new List<SelectListItem>();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = $"{rutaAPI}/VisitRecords/ConsultTypePartSkin";
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
                        listaTiposPartes = JsonConvert.DeserializeObject<List<GestionParametrica>>(respuesta.Response.ToString() ?? "") ?? new List<GestionParametrica>();
                    }
                    else
                        HttpContext.Session.SetString("token", "");

                }

                if (listaTiposPartes.Any())
                {
                    listaSelectTiposPartes = listaTiposPartes.Select(x => new SelectListItem(x.a008Valor, x.pkT008codigo.ToString())).ToList();
                }

                var opcionPorDefecto = new SelectListItem("Seleccione un tipo de parte", "0", true);
                listaSelectTiposPartes.Insert(0, opcionPorDefecto);

                return listaSelectTiposPartes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                var listaSelectTiposPartes = new List<SelectListItem>();
                var opcionPorDefecto = new SelectListItem("Seleccione un tipo de parte", "0", true);
                listaSelectTiposPartes.Insert(0, opcionPorDefecto);
                return listaSelectTiposPartes;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="valorIndice"></param>
        /// <returns></returns>
        private string ObtenerValorEstadoPiel(int valorIndice)
        {
            try
            {
                _logger.LogInformation("method called");
                var listaEstadosPiel = ObtenerListaEstadosPiel();

                var estadoPiel = listaEstadosPiel.Where(x => x.Id == valorIndice)
                                                  .Select(x => x.Nombre).FirstOrDefault() ?? "";

                return estadoPiel;
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
        /// <param name="estadoPiel"></param>
        /// <returns></returns>
        private int ObtenerIdEstadoPiel(string estadoPiel)
        {
            try
            {
                _logger.LogInformation("method called");
                if (String.IsNullOrEmpty(estadoPiel))
                    return 0;


                var listaEstadosPiel = ObtenerListaEstadosPiel();

                var idEstadoPiel = listaEstadosPiel.Where(x => x.Nombre == estadoPiel)
                                                  .Select(x => x.Id).FirstOrDefault();


                return idEstadoPiel;
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
        private List<PropsSelecListItems> ObtenerListaEstadosPiel()
        {
            try
            {
                _logger.LogInformation("method called");
                var listaEstadosPiel = new List<PropsSelecListItems>() {
                   new PropsSelecListItems()
                   {
                       Id = 0,
                       Nombre = "CRUDAS"
                   },
                   new PropsSelecListItems()
                   {
                       Id = 1,
                       Nombre = "AZÚL HUMEDO"
                   },
                   new PropsSelecListItems()
                   {
                       Id = 2,
                       Nombre = "CURTIDAS"
                   },
                   new PropsSelecListItems()
                   {
                       Id = 3,
                       Nombre = "TERMINADAS"
                   }

            };

                return listaEstadosPiel;
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
        /// <param name="documentos"></param>
        /// <param name="codigoActa"></param>
        /// <returns></returns>
        private async Task<bool> IngresarArchivosActaVisita(List<ActaVisitasPropArchivos> documentos, int codigoActa)
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = rutaAPI + "/VisitRecords/InsertFileActVisit";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                if (documentos.Any())
                {
                    foreach (var item in documentos)
                    {
                        item.Code = codigoActa;
                        var response = await httpClient.PostAsJsonAsync(URI, item);

                        if (response.IsSuccessStatusCode)
                        {
                            string responseString = await response.Content.ReadAsStringAsync();
                            Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                            if (!String.IsNullOrEmpty(respuesta.Token))
                            {
                                HttpContext.Session.SetString("token", respuesta.Token);
                            }
                            else
                                HttpContext.Session.SetString("token", "");

                            if (respuesta.Error)
                                return false;
                        }
                    }
                }
                return true;
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
        /// <param name="departamento"></param>
        /// <param name="ciudad"></param>
        /// <returns></returns>
        private async Task<string> ObtenerNombreCiudadDepartamento(decimal departamento, decimal ciudad)
        {

            try
            {
                _logger.LogInformation("method called");
                var departamentos = await ObtenerDepartamentos();
                var ciudades = await ObtenerCiudades(departamento);

                var nombreDepartamento = "";
                var nombreCiudad = "";

                nombreDepartamento = departamentos.Where(x => x.Value == departamento.ToString())
                                                  .Select(x => x.Text).FirstOrDefault();
                nombreCiudad = ciudades.Where(x => x.Value == ciudad.ToString())
                                       .Select(x => x.Text).FirstOrDefault();

                return $"{nombreCiudad}, {nombreDepartamento}";
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
        /// <param name="primeraVisita"></param>
        /// <param name="SegundaVisita"></param>
        /// <returns></returns>
        private string ObtenerVisitasString(bool primeraVisita, bool SegundaVisita)
        {
            try
            {
                _logger.LogInformation("method called");
                var visitas = "";

                if (primeraVisita)
                    visitas = "1 -";
                if (SegundaVisita)
                    visitas += " 2";

                if (visitas.Length > 0)
                {
                    var ultimoCaracter = visitas.Substring(visitas.Length - 1);
                    if (ultimoCaracter == "-")
                        visitas = visitas.Remove(visitas.Length - 1);
                }

                return visitas;
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
        /// <param name="criterosBusqueda"></param>
        /// <returns></returns>
        private int DefinirTipoBusquedaActas(RegistroActasBusqueda criterosBusqueda)
        {
            try
            {
                _logger.LogInformation("method called");
                if (criterosBusqueda.IdTipoEstablecimiento > 0 && criterosBusqueda.FechaVisita is null && (criterosBusqueda.IdEstablecimiento <= 0))
                {
                    return 1;
                }
                if (criterosBusqueda.IdTipoEstablecimiento > 0 && criterosBusqueda.FechaVisita is not null && (criterosBusqueda.IdEstablecimiento <= 0))
                {
                    return 2;
                }
                if (criterosBusqueda.IdEstablecimiento > 0 && criterosBusqueda.FechaVisita is null)
                {
                    return 3;
                }
                if (criterosBusqueda.IdEstablecimiento > 0 && criterosBusqueda.FechaVisita is not null)
                {
                    return 4;
                }
                if (criterosBusqueda.FechaVisita is not null && criterosBusqueda.IdEstablecimiento <= 0 && criterosBusqueda.IdTipoEstablecimiento <= 0)
                {
                    return 5;
                }
                if (criterosBusqueda.FechaVisita is null && criterosBusqueda.IdEstablecimiento <= 0 && criterosBusqueda.IdTipoEstablecimiento <= 0)
                {
                    return -1;
                }

                return 0;
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
        /// <param name="fecha"></param>
        /// <returns></returns>
        private string FormatearFechaDecimal(decimal fecha)
        {
            try
            {

                _logger.LogInformation("method called");

                if (fecha > 10000000)
                {
                    string fechaString = fecha.ToString();
                    string fechaSietePrimerosNUmeros = fechaString.Substring(0, 8);
                    DateTime fechaFormat = DateTime.ParseExact(fechaSietePrimerosNUmeros, "yyyyMMdd", CultureInfo.InvariantCulture);
                    return fechaFormat.ToString("dd/MM/yyyy");
                }
                else
                    return "";

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }
    }
}
