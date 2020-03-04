using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using OfficeOpenXml;
using System.Net;

namespace SisPer
{
    public class GridViewExportUtil
    {

        public static void Export(string fileName, GridView gv)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader(
                "content-disposition", string.Format("attachment; filename={0}", fileName));
            HttpContext.Current.Response.ContentType = "application/ms-excel";

            using (ExcelPackage eep = new ExcelPackage())
            {
                ExcelWorksheet ews = eep.Workbook.Worksheets.Add("Hoja 1");
                int fila = 1;
                int columna = 1;

                if (gv.HeaderRow != null)
                {
                    GridViewExportUtil.PrepareControlForExport(gv.HeaderRow);
                    foreach (TableCell item in gv.HeaderRow.Cells)
                    {
                        ews.Cells[fila, columna].Value = WebUtility.HtmlDecode(item.Text);
                        columna++;
                    }

                    fila++;
                    columna = 1;
                }

                //  add each of the data rows to the table
                foreach (GridViewRow row in gv.Rows)
                {
                    GridViewExportUtil.PrepareControlForExport(row);
                    foreach (TableCell item in row.Cells)
                    {
                        ews.Cells[fila, columna].Value = WebUtility.HtmlDecode(item.Text);
                        columna++;
                    }

                    fila++;
                    columna = 1;
                }

                //  add the footer row to the table
                if (gv.FooterRow != null)
                {
                    GridViewExportUtil.PrepareControlForExport(gv.FooterRow);
                    foreach (TableCell item in gv.FooterRow.Cells)
                    {
                        ews.Cells[fila, columna].Value = WebUtility.HtmlDecode(item.Text);
                        ews.Column(columna).AutoFit();
                        columna++;
                    }
                }

                //  render the htmlwriter into the response
                HttpContext.Current.Response.BinaryWrite(eep.GetAsByteArray());
                HttpContext.Current.Response.End();
            }
        }

        public static void Export(string fileName, GridView gv, string encabezado)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader(
                "content-disposition", string.Format("attachment; filename={0}", fileName));
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (ExcelPackage eep = new ExcelPackage())
            {
                ExcelWorksheet ews = eep.Workbook.Worksheets.Add("Hoja 1");
                int columnasgrilla = gv.Columns.Count;
                int fila = 1;
                int columna = 2;

                ews.Cells[1, 1].Value = encabezado;
                ews.Cells[1, 1].Style.Font.Bold = true;
                ews.Cells[1, 1].Style.Font.Size = 16;
                ews.Cells[1, 1, fila, columna + columnasgrilla+5].Merge = true;

                fila++;
                fila++;

                if (gv.HeaderRow != null)
                {
                    int columnaAlIngresar = columna;
                    GridViewExportUtil.PrepareControlForExport(gv.HeaderRow);
                    foreach (TableCell item in gv.HeaderRow.Cells)
                    {
                        ews.Cells[fila, columna].Value = WebUtility.HtmlDecode(item.Text);
                        ews.Cells[fila, columna].Style.Font.Bold = true;
                        columna++;
                    }

                    fila++;
                    columna = columnaAlIngresar;
                }

                //  add each of the data rows to the table
                foreach (GridViewRow row in gv.Rows)
                {
                    int columnaAlIngresar = columna;
                    GridViewExportUtil.PrepareControlForExport(row);
                    foreach (TableCell item in row.Cells)
                    {
                        ews.Cells[fila, columna].Value = WebUtility.HtmlDecode(item.Text);
                        columna++;
                    }

                    fila++;
                    columna = columnaAlIngresar;
                }

                //  add the footer row to the table
                if (gv.FooterRow != null)
                {
                    GridViewExportUtil.PrepareControlForExport(gv.FooterRow);
                    foreach (TableCell item in gv.FooterRow.Cells)
                    {
                        ews.Cells[fila, columna].Value = WebUtility.HtmlDecode(item.Text);
                        ews.Column(columna).AutoFit();
                        columna++;
                    }
                }

                //  render the htmlwriter into the response
                HttpContext.Current.Response.BinaryWrite(eep.GetAsByteArray());
                HttpContext.Current.Response.End();
            }
        }

        /// <summary>
        /// Replace any of the contained controls with literals
        /// </summary>
        /// <param name="control"></param>
        private static void PrepareControlForExport(Control control)
        {
            for (int i = 0; i < control.Controls.Count; i++)
            {
                Control current = control.Controls[i];
                if (current is LinkButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
                }
                else if (current is ImageButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
                }
                else if (current is HyperLink)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
                }
                else if (current is DropDownList)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
                }
                else if (current is CheckBox)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
                }

                if (current.HasControls())
                {
                    GridViewExportUtil.PrepareControlForExport(current);
                }
            }
        }
    }
}