let high_contrast = document.getElementById("high_contrast");
let mensajeModalCerrarSesion = document.getElementById(
  "mensajeModalCerrarSesion"
);
let btnSiNoCerrarSsesion = document.getElementById("btnSiNoCerrarSsesion");
let btnAceptarCerrarSsesion = document.getElementById(
  "btnAceptarCerrarSsesion"
);

//Pagina en 0 0
$(document).ready(function () {
    window.scrollTo(0, 0);
});

let token = "";

async function Post(url, params = null) {
  var settings = {
    type: "POST",
    async: true,
    crossDomain: true,
    url: url,
    method: "POST",
    data: params,
  };

  if (token != "") {
    settings.headers = {
      Authorization: "Bearer " + token,
    };
  }

  let result;
  try {
    result = await $.ajax(settings);
    return result;
  } catch (error) {
    console.error(error);
  }
}

async function Get(url, params = null) {
  $.ajaxSetup({
    beforeSend: function (xhr) {
      xhr.setRequestHeader("Authorization", token);
    },
  });

  let result;
  try {
    result = await $.ajax({
      url: url,
      type: "POST",
      data: params,
    });
    return result;
  } catch (error) {
    console.error(error);
  }
}

//funcion para mostrar o no las tabs al seleccionarlas
function loadTab(tab) {
  $(".tab-contenido").hide();
  $(`.contenido-${tab}`).show();
}

//carga el selector del tab
function loadTabSelector(tab) {
  $(".tab-selector").prop("checked", false);
  $(`.tab-selector-${tab}`).prop("checked", true);
}

//ocultar elemento
function ocultarElemento(elemento, validar) {
  if (validar) {
    if (!$(elemento).hasClass("d-none")) {
      $(elemento).addClass("d-none");
    }
  } else {
    if ($(elemento).hasClass("d-none")) {
      $(elemento).removeClass("d-none");
    }
  }
}

//funcion para deshabilitar controles
function disabledControl(elemento, validar) {
  if (validar) {
    if (!elemento.hasAttribute("disabled")) {
      $(elemento).prop("disabled", true);
    }
  } else {
    if (elemento.hasAttribute("disabled")) {
      elemento.removeAttribute("disabled");
    }
  }
}

async function GetBussiness(slt, url){
  let empresas = await Get(url);
  slt.innerHTML=`<option value="0">Seleccione establecimiento...</option>`;
  empresas.forEach(el=>{
    slt.innerHTML+=`<option value="${el.code}">${el.name}</option>`;
  });
  return empresas;
}

//funcion para poner controles de solo lectura
function readOnly(elemento, validar, val = false) {
  if (validar) {
    if (val) {
      if (!elemento.hasAttribute("readonly")) {
        $(elemento).attr("readonly", true);
      }
      return;
    }
    if (!elemento.hasAttribute("readonly")) {
      $(elemento).prop("readonly", true);
    }
  } else {
    if (elemento.hasAttribute("readonly")) {
      elemento.removeAttribute("readonly");
    }
  }
}

//reinicia los elementos de advertencia
function reiniciarElementos(elemento, span, val = false) {
  if (val) {
    if (elemento.hasClass("required-validate")) {
      elemento.removeClass("required-validate");
    }
    ocultarElemento(span, true);
    return;
  }
  if ($(elemento).hasClass("required-validate")) {
    $(elemento).removeClass("required-validate");
  }
  ocultarElemento(span, true);
}

//se traen los tipos de documentos y se llena el select
async function traerTiposDocumento(slt, url) {
  let r = await Get(url);
  if (r.volverInicio != undefined) {
    $("#modalConfimaciones").modal("hide");
    cerrarSesionCaducada();
    return;
  }
  slt.innerHTML = `<option value="0">Tipo de documento</option>`;
  r.forEach((el) => {
    slt.innerHTML += `<option value="${el.code}">${el.name}</option>`;
  });
}

//se traen los tipos de documentos y se llena el select
async function traerCiudades(slt, url) {
  let r = await Get(url);
  if (r.volverInicio != undefined) {
    cerrarSesionCaducada();
    return;
  }
  $(slt).select2({
    data: r,
    query: function (q) {
      var pageSize = 10,
        results = this.data.filter(function (e) {
          return contains(e.text, q.term);
        });

      // Get a page sized slice of data from the results of filtering the data set.
      var paged = results.slice((q.page - 1) * pageSize, q.page * pageSize);

      q.callback({
        results: paged,
        more: results.length >= q.page * pageSize,
      });
    },
  });
  $(slt).val("").trigger("change");
}

