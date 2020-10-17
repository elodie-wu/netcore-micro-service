using MicroService.Basic.Data;
using MicroService.Basic.Data.DBContext;
using MicroService.Basic.Domain.Entity;
using MicroService.Basic.Domain.IRepository;

namespace MicroService.Basic.Repository
{
    public class UserRepository:BaseRepository<UserEntity,string>,IUserRepository
    {
        public UserRepository(BasicDBContext dbContext) :base(dbContext)
        {
        }
    }
}
