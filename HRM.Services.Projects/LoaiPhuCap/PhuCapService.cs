using HRM.Common;
using HRM.Common.Models;
using HRM.Models.Entities;
using HRM.Models.Projects.LoaiPhuCap;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Services.Projects.LoaiPhuCap
{
    public class PhuCapService : IPhuCapService
    {
        private readonly CoreProjectContext _sqlContext;
        public PhuCapService(CoreProjectContext sqlContext)
        {
            this._sqlContext = sqlContext;
        }

        /// <summary>
        /// Tìm kiếm phụ cấp
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<PhuCapSearchResultModel>> SearchPhuCapAsync(PhuCapSearchModel searchModel)
        {
            SearchBaseResultModel<PhuCapSearchResultModel> result = new SearchBaseResultModel<PhuCapSearchResultModel>();
            var dataQuery = (from a in _sqlContext.PhuCap.AsNoTracking()
                             select new PhuCapSearchResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 SoTien = a.SoTien,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                dataQuery = dataQuery.Where(x => x.Name.Contains(searchModel.Name));
            }
            result.TotalItems = dataQuery.Count();
            result.DataResults = dataQuery.OrderBy(o => o.Name).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            return result;
        }

        /// <summary>
        /// Thêm mới phụ cấp
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task CreatePhuCapAsync(PhuCapCreateModel model, string userId)
        {
            var checkName = _sqlContext.PhuCap.AsNoTracking().Where(a => model.Name.ToLower().Trim().Equals(a.Name.ToLower().Trim())).FirstOrDefault();
            if (checkName != null)
            {
                throw HRMException.CreateInstance("Tên phụ cấp đã tồn tại!");
            }
            PhuCap phuCap = new PhuCap()
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                SoTien = model.SoTien,
            };

            _sqlContext.PhuCap.Add(phuCap);

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
        /// Xoá phụ cấp
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeletePhuCapAsync(string id)
        {
            var phuCap = _sqlContext.PhuCap.FirstOrDefault(e => id.Equals(e.Id));
            if (phuCap == null)
            {
                throw HRMException.CreateInstance("Phụ cấp không tồn tại");
            }
            _sqlContext.PhuCap.Remove(phuCap);
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
        /// Lấy phụ cấp theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PhuCapSearchResultModel> GetPhuCapByIdAsync(string id)
        {
            var phuCap = _sqlContext.PhuCap.AsNoTracking().FirstOrDefault(x => x.Id.Equals(id));
            if (phuCap == null)
            {
                throw HRMException.CreateInstance("Phụ cấp không tồn tại");
            }
            return new PhuCapSearchResultModel()
            {
                Id = phuCap.Id,
                Name = phuCap.Name,
                SoTien = phuCap.SoTien,
            };
        }

        /// <summary>
        /// Cập nhật phụ cấp
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task UpdatePhuCapAsync(string id, PhuCapCreateModel model, string userId)
        {
            var phuCap = _sqlContext.PhuCap.FirstOrDefault(x => x.Id.Equals(id));
            if (phuCap == null)
            {
                throw HRMException.CreateInstance("Phụ cấp không tồn tại");
            }
            var checkName = _sqlContext.PhuCap.AsNoTracking().FirstOrDefault(a => model.Name.ToLower().Trim().Equals(a.Name.ToLower().Trim()) && !a.Id.Equals(id));
            if (checkName != null)
            {
                throw HRMException.CreateInstance("Tên phụ cấp đã tồn tại!");
            }
            phuCap.Name = model.Name;
            phuCap.SoTien = model.SoTien;
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
