using iText.Kernel.Colors;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace CUPOS_FRONT.Utilities
{

    public interface IPDFMetodos
    {
        void crearParrafo(Document doc, string texto, int sizefuente = 14, string alineacion = "left");
        void crearTabla<T>(Document doc, List<string> cabeceras, List<float> anchos, List<T> data, List<string> campos);
    }

    public class PDFMetodos : IPDFMetodos
    {
        public void crearParrafo(Document doc, string texto, int sizefuente = 14,
         string alineacion = "left")
        {
            Paragraph c1 = new Paragraph(texto);
            c1.SetFontSize(sizefuente);
            switch (alineacion)
            {
                case "center": c1.SetTextAlignment(TextAlignment.CENTER); break;
                case "left": c1.SetTextAlignment(TextAlignment.LEFT); break;
                case "right": c1.SetTextAlignment(TextAlignment.RIGHT); break;
                default: c1.SetTextAlignment(TextAlignment.LEFT); break;
            }

            doc.Add(c1);
        }

        public void crearTabla<T>(Document doc, List<string> cabeceras, List<float> anchos,
                                List<T> data, List<string> campos)
        {          
           
            for (int j = 0; j < data.Count; j++)
            {
                Table otable;
                if (anchos == null)
                    otable = new Table(cabeceras.Count);
                else
                    otable = new Table(UnitValue.CreatePercentArray(anchos.ToArray()));
                otable.SetWidth(UnitValue.CreatePercentValue(100));
                otable.SetMarginTop(15);
                Cell ocell;
              

                for (int i = 0; i < campos.Count; i++)
                {
                    ocell = new Cell();                 
                    ocell.Add(new Paragraph(cabeceras[i]));                   
                    var valor = data[j].GetType().GetProperty(campos[i]).GetValue(data[j], null);
                    ocell.Add(new Paragraph(data[j].GetType().GetProperty(campos[i]).GetValue(data[j], null) == null ? "" :
                        data[j].GetType().GetProperty(campos[i]).GetValue(data[j], null).ToString()));
                    otable.AddCell(ocell);
                }
                doc.Add(otable);
            }
       
        }

    }
}