//se convierte archivo a base64
const toBase64 = (file) =>
  new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result);
    reader.onerror = (error) => reject(error);
  });

function loadTabs(tab) {
  if ($(".nav-link").hasClass("active")) {
    $(".nav-link").removeClass("active");
  }

  if ($(".tab-pane").hasClass("active")) {
    $(".tab-pane").removeClass("active");
  }
  if ($(".tab-pane").hasClass("show")) {
    $(".tab-pane").removeClass("show");
  }

  if (!$(`#tab-${tab}`).hasClass("active")) {
    $(`#tab-${tab}`).addClass("active");
  }

  if (!$(`#tab${tab}`).hasClass("active")) {
    $(`#tab${tab}`).addClass("active");
  }
  if (!$(`#tab${tab}`).hasClass("show")) {
    $(`#tab${tab}`).addClass("show");
  }
}

function DataTableGenericoBotones(idTabla, columnas, datos, target) {
  $(idTabla).DataTable({
    destroy: true,
    scrollX: true,
    lengthChange: true,
    lengthMenu: [5, 10, 20, 50, 100],
    paging: true,
    info: true,
    dom:
      "<'row'<'col-md-12'f><'col-md-6 mt-1'B><'col-md-6 text-end'<'d-flex align-items-center mt-1'<'col-md-10'l><'col-md-2'i>>>>" +
      "<'row'<'col-md-12'rt><'col-md-12 text-center'p>>",
    buttons: [
      {
          extend: 'excel',
          title: 'Archivo',
          filename: 'Export_File',
          tag: 'button',
          className: 'btn btn-primary',
          text: `<i class="fa-solid fa-download"></i> EXCEL`
      },
      {
        extend: 'pdf',
        orientation: 'landscape',
        pageSize: 'TABLOID',
        title: 'Archivo PDF',
        filename: 'Export_File_pdf',
        tag: 'button',
        className: 'btn btn-danger',
        text: `<i class="fa-solid fa-download"></i> PDF`
      }
    ],
    language: {
      lengthMenu: "Resultados pág. _MENU_",
      info: "_START_ al _TOTAL_ Resultados",
      search: "",
      searchPlaceholder: "Buscar",
      zeroRecords: "No se encontraron resultados",
      infoEmpty: "0 al 0 Resultados",
      paginate: {
        previous: "Anterior",
        next: "Siguiente",
      },
    },
    data: datos,
    columns: columnas,
    columnDefs: [
      {
        targets: target,
      },
    ],
  });
  agregarClasesDatatable(idTabla);
}


function DataTableGenerico(idTabla, columnas, datos, target) {
  $(idTabla).DataTable({
    destroy: true,
    scrollX: true,
    lengthChange: true,
    lengthMenu: [5, 10, 20, 50, 100],
    paging: true,
    info: true,
    dom:
      "<'row'<'col-md-12'f><'col-md-12 text-end'<'d-flex align-items-center mt-1'<'col-md-10'l><'col-md-2'i>>>>" +
      "<'row'<'col-md-12'rt><'col-md-12 text-center'p>>",
    language: {
      lengthMenu: "Resultados pág. _MENU_",
      info: "_START_ al _TOTAL_ Resultados",
      search: "",
      searchPlaceholder: "Buscar",
      zeroRecords: "No se encontraron resultados",
      infoEmpty: "0 al 0 Resultados",
      paginate: {
        previous: "Anterior",
        next: "Siguiente",
      },
    },
    data: datos,
    columns: columnas,
    columnDefs: [
      {
        targets: target,
      },
    ],
  });
  agregarClasesDatatable(idTabla);
}

