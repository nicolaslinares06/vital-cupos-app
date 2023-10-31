const module = (() => {

    const btnConsultarDatos = document.querySelector('#btnConsultarDatos'),
        //btnExportarExcel = document.querySelector('#btnExportarExcel'),
        //btnExportarPDF = document.querySelector('#btnExportarPDF'),
        btnFechaEmisionResolucionHasta = document.querySelector('#FechaEmisionResolucionHasta'),
        btnFechaFechaEmisionResolucionDesde = document.querySelector('#FechaEmisionResolucionDesde');

    let datosPorDefecto;

    const tablaReportes = async () => {

        const parametros = datosInput();

        let urlBase = window.location.origin;
        let url = `${urlBase}/ReportesCuposEmpresasMarcaje/ObtenerDatosCuposEmpresas`;
        let resp = await Get(url, { filtros: parametros });        
        datosPorDefecto = datosInput();

        console.log(resp.listado);



        //TablaReporteriaCupos = $(`#TablaReporteriaCupos`).DataTable({
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
                "data": "tipoEmpresa"
            },
            {
                "data": "nombreEmpresa"
            },
            {
                "data": "nit"
            },
            {
                "data": "estado"
            },
            {
                "data": "estadoEmisionCITES"                
            },
            {
                "data": "numeroResolucion"

            },
            {
                "data": "fechaEmisionResolucion"

            },
            {
                "data": "especies"

            },
            {
                "data": "machos" 
            },
            {
                "data": "hembras"
            },
            {
                "data": "poblacionTotalParental"
            },
            {
                "data": "anioProduccion"
            },
            {
                "data": "cuposComercializacion"
            },
            {
                "data": "cuotaRepoblacion"
            },
            {
                "data": "cuposAsignadosTotal"
            },
            {
                "data": "soportesRepoblacion",
                render: function(data){
                    if(data){
                        return 'Si';
                    }
                    return 'No';
                }
            },
            {
                "data": "cupoUtilizado"
            },
            {
                "data": "cupoDisponible"
            },
        ];
            //columnDefs: [{
            //    targets: 5
            //}]

        //});
        //agregarClasesDatatable(`#TablaReporteriaCupos`);
        DataTableGenericoBotones('#TablaReporteriaCupos', columns, resp.listado, 17);
    };


    const datosInput = () => {
        const inputs = {};

        inputs.TipoEmpresa = document.querySelector('#TipoEmpresa').value;
        inputs.NIT = document.querySelector('#NIT').value;
        inputs.NombreEmpresa = document.querySelector('#NombreEmpresa').value;
        inputs.NumeroResolucion = document.querySelector('#NumeroResolucion').value;
        inputs.Estado = document.querySelector('#Estado').value;
        inputs.EstadoEmisionCITES = document.querySelector('#EstadoEmisionCITES').value;
        inputs.FechaEmisionResolucionDesde = document.querySelector('#FechaEmisionResolucionDesde').value;
        inputs.FechaEmisionResolucionHasta = document.querySelector('#FechaEmisionResolucionHasta').value;

        return inputs;
    }

    btnConsultarDatos.addEventListener("click", async () => {
        await tablaReportes();
    });


    //btnExportarExcel.addEventListener("click", async () => {
    
    //    let urlBase = window.location.origin;
    //    let urlPost = `${urlBase}/ReportesCuposEmpresasMarcaje/ExportarExcelDataEmpresas`;
    //    const resp = await Get(urlPost, { dataCuposActual: datosPorDefecto });


    //    let bytes = Base64ToBytes(resp);

    //    //Convert Byte Array to BLOB.
    //    let blob = new Blob([bytes], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });

    //    //Check the Browser type and download the File.
    //    let isIE = false || !!document.documentMode;
    //    if (isIE) {
    //        window.navigator.msSaveBlob(blob, "Listado de Cupos, Empresas y marcaje.xlsx");
    //    } else {
    //        let url = window.URL || window.webkitURL;
    //        let link = url.createObjectURL(blob);
    //        let a = $("<a />");
    //        a.attr("download", "Listado de Cupos, Empresas y marcaje.xlsx");
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

    const inputsSoloNumeros = (nombreElemento)=> {
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

    btnFechaFechaEmisionResolucionDesde.addEventListener("change",  (event) => {

        let valorFechaHasta = btnFechaEmisionResolucionHasta.value;
        if (event.target.value > valorFechaHasta)
            return;       

    });

    const archivoPDFBase64 = async () => {
        
        let urlBase = window.location.origin;
        let url = `${urlBase}/ReportesCuposEmpresasMarcaje/ExportarPDFDataEmpresas`;
        let resp = await Get(url, { dataCuposActual: datosPorDefecto });

        const data = await resp;
        let a = document.createElement("a");
        a.href = "data:application/pdf;base64," + data;
        a.download = "Reportes Empresas y Cupos.pdf";
        a.click();



    }

    const inicializar = () => {
        document.querySelector('#FechaEmisionResolucionDesde').value = null;
        document.querySelector('#FechaEmisionResolucionHasta').value = null;
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


