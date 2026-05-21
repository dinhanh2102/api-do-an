using HRM.Common;
using HRM.Common.Resource;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;
using Syncfusion.Presentation;
using Syncfusion.PresentationToPdfConverter;
using Syncfusion.XlsIORenderer;
using System;
using System.IO;

namespace HRM.Services.Cores.FileView
{
    public class FileViewService : IFileViewService
    {
        public FileViewService()
        {

        }

        public FileStream ConvertExcelToPDF(string pathXLS, string pathOutPdf)
        {
            try
            {
                if (!File.Exists(pathXLS))
                    throw HRMException.CreateInstance(MessageResourceKey.MSG0013);

                FileStream fileStreamPath = new FileStream(@$"{pathXLS}", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XlsIORenderer render = new XlsIORenderer();
                var pdfDocument = render.ConvertToPDF(fileStreamPath);

                FileStream outputStream = new FileStream(pathOutPdf, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                pdfDocument.Save(outputStream);
                //Closes the instance of PDF document object.
                pdfDocument.Close();
                //Dispose the instance of FileStream.
                return outputStream;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FileStream ConvertPowerPointToPDF(string pathPPT, string pathOutPdf)
        {
            try
            {
                if (!File.Exists(pathPPT))
                    throw HRMException.CreateInstance(MessageResourceKey.MSG0013);

                IPresentation pptxDoc = Presentation.Open(@$"{pathPPT}");

                //Converts the PowerPoint Presentation into PDF document
                PdfDocument pdfDocument = PresentationToPdfConverter.Convert(pptxDoc);
                FileStream outputStream = new FileStream(pathOutPdf, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                pdfDocument.Save(outputStream);
                pdfDocument.Close();
                return outputStream;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FileStream ConvertWordToPDF(string pathDoc,string pathOutPdf)
        {
            try
            {
                if (!File.Exists(pathDoc))
                    throw HRMException.CreateInstance(MessageResourceKey.MSG0013);

                FileStream fileStreamPath = new FileStream(@$"{pathDoc}", FileMode.Open, FileAccess.Read,FileShare.ReadWrite);
                fileStreamPath.Position = 0;
                DocIORenderer render = new DocIORenderer();
                var pdfDocument = render.ConvertToPDF(fileStreamPath);
                render.Dispose();

                FileStream outputStream = new FileStream(pathOutPdf, FileMode.OpenOrCreate, FileAccess.ReadWrite,FileShare.ReadWrite);
                pdfDocument.Save(outputStream);
                //Closes the instance of PDF document object.
                pdfDocument.Close();
                //Dispose the instance of FileStream.
              return  outputStream;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
