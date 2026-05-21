using HRM.Common;
using HRM.Common.Resource;
using HRM.Models.Entities;
using HRM.Models.Projects.LoaiCa;
using HRM.Common.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrafficControl.Core;

namespace HRM.Services.Projects.LoaiCa
{
    public class LoaiCaService : ILoaiCaService
    {
        private readonly CoreProjectContext _sqlContext;

        public LoaiCaService(CoreProjectContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        /// <summary>
        /// Tìm kiếm loại ca
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<LoaiCaSearchResultModel>> SearchLoaiCaAsync(LoaiCaSearchModel searchModel)
        {
            SearchBaseResultModel<LoaiCaSearchResultModel> result = new SearchBaseResultModel<LoaiCaSearchResultModel>();
            var dataQuery = (from a in _sqlContext.LoaiCa.AsNoTracking()
                             select new LoaiCaSearchResultModel
                             {
                                 Id = a.Id,
                                 HeSo = a.HeSo,
                                 Name = a.Name,
                             }).AsQueryable();
            var data = dataQuery.ToList();
            if (!string.IsNullOrEmpty(searchModel.TenLoaiCa))
            {
                dataQuery = dataQuery.Where(x => x.Name.Contains(searchModel.TenLoaiCa));
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
        public async Task CreateLoaiCaAsync(LoaiCaCreateModel model, string loaiCaId)
        {
            HRM.Models.Entities.LoaiCa loaiCa = new HRM.Models.Entities.LoaiCa
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                HeSo = model.HeSo,
            };


            _sqlContext.LoaiCa.Add(loaiCa);
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
        public async Task DeleteLoaiCaAsync(string id, string loaiCaId)
        {
            var loaiCa = _sqlContext.LoaiCa.FirstOrDefault(e => id.Equals(e.Id));
            if (loaiCa == null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0031);
            }

            _sqlContext.LoaiCa.Remove(loaiCa);

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
        public async Task<LoaiCaSearchResultModel> GetLoaiCaByIdAsync(string id)
        {
            var loaiCa = _sqlContext.LoaiCa.AsNoTracking().FirstOrDefault(x => x.Id.Equals(id));
            if (loaiCa == null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0031);
            }
            return new LoaiCaSearchResultModel()
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
        public async Task UpdateLoaiCaAsync(string id, LoaiCaCreateModel model, string loaiCaId)
        {
            var loaiCa = _sqlContext.LoaiCa.FirstOrDefault(x => x.Id.Equals(id));
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
