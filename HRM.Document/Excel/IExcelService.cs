using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HRM.Document.Excel
{
    public interface IExcelService
    {
        public MemoryStream ExportExcel<T>(List<T> datas, string teamplatePath, int columns, Dictionary<string, string> paramDic = null);
        public MemoryStream ExportExcel<T>(List<ItemDataSheet<T>> dataSheets, string teamplatePath, Dictionary<string, string> paramDic = null);
        public string ExportExcelBase64<T>(List<T> datas, string teamplatePath, int columns, Dictionary<string, string> paramDic = null);
        public MemoryStream ExportPdf<T>(List<T> datas, string teamplatePath, int columns, Dictionary<string, string> paramDic = null);
        string ExportPdfBase64<T>(List<T> datas, string teamplatePath, int columns, Dictionary<string, string> paramDic = null);
    }
}
