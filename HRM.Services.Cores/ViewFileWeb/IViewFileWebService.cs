using HRM.Common.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Services.Cores.ViewFileWeb
{
    public interface IViewFileWebService
    {
        /// <summary>
        /// Lấy thông tin view file
        /// </summary>
        /// <param name="path">Đường dẫn file</param>
        /// <returns></returns>
        Task<FileResultModel> GetFileViewAsync(string path);
    }
}
