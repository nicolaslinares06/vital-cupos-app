const moduleReportePrecintos = (() => {

    const btnConsultarDatos = document.querySelector('#btnConsultarDatos');
        //btnExportarExcel = document.querySelector('#btnExportarExcel'),
        //btnExportarPDF = document.querySelector('#btnExportarPDF');

    let dataPrecintosParameters;

    const tablaReportes = async () => {

        const parametros = datosInput();

        let urlBase = window.location.origin;
        let url = `${urlBase}/ReportesPrecintos/ObtenerDatosPrecintos`;
        let resp = await Get(url, { filtros: parametros });
        dataPrecintosParameters = datosInput();

        console.log(resp.listado);
       
        //TablaReporteriaCupos = $(`#TablaReporteriaPrecintos`).DataTable({
        //    destroy: true,
        //    scrollX: true,
        //    lengthChange: true,
        //    lengthMenu: [5, 10, 20, 50, 100],
        //    paging: true,
        //    info: true,
        //    filter: false,
        //    dom: "<'row'<'col-md-12 text-end'<'d-flex align-items-center mt-1'<'col-md-10'l><'col-md-2'i>>>>" + "<'row'<'col-md-12'rt><'col-md-12 text-center'p>>",
        //    language: {
        //        lengthMenu: "Resultados pág. _MENU_",
        //        info: "_START_ al _TOTAL_ Resultados",
        //        search: '',
        //        searchPlaceholder: "Buscar",
        //        zeroRecords: "No se encontraron resultados",
        //        infoEmpty: "0 al 0 Resultados",
        //        paginate: {
        //            previous: 'Anterior',
        //            next: 'Siguiente'
        //        }
        //    },
        //    data: resp.listado,
        var columns = [
            {
                "data": "numeroRadicado"
            },
            {
                "data": "fechaRadicado"
            },
            {
                "data": "nombreEmpresa"
            },
            {
                "data": "nit"
            },
            {
                "data": "ciudad"
            },
            {
                "data": "direccionEntrega"
            },
            {
                "data": "telefono"
            },
            {
                "data": "especie"
            },
            {
                "data": "longMenor"
            },
            {
                "data": "longMayor"
            },
            {
                "data": "cantidad"
            },
            {
                "data": "color"
            },
            {
                "data": "anioProduccion"
            },
            {
                "data": "numeracionInternaInicial"
            },
            {
                "data": "numeracionInternaFinal"
            },
            {
                "data": "numeracionInicial"
            },
            {
                "data": "numeracionInternaFinal"
            },
            {
                "data": "codigoEmpresa"
            },
            {
                "data": "valorConsignacion"
            },
            {
                "data": "analista"
            },
        ];
        //    columnDefs: [{
        //        targets: 19
        //    }]

        //});
        //agregarClasesDatatable(`#TablaReporteriaPrecintos`);
        DataTableGenericoBotones('#TablaReporteriaPrecintos', columns, resp.listado, 19);
    }


    const datosInput = () => {
        const inputs = {};

        inputs.NumeroResolucion = document.querySelector('#NumeroResolucion').value;
        inputs.Establecimiento = document.querySelector('#Establecimiento').value;
        inputs.NIT = document.querySelector('#NIT').value;
        inputs.FechaRadicacion = document.querySelector('#FechaRadicacion').value;
        return inputs;
    }

    btnConsultarDatos.addEventListener("click", async () => {
        await tablaReportes();
    });


    //btnExportarExcel.addEventListener("click", async () => {
      
    //    let urlBase = window.location.origin;
    //    let urlPost = `${urlBase}/ReportesPrecintos/ExportarExcelDataPrecintos`;
    //    const resp = await Get(urlPost, { filtros: dataPrecintosParameters });


    //    let bytes = Base64ToBytes(resp);       
    //    let blob = new Blob([bytes], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });

  
    //    let isIE = false || !!document.documentMode;
    //    if (isIE) {
    //        window.navigator.msSaveBlob(blob, "Listado de Cupos, Empresas y marcaje.xlsx");
    //    } else {
    //        let url = window.URL || window.webkitURL;
    //        let link = url.createObjectURL(blob);
    //        let a = $("<a />");
    //        a.attr("download", "Listado de Precintos.xlsx");
    //        a.attr("href", link);
    //        $("body").append(a);
    //        a[0].click();
    //        $("body").remove(a);
    //    }



    //    //console.log(resp);
    //    //const aElement = document.createElement('a');
    //    //aElement.setAttribute('download', "Listado de Cupos, Empresas y marcaje");
    //    //const href = URL.createObjectURL(resp);
    //    //aElement.href = href;
    //    //aElement.setAttribute('target', '_blank');
    //    //aElement.click();
    //    //URL.revokeObjectURL(href);


    //    //let urlBase = window.location.origin;
    //    //let url = `${urlBase}/ReportesCuposEmpresasMarcaje/ExportarExcelDataEmpresas`;
    //    //fetch(url, {

    //    //    method: 'POST', 
    //    //    headers: {
    //    //        'Content-Type': 'application/json',
    //    //    },
    //    //    body: JSON.stringify(dataCuposActual),


    //    //}).then(response => response.blob())
    //    //    .then((data) => {

    //    //    });


    //});

    const inputsSoloNumeros = (nombreElemento) => {
        $(`${nombreElemento}`).on('keypress', function (evt) {
            var regex = new RegExp("^[0-9]+$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        });
    }

    const inputsSoloLetras = (elemento) => {
        $(elemento).on('keypress', function (event) {
            var regex = new RegExp("^[a-zA-Z \s]+$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        });
    }

    //btnExportarPDF.addEventListener("click", async () => {


    //    await archivoPDFBase64();

    //});
     

    //const archivoPDFBase64 = async () => {

    //    const datos = datosInput();
    //    let urlBase = window.location.origin;
    //    let url = `${urlBase}/ReportesPrecintos/ExportarPDFDataPrecintos`;
    //    let resp = await Get(url, { filtros: dataPrecintosParameters });

    //    const data = await resp;
    //    let a = document.createElement("a");
    //    a.href = "data:application/pdf;base64," + data;
    //    a.download = "Reportes Precintos.pdf";
    //    a.click();



    //}

    const inicializar = () => {

        document.querySelector('#FechaRadicacion').value = null;
        inputsSoloNumeros('#NIT');

    }


    const Base64ToBytes = (base64) => {
        let s = window.atob(base64);
        let bytes = new Uint8Array(s.length);
        for (let i = 0; i < s.length; i++) {
            bytes[i] = s.charCodeAt(i);
        }
        return bytes;
    };




    return {
        inicializarProcesos: inicializar
    }


})();


