using MicroService.Basic.Domain.Entity;
using MicroService.Basic.DTO.Req;
using MicroService.Basic.DTO.VM;
using MicroService.Common.Page;
using System.Threading.Tasks;

namespace MicroService.Basic.Abstraction
{
    public interface IUserService
    {
        Task<Paging<UserInfo>> GetList(UserListReq req);
        Task<UserEntity> GetInfo(string keyValue);
        Task<bool> DeleteForm(string keyValue, string userID);
        Task<bool> SubmitForm(UserInfoReq req);
    }
}
