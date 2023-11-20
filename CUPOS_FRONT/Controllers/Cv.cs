using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.Helpers;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Security.Claims;
using Web.Models;
using WebFront.Controllers;
using static Web.Models.ResolucionPeces;

namespace Web.Controllers
{
    public class CvController : Controller
    {
        private readonly string UrlApi;
        private readonly ILogger<CvController> _logger;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public CvController(ILogger<CvController> logger)
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
        /// <param name="documentypecv"></param>
        /// <param name="documentid"></param>
        /// <returns></returns>
        public IActionResult Index(decimal documentypecv, decimal documentid)
        {
            try
            {
                _logger.LogInformation("method called");
                ViewBag.documentid = documentid;
                ViewBag.documentypecv = documentypecv;
                return View();
            }
            catch (Exception ex)
            {
               return ManejoIActionResultException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentypecv"></param>
        /// <param name="documentid"></param>
        /// <returns></returns>
        public IActionResult Index1(decimal documentypecv, decimal documentid)
        {
            try
            {
                _logger.LogInformation("method called");
                ViewBag.documentid = documentid;
                ViewBag.documentypecv = documentypecv;
                return View();
            }
            catch (Exception ex)
            {
                return ManejoIActionResultException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentypecv"></param>
        /// <param name="documentid"></param>
        /// <returns></returns>
        public IActionResult Index2(decimal documentypecv, decimal documentid)
        {
            try
            {
                _logger.LogInformation("method called");
                ViewBag.documentid = documentid;
                ViewBag.documentypecv = documentypecv;
                return View();
            }
            catch (Exception ex)
            {
                return ManejoIActionResultException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentypecv"></param>
        /// <param name="documentid"></param>
        /// <returns></returns>
        public IActionResult Ayuda()
        {
            try
            {
                _logger.LogInformation("method called");
                return View();
            }
            catch (Exception ex)
            {
                return ManejoIActionResultException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentypecv"></param>
        /// <param name="documentid"></param>
        /// <returns></returns>
        public object Buscar(decimal documentypecv, decimal documentid)
        {
            try
            {               
                string URI = UrlApi + "/CvA/Search?documentTypeCV=" + documentypecv + "&documentId=" + documentid;
                var respuesta = ProcesarDataApiGet<List<CvModel>>(URI);
                return respuesta;               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");               
                return new List<CvModel>(); 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentypecv"></param>
        /// <param name="documentid"></param>
        /// <returns></returns>
        public object Situacion(decimal documentypecv, decimal documentid)
        {
            try
            {         
                string URI = UrlApi + "/CvA/Situation?documentTypeCV=" + documentypecv + "&documentId=" + documentid;               
                var situacion = ProcesarDataApiGet<List<CvModel>>(URI);
                return situacion;               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");               
                return new List<CvModel>(); 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="documentypecv"></param>
        /// <param name="documentid"></param>
        /// <returns></returns>
        public object Resolucioncupos(ClaimsIdentity identity, decimal documentypecv, string documentid)
        {
            try
            {             
                string URI = UrlApi + "/Cva/QuotaResolution?documentTypeCV=" + documentypecv + "&documentId=" + documentid;
                var resolucionhj = (List<CvModel>)ProcesarDataApiGet<List<CvModel>>(URI);
                resolucionhj?.ForEach(el => el.anioProduccion = el.fechaProduccion.Year);
                return resolucionhj ?? new object{ };               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");               
                return new List<CvModel>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object ConsultCertificateshj()
        {
            try
            {             
                string URI = UrlApi + "/Cva/ConsultCertificateshj";
                var respuesta = ProcesarDataApiGet<List<CvModel>>(URI);
                return respuesta;      
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");            
                return new List<CvModel>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="documentid"></param>
        /// <returns></returns>
        public object ConsultPeces(ClaimsIdentity identity, decimal documentid)
        {
            try
            {              
                string URI = UrlApi + "/Cva/FishQuery?documentId=" + documentid;
                var respuesta = ProcesarDataApiGet<List<ResolucionPermisos>>(URI);
                return respuesta;               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");             
                return new List<ResolucionPermisos>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nit"></param>
        /// <returns></returns>
        public object DocumentoVenta(decimal nit)
        {
            try
            {              
                string URI = UrlApi + "/Cva/SalesDocument?nit=" + nit;
                var respuesta = ProcesarDataApiGet<List<CvModel>>(URI);
                return respuesta;
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<CvModel>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object DocumentoVenta2()
        {
            try
            {               
                string URI = UrlApi + "/Cva/SalesDocument";
                var respuesta = ProcesarDataApiGet<List<CvModel>>(URI);
                return respuesta;               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<CvModel>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="quotaCode"></param>
        /// <returns></returns>
        public object ConsultOneQuota2(ClaimsIdentity identity, decimal quotaCode)
        {
            try
            {            
               
                string URI = UrlApi + "/Cva/ConsultOneQuota2?quotaCode=" + quotaCode;
                var respuesta = ProcesarDataApiGet<resolutionQuota>(URI);
                return respuesta;         
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");               
                return new resolutionQuota();               
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="docuid"></param>
        /// <returns></returns>
        public object ConsultDocument2(ClaimsIdentity identity, decimal docuid)
        {
            try
            {
                string URI = UrlApi + "/Cva/ConsultDocument2?docId=" + docuid;
                var respuesta = ProcesarDataApiGet<CertificatesDatas>(URI);
                return respuesta;             
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new CertificatesDatas();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="docuid"></param>
        /// <returns></returns>
        public object ConsultDocumentid(ClaimsIdentity identity, decimal docuid)
        {
            try
            {              
                string URI = UrlApi + "/Cva/ConsultDocumentid?docId=" + docuid;
                var respuesta = ProcesarDataApiGet<List<CvModel>>(URI);
                return respuesta;             
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");              
                return new List<CvModel>(); 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="idcertificado"></param>
        /// <returns></returns>
        public object ConsultCertificateshj2(ClaimsIdentity identity, decimal idcertificado)
        {
            try
            {             
                string URI = UrlApi + "/Cva/ConsultCertificateshj2?certificateId=" + idcertificado;
                var respuesta = ProcesarDataApiGet<CertificatesDatas>(URI);
                return respuesta;           
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");              
                return new CertificatesDatas(); 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="situacionid"></param>
        /// <returns></returns>
        public object ConsultSituacionpdf(ClaimsIdentity identity, decimal situacionid)
        {
            try
            { 
                string URI = UrlApi + "/Cva/QuerySituationPDF?situationId=" + situacionid;
                var respuesta = ProcesarDataApiGet<CertificatesDatas>(URI);
                return respuesta;          
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new CertificatesDatas();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="codigoEmpresa"></param>
        /// <param name="situacionid"></param>
        /// <returns></returns>
        public object ConsultSituacionid(ClaimsIdentity identity, decimal codigoEmpresa, decimal situacionid)
        {
            try
            {             
                string URI = UrlApi + "/Cva/QuerySituationId?companyCode=" + codigoEmpresa + "&situationId=" + situacionid;
                var respuesta = ProcesarDataApiGet<List<CvModel>>(URI);
                return respuesta;
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");               
                return new List<CvModel>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="codigoEmpresa"></param>
        /// <returns></returns>
        public object ConsultSituacionnovedad(ClaimsIdentity identity, decimal codigoEmpresa)
        {
            try
            {               
                string URI = UrlApi + "/Cva/QueryNoveltySituation?companyCode=" + codigoEmpresa;
                var respuesta = ProcesarDataApiGet<List<CvModel>>(URI);
                return respuesta;               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<CvModel>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="codigoEmpresa"></param>
        /// <returns></returns>
        public object ConsultSituacionnovedadultima(ClaimsIdentity identity, decimal codigoEmpresa)
        {
            try
            {              
                string URI = UrlApi + "/Cva/QueryLatestNoveltySituation?companyCode=" + codigoEmpresa;
                var respuesta = ProcesarDataApiGet<List<CvModel>>(URI);
                return respuesta;            
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<CvModel>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idresolucionp"></param>
        /// <returns></returns>
        public object Consultpecespdf(decimal idresolucionp)
        {
            try
            {               
                string URI = UrlApi + "/Cva/QueryFishPDF?resolutionId=" + idresolucionp;
                var respuesta = ProcesarDataApiGet<ResolucionPermisos>(URI);
                return respuesta;               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");               
                return new ResolucionPermisos();
            }
        }


        private object ProcesarDataApiGet<T>(string URI) where T : new()
        {

            var httpClient = ConfigurarHttpClient();
            var response = httpClient.GetAsync(URI).Result;
            var data = ProcessHttpResponse<T>(response);
            return data ?? new object { };
        }
        private HttpClient ConfigurarHttpClient()
        {
            string? token = HttpContext.Session.GetString("token");
            var httpClient = getHttpClient();

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return httpClient;
        }

        private object ProcessHttpResponse<T>(HttpResponseMessage response) where T : new()
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return new { volverInicio = true };
            }

            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;
                Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                if (respuesta.Response != null)
                {
                    HttpContext.Session.SetString("token", respuesta.Token);
                    return JsonConvert.DeserializeObject<T>(respuesta.Response.ToString() ?? "") ?? new T();
                }
            }

            return new T();
        }

        private IActionResult ManejoIActionResultException(Exception ex)
        {
            _logger.LogError(ex, "An error occurred in the method.");
            string token = HttpContext.Session.GetString("token") ?? "";

            if (!String.IsNullOrEmpty(token))
                return RedirectToAction("FlujoNegocio", "Home");
            else
                return RedirectToAction("Index", "Login");
        }

    }
}
