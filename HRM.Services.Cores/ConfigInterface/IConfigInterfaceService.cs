using HRM.Models.Cores.ConfigInterface;
using System.Threading.Tasks;

namespace HRM.Services.Cores.ConfigInterface
{
    public interface IConfigInterfaceService
    {
        /// <summary>
        /// Lấy thông tin cấu hình
        /// </summary>
        /// <returns></returns>
        Task<ConfigInterfaceModel> GetConfigAsync();

        /// <summary>
        /// Thêm mới cấu hình
        /// </summary>
        /// <param name="model">Thông tin cấu hình</param>
        /// <returns></returns>
        Task CreateOrUpdateAsync(ConfigInterfaceModel model);
    }
}
