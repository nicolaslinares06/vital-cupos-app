const moduleReportePrecintos = (() => {

    const btnConsultarDatos = document.querySelector('#btnConsultarDatos');
        //btnExportarExcel = document.querySelector('#btnExportarExcel'),
        //btnExportarPDF = document.querySelector('#btnExportarPDF');

    

    const tablaReportes = async () => {

        const parametros = datosInput();

        let urlBase = window.location.origin;
        let url = `${urlBase}/ReportesPrecintos/ObtenerDatosPrecintos`;
        let resp = await Get(url, { filtros: parametros });
        let dataPrecintosParameters = datosInput();

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
        let columns = [
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




    const inputsSoloNumeros = (nombreElemento) => {
        $(`${nombreElemento}`).on('keypress', function (evt) {
            let regex = /^[0-9]+$/;
            let key = String.fromCharCode(!evt.charCode ? evt.which : evt.charCode);
            if (!regex.test(key)) {
                evt.preventDefault();
                return false;
            }
        });
    }



    const inicializar = () => {

        document.querySelector('#FechaRadicacion').value = null;
        inputsSoloNumeros('#NIT');

    }



    return {
        inicializarProcesos: inicializar
    }


})();


