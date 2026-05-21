using Newtonsoft.Json;
using HRM.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HRM.Common
{
    public class HRMConstants
    {
        #region Search helpers

        /// <summary>
        /// Ngày hôm nay
        /// </summary>
        public const string TimeType_Today = "1";

        /// <summary>
        /// Ngày qua
        /// </summary>
        public const string TimeType_Yesterday = "2";

        /// <summary>
        /// Tuần này
        /// </summary>
        public const string TimeType_ThisWeek = "3";

        /// <summary>
        /// Tuần trước
        /// </summary>
        public const string TimeType_LastWeek = "4";

        /// <summary>
        /// 7 ngày gần đây
        /// </summary>
        public const string TimeType_SevenDay = "5";

        /// <summary>
        /// Tháng này
        /// </summary>
        public const string TimeType_ThisMonth = "6";

        /// <summary>
        /// Tháng trước
        /// </summary>
        public const string TimeType_LastMonth = "7";

        /// <summary>
        /// Tháng
        /// </summary>
        public const string TimeType_Month = "8";

        /// <summary>
        /// Quý
        /// </summary>
        public const string TimeType_Quarter = "9";

        /// <summary>
        /// Năm nay
        /// </summary>
        public const string TimeType_ThisYear = "10";

        /// <summary>
        /// Năm trước
        /// </summary>
        public const string TimeType_LastYear = "11";

        /// <summary>
        /// Năm 
        /// </summary>
        public const string TimeType_Year = "12";

        /// <summary>
        /// Khoảng thời gian
        /// </summary>
        public const string TimeType_Between = "13";

        /// <summary>
        /// Quý này
        /// </summary>
        public const string TimeType_ThisQuarter = "14";

        /// <summary>
        /// Quý trước
        /// </summary>
        public const string TimeType_LastQuarter = "15";

        #endregion Cấu hình layout chức năng động
        /// <summary>
        /// Kiểu mặc định truyền thống
        /// </summary>
        public const int LayoutTypeDefault = 1;
        /// <summary>
        /// Kiểu cây thư mục
        /// </summary>
        public const int LayoutTypeTreeview = 2;
        #region 
        #endregion

        #region Giới tính
        /// <summary>
        /// Nam
        /// </summary>
        public const int Male = 1;

        /// <summary>
        /// Nữ
        /// </summary>
        public const int Female = 2;
        #endregion

        #region Template
        /// <summary>
        /// Đường dẫn thư mục chua file mẫu
        /// </summary>
        public const string TemplatePath = "Template";
        /// <summary>
        /// Template lịch sử
        /// </summary>
        public const string TemplateHistory = "Template/LichSuThaoTac.xlsx";
        #endregion

        public enum OptionExport
        {
            /// <summary>
            /// Xuất file excel
            /// </summary>
            Excel = 1,
            /// <summary>
            /// Xuất word
            /// </summary>
            Word = 2,
            /// <summary>
            /// Xuất pdf
            /// </summary>
            Pdf = 3
        }       
        /// <summary>
        /// Số bản gi lưu vào CSDL một lần sử dụng cho insert list
        /// </summary>
        public const int TotalSaveChange = 100;

        public const string DanhMuc = "DanhMuc";

        /// <summary>
        /// Id user mặc định
        /// </summary>
        public const string IdUserRootFix = "UFR01";
        /// <summary>
        /// Id user mặc định
        /// </summary>
        public const string IdUserAdminFix = "UFA01";

        /// <summary>
        /// id mặc định của group user admin
        /// </summary>
        public const string GroupAdminId = "GF01";

        /// <summary>
        /// Khóa tài khoản
        /// </summary>
        public const int Lock = 1;

        /// <summary>
        /// Mở tài khoản
        /// </summary>
        public const int UnLock = 2;

        public const string NoLogEvent = "NoLogEvent";

        public const int UserHistory_Type_Login = 1;
        public const int UserHistory_Type_Data = 2;

        /// <summary>
        /// Địa chỉ email gửi thông tin đăng nhập
        /// </summary>
        public const string SystemParam_SP01 = "SP01";

        /// <summary>
        /// Mật khẩu email gửi thông tin đăng nhập
        /// </summary>
        public const string SystemParam_SP02 = "SP02";

        /// <summary>
        /// Nội dung gửi email thông tin đăng nhập
        /// </summary>
        public const string SystemParam_SP03 = "SP03";      

        public const string ExtensionPDF = ".pdf";
        public static string FolderExportData = "Export";

        public const string DateFormatKey = "yyMMdd.HHmmssfff";
      
        public const string UrlOutXSS = "/api/about;/api/file/upload-file;/api/file/upload-files";
        public static string[] KeyMassAssignment = { "isadmin", "issso", "role", "is-admin", "is-sso", "is_admin", "is_sso" };


        /// <summary>
        /// code mã nhân viên trong bảng SystemParams
        /// </summary>
        public const string Ma_Nhan_Vien = "Ma_Nhan_Vien";

        public const int TangCaChuNhat = 3;
    }
}
