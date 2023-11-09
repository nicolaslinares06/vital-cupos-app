
let fileIndividualFile = document.getElementById('fileIndividualFile');
let contenedorIndividualFile = document.getElementById('contenedorIndividualFile');
let spanFileIndividualFile = document.getElementById('spanFileIndividualFile');
let fileContenedorIndividualFile = document.getElementById('fileContenedorIndividualFile');


let IndividualFileBase64 = '';
let nombreIndividualFile = '';
let tipoAdjuntoIndividualFile = '';
let inc = 0;



document.addEventListener("DOMContentLoaded", () => {
    $("#tablaAnalistasAsignacion").DataTable({
        lengthChange: false,
        lengthMenu: [3],
        paging: true,
        info: false,
        searching: false
    });
    let btnModalTablaAsignacion = document.getElementById('btnModalTablaAsignacion');
    let btnAceptarAsignacion = document.getElementById('btnAceptarAsignacion');
    let btnAceptarModal = document.getElementById('btnAceptarModal');

    btnModalTablaAsignacion.addEventListener('click', function () {
        $('#modalTablaAnalistas').modal('show');
    });

    btnAceptarAsignacion.addEventListener('click', function () {
        EnviarDatosAsignacionAnalista();
    });

    btnAceptarModal.addEventListener('click', function () {
        let urlBase = window.location.origin;
        let url = `${urlBase}/CoordinadorGestionPrecintosNacionales/Index`;

        location.href = url;
    });

    $('#tablaAnalistasAsignacion_paginate').on('click .paginate_button', function () {
        $('.tr-anls').removeClass('selected-datatable');
        let hiddenTRSeleccionado = document.getElementById('HiddenTRSeleccionado');
        if (hiddenTRSeleccionado.value != "") {
            $('#' + hiddenTRSeleccionado.value).addClass('selected-datatable');
        }
    });

});




function adjuntoHTML(contenedor, Base64, tipoAdjunto, nombre, inputFile, codigo = null) {
    if (codigo != null) {
        contenedor.innerHTML +=
            `<div class="row w-100 mt-2 contenerAdjuntos" id="divAdjunto${codigo}">
                            <div class="col-11">
                                <a type="buttton" class="btnPrevisualizar" id="btnPrevisualizar${codigo}">${nombre}</a>
                            </div>
                            <div class="text-end col-1">
                                <a type="button" class="btnEliminarAdjunto" id="btnEliminarAdjunto${codigo}"><span class="fas fa-times"></span></a>
                            <div>
                        <div>`;
        return;
    }
    contenedor.innerHTML =
        `<div class="row w-100 mt-2 contenerAdjuntos">
                        <div class="col-11">
                            <a type="buttton" id="btnPrevisualizar">${nombre}</a>
                        </div>
                        <div class="text-end col-1">
                            <a type="button" id="btnEliminarAdjunto"><span class="fas fa-times"></span></a>
                        <div>
                    <div>`;
    if (inputFile != null) {
        ocultarElemento(inputFile, true);
    }
    $('#btnPrevisualizar').on('click', function () {
        let newWindow = window.open();
        newWindow.document.write('<iframe src=' + Base64 + ' style="height:100%; width:100%;"></iframe>');
    });
    $('#btnEliminarAdjunto').on('click', function () {
        contenedor.innerHTML = '';
        ocultarElemento(inputFile, false);
    });
    let adjunto = {
        'Base64': Base64,
        'tipoAdjunto': tipoAdjunto,
        'nombre': nombre
    }
    return adjunto;
}

async function mostrarAdjunto(file, contenedor, span, extencionesPermitidas, strExtenciones, inputFile = null, adjuntoMultiple = []) {
    if (file.size > 2000000) {
        span.innerText = 'Tamaño maximo permitido es de 2MB';
        ocultarElemento(span, false);
        file = '';
        return;
    }
    if (extencionesPermitidas.includes(file.type)) {
        const Base64 = await toBase64(file);
        if (inputFile == null) {
            inc++;
            let codigo = 0;
            if (adjuntosMultipleFile.length > 0) {
                adjuntosMultipleFile.forEach(el => {
                    if (codigo < el.codigo) {
                        codigo = el.codigo;
                    }
                });
            }
            adjuntoMultiple.push({
                'codigo': codigo == 0 ? inc : codigo + 1,
                'Base64': Base64,
                'nombreAdjunto': file.name,
                'tipoAdjunto': file.type
            });
            contenedor.innerHTML = "";
            adjuntoMultiple.forEach(el => {
                adjuntoHTML(contenedor, el.Base64, el.tipoAdjunto, el.nombreAdjunto, inputFile, el.codigo);
            });
            return;
        }
        return adjuntoHTML(contenedor, Base64, file.type, file.name, inputFile);
    }
    span.innerText = 'Solo se admiten tipos de documentos ' + strExtenciones;
    ocultarElemento(span, false);
    file = '';
}

