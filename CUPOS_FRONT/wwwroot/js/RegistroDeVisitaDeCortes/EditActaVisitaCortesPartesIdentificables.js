
document.addEventListener("DOMContentLoaded", async() => {
    visualizarFondoProcesando();

    let btnEditFormularioCorteIdentificable = document.getElementById('btnEditFormularioCorteIdentificable');
    let btnConfirmarGuardar = document.getElementById('btnConfirmarGuardar');
    let btnAceptarModal = document.getElementById('btnAceptarModal');

    let hiddenRadioEstadoPiel = parseInt(document.getElementById('HiddenEstadoPiel').value);
    let radioButtonEstadoPiel = document.getElementById(`EstadoPiel${hiddenRadioEstadoPiel}`);
    radioButtonEstadoPiel.checked = true;


  

    await ObtenerArchivosActaVisita();

  



    btnEditFormularioCorteIdentificable.addEventListener('click', function () {
        const DatosValidos = ValidarDatosFormularioIdentificables();
        if (DatosValidos)
            $('#modalGuardar').modal('show');
        else
            $('#modalAlerta').modal('show');
    });




    btnConfirmarGuardar.addEventListener('click', function () {
        $('#modalGuardar').modal('hide');
        EditarDatosCortesPielIdentificables();
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

    await ObtenerArchivoExcelPrecinto();
    RefrescarEventosPartesIdentificables();
    ocultarFondoProcesando();
});



async function EditarDatosCortesPielIdentificables() {
    visualizarFondoProcesando();
    let datosFormulario = {};
    datosFormulario.ActaVisitaCortes = ObtenerDatosActaVisita();
    datosFormulario.TipoCortesPiel = ObtenerDatosTipoPiel();
    datosFormulario.TipoPartes = ObtenerTipoParteIdentificable();
    datosFormulario.DocumentoOrigenPiel = ObtenerDatosInputDocumentos('DocumentoOrigenPiel');
    datosFormulario.ResolucionorigenPiel = ObtenerDatosInputDocumentos('ResolucionNroOrigenPiel');
    datosFormulario.SalvoCondcutoNumeroOrigenPiel = ObtenerDatosInputDocumentos('SalvoConductoNro');
    datosFormulario.Archivos = ObtenerArchivosBase64();
    let url = `/RegistroVisitaDeCortes/EditActaCortePartesIdentificables`;
    let resp = await Get(url, datosFormulario)
    let resultadoProceso = 0;
    if (resp !== null) {
        if (!resp.Error)
            resultadoProceso = 1;
        document.getElementById('PmensajeProcesado').innerHTML = resp.mensaje;

    }
    document.getElementById('HiddenResultadoProceso').value = resultadoProceso;
    ocultarFondoProcesando();
    $('#modalMensaje').modal('show');

}

function ObtenerDatosActaVisita() {
    let datosActaVisita = {};

    datosActaVisita.ActaVisitaId = document.getElementById("ActaVisitaId").value;
    datosActaVisita.VisitaNumero = ObtenerValorNumeroVisitaActa();
    datosActaVisita.VisitaNumero1 = document.getElementById("radioVisita1").checked;
    datosActaVisita.VisitaNumero2 = document.getElementById("radioVisita2").checked;
    datosActaVisita.TipoEstablecimiento = document.getElementById('TipoEstablecimiento').value;
    datosActaVisita.EstablecimientoID = document.getElementById('EstablecimientoID').value;
    datosActaVisita.CantidadPielACortar = document.getElementById('CantidadPielACortar').value; 
    datosActaVisita.EstadoPiel = ObtenerValorEstadoPiel();   
    datosActaVisita.RepresentanteEstablecimiento = document.getElementById('RepresentanteEstablecimiento').value;
    datosActaVisita.DocumentoRepresentante = 0;
    datosActaVisita.Ciudad = document.getElementById('Ciudad').value;
    datosActaVisita.Departamento = document.getElementById('Departamento').value;
    datosActaVisita.Fecha = document.getElementById('Fecha').value;
    datosActaVisita.ArchivoExcelPrecinto = archivoExcel;

    return datosActaVisita;
}