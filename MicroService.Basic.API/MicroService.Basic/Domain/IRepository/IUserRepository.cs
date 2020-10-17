using MicroService.Basic.Data;
using MicroService.Basic.Domain.Entity;

namespace MicroService.Basic.Domain.IRepository
{
    public interface IUserRepository : IBaseRepository<UserEntity, string>
    {
    }
}
