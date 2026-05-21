using HRM.Common.Files;
using HRM.Common.Helpers;
using Syncfusion.Pdf;
using Syncfusion.XlsIO;
using Syncfusion.XlsIORenderer;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace HRM.Document.Excel
{
    public class ExcelService : IExcelService
    {
        public MemoryStream ExportExcel<T>(List<T> datas, string teamplatePath, int columns, Dictionary<string, string> paramDic = null)
        {
            MemoryStream stream = new MemoryStream();

            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                try
                {
                    IApplication application = excelEngine.Excel;
                    FileStream inputStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), teamplatePath), FileMode.Open);
                    IWorkbook workbook = application.Workbooks.Open(inputStream);
                    IWorksheet sheet = workbook.Worksheets[0];

                    ExportData(workbook, sheet, datas, teamplatePath, true, columns, paramDic);


                    workbook.SaveAs(stream);

                    inputStream.Close();
                    workbook.Close();
                }
                catch (Exception ex)
                {

                }

                excelEngine.Dispose();
            }

            return stream;
        }

        public MemoryStream ExportExcel<T>(List<ItemDataSheet<T>> dataSheets, string teamplatePath, Dictionary<string, string> paramDic = null)
        {
            MemoryStream stream = new MemoryStream();

            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                FileStream inputStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), teamplatePath), FileMode.Open);
                IWorkbook workbook = null;
                try
                {
                    //IApplication application = excelEngine.Excel;
                    //FileStream inputStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), teamplatePath), FileMode.Open);
                     workbook = application.Workbooks.Open(inputStream);

                    IWorksheet sheet = null;
                    foreach (var itemSheet in dataSheets)
                    {
                        sheet = workbook.Worksheets[itemSheet.SheetIndex];

                        ExportData(workbook, sheet, itemSheet.Datas, teamplatePath, true, itemSheet.Columns, paramDic);
                    }
                    workbook.SaveAs(stream);

                    inputStream.Close();
                    workbook.Close();
                }
                catch (Exception ex)
                {
                    workbook?.Close();
                }
            }

            return stream;
        }

        /// <summary>
        /// Xuất dữ liệu ra excel trả vè base64
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu xuất</typeparam>
        /// <param name="datas">Dữ liệu xuất</param>
        /// <param name="teamplatePath">Đường dẫn file mẫu</param>
        /// <param name="columns">Cột cần autofit</param>
        /// <param name="paramDic">Danh sách param cần replace</param>
        /// <returns></returns>
        public string ExportExcelBase64<T>(List<T> datas, string teamplatePath, int columns, Dictionary<string, string> paramDic = null)
        {
            string base64String = string.Empty;
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                try
                {
                    IApplication application = excelEngine.Excel;
                    FileStream inputStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), teamplatePath), FileMode.Open);
                    IWorkbook workbook = application.Workbooks.Open(inputStream);
                    IWorksheet sheet = workbook.Worksheets[0];
                    ExportData(workbook, sheet, datas, teamplatePath, true, columns, paramDic);

                    using (MemoryStream stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        base64String = Convert.ToBase64String(stream.ToArray());
                        stream.Dispose();
                    }

                    inputStream.Close();
                    workbook.Close();
                }
                catch (Exception ex)
                {

                }

                excelEngine.Dispose();
            }
            return base64String;
        }

        /// <summary>
        /// Xuất dữ liệu ra excel trả  về file
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu xuất</typeparam>
        /// <param name="datas">Dữ liệu xuất</param>
        /// <param name="teamplatePath">Đường dẫn file mẫu</param>
        /// <param name="columns">Cột cần autofit</param>
        /// <param name="paramDic">Danh sách param cần replace</param>
        /// <returns></returns>
        public FileResultModel ExportExcelFile<T>(List<T> datas, string teamplatePath, int columns, Dictionary<string, string> paramDic = null)
        {
            FileResultModel fileResultModel = new FileResultModel();
            fileResultModel.ContentType = FileHelper.GetContentType(".xlsx");
            fileResultModel.FileName = Path.GetFileName(teamplatePath);

            // Khỏi tạo bảng excel
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                FileStream inputStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), teamplatePath), FileMode.Open);
                try
                {
                    IApplication application = excelEngine.Excel;
                    IWorkbook workbook = application.Workbooks.Open(inputStream);
                    IWorksheet sheet = workbook.Worksheets[0];
                    ExportData(workbook, sheet, datas, teamplatePath, true, columns, paramDic);

                    using (MemoryStream stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        fileResultModel.FileStream = stream.ToArray();
                        stream.Dispose();
                    }

                    inputStream.Close();
                    workbook.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    inputStream.Close();

                }

                excelEngine.Dispose();
            }

            return fileResultModel;
        }

        public FileResultModel ExportExcelMultiSheetFile<T>(List<List<T>> datas, string teamplatePath, int columns, Dictionary<string, string> paramDic = null)
        {
            FileResultModel fileResultModel = new FileResultModel();
            fileResultModel.ContentType = FileHelper.GetContentType(".xlsx");
            fileResultModel.FileName = Path.GetFileName(teamplatePath);

            // Khỏi tạo bảng excel
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                FileStream inputStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), teamplatePath), FileMode.Open);
                try
                {
                    IApplication application = excelEngine.Excel;
                    IWorkbook workbook = application.Workbooks.Open(inputStream);

                    int index = 0;
                    IWorksheet sheet;
                    foreach (var data in datas)
                    {
                        sheet = workbook.Worksheets[index];
                        ExportData(workbook, sheet, data, teamplatePath, true, columns, paramDic);
                        index++;
                    }


                    using (MemoryStream stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        fileResultModel.FileStream = stream.ToArray();
                        stream.Dispose();
                    }

                    inputStream.Close();
                    workbook.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    inputStream.Close();

                }

                excelEngine.Dispose();
            }

            return fileResultModel;
        }

        public MemoryStream ExportPdf<T>(List<T> datas, string teamplatePath, int columns, Dictionary<string, string> paramDic = null)
        {
            MemoryStream stream = new MemoryStream();
            // Khỏi tạo bảng excel
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                try
                {
                    IApplication application = excelEngine.Excel;
                    FileStream inputStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), teamplatePath), FileMode.Open);
                    IWorkbook workbook = application.Workbooks.Open(inputStream);
                    IWorksheet sheet = workbook.Worksheets[0];
                    ExportData(workbook, sheet, datas, teamplatePath, true, columns, paramDic);

                    //Initialize XlsIORendererSettings
                    XlsIORendererSettings settings = new XlsIORendererSettings();

                    //Enable AutoDetectComplexScript property
                    settings.AutoDetectComplexScript = true;
                    settings.LayoutOptions = LayoutOptions.FitAllColumnsOnOnePage;

                    XlsIORenderer render = new XlsIORenderer();

                    PdfDocument pdfDocument = render.ConvertToPDF(workbook.Worksheets[0], settings);



                    pdfDocument.Save(stream);

                    pdfDocument.Close(true);

                    inputStream.Close();
                    workbook.Close();
                }
                catch (Exception ex)
                {

                }

                excelEngine.Dispose();
            }

            return stream;
        }

        public string ExportPdfBase64<T>(List<T> datas, string teamplatePath, int columns, Dictionary<string, string> paramDic = null)
        {
            string base64String = string.Empty;
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                try
                {

                    IApplication application = excelEngine.Excel;
                    FileStream inputStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), teamplatePath), FileMode.Open);
                    IWorkbook workbook = application.Workbooks.Open(inputStream);
                    IWorksheet sheet = workbook.Worksheets[0];
                    ExportData(workbook, sheet, datas, teamplatePath, true, columns, paramDic);

                    XlsIORenderer render = new XlsIORenderer();
                    PdfDocument pdfDocument = render.ConvertToPDF(workbook.Worksheets[0]);

                    using (MemoryStream stream = new MemoryStream())
                    {
                        pdfDocument.Save(stream);
                        base64String = Convert.ToBase64String(stream.ToArray());
                        stream.Dispose();
                    }

                    pdfDocument.Close(true);

                    inputStream.Close();
                    workbook.Close();
                }
                catch (Exception ex)
                {

                }

                excelEngine.Dispose();
            }

            return base64String;
        }

        private void ExportData<T>(IWorkbook workbook, IWorksheet sheet, List<T> datas, string teamplatePath, bool autofitRows, int columns, Dictionary<string, string> paramDic = null)
        {
            if (paramDic != null)
            {
                IRange range;
                foreach (KeyValuePair<string, string> item in paramDic)
                {
                    range = sheet.FindFirst(item.Key, ExcelFindType.Text, ExcelFindOptions.MatchCase);

                    if (range != null)
                    {
                        range.Replace(item.Key, item.Value != null ? item.Value.ToString() : "");
                    }
                }
            }

            int total = datas.Count;

            IRange iRangeData = sheet.FindFirst("<data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            if (iRangeData == null)
            {
                return;
            }

            iRangeData.Text = iRangeData.Text.Replace("<data>", string.Empty);

            int rowData = iRangeData.Row;
            int columnData = iRangeData.Column;
            List<string> cplumnExports = new List<string>();
            int index = 0;
            string cellValue;
            bool isExist = false;
            int cellIndex = 0;
            while (sheet.Rows.Length > rowData)
            {
                cellIndex = columnData + index - 1;
                if (sheet.Rows[rowData].Cells.Length <= cellIndex)
                {
                    break;
                }

                cellValue = sheet.Rows[rowData].Cells[cellIndex].Value;

                if (!string.IsNullOrEmpty(cellValue))
                {

                    cplumnExports.Add(cellValue.Replace("<", string.Empty).Replace(">", string.Empty));
                    isExist = true;
                    sheet.Rows[rowData].Cells[cellIndex].Value = string.Empty;
                }
                else
                {
                    break;
                }

                index++;
            }

            if (total == 0)
            {
                sheet.DeleteRow(rowData);
            }

            if (total > 1)
            {
                sheet.InsertRow(rowData + 1, total - 1, ExcelInsertOptions.FormatAsBefore);
            }

            if (isExist)
            {
                DataTable dtb = new DataTable();

                var exportData = datas.ToDataTableExport(cplumnExports);

                sheet.ImportDataTable(exportData, rowData, columnData, false, false);
            }
            else
            {
                sheet.ImportData(datas, rowData, columnData, false);
            }

            // Autofit dòng đầu tiên của Data khi import, thường khi xuất Pdf thì dòng đầu tiên đang không autofit
            string columnName = GetExcelColumnName(columns);
            if (autofitRows && total > 0)
            {
                sheet.Range["A" + rowData + ":" + columnName + (rowData + total - 1)].AutofitRows();
            }

            //SetBorder(workbook);

            //sheet.Range["A" + rowData + ":" + columnName + (rowData + total - 1)].CellStyleName = "BodyStyle";
        }

        //private void SetBorder(IWorkbook workbook)
        //{
        //    IStyle bodyStyle = workbook.Styles.Add("BodyStyle");

        //    bodyStyle.BeginUpdate();
        //    bodyStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
        //    bodyStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
        //    bodyStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
        //    bodyStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;

        //    bodyStyle.EndUpdate();
        //}

        private string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

    }
}
