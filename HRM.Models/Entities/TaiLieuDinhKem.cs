using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class TaiLieuDinhKem
    {
        public string Id { get; set; }
        public string ObjectId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int FileSize { get; set; }
        public string Thumbnail { get; set; }
        public string Extention { get; set; }
        public string HashValue { get; set; }
        public string Description { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
