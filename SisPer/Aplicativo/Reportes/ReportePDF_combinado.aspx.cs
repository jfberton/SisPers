using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo.Reportes
{
    public partial class ReportePDF_combinado : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                byte[] bytesResol = Session["BytesFrente"] as byte[];
                byte[] bytesAnexo = Session["BytesReverso"] as byte[];

                byte[] ReporteTotal = PDFLibrary.PdfMerger.MergeFiles(new List<byte[]> { bytesResol, bytesAnexo });

                if (ReporteTotal != null)
                {
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.ContentType = "application/pdf";
                    Response.BinaryWrite(ReporteTotal);
                    Response.End();
                }
            }
        }
    }
}