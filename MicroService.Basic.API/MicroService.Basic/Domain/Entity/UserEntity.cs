using MicroService.Basic.Data.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroService.Basic.Domain.Entity
{
    [Table("User")]
    public class UserEntity:IEntity<string>
    { 
        public string Name { get; set; }
    }
}
