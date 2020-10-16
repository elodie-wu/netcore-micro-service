using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroService.Basic.Data.DBContext
{
    public class BasicDBContext:DbContext
    {
        public BasicDBContext(DbContextOptions<BasicDBContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
            //Database.Migrate();

        }
    }
}
