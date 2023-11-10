const module = (() => {

    const btnConsultarDatos = document.querySelector('#btnConsultarDatos'),
        //btnExportarExcel = document.querySelector('#btnExportarExcel'),
        //btnExportarPDF = document.querySelector('#btnExportarPDF'),
        btnFechaEmisionResolucionHasta = document.querySelector('#FechaEmisionResolucionHasta'),
        btnFechaFechaEmisionResolucionDesde = document.querySelector('#FechaEmisionResolucionDesde');



    const tablaReportes = async () => {

        const parametros = datosInput();

        let urlBase = window.location.origin;
        let url = `${urlBase}/ReportesCuposEmpresasMarcaje/ObtenerDatosCuposEmpresas`;
        let resp = await Get(url, { filtros: parametros });        
        let datosPorDefecto = datosInput();

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
        let columns = [
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




    const inputsSoloNumeros = (nombreElemento)=> {
        $(`${nombreElemento}`).on('keypress', function (evt) {
            let regex = new RegExp("^[0-9]+$");
            let key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                evt.preventDefault();
                return false;
            }
        });
    }

  



    btnFechaFechaEmisionResolucionDesde.addEventListener("change",  (event) => {

        let valorFechaHasta = btnFechaEmisionResolucionHasta.value;
        if (event.target.value > valorFechaHasta)
            return;       

    });

   

    const inicializar = () => {
        document.querySelector('#FechaEmisionResolucionDesde').value = null;
        document.querySelector('#FechaEmisionResolucionHasta').value = null;
        inputsSoloNumeros('#NIT');

    }





 

    return {
        inicializarProcesos: inicializar
    }

    
})();


