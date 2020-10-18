using MicroService.Common.Page;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroService.Basic.DTO.Req
{
    public class UserListReq 
    {
        public string Name { get; set; } 
        public PaginationReq pagination { get; set; }
    }
}
