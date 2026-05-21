using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.Common.Models
{
    public class SearchBaseResultModel<T>
    {
        public int TotalItems { get; set; }
        public List<T> DataResults { get; set; }
        public string Title { get; set; }

        public SearchBaseResultModel()
        {
            DataResults = new List<T>();
        }
    }
}
