

document.addEventListener("DOMContentLoaded", () => {


    let btnConfirmarGuardar = document.getElementById('btnConfirmarGuardar');
    let btnAceptarModal = document.getElementById('btnAceptarModal');
    let btnGuardarFormulario = document.getElementById('btnGuardarFormulario');
    const select = document.querySelector('#SelectTipoActaCorte');
    select.value = '1';



    btnGuardarFormulario.addEventListener('click', function () {       
        const DatosValidos = ValidarDatosFormularioIdentificables();
        if (DatosValidos)
            $('#modalGuardar').modal('show');
        else
            $('#modalAlerta').modal('show');
    });


    btnConfirmarGuardar.addEventListener('click', function () {
        $('#modalGuardar').modal('hide');
        GuardarDatosRecortesIdentificables();
    });

    btnAceptarModal.addEventListener('click', function () {
        let valorResultado = parseInt(document.getElementById('HiddenResultadoProceso').value);
        if (valorResultado > 0) {
            let urlBase = window.location.origin;
            window.location = `${urlBase}/RegistroVisitaDeCortes/Index`;
        }
           
        else {
            $('#modalMensaje').modal('hide');
        }
    });


    let btnAgregarTipoPiel = document.getElementById('btnAgregarTipoPiel');
    btnAgregarTipoPiel.addEventListener('click', function () {
        AgregarSeccionINputTipoPiel();
        RefrescarEventosPartesIdentificables();
    });

    let btnAgregarParte = document.getElementById("btnAgregarParte");
    btnAgregarParte.addEventListener('click', function () {
        const propertyElements = {
            divPadre1: "divSubSeccion1TipoParte",
            divPadre2: "divSubSeccion2TipoParte",
            nameElementos1: "TipoParteidntificable",
            nameElementos2: "CantidadTipoParteidntificable",
            divTataraAbuelo: "divContenedorCantidadPartesPieles",
            placeholderInputSeccion1: "Inserte un tipo de parte"
        };
        AgregarSeccionInputTipoParte(propertyElements);
        RefrescarEventosPartesIdentificables();
    });

    RefrescarEventosPartesIdentificables();
});



async function GuardarDatosRecortesIdentificables() {

    visualizarFondoProcesando();

    let datosFormulario = {};
    datosFormulario.VisitaNumero1 = document.getElementById("radioVisita1").checked;
    datosFormulario.VisitaNumero2 = document.getElementById("radioVisita2").checked;
    datosFormulario.TipoEstablecimiento = document.getElementById('TipoEstablecimiento').value;
    datosFormulario.EstablecimientoID = document.getElementById('EstablecimientoID').value;
    datosFormulario.CantidadPielACortar = document.getElementById('CantidadPielACortar').value;
   /* datosFormulario.PrecintoIdentificacion = document.getElementById('PrecintoIdentificacion').value;*/
    datosFormulario.EstadoPiel = ObtenerValorEstadoPiel();
    //datosFormulario.FuncionarioAutoridadCites = document.getElementById('FuncionarioAutoridadCites').value;
    datosFormulario.RepresentanteEstablecimiento = document.getElementById('RepresentanteEstablecimiento').value;
    datosFormulario.DocumentoRepresentante = 0;
    datosFormulario.Ciudad = document.getElementById('Ciudad').value;
    datosFormulario.Fecha = document.getElementById('Fecha').value;
    datosFormulario.TipoCortesPiel = ObtenerDatosTipoPiel();
    datosFormulario.TipoPartes = ObtenerTipoParteIdentificable();
    datosFormulario.DocumentoOrigenPiel = ObtenerDatosInputDocumentos('DocumentoOrigenPiel');
    datosFormulario.ResolucionorigenPiel = ObtenerDatosInputDocumentos('ResolucionNroOrigenPiel');
    datosFormulario.SalvoCondcutoNumeroOrigenPiel = ObtenerDatosInputDocumentos('SalvoConductoNro');
    datosFormulario.Archivos = ObtenerArchivosBase64();
    datosFormulario.ArchivoExcelPrecinto = archivoExcel;
    let resultadoProceso = 0;
    let url = `/RegistroVisitaDeCortes/CreateActaCortePartesIdentificables`;
    let resp = await Get(url, datosFormulario);
    if (resp !== null) {
        if (!resp.Error)
            resultadoProceso = 1;
        document.getElementById('PmensajeProcesado').innerHTML = resp.mensaje;

    }
    document.getElementById('HiddenResultadoProceso').value = resultadoProceso;
    ocultarFondoProcesando();
    $('#modalMensaje').modal('show');


}