using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using HRM.Common.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
namespace HRM.Common.Users
{
   
    public class HRMLogInModel
    {
        /// <summary>
        /// Tài khoản
        /// </summary>
        [Required(ErrorMessageResourceName = MessageResourceKey.MSG0020, ErrorMessageResourceType = typeof(MessageResource))]
        [Display(Name = "Username")]
        [MaxLength(100, ErrorMessageResourceName = MessageResourceKey.MSG0021, ErrorMessageResourceType = typeof(MessageResource))]
        [BindRequired]
        public string Username { get; set; }

        /// <summary>
        /// Mật khẩu
        /// </summary>
        [Required(ErrorMessageResourceName = MessageResourceKey.MSG0020, ErrorMessageResourceType = typeof(MessageResource))]
        [Display(Name = "Password")]
        [MaxLength(64, ErrorMessageResourceName = MessageResourceKey.MSG0021, ErrorMessageResourceType = typeof(MessageResource))]
        public string Password { get; set; }

        public bool IsRemember { get; set; }
       
    }
}
