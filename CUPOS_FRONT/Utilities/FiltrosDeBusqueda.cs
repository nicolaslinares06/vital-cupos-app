using static CUPOS_FRONT.Models.ReportesCuposEmpresasMarcajeModels;

namespace CUPOS_FRONT.Utilities
{
    public interface IFiltrosDeBusqueda
    {
        List<FiltrosEmpresasBooleanos> CombinacionesDeDosFiltros();
        List<FiltrosEmpresasBooleanos> CombinacionesDeUnFiltro();
        int factorial(int numero);
    }
    public class FiltrosDeBusqueda : IFiltrosDeBusqueda
    {
        public int factorial(int numero)
        {
            int total = 1;
            for (int i = 1; i <= numero; i++)
            {
                total *= i;
            }

            return total;

        }

        public List<FiltrosEmpresasBooleanos> CombinacionesDeUnFiltro()
        {
            var listaFiltros = new List<FiltrosEmpresasBooleanos>();
            FiltrosEmpresasBooleanos empresa = new FiltrosEmpresasBooleanos();
            int cantidadPropiedades = empresa.GetType().GetProperties().Count();

            bool[] listabooleanos = new bool[cantidadPropiedades - 1];
            string[] nombrePropiedades = new string[cantidadPropiedades];

           

            int conteo = 0;
            foreach (var propertyInfo in empresa.GetType().GetProperties())
            {
                nombrePropiedades[conteo] = propertyInfo.Name;
                conteo++;

            }


            for (int i = 0; i < listabooleanos.Length; i++)
            {
                listabooleanos = new bool[cantidadPropiedades - 1];
                listabooleanos[i] = true;
                empresa = new FiltrosEmpresasBooleanos();
                empresa.GetType().GetProperty(nombrePropiedades[i]).SetValue(empresa, listabooleanos[i]);
                empresa.BusquedaEspecifica = i + 1;
                listaFiltros.Add(empresa);

            }

            return listaFiltros;




        }

        public List<FiltrosEmpresasBooleanos> CombinacionesDeDosFiltros()
        {
            var listaFiltros = new List<FiltrosEmpresasBooleanos>();
            FiltrosEmpresasBooleanos empresa = new FiltrosEmpresasBooleanos();
            int cantidadPropiedades = empresa.GetType().GetProperties().Count();

            bool[] listabooleanos = new bool[cantidadPropiedades - 1];
            string[] nombrePropiedades = new string[cantidadPropiedades];

            int conteo = 0;
            foreach (var propertyInfo in empresa.GetType().GetProperties())
            {
                nombrePropiedades[conteo] = propertyInfo.Name;
                conteo++;

            }
            int conteoProps = 1;

            for (int i = 0; i < listabooleanos.Length; i++)
            {


                for (int j = i + 1; j < listabooleanos.Length; j++)
                {
                    listabooleanos = new bool[8];
                    empresa = new FiltrosEmpresasBooleanos();
                    listabooleanos[i] = true;
                    listabooleanos[j] = true;
                    empresa.GetType().GetProperty(nombrePropiedades[i]).SetValue(empresa, listabooleanos[i]);
                    empresa.GetType().GetProperty(nombrePropiedades[j]).SetValue(empresa, listabooleanos[j]);
                    empresa.BusquedaEspecifica = conteoProps++;
                    listaFiltros.Add(empresa);



                }

            }

            return listaFiltros;




        }

    }

   
}
