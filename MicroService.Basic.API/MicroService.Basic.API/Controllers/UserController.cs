using MicroService.Basic.Abstraction;
using MicroService.Basic.Domain.Entity;
using MicroService.Basic.DTO.Req;
using MicroService.Common.Operator;
using MicroService.Common.Page;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroService.Basic.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        readonly IUserService _userService;
        readonly IOperatorProvider _operatorProvider;
        public UserController(IUserService userService, IOperatorProvider operatorProvider)
        {
            _userService = userService;
            _operatorProvider = operatorProvider;
        }
        [HttpPost]
        [Route("GetList")]
        public async Task<ActionResult<Paging<UserEntity>>> GetList(UserListReq req)
        {
            var result =  await _userService.GetList(req);
            return Success(result);
        }

        // GET: api/User/5
        [HttpGet("{keyValue}", Name = "Get")]
        public async Task<ActionResult<UserEntity>> Get(string keyValue)
        {
            var result = await _userService.GetInfo(keyValue);
            return Success(result);
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<bool>> Post([FromBody] UserInfoReq req)
        {
            req.operatorID = _operatorProvider.ID; 

            var result = await _userService.SubmitForm(req);
            return result == true ? Success(result) : Fail("操作失败");
        }
         
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{keyValue}")]
        public async Task<ActionResult<bool>> Delete(string keyValue)
        {
            var result = await _userService.DeleteForm(keyValue,_operatorProvider.ID);
            return result == true ? Success(result) : Fail("操作失败");
        }
    }
}
