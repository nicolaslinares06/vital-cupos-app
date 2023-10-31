using CUPOS_FRONT.Models;
using DocumentFormat.OpenXml.Math;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.Helpers;
using Repository.Helpers.Models;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Xml.Linq;
using Web.Models;
using static Web.Models.TrayForNationalSealsExternUsers;

namespace Web.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]


    public class TrayForNationalSealsExternUsersController : Controller
    {
        private readonly string UrlApi;
        private readonly ILogger<TrayForNationalSealsExternUsersController> _logger;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";


        public TrayForNationalSealsExternUsersController(ILogger<TrayForNationalSealsExternUsersController> logger)
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
        public IActionResult TrayForNationalSealsExternUsers(decimal codigoEmpresa, decimal nitEmpresa, decimal tipoEmpresa)
        {
            try
            {
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

                _logger.LogInformation("method called");
                ViewBag.nitEmpresa = nitEmpresa;
                ViewBag.codigoEmpresa = codigoEmpresa;
                ViewBag.tipoEmpresa = tipoEmpresa;
                ViewBag.ConsultDepartment = ConsultDepartments();
                ViewBag.tipoDato = ConsultRequestTypes();
                ViewBag.tipoDocument = ConsultDocumentsTypes();
                ViewBag.consulRepresen = ConsultBussinesAndLegalRepresentant(codigoEmpresa);
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string? token = HttpContext.Session.GetString("token") ?? "";

                if (String.IsNullOrEmpty(token))
                    return View("Views/Login/Index.cshtml");
                else
                    return View("Views/Home/FlujoNegocio.cshtml");
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
                    return View("Views/TrayForNationalSealsExternUsers/Partials/Ayuda.cshtml");

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }
        public object ConsultDocumentsTypes()
        {
            try
            {
                _logger.LogInformation("method called");

                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

                List<ElementTypes>? DocumentsTypes = new List<ElementTypes>();
                string URI = UrlApi + "/NonTimberFloraCertificate/ConsultDocumentsTypes";
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
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                    if (respuesta.Response != null)
                    {
                        DocumentsTypes = JsonConvert.DeserializeObject<List<ElementTypes>>(respuesta.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }

                }

                return DocumentsTypes ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<ElementTypes>? DocumentsTypes = new List<ElementTypes>();
                return DocumentsTypes;
            }
        }

        /// <summary>   
        /// consultar tipos solicitud
        /// </summary>
        /// <returns></returns>
        public object ConsultRequestTypes()
        {
            try
            {
                _logger.LogInformation("method called");

                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

                List<ElementTypes>? datos = new List<ElementTypes>();
                string URI = UrlApi + "/TrayForNationalSealsExternUsers/ConsultRequestTypes";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                    if (respuesta.Response != null)
                    {
                        datos = JsonConvert.DeserializeObject<List<ElementTypes>>(respuesta.Response.ToString() ?? "") ?? new List<ElementTypes>();
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }

                }
                return datos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<ElementTypes>? datos = new List<ElementTypes>();
                return datos;
            }
        }

        /// <summary>
        /// consultar empresas
        /// </summary>
        /// <returns></returns>
        public object ConsultBussiness()
        {
            try
            {
                _logger.LogInformation("method called");

                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

                List<ElementTypes>? datos = new List<ElementTypes>();
                string URI = UrlApi + "/TrayForNationalSealsExternUsers/ConsultBussiness";
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
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                    if (respuesta.Response != null)
                    {
                        datos = JsonConvert.DeserializeObject<List<ElementTypes>>(respuesta.Response.ToString() ?? "") ?? new List<ElementTypes>();
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }

                }
                return datos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<ElementTypes>? datos = new List<ElementTypes>();
                return datos;
            }
        }

        /// <summary>
        /// consultar empresa y representante legal
        /// </summary>
        /// <returns></returns>
        public object ConsultBussinesAndLegalRepresentant(decimal codigoEmpresa)
        {
            try
            {
                _logger.LogInformation("method called");

                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

                RepresentaeLegalEmpresa? datos = new RepresentaeLegalEmpresa();
                string URI = UrlApi + "/TrayForNationalSealsExternUsers/ConsultBussinesAndLegalRepresentant?codeBussines=" + codigoEmpresa;
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
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                    if (respuesta.Response != null)
                    {
                        datos = JsonConvert.DeserializeObject<RepresentaeLegalEmpresa>(respuesta.Response.ToString() ?? "") ?? new RepresentaeLegalEmpresa();
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }
                return datos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                RepresentaeLegalEmpresa? datos = new RepresentaeLegalEmpresa();
                return datos;

            }
        }

        /// <summary>
        /// consultar ciudades
        /// </summary>
        /// <returns></returns>
        public object ConsultCities()
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

                List<ElementTypesEspecies>? cities = new List<ElementTypesEspecies>();
                string URI = UrlApi + "/TrayForNationalSealsExternUsers/ConsultCities";
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
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                    if (respuesta.Response != null)
                    {
                        cities = JsonConvert.DeserializeObject<List<ElementTypesEspecies>>(respuesta.Response.ToString() ?? "") ?? new List<ElementTypesEspecies>();
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }

                }
                return cities;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<ElementTypesEspecies>? cities = new List<ElementTypesEspecies>();
                return cities;

            }
        }

        /// <summary>
        /// gusardar solicitud
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public object RegisterRequest(Request request)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

                string URI = UrlApi + "/TrayForNationalSealsExternUsers/RegisterRequest";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var req = new
                {
                    companyCode = request.codigoEmpresa,
                    requestCode = request.codigoSolicitud,
                    date = request.fecha,
                    representativeCity = request.ciudadRepresentante,
                    deliveryAddress = request.direccionEntrega,
                    quantity = request.cantidad,
                    specimens = request.especimenes,
                    initialSkinCode = request.codigoInicialPieles,
                    finalSkinCode = request.codigoFinalPieles,
                    minorLength = request.longitudMenor,
                    majorLength = request.longitudMayor,
                    generateSealsForConsignation = false,
                    representativeDate = request.fechaRepresentante,
                    observations = request.observaciones,
                    response = request.respuesta,
                    requestStatus = request.estadoSolicitud,
                    registrationDate = request.fechaRadicado,
                    requestType = request.tipoSolicitud,
                    statusChangeDate = request.fechaCambioEstado,
                    withdrawalObservations = request.observacionesDesistimiento,
                    numerations = request.numeraciones?.Select(n => new
                    {
                        code = n.codigo,
                        initial = n.inicial,
                        final = n.final,
                        origen = n.origen
                    }).ToList(),
                    invoiceAttachment = new
                    {
                        code = request.facturaAdjunto.codigo,
                        base64Attachment = request.facturaAdjunto.adjuntoBase64,
                        attachmentName = request.facturaAdjunto.nombreAdjunto,
                        attachmentType = request.facturaAdjunto.tipoAdjunto
                    },
                    letterAttachment = request.adjuntoCarta != null ? new
                    {
                        code = request.adjuntoCarta.codigo,
                        base64Attachment = request.adjuntoCarta.adjuntoBase64,
                        attachmentName = request.adjuntoCarta.nombreAdjunto,
                        attachmentType = request.adjuntoCarta.tipoAdjunto
                    } : null,
                    attachedAnnexes = request.AnexosAdjuntos?.Select(a => new
                    {
                        code = a.codigo,
                        base64Attachment = a.adjuntoBase64,
                        attachmentName = a.nombreAdjunto,
                        attachmentType = a.tipoAdjunto
                    }).ToList(),
                    attachedAnnexesToDelete = request.anexosAdjuntosEliminar?.Select(a => new
                    {
                        code = a.codigo,
                        base64Attachment = a.adjuntoBase64,
                        attachmentName = a.nombreAdjunto,
                        attachmentType = a.tipoAdjunto
                    }).ToList(),
                    responseAttachments = request.adjuntosRespuesta?.Select(a => new
                    {
                        code = a.codigo,
                        base64Attachment = a.adjuntoBase64,
                        attachmentName = a.nombreAdjunto,
                        attachmentType = a.tipoAdjunto
                    }).ToList(),
                    responseAttachmentsToDelete = request.adjuntosRespuestaEliminar?.Select(a => new
                    {
                        code = a.codigo,
                        base64Attachment = a.adjuntoBase64,
                        attachmentName = a.nombreAdjunto,
                        attachmentType = a.tipoAdjunto
                    }).ToList(),
                    representativeDepartment = request.departamentoRepresentante,
                    safeGuardNumbers = request.safeGuardNumbers,
                    cuttingSave = request.cuttingSave
                };

                var response = httpClient.PostAsJsonAsync(URI, req).Result;
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return false;
            }
        }


        /// <summary>
        /// consultar solicitudes radicadas
        /// </summary>
        /// <param name="codigoEmpresa"></param>
        /// <returns></returns>
        public object ConsultRegisteredRecuest(decimal codigoEmpresa)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

                List<solicitudes>? RegisteredRecuests = new List<solicitudes>();
                string URI = UrlApi + "/TrayForNationalSealsExternUsers/ConsultRegisteredRecuest?companyCode=" + codigoEmpresa;
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
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                    if (respuesta.Response != null)
                    {
                        RegisteredRecuests = JsonConvert.DeserializeObject<List<solicitudes>>(respuesta.Response.ToString() ?? "") ?? new List<solicitudes>();
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }
                return RegisteredRecuests;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<solicitudes>? RegisteredRecuests = new List<solicitudes>();
                return RegisteredRecuests;
            }
        }

        /// <summary>
        /// consultar solicitudes en requerieminto
        /// </summary>
        /// <param name="codigoEmpresa"></param>
        /// <returns></returns>
        public object ConsultRequirements(decimal codigoEmpresa)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

                List<solicitudes>? RegisteredRecuests = new List<solicitudes>();
                string URI = UrlApi + "/TrayForNationalSealsExternUsers/ConsultRequirements?companyCode=" + codigoEmpresa;
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
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                    if (respuesta.Response != null)
                    {
                        RegisteredRecuests = JsonConvert.DeserializeObject<List<solicitudes>>(respuesta.Response.ToString() ?? "") ?? new List<solicitudes>();
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }
                return RegisteredRecuests;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<solicitudes>? RegisteredRecuests = new List<solicitudes>();
                return RegisteredRecuests;
            }
        }

        /// <summary>
        /// consultar solicitudes aprobadas
        /// </summary>
        /// <param name="codigoEmpresa"></param>
        /// <returns></returns>
        public object ConsultApproved(decimal codigoEmpresa)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

                List<solicitudes>? RegisteredRecuests = new List<solicitudes>();
                string URI = UrlApi + "/TrayForNationalSealsExternUsers/ConsultApproved?companyCode=" + codigoEmpresa;
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
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                    if (respuesta.Response != null)
                    {
                        RegisteredRecuests = JsonConvert.DeserializeObject<List<solicitudes>>(respuesta.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }
                return RegisteredRecuests ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// cponsultar solicitudes desistidas
        /// </summary>
        /// <param name="codigoEmpresa"></param>
        /// <returns></returns>
        public object ConsultDesisted(decimal codigoEmpresa)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

                List<solicitudes>? RegisteredRecuests = new List<solicitudes>();
                string URI = UrlApi + "/TrayForNationalSealsExternUsers/ConsultDesisted?companyCode=" + codigoEmpresa;
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
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                    if (respuesta.Response != null)
                    {
                        RegisteredRecuests = JsonConvert.DeserializeObject<List<solicitudes>>(respuesta.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }
                return RegisteredRecuests ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<solicitudes>? RegisteredRecuests = new List<solicitudes>();
                return RegisteredRecuests;
            }
        }

        /// <summary>
        /// consultar una solicitud
        /// </summary>
        /// <param name="codigoSolicitud"></param>
        /// <returns></returns>
        public object ConsultOnePendientRegister(decimal codigoSolicitud)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

                Request? solicitud = new Request();
                string URI = UrlApi + "/TrayForNationalSealsExternUsers/ConsultOnePendientRegister?codeRequest=" + codigoSolicitud;
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
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                    if (respuesta.Response != null)
                    {
                        solicitud = JsonConvert.DeserializeObject<Request>(respuesta.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }
                return solicitud ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                Request? solicitud = new Request();
                return solicitud;
            }
        }

        /// <summary>
        /// editar solicitud
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public object EditRequest(Request request)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

                string URI = UrlApi + "/TrayForNationalSealsExternUsers/EditRequest";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var req = new
                {

                    companyCode = request.codigoEmpresa,
                    requestCode = request.codigoSolicitud,
                    date = request.fecha,
                    representativeCity = request.ciudadRepresentante,
                    deliveryAddress = request.direccionEntrega,
                    quantity = request.cantidad,
                    specimens = request.especimenes,
                    initialSkinCode = request.codigoInicialPieles,
                    finalSkinCode = request.codigoFinalPieles,
                    minorLength = request.longitudMenor,
                    majorLength = request.longitudMayor,
                    generateSealsForConsignation = false,
                    representativeDate = request.fechaRepresentante,
                    observations = request.observaciones,
                    response = request.respuesta,
                    requestStatus = request.estadoSolicitud,
                    registrationDate = request.fechaRadicado,
                    requestType = request.tipoSolicitud,
                    statusChangeDate = request.fechaCambioEstado,
                    withdrawalObservations = request.observacionesDesistimiento,
                    numerations = request.numeraciones?.Select(n => new
                    {
                        code = n.codigo,
                        initial = n.inicial,
                        final = n.final,
                        origen = n.origen
                    }).ToList(),
                    invoiceAttachment = new
                    {
                        code = request.facturaAdjunto.codigo,
                        base64Attachment = request.facturaAdjunto.adjuntoBase64,
                        attachmentName = request.facturaAdjunto.nombreAdjunto,
                        attachmentType = request.facturaAdjunto.tipoAdjunto
                    },
                    letterAttachment = request.adjuntoCarta != null ? new
                    {
                        code = request.adjuntoCarta.codigo,
                        base64Attachment = request.adjuntoCarta.adjuntoBase64,
                        attachmentName = request.adjuntoCarta.nombreAdjunto,
                        attachmentType = request.adjuntoCarta.tipoAdjunto
                    } : null,
                    attachedAnnexes = request.AnexosAdjuntos?.Select(a => new
                    {
                        code = a.codigo,
                        base64Attachment = a.adjuntoBase64,
                        attachmentName = a.nombreAdjunto,
                        attachmentType = a.tipoAdjunto
                    }).ToList(),
                    attachedAnnexesToDelete = request.anexosAdjuntosEliminar?.Select(a => new
                    {
                        code = a.codigo,
                        base64Attachment = a.adjuntoBase64,
                        attachmentName = a.nombreAdjunto,
                        attachmentType = a.tipoAdjunto
                    }).ToList(),
                    responseAttachments = request.adjuntosRespuesta?.Select(a => new
                    {
                        code = a.codigo,
                        base64Attachment = a.adjuntoBase64,
                        attachmentName = a.nombreAdjunto,
                        attachmentType = a.tipoAdjunto
                    }).ToList(),
                    responseAttachmentsToDelete = request.adjuntosRespuestaEliminar?.Select(a => new
                    {
                        code = a.codigo,
                        base64Attachment = a.adjuntoBase64,
                        attachmentName = a.nombreAdjunto,
                        attachmentType = a.tipoAdjunto
                    }).ToList(),
                    representativeDepartment = request.departamentoRepresentante
                };

                var response = httpClient.PostAsJsonAsync(URI, req).Result;
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return false;
            }
        }

        /// <summary>
        /// Obtiene los cupos
        /// </summary>
        /// <returns></returns>
        public object GetQuotas(string documentNumber, string especie)
        {
            try
            {
                _logger.LogInformation("method called");
                List<Quota>? quotaList = new List<Quota>();

                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

                string uri = String.Format("{0}/TrayForNationalSealsExternUsers/GetQuotas?documentNumber={1}&species={2}", UrlApi, documentNumber, especie);
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(uri).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return new
                    {
                        volverInicio = true
                    };
                }
                if (response.IsSuccessStatusCode)
                {
                    string jsonInput = response.Content.ReadAsStringAsync().Result;
                    Responses? responseData = JsonConvert.DeserializeObject<Responses>(jsonInput);
                    if (responseData != null)
                    {
                        quotaList = JsonConvert.DeserializeObject<List<Quota>>(responseData.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", responseData.Token);
                    }
                }

                return quotaList ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// Obtiene los cupos
        /// </summary>
        /// <returns></returns>
        public object GetInventory(string documentNumber, string especie)
        {
            try
            {
                _logger.LogInformation("method called");
                List<Inventory>? inventario = new List<Inventory>();

                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

                string uri = String.Format("{0}/TrayForNationalSealsExternUsers/GetInventory?documentNumber={1}&species={2}", UrlApi, documentNumber, especie);
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(uri).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return new
                    {
                        volverInicio = true
                    };
                }
                if (response.IsSuccessStatusCode)
                {
                    string jsonInput = response.Content.ReadAsStringAsync().Result;
                    Responses? responseData = JsonConvert.DeserializeObject<Responses>(jsonInput);
                    if (responseData != null)
                    {
                        inventario = JsonConvert.DeserializeObject<List<Inventory>>(responseData.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", responseData.Token);
                    }
                }

                return inventario ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// consultar departamentos
        /// </summary>
        /// <returns></returns>
        public object ConsultDepartments()
        {
            try
            {
                _logger.LogInformation("method called");
                List<ElementTypesEspecies>? departments = new List<ElementTypesEspecies>();
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

                string URI = UrlApi + "/TrayForNationalSealsExternUsers/ConsultDepartments";
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
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                    if (respuesta.Response != null)
                    {
                        departments = JsonConvert.DeserializeObject<List<ElementTypesEspecies>>(respuesta.Response.ToString() ?? "") ?? new List<ElementTypesEspecies>();
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }

                }
                return departments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<ElementTypesEspecies>? departments = new List<ElementTypesEspecies>();
                return departments;
            }
        }

        /// <summary>
        /// Consultar ciudades por id de departamento
        /// 
        /// /// </summary>
        /// <returns></returns>
        public object ConsultCitiesByDepartmentId(decimal departamentoId)
        {
            try
            {
                _logger.LogInformation("method called");
                List<ElementTypesEspecies>? cities = new List<ElementTypesEspecies>();
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

                string URI = UrlApi + "/TrayForNationalSealsExternUsers/ConsultCitiesByIdDepartment?idDepartment=" + departamentoId;
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
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                    if (respuesta.Response != null)
                    {
                        cities = JsonConvert.DeserializeObject<List<ElementTypesEspecies>>(respuesta.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }

                }
                return cities ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<ElementTypesEspecies>? cities = new List<ElementTypesEspecies>();
                return cities;
            }
        }

        /// <summary>
        /// Consultar numeraciones solicitudes no disponibles
        /// <returns></returns>
        public object getNumbersRequest(ConsultUnableNumbersModel data)
        {
            try
            {
                _logger.LogInformation("method called");
                List<numbers>? numbers = new List<numbers>();
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

                string URI = UrlApi + "/TrayForNationalSealsExternUsers/getNumbersRequest";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var req = new
                {
                    code = data.code,
                    companyNit = data.nitEmpresa,
                    quota = data.cupo,
                    inventory = data.inventario
                };

                var response = httpClient.PostAsJsonAsync(URI, req).Result;
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
                        numbers = JsonConvert.DeserializeObject<List<numbers>>(respuesta.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }

                }
                return numbers ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<numbers>? numbers = new List<numbers>();
                return numbers;
            }
        }

        /// <summary>
        /// Valida si estan disponibvles las numeraciones
        /// </summary>
        /// <returns></returns>
        public object ValidateNumbers(validateNumbersModel numbers)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

                string uri = String.Format("{0}/TrayForNationalSealsExternUsers/ValidateNumbers", UrlApi);
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var req = new
                {
                    codeCompany = numbers.codeCompany,
                    numbers = numbers.numeros != null ? new
                    {
                        initial = numbers.numeros.initial,
                        final = numbers.numeros.final
                    } : null,
                    origin = numbers.origen
                };

                var response = httpClient.PostAsJsonAsync(uri, req).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return new
                    {
                        volverInicio = true
                    };
                }

                if (response.IsSuccessStatusCode)
                {
                    string jsonInput = response.Content.ReadAsStringAsync().Result;
                    Responses? responseData = JsonConvert.DeserializeObject<Responses>(jsonInput);
                    if (responseData != null)
                    {
                        HttpContext.Session.SetString("token", responseData.Token);
                        return responseData.Response;
                    }
                }

                return new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// obtener actas de corte
        /// </summary>
        /// <param name="documentNumber"></param>
        /// <returns></returns>
        public object getActaData(string documentNumber)
        {
            List<cuttingReport>? datos = new List<cuttingReport>();
            string? token = HttpContext.Session.GetString("token");

            if (token == null)
                return View("Views/Login/Index.cshtml");

            string URI = UrlApi + "/TrayForNationalSealsExternUsers/getActaData?documentNumber=" + documentNumber;
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
                Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                if (respuesta.Response != null)
                {
                    datos = JsonConvert.DeserializeObject<List<cuttingReport>>(respuesta.Response.ToString() ?? "");
                    HttpContext.Session.SetString("token", respuesta.Token);
                }

            }
            return datos ?? new object { };
        }

        /// <summary>
        /// obtener fracciones
        /// </summary>
        /// <param name="cuttingCode"></param>
        /// <returns></returns>
        public object getFractions(int cuttingCode)
        {
            List<cutting>? datos = new List<cutting>();
            string? token = HttpContext.Session.GetString("token");

            if (token == null)
                return View("Views/Login/Index.cshtml");

            string URI = UrlApi + "/TrayForNationalSealsExternUsers/getFractions?cuttingCode=" + cuttingCode;
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
                Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                if (respuesta.Response != null)
                {
                    datos = JsonConvert.DeserializeObject<List<cutting>>(respuesta.Response.ToString() ?? "");
                    HttpContext.Session.SetString("token", respuesta.Token);
                }

            }
            return datos ?? new object { };
        }

        /// <summary>
        /// obtener salvoconducto
        /// </summary>
        /// <param name="cuttingCode"></param>
        /// <returns></returns>
        public object getSafeguard(int reportCode)
        {
            List<Safeguard>? datos = new List<Safeguard>();
            string? token = HttpContext.Session.GetString("token");

            if (token == null)
                return View("Views/Login/Index.cshtml");

            string URI = UrlApi + "/TrayForNationalSealsExternUsers/getSafeguard?reportCode=" + reportCode;
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
                Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                if (respuesta.Response != null)
                {
                    datos = JsonConvert.DeserializeObject<List<Safeguard>>(respuesta.Response.ToString() ?? "");
                    HttpContext.Session.SetString("token", respuesta.Token);
                }

            }
            return datos ?? new object { };
        }

    }
}
