using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MicroService.Common.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static MicroService.Common.JWT.JwtHelper;

namespace MicroService.Basic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetToken(string username, string password)
        {
            if (!string.IsNullOrEmpty(username))
            {
                var model = new TokenModel()
                {
                    Id = "test111",
                    UserName = username
                };
                var token = JwtHelper.IssueJwt(model);
                return Ok(new { Token = token, Status = true });
            }
            else
            {
                return BadRequest(new { Message = "用户名密码错误" });
            } 

        }
    }
}