function DataTableGenericoSinBuscador(idTabla, columnas, datos, target) {
  $(idTabla).DataTable({
    destroy: true,
    scrollX: true,
    lengthChange: true,
    lengthMenu: [5, 10, 20, 50, 100],
    paging: true,
    info: true,
    searching:false,
    dom:
      "<'row'<'col-md-12'f><'col-md-12 text-end'<'d-flex align-items-center mt-1'<'col-md-10'l><'col-md-2'i>>>>" +
      "<'row'<'col-md-12'rt><'col-md-12 text-center'p>>",
    language: {
      lengthMenu: "Resultados pág. _MENU_",
      info: "_START_ al _TOTAL_ Resultados",
      search: "",
      searchPlaceholder: "Buscar",
      zeroRecords: "No se encontraron resultados",
      infoEmpty: "0 al 0 Resultados",
      paginate: {
        previous: "Anterior",
        next: "Siguiente",
      },
    },
    data: datos,
    columns: columnas,
    columnDefs: [
      {
        targets: target,
      },
    ],
  });
  agregarClasesDatatable(idTabla);
}

function agregarClasesDatatable(idTabla) {
  $(`${idTabla}_filter label`).addClass("col-md-12 label-datatable");
  $(`${idTabla}_filter input`).addClass("input-datatable");

  if ($(idTabla).find("tbody tr").length < 5) {
    $(`${idTabla}_paginate`).hide();
  } else {
    $(`${idTabla}_paginate`).show();
  }
}

function cerrarSesion() {
  limpiarModalCerrarSesion();
  ocultarElemento("#btnSiNoCerrarSsesion", false);
  $("#mensajeModalCerrarSesion").text("¿Está seguro de que desea salir?");
}

function limpiarModalCerrarSesion() {
  ocultarElemento("#btnSiNoCerrarSsesion", true);
  ocultarElemento("#btnAceptarCerrarSsesion", true);
}

function cerrarSesionCaducada() {
  limpiarModalCerrarSesion();
  ocultarElemento("#btnAceptarCerrarSsesion", false);
  $("#mensajeModalCerrarSesion").text(
    "La sesión ah caducado, debe volver a iniciar"
  );
  $("#modalCerrarSesion").modal("show");
}

function adjuntoHTML(
  contenedor,
  Base64,
  tipoAdjunto,
  nombre,
  inputFile,
  codigo = null,
  ver=false
) {
  if (codigo != null) {
    if(ver){
      contenedor.innerHTML += `<div class="row w-100 mt-2 contenerAdjuntos" id="divAdjunto${codigo}">
                        <div class="col-11">
                            <a type="buttton" class="btnPrevisualizar" id="btnPrevisualizar${codigo}">${nombre}</a>
                        </div>
                    <div>`;
    }else{
      contenedor.innerHTML += `<div class="row w-100 mt-2 contenerAdjuntos" id="divAdjunto${codigo}">
                        <div class="col-11">
                            <a type="buttton" class="btnPrevisualizar" id="btnPrevisualizar${codigo}">${nombre}</a>
                        </div>
                        <div class="text-end col-1">
                            <a type="button" class="btnEliminarAdjunto" id="btnEliminarAdjunto${codigo}"><span class="fas fa-times"></span></a>
                        <div>
                    <div>`;
    }
    return;
  }
  if(ver){
    contenedor.innerHTML = `<div class="row w-100 mt-2 contenerAdjuntos">
                    <div class="col-11">
                        <a type="buttton" id="btnPrevisualizar">${nombre}</a>
                    </div>
                <div>`;
  }else{
    contenedor.innerHTML = `<div class="row w-100 mt-2 contenerAdjuntos">
                    <div class="col-11">
                        <a type="buttton" id="btnPrevisualizar">${nombre}</a>
                    </div>
                    <div class="text-end col-1">
                        <a type="button" id="btnEliminarAdjunto"><span class="fas fa-times"></span></a>
                    <div>
                <div>`;
  }
  if (inputFile != null) {
    ocultarElemento(inputFile, true);
  }
  $("#btnPrevisualizar").on("click", function () {
    var newWindow = window.open();
    newWindow.document.write(
      "<iframe src=" + Base64 + ' style="height:100%; width:100%;"></iframe>'
    );
  });
  $("#btnEliminarAdjunto").on("click", function () {
    contenedor.innerHTML = "";
    ocultarElemento(inputFile, false);
  });
  var adjunto = {
    adjuntoBase64: Base64,
    tipoAdjunto: tipoAdjunto,
    nombreAdjunto: nombre,
  };
  return adjunto;
}

