using Microsoft.AspNetCore.Http;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Document.Word
{
    public class WordService : IWordService
    {
        /// <summary>
        /// Export word Out MemoryStream
        /// </summary>
        /// <param name="templatePath">Đường dẫn file template</param>
        /// <param name="replateTexts">List các đối tượng text, html cần replate</param>
        /// <param name="dataTables">List danh sách cá bảng cần fill dữ liệu</param>
        /// <param name="replateImages">List các ảnh cần fill lên template</param>
        /// <returns></returns>
        public MemoryStream ExportWord(string templatePath, List<ItemTextReplate> replateTexts = null, Dictionary<string, List<object>> dataTables = null, List<ItemImageReplate> replateImages = null)
        {
            MemoryStream stream = new MemoryStream();

            if (!File.Exists(templatePath))
                return stream;

            using (WordDocument document = new WordDocument())
            {
                try
                {
                    FileStream fileStreamPath = new FileStream(@$"{templatePath}", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                    document.Open(fileStreamPath, FormatType.Automatic);

                    //Replace text, html in template
                    if (replateTexts?.Count > 0)
                    {
                        foreach (ItemTextReplate item in replateTexts)
                        {
                            document.ReplaceItem(item);
                        }
                    }

                    //Replace image in template
                    if (replateImages?.Count > 0)
                    {
                        foreach (ItemImageReplate item in replateImages)
                        {
                            document.ReplaceImage(item);
                        }
                    }

                    //Fill data to table
                    if (dataTables?.Count > 0)
                    {
                        foreach (KeyValuePair<string, List<object>> itemDataTable in dataTables)
                        {
                            WTable tableIndex;
                            string keyTable = itemDataTable.Key;
                            tableIndex = document.GetTableByFindText(keyTable);
                            //Row bắt đầu fill data
                            WTableRow tableRowData = document.GetTableRowByFindText(keyTable);
                            WTableCell tableCellData = document.GetTableCellByFindText(keyTable);
                            if (tableIndex != null)
                            {
                                int indexRow = tableRowData.GetRowIndex();
                                //Xóa row data dầu tiên
                                tableIndex.Rows.RemoveAt(indexRow);
                                if (dataTables?.Count > 0)
                                {
                                    WTableRow row;
                                    foreach (var item in itemDataTable.Value)
                                    {
                                        tableIndex.Rows.Insert(indexRow, tableRowData.Clone());

                                        row = tableIndex.Rows[indexRow];

                                        int indexCell = tableCellData.GetCellIndex();
                                        foreach (PropertyInfo prop in item.GetType().GetProperties())
                                        {
                                            if (indexCell < row.Cells.Count)
                                            {
                                                row.Cells[indexCell].Paragraphs[0].Text = prop.GetValue(item, null) != null ? prop.GetValue(item, null).ToString() : string.Empty;
                                                indexCell++;
                                            }
                                        }

                                        indexRow++;
                                    }
                                }
                                else
                                {
                                    document.Sections[0].Body.ChildEntities.Remove(tableIndex);
                                }
                            }
                        }
                    }
                    //Saves the document to stream
                    document.Save(stream, FormatType.Docx);
                    //Closes the document
                    document.Close();
                }
                catch (Exception ex)
                {
                    //Closes the document
                    document?.Close();
                }
            }

            return stream;
        }

        /// <summary>
        ///  Export word Out Path
        /// </summary>
        /// <param name="templatePath">Đường dẫn file template</param>
        /// <param name="outPath">Đường dẫn ghi file ra</param>
        /// <param name="replateTexts">List các đối tượng text, html cần replate</param>
        /// <param name="dataTables">List danh sách cá bảng cần fill dữ liệu</param>
        /// <param name="replateImages">List các ảnh cần fill lên template</param>
        public void ExportWord(string templatePath, string outPath, List<ItemTextReplate> replateTexts = null, Dictionary<string, List<object>> dataTables = null, List<ItemImageReplate> replateImages = null)
        {
            var streamWord = ExportWord(templatePath, replateTexts, dataTables, replateImages);
            FileStream file = null;
            try
            {
                using (file = new FileStream(outPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    streamWord.Position = 0;
                    streamWord.CopyTo(file);
                    file.Close();
                }
            }
            catch { file?.Close(); }
        }

        /// <summary>
        /// Export word to pdf Out MemoryStream
        /// </summary>
        /// <param name="templatePath"></param>
        /// <param name="replateTexts"></param>
        /// <param name="dataTables"></param>
        /// <param name="replateImages"></param>
        /// <returns></returns>
        public MemoryStream ExportWordConvertToPdf(string templatePath, List<ItemTextReplate> replateTexts = null, Dictionary<string, List<object>> dataTables = null, List<ItemImageReplate> replateImages = null)
        {
            MemoryStream outputStream = new MemoryStream();
            PdfDocument pdfDocument = null;
            try
            {
                var streamWord = ExportWord(templatePath, replateTexts, dataTables, replateImages);
                DocIORenderer render = new DocIORenderer();
                pdfDocument = render.ConvertToPDF(streamWord);
                render.Dispose();
                pdfDocument.Save(outputStream);
                //Closes the instance of PDF document object.
                pdfDocument.Close();
                //Dispose the instance of FileStream.
                outputStream.Dispose();
                return outputStream;
            }
            catch
            {
                pdfDocument?.Close();
                return outputStream;
            }

        }

        /// <summary>
        ///  Export word to pdf Out Path
        /// </summary>
        /// <param name="templatePath">Đường dẫn file template</param>
        /// <param name="outPath">Đường dẫn ghi file ra</param>
        /// <param name="replateTexts">List các đối tượng text, html cần replate</param>
        /// <param name="dataTables">List danh sách cá bảng cần fill dữ liệu</param>
        /// <param name="replateImages">List các ảnh cần fill lên template</param>
        public void ExportWordConvertToPdf(string templatePath, string outPath, List<ItemTextReplate> replateTexts = null, Dictionary<string, List<object>> dataTables = null, List<ItemImageReplate> replateImages = null)
        {
            try
            {
                var streamWord = ExportWordConvertToPdf(templatePath, replateTexts, dataTables, replateImages);
                using (FileStream file = new FileStream(outPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    streamWord.Position = 0;
                    streamWord.CopyTo(file);
                    file.Close();
                }
            }
            catch { }
        }

        public MemoryStream ImportWordContent(MemoryStream streamSource, MemoryStream streamImport)
        {
            MemoryStream streamOutput = new MemoryStream();
            if (streamSource == null || streamImport == null)
                return null;
            try
            {
                using (WordDocument documeHRMource = new WordDocument())
                {
                    documeHRMource.Open(streamSource, FormatType.Automatic);
                    using (WordDocument documentImport = new WordDocument())
                    {
                        documentImport.Open(streamImport, FormatType.Automatic);

                        documeHRMource.ImportContent(documentImport);
                        documentImport.Close();
                    }
                    //Saves the document to stream
                    documeHRMource.Save(streamOutput, FormatType.Docx);
                    //Closes the document
                    documeHRMource.Close();
                    streamOutput.Position = 0;
                }
            }
            catch { }
            return streamOutput;
        }
    }
}
