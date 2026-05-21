using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using HRM.Common;
using HRM.Common.Files;
using HRM.Common.Resource;
using HRM.Models.Cores.ConfigInterface;
using HRM.Models.Entities;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Services.Cores.ConfigInterface
{
    public class ConfigInterfaceService : IConfigInterfaceService
    {
        private readonly CoreProjectContext _sqlContext;
        private readonly UploadSettingModel uploadSettingModel;

        public ConfigInterfaceService(CoreProjectContext sqlContext, IOptions<UploadSettingModel> option)
        {
            _sqlContext = sqlContext;
            this.uploadSettingModel = option.Value;
        }

        /// <summary>
        /// Lấy thông tin cấu hình
        /// </summary>
        /// <returns></returns>
        public async Task<ConfigInterfaceModel> GetConfigAsync()
        {
            ConfigInterfaceModel result = new ConfigInterfaceModel();
            //try
            //{
            //    result = (from a in _sqlContext.SystemConfig.AsNoTracking()
            //              select new ConfigInterfaceModel
            //              {
            //                  Id = a.Id,
            //                  SoftwareName = a.SoftwareName,
            //                  IsShowLogoTopBar = a.ShowLogoTopBar,
            //                  IsUseCaptcha = a.IsLoginCaptcha,
            //                  IsUseMultiLanguage = a.IsMultiLanguage,
            //                  Logo = a.Logo,
            //                  FilePathLogo = a.LogoFolded,
            //                  FilePathIcon = a.FaviconIcon,
            //                  MenuType = a.MenuType,

            //              }).FirstOrDefault();
            //}
            //catch (Exception ex)
            //{

            //}
            return result;
        }

        /// <summary>
        /// Tạo mới cấu hình
        /// </summary>
        /// <param name="model">Thông tin cấu hình</param>
        /// <returns></returns>
        public async Task CreateOrUpdateAsync(ConfigInterfaceModel model)
        {
            //Thêm mới cấu hình khi không tồn tại id
            //if (string.IsNullOrEmpty(model.Id))
            //{
            //    SystemConfig SystemConfig = new SystemConfig()
            //    {
            //        Id = Guid.NewGuid().ToString(),
            //        SoftwareName = model.SoftwareName,
            //        IsLoginCaptcha = model.IsUseCaptcha,
            //        IsMultiLanguage = model.IsUseMultiLanguage,
            //        LogoFolded = model.FilePathLogo,
            //        FaviconIcon = model.FilePathIcon,
            //        ShowLogoTopBar = model.IsShowLogoTopBar,
            //        Logo = model.Logo,
            //        MenuType = model.MenuType,
            //    };
            //    _sqlContext.SystemConfig.Add(SystemConfig);
            //}
            //else if (!string.IsNullOrEmpty(model.Id)) //Cập nhật cấu hình
            //{
            //    var config = _sqlContext.SystemConfig.FirstOrDefault(e => model.Id.Equals(e.Id));
            //    if (config == null)
            //    {
            //        throw HRMException.CreateInstance(MessageResourceKey.MSG0032);
            //    }
            //    config.SoftwareName = model.SoftwareName;
            //    config.IsLoginCaptcha = model.IsUseCaptcha;
            //    config.IsMultiLanguage = model.IsUseMultiLanguage;
            //    config.LogoFolded = CloneFileImage(model.CroppedImage, config.LogoFolded).Replace("\\", "/");
            //    config.FaviconIcon = model.FilePathIcon;
            //    config.ShowLogoTopBar = model.IsShowLogoTopBar;
            //    config.Logo = model.Logo;
            //    config.MenuType = model.MenuType;
            //    _sqlContext.SystemConfig.Update(config);

            //}

            //using (var trans = _sqlContext.Database.BeginTransaction())
            //{
            //    try
            //    {
            //        _sqlContext.SaveChanges();
            //        trans.Commit();

            //        RemoveFile(model.FilePathIcon);
            //    }
            //    catch (Exception ex)
            //    {
            //        trans.Rollback();
            //        _sqlContext.ChangeTracker.Clear();
            //        throw ex;
            //    }
            //}
        }

        /// <summary>
        /// Nhân file
        /// </summary>
        /// <param name="data">Chuỗi data</param>
        /// <param name="folder">Thư mục</param>
        /// <returns></returns>
        private string CloneFileImage(string data, string folder)
        {
            if (!string.IsNullOrEmpty(data))
            {
                var data_split = data.Split(";")[1];
                var base64 = data_split.Split(",")[1];
                if (base64 == null || base64.Length == 0)
                {
                    throw HRMException.CreateInstance(MessageResourceKey.MSG0017);
                }
                string fileName = "Logo.png";
                string pathFolder = Path.Combine(uploadSettingModel.FolderUpload, "ConfigInterface", "Logo");
                string pathFolderServer = Path.Combine(Directory.GetCurrentDirectory(), pathFolder);
                try
                {
                    if (!Directory.Exists(pathFolderServer))
                    {
                        Directory.CreateDirectory(pathFolderServer);
                    }
                    string pathFile = Path.Combine(pathFolderServer, fileName);

                    byte[] bytes = Convert.FromBase64String(base64);
                    File.WriteAllBytes(pathFile, bytes);
                    return Path.Combine(pathFolder, fileName);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            else
            {
                return folder;
            }

        }

        /// <summary>
        /// Xóa file icon
        /// </summary>
        /// <param name="icon">ĐƯờng dẫn icon</param>
        private void RemoveFile(string icon)
        {
            try
            {
                string folderPathIcon = Path.Combine(Directory.GetCurrentDirectory(), "FileUpload", "ConfigInterface", "Icon");
                string[] iconFiles = Directory.GetFiles(folderPathIcon);
                foreach (string iconFile in iconFiles)
                {
                    if (iconFile.Contains(Path.Combine(Directory.GetCurrentDirectory(), icon).Replace("/", @"\"))) continue;
                    if (File.Exists(iconFile))
                        File.Delete(iconFile);
                }

            }
            catch (Exception ex)
            {
            }
        }
    }
}
