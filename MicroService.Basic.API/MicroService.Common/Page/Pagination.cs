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
        public int PageSize { get; set; } = 1;
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; } = 10;
    } 
    public class PaginationReq : Pagination
    {
        /// <summary>
        /// 排序列
        /// </summary>
        public string Sidx { get; set; }
        /// <summary>
        /// 排序类型
        /// </summary>
        public bool IsAsc { get; set; }
    }
    public class Paging<T> : Pagination
    {
         public List<T> List { get; set; }
         public int TotalCount { get; set; } 
    }
}
