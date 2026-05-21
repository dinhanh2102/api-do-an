using HRM.Common;
using HRM.Common.Resource;
using HRM.Models.Entities;
using HRM.Models.Projects.LoaiCong;
using HRM.Common.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrafficControl.Core;

namespace HRM.Services.Projects.LoaiCong
{
    public class LoaiCongService : ILoaiCongService
    {
        private readonly CoreProjectContext _sqlContext;

        public LoaiCongService(CoreProjectContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        /// <summary>
        /// Tìm kiếm loại ca
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<LoaiCongSearchResultModel>> SearchLoaiCongAsync(LoaiCongSearchModel searchModel)
        {
            SearchBaseResultModel<LoaiCongSearchResultModel> result = new SearchBaseResultModel<LoaiCongSearchResultModel>();
            var dataQuery = (from a in _sqlContext.LoaiCong.AsNoTracking()
                             select new LoaiCongSearchResultModel
                             {
                                 Id = a.Id,
                                 HeSo = a.HeSo,
                                 Name = a.Name,
                             }).AsQueryable();
            var data = dataQuery.ToList();
            if (!string.IsNullOrEmpty(searchModel.TenLoaiCong))
            {
                dataQuery = dataQuery.Where(x => x.Name.Contains(searchModel.TenLoaiCong));
            }
            result.TotalItems = dataQuery.Count();
            result.DataResults = dataQuery.OrderBy(o => o.Name).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            return result;
        }

        /// <summary>
        /// Thêm mới loại ca
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task CreateLoaiCongAsync(LoaiCongCreateModel model, string loaiCaId)
        {
            HRM.Models.Entities.LoaiCong loaiCa = new HRM.Models.Entities.LoaiCong
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                HeSo = model.HeSo,
            };


            _sqlContext.LoaiCong.Add(loaiCa);
            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    _sqlContext.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _sqlContext.ChangeTracker.Clear();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Xoá loại ca
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteLoaiCongAsync(string id, string loaiCaId)
        {
            var loaiCa = _sqlContext.LoaiCong.FirstOrDefault(e => id.Equals(e.Id));
            if (loaiCa == null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0031);
            }

            _sqlContext.LoaiCong.Remove(loaiCa);

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    _sqlContext.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _sqlContext.ChangeTracker.Clear();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Lấy loại ca theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<LoaiCongSearchResultModel> GetLoaiCongByIdAsync(string id)
        {
            var loaiCa = _sqlContext.LoaiCong.AsNoTracking().FirstOrDefault(x => x.Id.Equals(id));
            if (loaiCa == null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0031);
            }
            return new LoaiCongSearchResultModel()
            {
                Id = loaiCa.Id,
                HeSo = loaiCa.HeSo,
                Name = loaiCa.Name,
            };
        }

        /// <summary>
        /// Cập nhật loại ca
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task UpdateLoaiCongAsync(string id, LoaiCongCreateModel model, string loaiCaId)
        {
            var loaiCa = _sqlContext.LoaiCong.FirstOrDefault(x => x.Id.Equals(id));
            if (loaiCa == null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0031);
            }
            loaiCa.Name = model.Name;
            loaiCa.HeSo = model.HeSo;

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    _sqlContext.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _sqlContext.ChangeTracker.Clear();
                    throw ex;
                }
            }
        }
    }
}
