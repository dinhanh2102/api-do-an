using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using HRM.Common.Attributes;
using HRM.Common.Files;
using HRM.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRM.Api.Controllers.Cores
{
    [ApiController]
    [Route("api/file")]
    [ValidateModel]
    [ApiHandleExceptionSystem]
    //[HRMAuthorize]
    public class FileController : BaseApiController
    {
        private readonly IUploadFileService uploadFileService;
        public FileController(IOptions<AppSettingModel> appSettingOptionss, IUploadFileService uploadFileService)
        {
            this.uploadFileService = uploadFileService;
        }

        /// <summary>
        /// Upload 1 file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        [Route("upload-file")]
        [HttpPost]
        public async Task<ActionResult<ApiResultModel<UploadResultModel>>> UploadFile([FromForm] IFormFile file, [FromForm] string folderName)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await uploadFileService.UploadFile(file, folderName);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Upload nhiều file
        /// </summary>
        /// <param name="files"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        [Route("upload-files")]
        [HttpPost]
        public async Task<ActionResult<ApiResultModel<UploadResultModel>>> UploadFile([FromForm] List<IFormFile> files, [FromForm] string folderName)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await uploadFileService.UploadFiles(files, folderName);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpPost]
        [Route("download-files")]
        public async Task<IActionResult> DownloadMedia(DownloadFileModel downloadFileModel)
        {
            var file = await uploadFileService.DownloadFiles(downloadFileModel);
            return File(file.FileStream, file.ContentType, file.FileName);
        }

        [HttpPost]
        [Route("download-file")]
        public async Task<IActionResult> DownloadFiles(FileModel fileModel)
        {
            var file = await uploadFileService.DownloadFile(fileModel);
            return File(file.FileStream, file.ContentType, file.FileName);
        }
    }
}