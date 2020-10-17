using MicroService.Basic.Domain.Entity;
using MicroService.Basic.DTO.Req;
using MicroService.Common.Page;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Basic.Abstraction
{
    public interface IUserService
    {
        Task<Paging<UserEntity>> GetList(UserListReq req);
        Task<UserEntity> GetInfo(string keyValue);
        Task<bool> DeleteForm(string keyValue);
        Task<bool> SubmitForm(UserInfoReq req);
    }
}
