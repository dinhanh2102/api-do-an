using Microsoft.AspNetCore.Mvc;
using HRM.Common.Attributes;
using HRM.Common.Models;
using HRM.Common.Resource;
using HRM.Api.Attributes;
using HRM.Common.Models;
using System.Threading.Tasks;
using HRM.Api.Controllers.Cores;
using HRM.Services.Projects.ChamCong;
using HRM.Models.Projects.ChamCong;
using HRM.Models.Entities;

namespace HRM.Api.Controllers.ChamCong
{
    [Route("api/cham-cong")]
    [ApiController]
    [ValidateModel]
    [Logging]
    [ApiHandleExceptionSystem]
    public class ChamCongController : BaseApiController
    {
        private readonly IChamCongService chamCong;
        public ChamCongController(IChamCongService chamCongService)
        {
            chamCong = chamCongService;
        }

        /// <summary>
        /// Tìm kiếm kỳ công
        /// </summary>
        /// <param name="modelSearch">Model điều kiện tìm kiếm</param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [ActionName(TextResourceKey.Action_Search)]
        public async Task<ActionResult<SearchBaseResultModel<ChamCongSearchResultModel>>> ChamCong([FromBody] ChamCongCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await chamCong.ChamCong(model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Chấm công get detail
        /// </summary>
        /// <param name="modelSearch">Model điều kiện tìm kiếm</param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-cham-cong-detail/{id}")]
        [ActionName(TextResourceKey.Action_Search)]
        public async Task<ActionResult<ChamCongGetModel>> GetChamCongInfor([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await chamCong.getChamCongDetail(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
        
        [HttpPut]
        [Route("update/{id}")]
        [ActionName(TextResourceKey.Action_Update)]
        public async Task<ActionResult> Update([FromRoute] string id, [FromBody] ChamCongGetModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await chamCong.updateChamCong(id, model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
       
        [HttpPost]
        [Route("create-bang-cong")]
        [ActionName(TextResourceKey.Action_Create)]
        public async Task<ActionResult> Create([FromBody] ChamCongCreateHandle model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await chamCong.createChamCong(model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-nhan-vien-theo-ma/{maNhanVien}")]
        [ActionName(TextResourceKey.Action_Create)]
        public async Task<ActionResult> GetNhanVienTheoMa(string maNhanVien)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await chamCong.getNhanVienTheoMaNhanVien(maNhanVien);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }


    }
}