using MicroService.Basic.Abstraction;
using MicroService.Basic.Domain.Entity;
using MicroService.Basic.Domain.IRepository;
using MicroService.Basic.DTO.Req;
using MicroService.Common.Page;
using System;
using System.Threading.Tasks;

namespace MicroService.Basic.Application
{
    public class UserService: IUserService
    {
        readonly IUserRepository _userRepository; 
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Paging<UserEntity>> GetList(UserListReq req)
        { 
            return await _userRepository.FindListAsync(x=>x.Name.Contains(req.Name),req.pagination);
        } 
        public async Task<UserEntity> GetInfo(string keyValue)
        {
            return await _userRepository.FindEntityAsync(keyValue);
        }
        public async Task<bool> DeleteForm(string keyValue)
        { 
            return await _userRepository.DeleteAsync(keyValue);
        }
        public async Task<bool> SubmitForm(UserInfoReq req)
        {
           
            if (!string.IsNullOrEmpty(req.ID))
            {
                var entity = await _userRepository.FindEntityAsync(req.ID);
                entity.LastModifyTime = DateTime.Now;
                entity.LastModifyUserId = req.operatorID;
                return await _userRepository.UpdateAsync(entity);
            }
            else
            {
                var entity = new UserEntity();
                entity.ID = Guid.NewGuid().ToString();  
                entity.CreateUserId = req.operatorID;
                return  await _userRepository.InsertAsync(entity);
            }
        } 
    }
}