async function mostrarAdjunto(
  file,
  contenedor,
  span,
  extencionesPermitidas,
  strExtenciones,
  inputFile = null,
  adjuntoMultiple = [],
  accionEditar = false,
  adjuntosNuevos = [],
  adjuntosOriginales = []
) {
  if (file.size > 2000000) {
    span.innerText = "Tamaño maximo permitido es de 2MB";
    ocultarElemento(span, false);
    file = "";
    return;
  }
  if (extencionesPermitidas.includes(file.type)) {
    let Base64 = await toBase64(file);
    if (inputFile == null) {
      let codigo = 0;
      if (adjuntoMultiple.length > 0) {
        adjuntoMultiple.forEach((el) => {
          if (codigo < el.codigo) {
            codigo = el.codigo;
          }
        });
      }
      if (accionEditar) {
        if (adjuntosOriginales.length > 0) {
          adjuntosOriginales.forEach((el) => {
            if (parseInt(el.codigo) > codigo) {
              codigo = parseInt(el.codigo);
            }
          });
        }

        adjuntosNuevos.push({
          codigo: codigo == 0 ? 1 : codigo + 1,
          adjuntoBase64: Base64,
          nombreAdjunto: file.name,
          tipoAdjunto: file.type,
        });
      }
      adjuntoMultiple.push({
        codigo: codigo == 0 ? 1 : codigo + 1,
        adjuntoBase64: Base64,
        nombreAdjunto: file.name,
        tipoAdjunto: file.type,
      });
      contenedor.innerHTML = "";
      adjuntoMultiple.forEach((el) => {
        adjuntoHTML(
          contenedor,
          el.adjuntoBase64,
          el.tipoAdjunto,
          el.nombreAdjunto,
          inputFile,
          el.codigo
        );
      });
      return;
    }
    return adjuntoHTML(contenedor, Base64, file.type, file.name, inputFile);
  }
  span.innerText = "Solo se admiten tipos de documentos " + strExtenciones;
  ocultarElemento(span, false);
  file = "";
}

function confSelectScrollInfite(idSelect, datos) {
  $(idSelect).select2({
    data: datos,
    query: function (q) {
      let pageSize = 10,
        results = this.data.filter(function (e) {
          return contains(e.text, q.term);
        });

      // Get a page sized slice of data from the results of filtering the data set.
      let paged = results.slice((q.page - 1) * pageSize, q.page * pageSize);

      q.callback({
        results: paged,
        more: results.length >= q.page * pageSize,
      });
    },
  });
  $(idSelect).val("").trigger("change");
}

function alertasValidacionesControles(modal, ul, camposObligatorios) {
  ul.innerHTML = "";
  camposObligatorios.forEach((el) => {
    ul.innerHTML += `<li class="list-group-item">${el}</li>`;
  });
  $(modal).modal("show");
}


function DatatableGenericoSinDatos(idTabla){
    $(idTabla).DataTable({
        destroy: true,
        scrollX: true,
        lengthChange: true,
        lengthMenu: [5, 10, 20, 50, 100],
        paging: true,
        info: true,
        dom:
            "<'row'<'col-md-12'f><'col-md-12 text-end'<'d-flex align-items-center mt-1'<'col-md-10'l><'col-md-2'i>>>>" +
            "<'row'<'col-md-12'rt><'col-md-12 text-center'p>>",
        language: {
            lengthMenu: "Resultados pág. _MENU_",
            info: "_START_ al _TOTAL_ Resultados",
            search: "",
            searchPlaceholder: "Buscar",
            zeroRecords: "No se encontraron resultados",
            infoEmpty: "0 al 0 Resultados",
            paginate: {
                previous: "Anterior",
                next: "Siguiente",
            },
        },
    });
    agregarClasesDatatable(idTabla);
}

const visualizarFondoProcesando = () => {
    $.LoadingOverlay("show", {
        image: "",
        text: "Procesando...",
        textColor: "#3366CC", // Cambia al color que prefieras para el texto
        background: "rgba(255, 255, 255, 0.8)", // Cambia al color que prefieras para el fondo

    });
}

const ocultarFondoProcesando = () => {
    $.LoadingOverlay("hide");
}

function ajustarColumnasTabla(idTabla) {
    var table = $('#' + idTabla).DataTable();
    table.columns.adjust().draw();
}