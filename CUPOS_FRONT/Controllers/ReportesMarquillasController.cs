using ClosedXML.Excel;
using CUPOS_FRONT.Utilities;
using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.Helpers;
using System.Data;
using System.Net.Http.Headers;
using static CUPOS_FRONT.Models.ReportesMarquillasModels;

namespace CUPOS_FRONT.Controllers
{
    public class ReportesMarquillasController : Controller
    {
        private readonly ILogger<ReportesMarquillasController> _logger;
        private readonly IPDFMetodos metodosPDF;
        private readonly string rutaAPI;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI");

        public ReportesMarquillasController(IConfiguration configuration, IPDFMetodos metodosPDF, ILogger<ReportesMarquillasController> logger    )
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
        public ActionResult Index()
        {
            try
            {
                _logger.LogInformation("method called");
                var modelo = new FiltrosMarquillas();
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
        public List<DatosMarquillaModel> ObtenerDatosMarquillas(FiltrosMarquillas filtros)
        {
            try
            {
                _logger.LogInformation("method called");
                List<DatosMarquillaModel> datos = new List<DatosMarquillaModel>();

                string? token = HttpContext.Session.GetString("token");
                string URI = $"{rutaAPI}/TagsReports/ConsultDataTags";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.PostAsJsonAsync(URI, filtros).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);

                    if (!String.IsNullOrEmpty(respuesta.Token))
                    {
                        HttpContext.Session.SetString("token", respuesta.Token);
                        datos = JsonConvert.DeserializeObject<List<DatosMarquillaModel>>(respuesta.Response.ToString());
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", "");
                    }
                }

                if (datos is null)
                    datos = new List<DatosMarquillaModel>();

                return datos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        //[HttpPost]
        //public string ExportarExcelDataMarquillas(FiltrosMarquillas filtros)
        //{
        //    var nombreArchivo = $"Listado de Marquillas.xlsx";         
        //    var lista = ObtenerDatosMarquillasAPI(filtros);
        //    var fileArchivo = GenerarExcel(nombreArchivo, lista);
        //    var archivoByte = fileArchivo.FileContents;
        //    string base64String = Convert.ToBase64String(archivoByte, 0, archivoByte.Length);
        //    return base64String;
        //}

        //[HttpPost]
        //public string ExportarPDFDataMarquillas(FiltrosMarquillas filtros)
        //{
        //    var lista = ObtenerDatosMarquillasAPI(filtros);

        //    List<string> cabeceras = new List<string>
        //    {
        //        "Radicado","Fecha Radicado","Nombre de Empresa","NIT", "Ciudad", "Direccion ", "Telefono","Especie"
        //    };

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        PdfWriter writer = new PdfWriter(ms);
        //        using (PdfDocument pdfDoc = new PdfDocument(writer))
        //        {

        //            Document doc = new Document(pdfDoc, iText.Kernel.Geom.PageSize.A4.Rotate());
        //            doc.SetMargins(10, 10, 10, 10);
        //            metodosPDF.crearParrafo(doc, "Reporte Cupos Empresas", 20, "center");
        //            metodosPDF.crearTabla<DatosMarquillaModel>(doc, cabeceras, new List<float> { 10, 10, 10, 10, 10, 10, 10, 10 }, lista, new List<string>
        //            {
        //                "Radicado","FechaRadicado","Establecimiento","NIT", "Ciudad", "Direccion", "Telefono",
        //                "Especie"
        //            });


        //            doc.Close();
        //            writer.Close();

        //        }
        //        return Convert.ToBase64String(ms.ToArray());
        //    }

        //}

        //private FileContentResult GenerarExcel(string nombreArchivo, List<DatosMarquillaModel> lista)
        //{
        //    DataTable dataTable = new DataTable("Precintos");
        //    dataTable.Columns.AddRange(new DataColumn[]
        //    {
        //        new DataColumn("Radicado"),
        //        new DataColumn("Fecha Radicado"),
        //        new DataColumn("Nombre Empresa"),
        //        new DataColumn("NIT"),
        //        new DataColumn("Ciudad"),
        //        new DataColumn("Direccion"),
        //        new DataColumn("Telefono"),
        //        new DataColumn("Especie"),
        //        new DataColumn("Tipo"),
        //        new DataColumn("Especimenes A Marcar"),
        //        new DataColumn("Cantidad"),
        //        new DataColumn("Color"),                     
        //        new DataColumn("Numeracion Inicial"),
        //        new DataColumn("Numeracion Final"),             
        //        new DataColumn("Valor Consignacion"),
        //        new DataColumn("Evaluador"),
        //        new DataColumn("Fecha Envio Respuesta"),


        //    });

        //    foreach (var item in lista)
        //    {
        //        dataTable.Rows.Add(
        //            item.Radicado,
        //            item.FechaRadicado,
        //            item.Establecimiento,
        //            item.NIT,
        //            item.Ciudad,
        //            item.Direccion,
        //            item.Telefono,
        //            item.Especie,
        //            item.Tipo,
        //            item.EspecimenesAMarcar,
        //            item.Cantidad,
        //            item.Color,                
        //            item.NumeracionInicial,
        //            item.NumeracionFinal,                 
        //            item.ValorConsignacion,
        //            item.Evaluador,
        //            item.FechaEnvioRespuesta
        //          );
        //    }

        //    using (XLWorkbook wb = new XLWorkbook())
        //    {
        //        var excel = wb.Worksheets.Add(dataTable);
        //        excel.Columns().AdjustToContents();

        //        using (MemoryStream stream = new MemoryStream())
        //        {
        //            wb.SaveAs(stream);
        //            return File(stream.ToArray(),
        //                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //                        nombreArchivo);
        //        }
        //    }
        //}
    }
}
