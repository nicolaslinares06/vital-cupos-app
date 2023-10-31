using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.Helpers;
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
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI");
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
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
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
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
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
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
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
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
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
                _logger.LogInformation("method called");
                List<CvModel> hojadevida = new List<CvModel>();
                string token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/CvA/Search?documentTypeCV=" + documentypecv + "&documentId=" + documentid;
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
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);

                    if (respuesta.Response != null && !string.IsNullOrEmpty(respuesta.Response as string))
                    {
                        hojadevida = JsonConvert.DeserializeObject<List<CvModel>>(respuesta.Response.ToString());
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }

                return hojadevida;
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
        /// <param name="documentypecv"></param>
        /// <param name="documentid"></param>
        /// <returns></returns>
        public object Situacion(decimal documentypecv, decimal documentid)
        {
            try
            {
                _logger.LogInformation("method called");
                List<CvModel> situacionhj = new List<CvModel>();
                string token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/CvA/Situation?documentTypeCV=" + documentypecv + "&documentId=" + documentid;
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
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                    if (respuesta.Response != null && !string.IsNullOrEmpty(respuesta.Response as string))
                    {
                        situacionhj = JsonConvert.DeserializeObject<List<CvModel>>(respuesta.Response.ToString());
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }

                return situacionhj;
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
        /// <param name="identity"></param>
        /// <param name="documentypecv"></param>
        /// <param name="documentid"></param>
        /// <returns></returns>
        public object Resolucioncupos(ClaimsIdentity identity, decimal documentypecv, string documentid)
        {
            try
            {
                _logger.LogInformation("method called");
                List<CvModel> resolucioncuposhj = new List<CvModel>();
                string token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/Cva/QuotaResolution?documentTypeCV=" + documentypecv + "&documentId=" + documentid;
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
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                    if (respuesta != null)
                    {
                        resolucioncuposhj = JsonConvert.DeserializeObject<List<CvModel>>(respuesta.Response.ToString());
                        HttpContext.Session.SetString("token", respuesta.Token);

                        if (resolucioncuposhj != null && resolucioncuposhj.Count > 0)
                        {
                            resolucioncuposhj.ForEach(el =>
                            {
                                el.anioProduccion = el.fechaProduccion.Year;
                            });
                        }
                    }
                }

                return resolucioncuposhj;
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
        public object ConsultCertificateshj()
        {
            try
            {
                _logger.LogInformation("method called");
                List<CvModel> certificadoshj = new List<CvModel>();

                string token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/Cva/ConsultCertificateshj";
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
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                    if (respuesta.Response != null && !string.IsNullOrEmpty(respuesta.Response as string))
                    {
                        certificadoshj = JsonConvert.DeserializeObject<List<CvModel>>(respuesta.Response.ToString());
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }

                return certificadoshj;
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
        /// <param name="identity"></param>
        /// <param name="documentid"></param>
        /// <returns></returns>
        public object ConsultPeces(ClaimsIdentity identity, decimal documentid)
        {
            try
            {
                _logger.LogInformation("method called");
                List<ResolucionPermisos> datos = new List<ResolucionPermisos>();

                string token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/Cva/FishQuery?documentId=" + documentid;
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
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                    if (respuesta.Response != null && !string.IsNullOrEmpty(respuesta.Response as string))
                    {
                        datos = JsonConvert.DeserializeObject<List<ResolucionPermisos>>(respuesta.Response.ToString());
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }

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
        /// <param name="nit"></param>
        /// <returns></returns>
        public object DocumentoVenta(decimal nit)
        {
            try
            {
                _logger.LogInformation("method called");
                List<CvModel> documento = new List<CvModel>();

                string token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/Cva/SalesDocument?nit=" + nit;
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
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                    if (respuesta.Response != null && !string.IsNullOrEmpty(respuesta.Response as string))
                    {
                        documento = JsonConvert.DeserializeObject<List<CvModel>>(respuesta.Response.ToString());
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }


                return documento;
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
        public object DocumentoVenta2()
        {
            try
            {
                _logger.LogInformation("method called");
                List<CvModel> documento2 = new List<CvModel>();

                string token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/Cva/SalesDocument";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;
                //if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                //{
                //    return new
                //    {
                //        volverInicio = true
                //    };
                //}

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                    if (respuesta.Response != null && !string.IsNullOrEmpty(respuesta.Response as string))
                    {
                        documento2 = JsonConvert.DeserializeObject<List<CvModel>>(respuesta.Response.ToString());
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }

                return documento2;
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
        /// <param name="identity"></param>
        /// <param name="quotaCode"></param>
        /// <returns></returns>
        public object ConsultOneQuota2(ClaimsIdentity identity, decimal quotaCode)
        {
            try
            {
                _logger.LogInformation("method called");
                resolutionQuota resolutionQuota = new resolutionQuota();

                string token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/Cva/ConsultOneQuota2?quotaCode=" + quotaCode;
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
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                    if (respuesta.Response != null && !string.IsNullOrEmpty(respuesta.Response as string))
                    {
                        resolutionQuota = JsonConvert.DeserializeObject<resolutionQuota>(respuesta.Response.ToString());
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }


                return resolutionQuota;
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
        /// <param name="identity"></param>
        /// <param name="docuid"></param>
        /// <returns></returns>
        public object ConsultDocument2(ClaimsIdentity identity, decimal docuid)
        {
            try
            {
                _logger.LogInformation("method called");
                CertificatesDatas resouna2 = new CertificatesDatas();

                string token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/Cva/ConsultDocument2?docId=" + docuid;
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
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                    resouna2 = JsonConvert.DeserializeObject<CertificatesDatas>(respuesta.Response.ToString());
                    HttpContext.Session.SetString("token", respuesta.Token);
                }

                return resouna2;
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
        /// <param name="identity"></param>
        /// <param name="docuid"></param>
        /// <returns></returns>
        public object ConsultDocumentid(ClaimsIdentity identity, decimal docuid)
        {
            try
            {
                _logger.LogInformation("method called");
                List<CvModel> resouna2 = new List<CvModel>();

                string token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/Cva/ConsultDocumentid?docId=" + docuid;
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
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                    if (respuesta.Response != null && !string.IsNullOrEmpty(respuesta.Response as string))
                    {
                        resouna2 = JsonConvert.DeserializeObject<List<CvModel>>(respuesta.Response.ToString());
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }


                return resouna2;
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
        /// <param name="identity"></param>
        /// <param name="idcertificado"></param>
        /// <returns></returns>
        public object ConsultCertificateshj2(ClaimsIdentity identity, decimal idcertificado)
        {
            try
            {
                _logger.LogInformation("method called");
                CertificatesDatas resouna3 = new CertificatesDatas();

                string token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/Cva/ConsultCertificateshj2?certificateId=" + idcertificado;
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
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                    if (respuesta.Response != null && !string.IsNullOrEmpty(respuesta.Response as string))
                    {
                        resouna3 = JsonConvert.DeserializeObject<CertificatesDatas>(respuesta.Response.ToString());
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }

                return resouna3;
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
        /// <param name="identity"></param>
        /// <param name="situacionid"></param>
        /// <returns></returns>
        public object ConsultSituacionpdf(ClaimsIdentity identity, decimal situacionid)
        {
            try
            {
                _logger.LogInformation("method called");
                CertificatesDatas resopdf = new CertificatesDatas();

                string token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/Cva/QuerySituationPDF?situationId=" + situacionid;
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
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                    if (respuesta.Response != null && !string.IsNullOrEmpty(respuesta.Response as string))
                    {
                        resopdf = JsonConvert.DeserializeObject<CertificatesDatas>(respuesta.Response.ToString());
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }

                return resopdf;
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
        /// <param name="identity"></param>
        /// <param name="codigoEmpresa"></param>
        /// <param name="situacionid"></param>
        /// <returns></returns>
        public object ConsultSituacionid(ClaimsIdentity identity, decimal codigoEmpresa, decimal situacionid)
        {
            try
            {
                _logger.LogInformation("method called");
                List<CvModel> resoidd = new List<CvModel>();

                string token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/Cva/QuerySituationId?companyCode=" + codigoEmpresa + "&situationId=" + situacionid;
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
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                    if (respuesta.Response != null && !string.IsNullOrEmpty(respuesta.Response as string))
                    {
                        resoidd = JsonConvert.DeserializeObject<List<CvModel>>(respuesta.Response.ToString());
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }

                return resoidd;
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
        /// <param name="identity"></param>
        /// <param name="codigoEmpresa"></param>
        /// <returns></returns>
        public object ConsultSituacionnovedad(ClaimsIdentity identity, decimal codigoEmpresa)
        {
            try
            {
                _logger.LogInformation("method called");
                List<CvModel> resoidd = new List<CvModel>();

                string token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/Cva/QueryNoveltySituation?companyCode=" + codigoEmpresa;
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
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                    if (respuesta.Response != null && !string.IsNullOrEmpty(respuesta.Response as string))
                    {
                        resoidd = JsonConvert.DeserializeObject<List<CvModel>>(respuesta.Response.ToString());
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }

                return resoidd;
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
        /// <param name="identity"></param>
        /// <param name="codigoEmpresa"></param>
        /// <returns></returns>
        public object ConsultSituacionnovedadultima(ClaimsIdentity identity, decimal codigoEmpresa)
        {
            try
            {
                _logger.LogInformation("method called");
                List<CvModel> resoidd = new List<CvModel>();

                string token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/Cva/QueryLatestNoveltySituation?companyCode=" + codigoEmpresa;
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;
                //if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                //{
                //    return new
                //    {
                //        volverInicio = true
                //    };
                //}

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                    if (respuesta.Response != null && !string.IsNullOrEmpty(respuesta.Response as string))
                    {
                        resoidd = JsonConvert.DeserializeObject<List<CvModel>>(respuesta.Response.ToString());
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }

                return resoidd;
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
        /// <param name="idresolucionp"></param>
        /// <returns></returns>
        public object Consultpecespdf(decimal idresolucionp)
        {
            try
            {
                _logger.LogInformation("method called");
                ResolucionPermisos datos = new ResolucionPermisos();
                string token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/Cva/QueryFishPDF?resolutionId=" + idresolucionp;
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
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                    datos = JsonConvert.DeserializeObject<ResolucionPermisos>(respuesta.Response.ToString());
                    HttpContext.Session.SetString("token", respuesta.Token);
                }
                return datos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }
    }
}
