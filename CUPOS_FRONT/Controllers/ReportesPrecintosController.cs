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
using static CUPOS_FRONT.Models.ReportesPrecintosModels;


namespace CUPOS_FRONT.Controllers
{
    public class ReportesPrecintosController : Controller
    {
        private readonly ILogger<ReportesPrecintosController> _logger;
        private readonly string rutaAPI;
        private readonly IPDFMetodos metodosPDF;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="metodosPDF"></param>
        /// <param name="logger"></param>
        public ReportesPrecintosController(IConfiguration configuration, IPDFMetodos metodosPDF, ILogger<ReportesPrecintosController> logger  )
        {
            rutaAPI = string.IsNullOrEmpty(RUTAAPI) ? new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetValue<string>("Variables:RutaApi") : RUTAAPI;
            this.metodosPDF = metodosPDF;
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
                FiltrosPrecintos filtro = new FiltrosPrecintos();
                var modelo = new ReportesPrecintosViewModel();
                modelo.Establecimientos = ObtenerEstablecimientosSelect();
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
        /// <param name="filtros"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatosPrecintos(FiltrosPrecintos filtros)
        {
            try
            {
                _logger.LogInformation("method called");
                filtros = NormalizarFiltros(filtros);

                var datosEmpresas = ObtenerDatosPrecintosAPI(filtros);

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
        /// <param name="filtros"></param>
        /// <returns></returns>
        //[HttpPost]
        //public string ExportarExcelDataPrecintos(FiltrosPrecintos filtros)
        //{
        //    try
        //    {
        //        _logger.LogInformation("method called");
        //        var nombreArchivo = $"Listado de Precintos.xlsx";
        //        filtros = NormalizarFiltros(filtros);
        //        var lista = ObtenerDatosPrecintosAPI(filtros);
        //        var fileArchivo = GenerarExcel(nombreArchivo, lista);
        //        var archivoByte = fileArchivo.FileContents;
        //        string base64String = Convert.ToBase64String(archivoByte, 0, archivoByte.Length);
        //        return base64String;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred in the method.");
        //        throw;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filtros"></param>
        /// <returns></returns>
        [HttpPost]
        public  string ExportarPDFDataPrecintos(FiltrosPrecintos filtros)
        {
            try
            {
                _logger.LogInformation("method called");
                filtros = NormalizarFiltros(filtros);
                var lista = ObtenerDatosPrecintosAPI(filtros);

                List<string> cabeceras = new List<string>
            {
                "Numero de Radicado","Fecha Radicado","Nombre de Empresa","NIT", "Ciudad", "Direccion Entrega", "Telefono","Especie"
            };

                using (MemoryStream ms = new MemoryStream())
                {
                    PdfWriter writer = new PdfWriter(ms);
                    using (PdfDocument pdfDoc = new PdfDocument(writer))
                    {

                        Document doc = new Document(pdfDoc, iText.Kernel.Geom.PageSize.A4.Rotate());
                        doc.SetMargins(10, 10, 10, 10);
                        metodosPDF.crearParrafo(doc, "Reporte Cupos Empresas", 20, "center");
                        metodosPDF.crearTabla<DatosPrecintosModel>(doc, cabeceras, new List<float> { 10, 10, 10, 10, 10, 10, 10, 10 }, lista, new List<string>
                    {
                        "NumeroRadicado","FechaRadicado","NombreEmpresa","NIT", "Ciudad", "DireccionEntrega", "Telefono",
                        "Especie"
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
        //[HttpPost]
        //public string ExportarExcelDataPrecintos(FiltrosPrecintos filtros)
        //{

        //    var nombreArchivo = $"Listado de Precintos.xlsx";
        //    filtros = NormalizarFiltros(filtros);
        //    var lista =  ObtenerDatosPrecintosAPI(filtros);
        //    var fileArchivo = GenerarExcel(nombreArchivo, lista);
        //    var archivoByte = fileArchivo.FileContents;
        //    string base64String = Convert.ToBase64String(archivoByte, 0, archivoByte.Length);
        //    return base64String;
        //}


        //[HttpPost]
        //public  string ExportarPDFDataPrecintos(FiltrosPrecintos filtros)
        //{
        //    filtros = NormalizarFiltros(filtros);
        //    var lista =  ObtenerDatosPrecintosAPI(filtros);

        //    List<string> cabeceras = new List<string>
        //    {
        //        "Numero de Radicado","Fecha Radicado","Nombre de Empresa","NIT", "Ciudad", "Direccion Entrega", "Telefono","Especie"
        //    };

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        PdfWriter writer = new PdfWriter(ms);
        //        using (PdfDocument pdfDoc = new PdfDocument(writer))
        //        {

        //            Document doc = new Document(pdfDoc, iText.Kernel.Geom.PageSize.A4.Rotate());
        //            doc.SetMargins(10, 10, 10, 10);
        //            metodosPDF.crearParrafo(doc, "Reporte Cupos Empresas", 20, "center");
        //            metodosPDF.crearTabla<DatosPrecintosModel>(doc, cabeceras, new List<float> { 10, 10, 10, 10, 10, 10, 10, 10 }, lista, new List<string>
        //            {
        //                "NumeroRadicado","FechaRadicado","NombreEmpresa","NIT", "Ciudad", "DireccionEntrega", "Telefono",
        //                "Especie"
        //            });

        //            doc.Close();
        //            writer.Close();

        //        }
        //        return Convert.ToBase64String(ms.ToArray());
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filtros"></param>
        /// <returns></returns>
        [HttpPost]
        public List<DatosPrecintosModel> ObtenerDatosPrecintosAPI(FiltrosPrecintos? filtros)
        {
            try
            {
                _logger.LogInformation("method called");
                var datosEmpresas = new List<DatosPrecintosModel>();

                string? token = HttpContext.Session.GetString("token");
                string URI = $"{rutaAPI}/SealsReports/ConsultDataSeals";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var req = new
                {
                    ResolutionNumber = filtros.ResolutionNumber,
                    Establishment = filtros.Establishment,
                    NIT = filtros.NIT,
                    RadicationDate = filtros.RadicationDate,
                    SpecificSearch = filtros.SpecificSearch
                };

                var response = httpClient.PostAsJsonAsync(URI, req).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);

                    if (!String.IsNullOrEmpty(respuesta.Token))
                    {
                        HttpContext.Session.SetString("token", respuesta.Token);
                        datosEmpresas = JsonConvert.DeserializeObject<List<DatosPrecintosModel>>(respuesta.Response.ToString());
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", "");
                    }
                }

                if (datosEmpresas is null)
                    datosEmpresas = new List<DatosPrecintosModel>();

                return datosEmpresas;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }

        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="nombreArchivo"></param>
        ///// <param name="lista"></param>
        ///// <returns></returns>
        //private FileContentResult GenerarExcel(string nombreArchivo, List<DatosPrecintosModel> lista)
        //{
        //    try
        //    {
        //        _logger.LogInformation("method called");
        //        DataTable dataTable = new DataTable("Precintos");
        //        dataTable.Columns.AddRange(new DataColumn[]
        //        {
        //        new DataColumn("Numero Radicado"),
        //        new DataColumn("Fecha Radicado"),
        //        new DataColumn("Nombre Empresa"),
        //        new DataColumn("NIT"),
        //        new DataColumn("Ciudad"),
        //        new DataColumn("Direccion Entrega"),
        //        new DataColumn("Telefono"),
        //        new DataColumn("Especie"),
        //        new DataColumn("Longitud Menor"),
        //        new DataColumn("Longitud Mayor"),
        //        new DataColumn("Cantidad"),
        //        new DataColumn("Color"),
        //        new DataColumn("Año Produccion"),
        //        new DataColumn("Numeracion Interna Inicial"),
        //        new DataColumn("Numeracion Interna Final"),
        //        new DataColumn("Numeracion Inicial"),
        //        new DataColumn("Numeracion Final"),
        //        new DataColumn("Codigo Empresa"),
        //        new DataColumn("Valor Consignacion"),
        //        new DataColumn("Analista"),


        //        });

        //        foreach (var item in lista)
        //        {
        //            dataTable.Rows.Add(
        //                item.NumeroRadicado,
        //                item.FechaRadicado,
        //                item.NombreEmpresa,
        //                item.NIT,
        //                item.Ciudad,
        //                item.DireccionEntrega,
        //                item.Telefono,
        //                item.Especie,
        //                item.LongMenor,
        //                item.LongMayor,
        //                item.Cantidad,
        //                item.Color,
        //                item.AnioProduccion,
        //                item.NumeracionInternaInicial,
        //                item.NumeracionInternaFinal,
        //                item.NumeracionInicial,
        //                item.NumeracionFinal,
        //                item.CodigoEmpresa,
        //                item.ValorConsignacion,
        //                item.Analista
        //              );
        //        }

        //        using (XLWorkbook wb = new XLWorkbook())
        //        {
        //            var excel = wb.Worksheets.Add(dataTable);
        //            excel.Columns().AdjustToContents();

        //            using (MemoryStream stream = new MemoryStream())
        //            {
        //                wb.SaveAs(stream);
        //                return File(stream.ToArray(),
        //                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //                            nombreArchivo);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred in the method.");
        //        throw;
        //    }
        //}    

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filtros"></param>
        /// <returns></returns>
        private int ObtenerBusquedaEspecifica(FiltrosPrecintos filtros)
        {
            try
            {
                _logger.LogInformation("method called");
                var fechaPorDefecto = new DateTime().AddDays(1);

                //List<FiltrosPrecintos> datosPorDefectoFiltros = new List<FiltrosPrecintos>
                //{

                //    private FileContentResult GenerarExcel(string nombreArchivo, List<DatosPrecintosModel> lista)
                //    {
                //        DataTable dataTable = new DataTable("Precintos");
                //        dataTable.Columns.AddRange(new DataColumn[]
                //        {
                //                    new DataColumn("Numero Radicado"),
                //                    new DataColumn("Fecha Radicado"),
                //                    new DataColumn("Nombre Empresa"),
                //                    new DataColumn("NIT"),
                //                    new DataColumn("Ciudad"),
                //                    new DataColumn("Direccion Entrega"),
                //                    new DataColumn("Telefono"),
                //                    new DataColumn("Especie"),
                //                    new DataColumn("Longitud Menor"),
                //                    new DataColumn("Longitud Mayor"),
                //                    new DataColumn("Cantidad"),
                //                    new DataColumn("Color"),
                //                    new DataColumn("Año Produccion"),
                //                    new DataColumn("Numeracion Interna Inicial"),
                //                    new DataColumn("Numeracion Interna Final"),
                //                    new DataColumn("Numeracion Inicial"),
                //                    new DataColumn("Numeracion Final"),
                //                    new DataColumn("Codigo Empresa"),
                //                    new DataColumn("Valor Consignacion"),
                //                    new DataColumn("Analista"),


                //        });

                //        foreach (var item in lista)
                //        {
                //            dataTable.Rows.Add(
                //                item.NumeroRadicado,
                //                item.FechaRadicado,
                //                item.NombreEmpresa,
                //                item.NIT,
                //                item.Ciudad,
                //                item.DireccionEntrega,
                //                item.Telefono,
                //                item.Especie,
                //                item.LongMenor,
                //                item.LongMayor,
                //                item.Cantidad,
                //                item.Color,
                //                item.AnioProduccion,
                //                item.NumeracionInternaInicial,
                //                item.NumeracionInternaFinal,
                //                item.NumeracionInicial,
                //                item.NumeracionFinal,
                //                item.CodigoEmpresa,
                //                item.ValorConsignacion,
                //                item.Analista
                //              );
                //        }

                //        using (XLWorkbook wb = new XLWorkbook())
                //        {
                //            var excel = wb.Worksheets.Add(dataTable);
                //            excel.Columns().AdjustToContents();

                //            using (MemoryStream stream = new MemoryStream())
                //            {
                //                wb.SaveAs(stream);
                //                return File(stream.ToArray(),
                //                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                //                            nombreArchivo);
                //            }
                //        }
                //    }
                //}

                //private int ObtenerBusquedaEspecifica(FiltrosPrecintos filtros, FiltrosPrecintos? datosFiltrados)
                //{
                //    var fechaPorDefecto = new DateTime().AddDays(1);

                //    List<FiltrosPrecintos> datosPorDefectoFiltros = new List<FiltrosPrecintos> {
                //        new FiltrosPrecintos
                //        { NumeroResolucion = "a", Establecimiento = 0, NIT = 0, FechaRadicacion = fechaPorDefecto, BusquedaEspecifica = 1},
                //        new FiltrosPrecintos
                //        { NumeroResolucion = "", Establecimiento = 1, NIT = 0, FechaRadicacion = fechaPorDefecto,BusquedaEspecifica = 2},
                //        new FiltrosPrecintos
                //        { NumeroResolucion = "", Establecimiento = 0, NIT = 1, FechaRadicacion = fechaPorDefecto,BusquedaEspecifica = 3},
                //        new FiltrosPrecintos
                //        { NumeroResolucion = "a", Establecimiento = 1, NIT = 0, FechaRadicacion = fechaPorDefecto,BusquedaEspecifica = 4},
                //         new FiltrosPrecintos
                //        { NumeroResolucion = "a", Establecimiento = 0, NIT = 1, FechaRadicacion = fechaPorDefecto,BusquedaEspecifica = 5},
                //        new FiltrosPrecintos
                //        { NumeroResolucion = "", Establecimiento = 1, NIT = 1,FechaRadicacion = fechaPorDefecto,BusquedaEspecifica = 6},
                //         new FiltrosPrecintos
                //        { NumeroResolucion = "", Establecimiento = 1, NIT = 1, FechaRadicacion = fechaPorDefecto,BusquedaEspecifica = 7},

                //        new FiltrosPrecintos
                //        { NumeroResolucion = "", Establecimiento = 0, NIT = 0, FechaRadicacion = fechaPorDefecto,BusquedaEspecifica = 8},

                //         new FiltrosPrecintos
                //        { NumeroResolucion = "", Establecimiento = 1, NIT = 0, FechaRadicacion = fechaPorDefecto,BusquedaEspecifica = 9},

                //          new FiltrosPrecintos
                //        { NumeroResolucion = "a", Establecimiento = 0, NIT = 0, FechaRadicacion = fechaPorDefecto,BusquedaEspecifica = 10},
                //        new FiltrosPrecintos
                //        { NumeroResolucion = "", Establecimiento = 0, NIT = 1, FechaRadicacion = fechaPorDefecto, BusquedaEspecifica = 11},


                //    };

                //    var datosFiltrados = new FiltrosPrecintos();

                //    if (filtros.NumeroResolucion.Trim().Length > 0)
                //        datosFiltrados.NumeroResolucion = "a";

                //    if (filtros.Establecimiento > 0)
                //        datosFiltrados.Establecimiento = 1;

                //    if (filtros.NIT > 0)
                //        datosFiltrados.NIT = 1;

                //    if (filtros.FechaRadicacion > fechaPorDefecto)
                //        datosFiltrados.FechaRadicacion = filtros.FechaRadicacion;


                //    var numeroBusqueda = datosPorDefectoFiltros
                //                            .Where(d => d.NumeroResolucion == datosFiltrados.NumeroResolucion &&
                //                                   d.Establecimiento == datosFiltrados.Establecimiento &&
                //                                   d.NIT == datosFiltrados.NIT &&
                //                                   d.FechaRadicacion < datosFiltrados.FechaRadicacion
                //                                  )
                //                            .Select(s => s.BusquedaEspecifica).FirstOrDefault();

                //    return numeroBusqueda;
                //}
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
        private FiltrosPrecintos NormalizarFiltros(FiltrosPrecintos filtros)
        {
            try
            {
                _logger.LogInformation("method called");
                if (filtros.ResolutionNumber is null)
                    filtros.ResolutionNumber = "";

                filtros.SpecificSearch = ObtenerBusquedaEspecifica(filtros);

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
        private IEnumerable<SelectListItem> ObtenerEstablecimientosSelect()
        {
            try
            {
                _logger.LogInformation("method called");
                List<EstablecimientosPrecintos> tiposEstablecimientos = new List<EstablecimientosPrecintos>();

                string token = HttpContext.Session.GetString("token");
                string URI = rutaAPI + "/ReportesPrecintos/ConsultBussinesSeals";
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
                        tiposEstablecimientos = JsonConvert.DeserializeObject<List<EstablecimientosPrecintos>>(respuesta.Response.ToString());
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", "");
                    }


                }

                var resultadoListado = tiposEstablecimientos.Select(x => new SelectListItem(x.EstablishmentName, x.EstablishmentId.ToString())).ToList();

                var opcionPorDefecto = new SelectListItem("Seleccione un Establecimiento", "0", true);
                resultadoListado.Insert(0, opcionPorDefecto);



                return resultadoListado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }
    }
}
