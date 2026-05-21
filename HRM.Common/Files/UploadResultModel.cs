using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.Common.Files
{
    public class UploadResultModel
    {
        public string Id { get; set; }
        public string FileUrl { get; set; }
        public decimal FileSize { get; set; }
        public string FileName { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
