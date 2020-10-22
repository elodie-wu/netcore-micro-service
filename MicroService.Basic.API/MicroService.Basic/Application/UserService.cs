using AutoMapper;
using MicroService.Basic.Abstraction;
using MicroService.Basic.Domain.Entity;
using MicroService.Basic.Domain.IRepository;
using MicroService.Basic.DTO.Req;
using MicroService.Basic.DTO.VM;
using MicroService.Common.Page;
using System;
using System.Collections.Generic;
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

        public async Task<Paging<UserInfo>> GetList(UserListReq req)
        { 
            var entities =  await _userRepository.FindListAsync(x=> !x.IsDeleted,req.pagination); 
            var result = Mapper.Map<List<UserEntity>, List<UserInfo>>(entities.List);
        } 
        public async Task<UserEntity> GetInfo(string keyValue)
        {
            return await _userRepository.FindEntityAsync(keyValue);
        }
        public async Task<bool> DeleteForm(string keyValue,string userID)
        {
            var entity = await _userRepository.FindEntityAsync(keyValue);
            entity.LastModifyTime = DateTime.Now;
            entity.LastModifyUserId = userID;
            entity.IsDeleted = true; 
            return await _userRepository.UpdateAsync(entity);
             
        }
        public async Task<bool> SubmitForm(UserInfoReq req)
        {
           
            if (!string.IsNullOrEmpty(req.ID))
            {
                var entity = await _userRepository.FindEntityAsync(req.ID);
                entity.Name = req.Name; 
                entity.LastModifyTime = DateTime.Now;
                entity.LastModifyUserId = req.operatorID;
                return await _userRepository.UpdateAsync(entity);
            }
            else
            {
                var entity = new UserEntity();
                entity.ID = Guid.NewGuid().ToString();  
                entity.Name = req.Name;  
                entity.CreateUserId = req.operatorID;
                return  await _userRepository.InsertAsync(entity);
            }
        } 
    }
}
