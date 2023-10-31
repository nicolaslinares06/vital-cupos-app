using ClosedXML.Excel;
using CUPOS_FRONT.Utilities;
using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Repository.Helpers;
using System.Data;
using System.Net.Http.Headers;
using Web.Models;
using static CUPOS_FRONT.Models.ReportesCuposEmpresasMarcajeModels;

namespace CUPOS_FRONT.Controllers
{
    public class ReportesCuposEmpresasMarcajeController : Controller
    {
        private readonly ILogger<ReportesCuposEmpresasMarcajeController> _logger;
        private readonly string rutaAPI;
        private readonly IPDFMetodos metodosPDF;
        private readonly IFiltrosDeBusqueda classFiltros;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="metodosPDF"></param>
        /// <param name="classFiltros"></param>
        /// <param name="logger"></param>
        public ReportesCuposEmpresasMarcajeController(IConfiguration configuration, IPDFMetodos metodosPDF, IFiltrosDeBusqueda classFiltros, ILogger<ReportesCuposEmpresasMarcajeController> logger)
        {
            rutaAPI = string.IsNullOrEmpty(RUTAAPI) ? new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetValue<string>("Variables:RutaApi") : RUTAAPI;
            this.metodosPDF = metodosPDF;
            this.classFiltros = classFiltros;
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
                var modelo = new PropsViewModel();
                modelo.TipoEstablecimientos = await ObtenerTiposEstablecimientosSelect();
                modelo.Estados = await ObtenerEstadosParametricas();

                return View(modelo);
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
        public List<DatosEmpresasModel> DatosTablaReporte()
        {
            try
            {
                _logger.LogInformation("method called");
                List<DatosEmpresasModel> datos = new List<DatosEmpresasModel>();

                string? token = HttpContext.Session.GetString("token");
                string URI = $"{rutaAPI}/CompanyCheckInsReports/ConsultDataReportCompany";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);

                    if (!String.IsNullOrEmpty(respuesta.Token))
                    {
                        HttpContext.Session.SetString("token", respuesta.Token);
                        datos = JsonConvert.DeserializeObject<List<DatosEmpresasModel>>(respuesta.Response.ToString());
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", "");
                    }
                }

                if (datos is null)
                    datos = new List<DatosEmpresasModel>();

                return datos;
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
        /// <param name="filtros"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ObtenerDatosCuposEmpresas(ReporteCuposEmpresaViewModel filtros)
        {
            try
            {
                _logger.LogInformation("method called");
                filtros = NormalizarFiltros(filtros);

                var datosEmpresas = await ObtenerDatosEmpresaAPI(filtros);

                return new JsonResult(new { listado = datosEmpresas });
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
        /// <param name="dataCuposActual"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ExportarExcelDataEmpresas(ReporteCuposEmpresaViewModel dataCuposActual)
        {
            try
            {
                _logger.LogInformation("method called");
                var nombreArchivo = $"Listado de Cupos, Empresas y marcaje.xlsx";
                dataCuposActual = NormalizarFiltros(dataCuposActual);
                var lista = await ObtenerDatosEmpresaAPI(dataCuposActual);
                var fileArchivo = GenerarExcel(nombreArchivo, lista);
                var archivoByte = fileArchivo.FileContents;
                string base64String = Convert.ToBase64String(archivoByte, 0, archivoByte.Length);
                return base64String;
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
        /// <param name="dataCuposActual"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ExportarPDFDataEmpresas(ReporteCuposEmpresaViewModel dataCuposActual)
        {
            try
            {
                _logger.LogInformation("method called");
                dataCuposActual = NormalizarFiltros(dataCuposActual);
                var lista = await ObtenerDatosEmpresaAPI(dataCuposActual);

                List<string> cabeceras = new List<string>
            {
                "Tipo de Empresa","Nombre de Empresa","NIT de Empresa","Estado de Cupo", "Emision Cites", "Numero Resolucion", "Fecha Emision Resolucion","Especie", "Machos", "Hembras", "Poblacion Total", "Año de Produccion", "Cupos Comercializacion", "Cuota Repoblacion",
                 "Cupos Asignados", "Soportes Repoblacion", "Cupo Utilizado", "Cupo Disponible"
            };

                using (MemoryStream ms = new MemoryStream())
                {
                    PdfWriter writer = new PdfWriter(ms);
                    using (PdfDocument pdfDoc = new PdfDocument(writer))
                    {

                        Document doc = new Document(pdfDoc, iText.Kernel.Geom.PageSize.A4.Rotate());
                        doc.SetMargins(10, 10, 10, 10);
                        metodosPDF.crearParrafo(doc, "Reporte Cupos Empresas", 20, "center");
                        metodosPDF.crearTabla<DatosEmpresasModel>(doc, cabeceras, new List<float> { 10, 10, 10, 10, 10, 10, 10, 10 }, lista, new List<string>
                    {
                        "TipoEmpresa","NombreEmpresa","NIT","Estado", "EstadoEmisionCITES", "NumeroResolucion", "FechaEmisionResolucion",
                        "Especies", "Machos", "Hembras", "PoblacionTotalParental", "AnioProduccion", "CuposComercializacion", "CuotaRepoblacion",
                        "CuposAsignadosTotal", "SoportesRepoblacion", "CupoUtilizado", "CupoDisponible"
                    });


                        doc.Close();
                        writer.Close();

                    }
                    return Convert.ToBase64String(ms.ToArray());
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
        /// <param name="filtros"></param>
        /// <returns></returns>
        private async Task<List<DatosEmpresasModel>> ObtenerDatosEmpresaAPI(ReporteCuposEmpresaViewModel filtros)
        {
            try
            {
                _logger.LogInformation("method called");
                var datosEmpresas = new List<DatosEmpresasModel>();

                string? token = HttpContext.Session.GetString("token");
                string URI = $"{rutaAPI}/CompanyCheckInsReports/ConsultDataQuotaBussiness";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var req = new
                {
                    BusinessType = filtros.TipoEmpresa,
                    CompanyName = filtros.NombreEmpresa,
                    NIT = filtros.NIT,
                    Status = filtros.Estado,
                    CITESIssuanceStatus = filtros.EstadoEmisionCITES,
                    ResolutionNumber = filtros.NumeroResolucion,
                    ResolutionIssuanceStartDate = filtros.FechaEmisionResolucionDesde,
                    ResolutionIssuanceEndDate = filtros.FechaEmisionResolucionHasta,
                    SpecificSearch = filtros.BusquedaEspecifica,
                    CombinationType = filtros.CombinacionTipo
                };

                var response = httpClient.PostAsJsonAsync(URI, req).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);

                    if (!String.IsNullOrEmpty(respuesta.Token))
                    {
                        HttpContext.Session.SetString("token", respuesta.Token);
                        datosEmpresas = JsonConvert.DeserializeObject<List<DatosEmpresasModel>>(respuesta.Response.ToString());
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", "");
                    }
                }

                if (datosEmpresas is null)
                    datosEmpresas = new List<DatosEmpresasModel>();

                return datosEmpresas;
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
        /// <param name="nombreArchivo"></param>
        /// <param name="lista"></param>
        /// <returns></returns>
        private FileContentResult GenerarExcel(string nombreArchivo, List<DatosEmpresasModel> lista)
        {
            try
            {
                _logger.LogInformation("method called");
                DataTable dataTable = new DataTable("Cupos, Empresas y Marcaje");
                dataTable.Columns.AddRange(new DataColumn[]
                {
                new DataColumn("Nombre Empresa"),
                new DataColumn("Tipo Empresa"),
                new DataColumn("NIT"),
                new DataColumn("Estado Emision Cites"),
                new DataColumn("Numero Resolucion"),
                new DataColumn("Fecha Emision Resolucion"),
                new DataColumn("Especie"),
                new DataColumn("Machos"),
                new DataColumn("Hembras"),
                new DataColumn("Poblacion Total"),
                new DataColumn("Anio Produccion"),
                new DataColumn("Cupos Comercializacion"),
                new DataColumn("Cuota Repoblacion"),
                new DataColumn("Cupos totales"),
                new DataColumn("Soporte Repoblacion"),
                new DataColumn("Cupo Utilizado"),
                new DataColumn("Cupo  Disponible"),


                });

                foreach (var item in lista)
                {
                    dataTable.Rows.Add(item.NombreEmpresa,
                        item.TipoEmpresa,
                        item.NIT,
                        item.EstadoEmisionCITES,
                        item.NumeroResolucion,
                        item.FechaEmisionResolucion,
                        item.Especies,
                        item.Machos,
                        item.Hembras,
                        item.PoblacionTotalParental,
                        item.AnioProduccion,
                        item.CuposComercializacion,
                        item.CuotaRepoblacion,
                        item.CuposAsignadosTotal,
                        item.SoportesRepoblacion,
                        item.CupoUtilizado,
                        item.CupoDisponible
                      );
                }

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dataTable);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(),
                                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    nombreArchivo);
                    }
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
        /// <param name="filtros"></param>
        /// <returns></returns>
        private ReporteCuposEmpresaViewModel ObtenerBusquedaEspecifica(ReporteCuposEmpresaViewModel filtros)
        {
            try
            {
                _logger.LogInformation("method called");
                List<FiltrosEmpresasBooleanos> datosPorDefectoFiltros = new List<FiltrosEmpresasBooleanos>();



                var datosFiltrados = new FiltrosEmpresasBooleanos();
                int combinaciones = 0;
                DateTime fechaPordefecto = new DateTime();
                fechaPordefecto = fechaPordefecto.AddDays(1);

                if (filtros.TipoEmpresa > 0)
                {
                    combinaciones++;
                    datosFiltrados.TipoEmpresa = true;
                }


                if (filtros.NombreEmpresa.Trim().Length > 0)
                {
                    combinaciones++;
                    datosFiltrados.NombreEmpresa = true;

                }

                if (filtros.NIT > 0)
                {
                    datosFiltrados.NIT = true;
                    combinaciones++;
                }


                if (filtros.Estado > 0)
                {
                    datosFiltrados.Estado = true;
                    combinaciones++;
                }


                if (filtros.EstadoEmisionCITES > 0)
                {
                    datosFiltrados.EstadoEmisionCITES = true;
                    combinaciones++;
                }


                if (filtros.NumeroResolucion > 0)
                {
                    datosFiltrados.NumeroResolucion = true;
                    combinaciones++;
                }

                if (filtros.FechaEmisionResolucionDesde > fechaPordefecto)
                {
                    datosFiltrados.FechaEmisionResolucionDesde = true;
                    combinaciones++;

                }

                if (filtros.FechaEmisionResolucionHasta > fechaPordefecto)
                {
                    datosFiltrados.FechaEmisionResolucionHasta = true;
                    combinaciones++;

                }



                if (combinaciones == 1)
                {
                    datosPorDefectoFiltros = classFiltros.CombinacionesDeUnFiltro();
                    filtros.CombinacionTipo = 1;
                }

                if (combinaciones == 2)
                {
                    datosPorDefectoFiltros = classFiltros.CombinacionesDeDosFiltros();
                    filtros.CombinacionTipo = 2;
                }

                var numeroBusqueda = datosPorDefectoFiltros
                                        .Where(d => d.TipoEmpresa == datosFiltrados.TipoEmpresa &&
                                               d.NombreEmpresa == datosFiltrados.NombreEmpresa &&
                                               d.NIT == datosFiltrados.NIT &&
                                               d.Estado == datosFiltrados.Estado &&
                                               d.EstadoEmisionCITES == datosFiltrados.EstadoEmisionCITES &&
                                               d.NumeroResolucion == datosFiltrados.NumeroResolucion)
                                        .Select(s => s.BusquedaEspecifica).FirstOrDefault();

                filtros.BusquedaEspecifica = numeroBusqueda;




                return filtros;
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
        private DateTime ObtenerFechaHastaMediaNoche(DateTime fecha)
        {
            try
            {
                _logger.LogInformation("method called");
                fecha = fecha.AddDays(1);
                fecha = fecha.AddSeconds(-1);

                return fecha;
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
        private DateTime EstandarizarFecha(DateTime fecha)
        {
            try
            {
                _logger.LogInformation("method called");
                var fechaEstandar = new DateTime(fecha.Year, fecha.Month, fecha.Day);
                return fechaEstandar;
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
        /// <param name="filtros"></param>
        /// <returns></returns>
        private ReporteCuposEmpresaViewModel NormalizarFiltros(ReporteCuposEmpresaViewModel filtros)
        {
            try
            {
                _logger.LogInformation("method called");
                if (filtros.NombreEmpresa is null)
                    filtros.NombreEmpresa = "";





                filtros.FechaEmisionResolucionDesde = EstandarizarFecha(filtros.FechaEmisionResolucionDesde);
                filtros.FechaEmisionResolucionHasta = EstandarizarFecha(filtros.FechaEmisionResolucionHasta);
                filtros.FechaEmisionResolucionHasta = ObtenerFechaHastaMediaNoche(filtros.FechaEmisionResolucionHasta);
                filtros = ObtenerBusquedaEspecifica(filtros);

                return filtros;
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
        private async Task<IEnumerable<SelectListItem>> ObtenerTiposEstablecimientosSelect()
        {
            try
            {
                _logger.LogInformation("method called");
                List<GestionParametrica> tiposEstablecimientos = new List<GestionParametrica>();

                string token = HttpContext.Session.GetString("token");
                string URI = rutaAPI + "/VisitRecords/ConsultTypesOfEstablishments";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = httpClient.GetAsync(URI).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);

                    if (!String.IsNullOrEmpty(respuesta.Token))
                    {
                        HttpContext.Session.SetString("token", respuesta.Token);
                        tiposEstablecimientos = JsonConvert.DeserializeObject<List<GestionParametrica>>(respuesta.Response.ToString());
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
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<SelectListItem>> ObtenerEstadosParametricas()
        {
            try
            {
                _logger.LogInformation("method called");
                var estados = new List<SelectListItem>() {
                new SelectListItem( "Activo" , StringHelper.estadoActivo.ToString()),
                new SelectListItem( "Inactivo" , StringHelper.estadoInactivo.ToString())
            };

                var opcionPorDefecto = new SelectListItem("Seleccione un Estado", "0", true);
                estados.Insert(0, opcionPorDefecto);
                return estados;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }
    }
}