$('#fileIndividualFile').on('change', async function () {
    ocultarElemento(spanFileIndividualFile, true);
    contenedorIndividualFile.innerHTML = "";
    IndividualFileBase64 = '';
    nombreIndividualFile = '';
    tipoAdjuntoIndividualFile = '';
    let dato_archivo = $('#fileIndividualFile').prop("files")[0];
    let extencionesPermitidas = ["application/pdf"];
    let strExtenciones = ".pdf";
    let adjunto = await mostrarAdjunto(dato_archivo, contenedorIndividualFile, spanFileIndividualFile, extencionesPermitidas, strExtenciones, fileContenedorIndividualFile);
    IndividualFileBase64 = adjunto.Base64;
    nombreIndividualFile = adjunto.nombre;
    tipoAdjuntoIndividualFile = adjunto.tipoAdjunto;
});

//fileContenedorIndividualFile.addEventListener('dragover', (e) => {
//    e.preventDefault();
//    ocultarElemento(spanFileIndividualFile, true);
//    $(fileContenedorIndividualFile).addClass('activeFile');
//});

//fileContenedorIndividualFile.addEventListener('dragleave', (e) => {
//    e.preventDefault();
//    ocultarElemento(spanFileIndividualFile, true);
//    $(fileContenedorIndividualFile).removeClass('activeFile');
//});

//fileContenedorIndividualFile.addEventListener('drop', (e) => {
//    e.preventDefault();
//    contenedorIndividualFile.innerHTML = "";
//    ocultarElemento(spanFileIndividualFile, true);
//    $(fileContenedorIndividualFile).removeClass('activeFile');
//    var files = e.dataTransfer.files;
//    if (files.length > 1) {
//        spanFileIndividualFile.innerText = 'Solo se admite un adjunto.';
//        ocultarElemento(spanFileIndividualFile, false);
//        return
//    }
//    var extencionesPermitidas = ["application/pdf"];
//    var strExtenciones = ".pdf";
//    var adjunto = mostrarAdjunto(files[0], contenedorIndividualFile, spanFileIndividualFile, extencionesPermitidas, strExtenciones, fileContenedorIndividualFile);
//    IndividualFileBase64 = adjunto.Base64;
//    nombreIndividualFile = adjunto.nombre;
//    tipoAdjuntoIndividualFile = adjunto.tipoAdjunto;
//});

//function show() {
//    var rowId =
//        event.target.parentNode.parentNode.id;
//    //this gives id of tr whose button was clicked
//    var data =
//        document.getElementById(rowId).querySelectorAll(".row-data");
//    /*returns array of all elements with 
//    "row-data" class within the row with given id*/

//    var name = data[0].innerHTML;
//    var age = data[1].innerHTML;

//    alert("Name: " + name + "\nAge: " + age);
//}

$("#tablaAnalistasAsignacion tr").click(function () {
    let hiddenTRSeleccionado = document.getElementById('HiddenTRSeleccionado');
    $(this).addClass('selected-datatable').siblings().removeClass('selected-datatable');
    let value = this.id;
    hiddenTRSeleccionado.value = value;
    console.log(value);
});

async function EnviarDatosAsignacionAnalista() {
    visualizarFondoProcesando();
    let valorAnalistaSeleccionado = parseInt(document.getElementById('HiddenTRSeleccionado').value);
    let spanTablaAnalistaSeleccion = document.getElementById('spanTablaAnalistaSeleccion');
    spanTablaAnalistaSeleccion.textContent = "";

    if (!(valorAnalistaSeleccionado > 0)) {
        spanTablaAnalistaSeleccion.textContent = "Debe Seleccionar un registro";
        ocultarFondoProcesando();
        return;
    }


    let urlBase = window.location.origin;
    let url = `${urlBase}/CoordinadorGestionPrecintosNacionales/EnviarDatosAsignacionAnalista`;
    let datos = ObtenerDatosEnvioAsignacion();

    let resp = await Get(url, { datosActualizacion: datos });


    $('#modalTablaAnalistas').modal('hide');
    ocultarFondoProcesando();
    $('#modalMensajeAsignacion').modal('show');

    let mensaje = document.getElementById('PmensajeProcesado');
    mensaje.innerHTML = resp.mensaje;



    console.log(resp.datosActuales);
}

function ObtenerDatosEnvioAsignacion() {
    let datos = {
        AnalistaId: document.getElementById('HiddenTRSeleccionado').value,
        CodigoSolicitud: document.querySelector('#IdSolicitud').value
    }

    return datos;
}



