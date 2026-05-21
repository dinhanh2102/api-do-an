using HRM.Common;
using HRM.Common.Files;
using HRM.Common.Helpers;
using HRM.Common.Resource;
using HRM.Services.Cores.FileView;
using System.IO;
using System.Threading.Tasks;

namespace HRM.Services.Cores.ViewFileWeb
{
    public class ViewFileWebService : IViewFileWebService
    {
        private readonly IFileViewService _fileViewService;

        public ViewFileWebService(IFileViewService fileViewService)
        {
            this._fileViewService = fileViewService;
        }

        /// <summary>
        /// Lấy thông tin view file
        /// </summary>
        /// <param name="path">Đường dẫn file</param>
        /// <returns></returns>
        public async Task<FileResultModel> GetFileViewAsync(string path)
        {
            if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), path)))
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0013);
            }

            string pathFile = Path.Combine(Directory.GetCurrentDirectory(), path);
            string extension = Path.GetExtension(path);

            FileResultModel fileResult = new FileResultModel();
            string pathOutPdf = Path.Combine(Directory.GetCurrentDirectory(), HRMConstants.FolderExportData, "fileview_temp" + HRMConstants.ExtensionPDF);
            using (var memory = new MemoryStream())
            {
                if (Path.GetExtension(path).ToLower().Equals(".pdf") || Path.GetExtension(path).ToLower().Equals(".jpg") || Path.GetExtension(path).ToLower().Equals(".png"))
                {
                    using (var stream = new FileStream(pathFile, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                        stream.Dispose();
                    }
                    memory.Position = 0;

                    fileResult.FileStream = memory.ToArray();
                    fileResult.ContentType = FileHelper.GetContentType(extension);
                }
                else if (Path.GetExtension(path).ToLower().Equals(".doc") || Path.GetExtension(path).ToLower().Equals(".docx"))
                {
                    using (var stream = _fileViewService.ConvertWordToPDF(pathFile, pathOutPdf))
                    {
                        stream.Position = 0;
                        await stream.CopyToAsync(memory);
                        stream.Dispose();
                    }
                    memory.Position = 0;

                    fileResult.FileStream = memory.ToArray();
                    fileResult.ContentType = FileHelper.GetContentType(".pdf");
                }
                else if (Path.GetExtension(path).ToLower().Equals(".xls") || Path.GetExtension(path).ToLower().Equals(".xlsx"))
                {
                    using (var stream = _fileViewService.ConvertExcelToPDF(pathFile, pathOutPdf))
                    {
                        stream.Position = 0;
                        await stream.CopyToAsync(memory);
                        stream.Dispose();
                    }

                    memory.Position = 0;

                    fileResult.FileStream = memory.ToArray();
                    fileResult.ContentType = FileHelper.GetContentType(".pdf");
                }
                else if (Path.GetExtension(path).ToLower().Equals(".ppt") || Path.GetExtension(path).ToLower().Equals(".pptx"))
                {
                    using (var stream = _fileViewService.ConvertPowerPointToPDF(pathFile, pathOutPdf))
                    {
                        stream.Position = 0;
                        await stream.CopyToAsync(memory);
                        stream.Dispose();
                    }
                    memory.Position = 0;

                    fileResult.FileStream = memory.ToArray();
                    fileResult.ContentType = FileHelper.GetContentType(".pdf");
                }
                else
                {
                    using (var stream = new FileStream(path, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }
                    memory.Position = 0;

                    fileResult.FileStream = memory.ToArray();
                    fileResult.ContentType = FileHelper.GetContentType(extension);
                }
                memory.Dispose();
            }

            fileResult.FileName = Path.GetFileName(path);

            return fileResult;
        }
    }
}
