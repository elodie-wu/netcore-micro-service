using MicroService.Basic.Data.Infrastructure;

namespace MicroService.Basic.Domain.Entity
{
    public class UserEntity:IEntity<string>
    { 
        public string Name { get; set; }
    }
}
