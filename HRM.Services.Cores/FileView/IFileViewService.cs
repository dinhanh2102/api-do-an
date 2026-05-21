using System.IO;

namespace HRM.Services.Cores.FileView
{
    public interface IFileViewService
    {
        /// <summary>
        /// Chuyển đổi file word sang pdf
        /// </summary>
        /// <param name="pathDoc"></param>
        /// <returns>file pdf</returns>
        FileStream ConvertWordToPDF(string pathDoc, string pathOutPdf);

        /// <summary>
        /// Chuyển đổi file excel sang pdf
        /// </summary>
        /// <param name="pathXLS"></param>
        /// <returns>file pdf</returns>
        FileStream ConvertExcelToPDF(string pathXLS, string pathOutPdf);

        /// <summary>
        /// Chuyển đổi file PowerPoint sang pdf
        /// </summary>
        /// <param name="pathPPT"></param>
        /// <returns>file pdf</returns>
        FileStream ConvertPowerPointToPDF(string pathPPT, string pathOutPdf);
    }
}
