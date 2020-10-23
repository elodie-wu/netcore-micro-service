using System;
using System.Collections.Generic;
using System.Text;

namespace MicroService.Basic.Data.Infrastructure
{
    public class IEntity<T>
    {
        public T ID { get; set; }
        public T CreateUserId { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public T LastModifyUserId { get; set; }
        public DateTime? LastModifyTime { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class IEntity
    { 
        public string ID { get; set; }
        public string CreateUserId { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public string LastModifyUserId { get; set; }
        public DateTime? LastModifyTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
