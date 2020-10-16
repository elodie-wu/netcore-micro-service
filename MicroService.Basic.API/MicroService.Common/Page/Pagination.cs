using System;
using System.Collections.Generic;
using System.Text;

namespace MicroService.Common.Page
{

    public class Pagination
    {
        /// <summary>
        /// 每页行数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 排序列
        /// </summary>
        public string Sidx { get; set; }
        /// <summary>
        /// 排序类型
        /// </summary>
        public bool isAsc { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; } 
    }
}
