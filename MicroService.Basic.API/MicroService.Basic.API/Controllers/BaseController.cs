using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.Basic.API.Controllers
{
    [ApiController]
    public abstract class BaseController
    {
        protected virtual OkObjectResult Success()
        {
            return new OkObjectResult(new ApiResult { Status = 1, Data = default(object) });
        }
        protected virtual OkObjectResult Success<T>(T value)
        {
            return new OkObjectResult(new ApiResult<T> { Status = 1, Data = value });
        }
        protected virtual OkObjectResult Success<T>(T value, string msg)
        {
            return new OkObjectResult(new ApiResult<T> { Status = 1, Data = value, Msg = msg });
        }
        protected virtual OkObjectResult Fail(string errMsg)
        {
            return new OkObjectResult(new ApiResult { Status = 0, Msg = errMsg });
        }
        protected virtual OkObjectResult Fail<T>(string errMsg)
        {
            return new OkObjectResult(new ApiResult<T> { Status = 0, Data = default(T), Msg = errMsg });
        }

        protected virtual OkObjectResult Fail<T>(string errCode, string errMsg)
        {
            return new OkObjectResult(new ApiResult<T> { Status = 0, Data = default(T), ErrorCode = errCode, Msg = errMsg });
        }
    }
    public class ApiResult : ApiResult<object>
    {
    }
    public class ApiResult<T>
    {
        /// <summary>
        /// 返回状态 1成功，0 失败
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 返回数据内容
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 返回数据类型
        /// </summary>
        public string DataType => typeof(T).Name;

        /// <summary>
        /// 返回数据错误码
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 返回提醒信息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 错误源
        /// </summary>
        public string ErrorSource { get; set; }
    }

}
