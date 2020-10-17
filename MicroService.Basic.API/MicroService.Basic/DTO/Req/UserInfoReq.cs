using System;
using System.Collections.Generic;
using System.Text;

namespace MicroService.Basic.DTO.Req
{
    public class UserInfoReq:ReqBase
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
}
