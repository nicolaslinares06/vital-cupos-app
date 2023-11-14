using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.Helpers;
using System.Data;
using System.Net.Http.Headers;
using Web.Models;

namespace WebFront.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class NonTimberFloraCertificateController : Controller
    {

        private readonly string UrlApi;
        private readonly ILogger<NonTimberFloraCertificateController> _logger;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public NonTimberFloraCertificateController(ILogger<NonTimberFloraCertificateController> logger)
        {
            UrlApi = string.IsNullOrEmpty(RUTAAPI) ? new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetValue<string>("Variables:RutaApi") : RUTAAPI;
            _logger = logger;
        }
        /// <summary>
        /// htttp client
        /// </summary>
        /// <returns></returns>
        public HttpClient getHttpClient()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            return new HttpClient(clientHandler);
        }
        /// <summary>
        /// retornar vista
        /// </summary>
        /// <returns></returns>
        public IActionResult NonTimberFloraCertificate()
        {
            try
            {
                _logger.LogInformation("method called");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return ManejoIActionResultException(ex);
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

                if (IsTokenValid())
                    return View("Views/NonTimberFloraCertificate/Partials/Ayuda.cshtml");

                return View();
            }
            catch (Exception ex)
            {
                return ManejoIActionResultException(ex);
            }
        }
        /// <summary>
        /// consultar autoridades
        /// </summary>
        /// <returns></returns>
        public object ConsultAuthority()
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/NonTimberFloraCertificate/ConsultAuthority";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;
                var respuesta = ProcessHttpResponse<List<ElementTypes>>(response);

                return respuesta ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<ElementTypes>? Authority = new List<ElementTypes>();
                return Authority;
            }
        }

        /// <summary>
        /// consultar tipos especimenes
        /// </summary>
        /// <returns></returns>
        public object ConsultEspecimensProductsType()
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/NonTimberFloraCertificate/ConsultEspecimensProductsType";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;
                var respuesta = ProcessHttpResponse<List<ElementTypes>>(response);

                return respuesta ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<ElementTypes>? EspecimensProductsType = new List<ElementTypes>();
                return EspecimensProductsType;
            }
        }

        /// <summary>
        /// consultar tipos de documentos
        /// </summary>
        /// <returns></returns>
        public object ConsultDocumentsTypes()
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/NonTimberFloraCertificate/ConsultDocumentsTypes";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;
                var respuesta = ProcessHttpResponse<List<ElementTypes>>(response);

                return respuesta ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<ElementTypes>? DocumentsTypes = new List<ElementTypes>();
                return DocumentsTypes;
            }
        }

        /// <summary>
        /// consultar certificados
        /// </summary>
        /// <returns></returns>
        public object ConsultCertificates()
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/NonTimberFloraCertificate/ConsultCertificates";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;

                var respuesta = ProcessHttpResponse<List<CertificadosFloraNoMaderable>>(response);

                return respuesta ?? new object { };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<CertificadosFloraNoMaderable>? certificados = new List<CertificadosFloraNoMaderable>();
                return certificados;
            }
        }

        /// <summary>
        /// consultar datos entidad
        /// </summary>
        /// <param name="documentType"></param>
        /// <param name="DocumentNumber"></param>
        /// <returns></returns>
        public object ConsultEntityDates(decimal documentType, decimal DocumentNumber)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/NonTimberFloraCertificate/ConsultEntityDates?nitBussines=" + DocumentNumber + "&documentType=" + documentType;
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;

                var respuesta = ProcessHttpResponse<List<DatosEntidad>>(response);

                return respuesta ?? new object { };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<DatosEntidad>? datos = new List<DatosEntidad>();
                return datos;
            }
        }

        /// <summary>
        /// consultar certificados por nit, numero de certificado o ambas
        /// </summary>
        /// <param name="documentType"></param>
        /// <param name="DocumentNumber"></param>
        /// <param name="CertificateNumber"></param>
        /// <returns></returns>
        public object ConsultCertificatesForNit(decimal documentType, decimal DocumentNumber = 0, string CertificateNumber = "0")
        {
            List<CertificadosFloraNoMaderable>? certificates = new List<CertificadosFloraNoMaderable>();
            string? token = HttpContext.Session.GetString("token");
            string URI = UrlApi + "/NonTimberFloraCertificate/ConsultCertificatesForNit?nitBussines=" + DocumentNumber + "&documentType=" + documentType + "&CertificateNumber=" + CertificateNumber;
            var httpClient = getHttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = httpClient.GetAsync(URI).Result;

            var respuesta = ProcessHttpResponse<List<CertificadosFloraNoMaderable>>(response);

            return respuesta ?? new object { };

        }

        /// <summary>
        /// consultar tipos de especimenes
        /// </summary>
        /// <returns></returns>
        public object ConsultEspecimensTypes()
        {
            try
            {
                _logger.LogInformation("method called");
                List<ElementTypesEspecies>? EspecimentsTypes = new List<ElementTypesEspecies>();
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/NonTimberFloraCertificate/ConsultEspecimensTypes";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;

                var respuesta = ProcessHttpResponse<List<ElementTypesEspecies>>(response);

                return respuesta ?? new object { };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<ElementTypesEspecies>? EspecimentsTypes = new List<ElementTypesEspecies>();
                return EspecimentsTypes;
            }
        }

        /// <summary>
        /// guardar certificado nuevo
        /// </summary>
        /// <param name="datosGuardar"></param>
        /// <returns></returns>
        public object SaveCertificate(CertificatesDatas datosGuardar)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/NonTimberFloraCertificate/SaveCertificate";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var req = ObjetoFormulario(datosGuardar);
                var response = httpClient.PostAsJsonAsync(URI, req).Result;

                return ProcesarPeticion(response);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return false;
            }
        }

        /// <summary>
        /// consultar un certificado por codigo
        /// </summary>
        /// <param name="codeCertificate"></param>
        /// <returns></returns>
        public object ConsultDatasCertificate(decimal codeCertificate)
        {
            try
            {
                _logger.LogInformation("method called");
                CertificatesDatas? certificates = new CertificatesDatas();
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/NonTimberFloraCertificate/ConsultDatasCertificate?codeCertificate=" + codeCertificate;
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;

                var respuesta = ProcessHttpResponse<CertificatesDatas>(response);

                return respuesta ?? new object { };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                CertificatesDatas? certificates = new CertificatesDatas();
                return certificates;
            }
        }

        /// <summary>
        /// editar certificado
        /// </summary>
        /// <param name="datosGuardar"></param>
        /// <returns></returns>
        public object SaveEditCertificate(CertificatesDatas datosGuardar)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/NonTimberFloraCertificate/SaveEditCertificate";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var req = ObjetoFormulario(datosGuardar);

                var response = httpClient.PostAsJsonAsync(URI, req).Result;
                return ProcesarPeticion(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return false;
            }
        }

        /// <summary>
        /// deshabilitar certificado
        /// </summary>
        /// <param name="codeCertificate"></param>
        /// <returns></returns>
        public object DeleteCertificate(decimal codeCertificate)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/NonTimberFloraCertificate/DeleteCertificate";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = httpClient.PostAsJsonAsync(URI, codeCertificate).Result;
                return ProcesarPeticion(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return false;
            }
        }

        private bool IsTokenValid()
        {
            string token = HttpContext.Session.GetString("token") ?? "";
            return !String.IsNullOrEmpty(token);
        }

        private IActionResult ManejoIActionResultException(Exception ex)
        {
            _logger.LogError(ex, "An error occurred in the method.");
            if (IsTokenValid())
                return RedirectToAction("FlujoNegocio", "Home");
            else
                return RedirectToAction("Index", "Login");
        }

        private object ObjetoFormulario(CertificatesDatas datosGuardar)
        {
            var req = new
            {
                code = datosGuardar.codigo,
                issuingAuthority = datosGuardar.autoridadEmiteCertificacion,
                certificateNumber = datosGuardar.numeroCertificado,
                certificationDate = datosGuardar.fechaCertificacion,
                certificationValidity = datosGuardar.vigenciaCertificacion,
                permissionType = datosGuardar.tipoPermiso,
                specimenProductImpExpType = datosGuardar.tipoEspecimenProductoImpExp,
                certificateRemarks = datosGuardar.observacionesCertificado,
                companyNit = datosGuardar.nitEmpresa,
                supportingDocuments = datosGuardar.documentosSoporte?.Select(d => new
                {
                    code = d.codigo,
                    base64Attachment = d.adjuntoBase64,
                    attachmentName = d.nombreAdjunto,
                    attachmentType = d.tipoAdjunto
                }).ToList(),
                newSupportingDocuments = datosGuardar.documentosSoporteNuevo?.Select(d => new
                {
                    code = d.codigo,
                    base64Attachment = d.adjuntoBase64,
                    attachmentName = d.nombreAdjunto,
                    attachmentType = d.tipoAdjunto
                }).ToList(),
                deletedSupportingDocuments = datosGuardar.documentosSoporteEliminar?.Select(d => new
                {
                    code = d.codigo,
                    base64Attachment = d.adjuntoBase64,
                    attachmentName = d.nombreAdjunto,
                    attachmentType = d.tipoAdjunto
                }).ToList()
            };

            return req;
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

        private object ProcesarPeticion(HttpResponseMessage response)
        {
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
                Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                if (respuesta.Response != null)
                {
                    HttpContext.Session.SetString("token", respuesta.Token);
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
