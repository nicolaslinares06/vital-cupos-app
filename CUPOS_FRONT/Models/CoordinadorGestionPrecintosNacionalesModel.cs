using DocumentFormat.OpenXml.Drawing;
using Newtonsoft.Json;
using Org.BouncyCastle.Math;
using System.ComponentModel.DataAnnotations;
using static Web.Models.TrayForNationalSealsExternUsers;

namespace Web.Models
{
    public class CoordinadorGestionPrecintosNacionalesModel
    {
        public decimal CodigoSolicitud { get; set; }
        public string? NumeroRadicado { get; set; } = "";
        public string? SolicitudPrecintoNacional { get; set; } = "";
        public string? NombreEntidadSolicitante { get; set; } = "";
        public string? FechaSolicitud { get; set; } = "";
        public string? FechaRadicacion { get; set; } = "";
        public string? Estado { get; set; } = "";
        public string? AccionVisual { get; set; } = "";
        public string? NumeroRadicacionSalida { get; set; } = "";
        public string? FechaRadicacionSalida { get; set; } = "";

    }

    public class InformacionSolicitud
    {
        public decimal IdSolicitud { get; set; }
        public string? Ciudad { get; set; } = "";
        public string? Establecimiento { get; set; } = "";
        [DataType(DataType.Date)]
        public DateTime? Fecha { get; set; } = DateTime.Now;
        public string? PrimerNombre { get; set; } = "";
        public string? SegundoNombre { get; set; } = "";
        public string? PrimerApellido { get; set; } = "";
        public string? SegundoApellido { get; set; } = "";
        public string? TipoIdentificacion { get; set; } = "";
        public string? NumeroIdentificacion { get; set; } = "";
        public string? DireccionEntrega { get; set; } = "";
        public string? CiudadEntrega { get; set; } = "";
        public string? Telefonos { get; set; } = "";
        public string? Fax { get; set; } = "";
        public decimal Cantidad { get; set; } = 0;
        public string? EspeciesSubEspecies { get; set; } = "";
        public decimal? CodigoInicialPieles { get; set; } = 0;
        public decimal? CodigoFinalPieles { get; set; } = 0;
        public decimal? LongitudMenor { get; set; } = 0;
        public decimal? LongitudMayor { get; set; } = 0;
        public string? SoporteConsignacionBase64 { get; set; }
        [DataType(DataType.Date)]
        public DateTime? FechaAsignacion { get; set; } = DateTime.Now;
        public bool EsSoporteConsignacion { get; set; } = true;

        public string? Observaciones { get; set; } = "Lorem Ipsum es simplemente el texto de relleno de las imprentas y archivos de texto. Lorem Ipsum ha sido el texto de relleno estándar de las industrias desde el año 1500";

        public decimal? Analista { get; set; } = 0;
        public string? NumeroRadicado { get; set; } = "";
        public decimal? Nit { get; set; } = 0;
        public decimal ValorConsignacion { get; set; } = 0;
        public string? CodigoInicial { get; set; } = "";
        public string? CodigoFinal { get; set; } = "";
        public string? Color { get; set; } = "";
        public string? CorreoElectronico { get; set; } = "";
        public string? DepartamentoEntrega { get; set; } = "";
        public ArchivoSolicitud archivoConsignacion { get; set; } = new ArchivoSolicitud();
        public List<ArchivoSolicitud> ArchivoCartaVenta { get; set; } = new List<ArchivoSolicitud>();
        public List<ArchivoSolicitud> ArchivosRespuesta { get; set; } = new List<ArchivoSolicitud>();
        public string? Respuesta { get; set; } = "";
        public string? TipoSolicitud { get; set; }
        public decimal? TypeRequestCode { get; set; } = 0;
        public List<ParametricaTipoSolicitud> ListaTiposSolicitud { get; set; } = new List<ParametricaTipoSolicitud>();
        public List<cuttingSaveModel> FraccionesSolicitud { get; set; } = new List<cuttingSaveModel>();
    }

    public class ActualizacionAsignacionAnalista
    {
        public decimal AnalistaId { get; set; }
        public decimal CodigoSolicitud { get; set; }

    }

    public class ArchivoSolicitud
    {
        public string? adjuntoBase64 { get; set; } = "";

        public string? tipoAdjunto { get; set; } = "";

        public string? nombreAdjunto { get; set; } = "";
    }


    public class EstadoSolicitudCoordinador
    {
        public decimal RequestId { get; set; }
        public string RequestStatus { get; set; } = string.Empty;
    }
    public class ActualizacionAprobacionAnalista
    {
        public decimal IdSolicitud { get; set; }
        public decimal EstadoSolicitud { get; set; }
        public int CargoAsignacion { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string? Observaciones { get; set; }

    }

    public class ActualizacionAprobacionViewModel

    {
        public bool SolicitudAprobada { get; set; } = false;
        public decimal? IdSolicitud { get; set; }
        public ArchivoSolicitud? ArchivoAprobacionAnalista { get; set; }

        public AnalistaSolicitud? AnlistaAsignado { get; set; }

        public InformacionSolicitud? InfoSolicitud { get; set; }


    }

    public class AnalistaAsignacion
    {
        public int CantidadSolicitudes { get; set; }
        public string? RolAnalista { get; set; }
        public string? NombresApellidos { get; set; }
        public decimal? CodigoAnalista { get; set; }
    }

    public class CoordinadorAsignacionViewModel
    {
        public List<AnalistaAsignacion>? ListadoAnalistas { get; set; }

        public InformacionSolicitud? InfoSolicitud { get; set; }
    }

    public class CoordinadorSolicitudAprobada
    {
        public InformacionSolicitud? InfoSolicitud { get; set; }
        public ArchivoSolicitud? ArchivoAprobacionAnalista { get; set; }
    }

    public class AnalistaSolicitud
    {
        public string? NombresApellidos { get; set; } = String.Empty;

        public DateTime? Fecha { get; set; } = DateTime.Now;
    }

    public class CoordinadorGPNDesistimientoModel
    {
        public InformacionSolicitud? InfoSolicitud { get; set; }
        public CoordinadorSolicitudDesistida? DatosDesistimiento { get; set; }
    }

    public class CoordinadorSolicitudDesistida
    {
        public string? ObservacionesDesistimiento { get; set; }
        public DateTime? FechaRadicacionDesistimiento { get; set; }

    }

    public class PropsEnvioCorreoCoordinador
    {
        public decimal IdSolicitud { get; set; }
        public string? Asunto { get; set; }
        public string? Correo { get; set; }
        public string? Body { get; set; }
    }

    public class NumeracionesSolicitudCoordinador
    {
        public string Especie { get; set; } = "";
        public decimal? NumeracionInicial { get; set; } = 0;
        public decimal? NumeracionFinal { get; set; } = 0;
    }

    public class ParametricaTipoSolicitud
    {
        public int IdTipoSolicitud { get; set; }
        public string VistaParcial { get; set; } = string.Empty;
        public string TipoSolicitud { get; set; } = string.Empty;
    }



}
