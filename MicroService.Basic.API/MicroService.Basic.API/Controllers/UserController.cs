using MicroService.Basic.Abstraction;
using MicroService.Basic.Domain.Entity;
using MicroService.Basic.DTO.Req;
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
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<ActionResult<Paging<UserEntity>>> Get(UserListReq req)
        {
            var result =  await _userService.GetList(req);
            return Success(result);
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<UserEntity>> Get(string keyValue)
        {
            var result = await _userService.GetInfo(keyValue);
            return Success(result);
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<bool>> Post([FromBody] UserInfoReq req)
        {
            var result = await _userService.SubmitForm(req);
            return result == true ? Success(result) : Fail("操作失败");
        }
         
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(string keyValue)
        {
            var result = await _userService.DeleteForm(keyValue);
            return result == true ? Success(result) : Fail("操作失败");
        }
    }
}
