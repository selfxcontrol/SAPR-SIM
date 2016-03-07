using Entities;
using Entities.impl;
using Statistics.Beans;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    class SaprSimDbContext : DbContext
    {
        public SaprSimDbContext() : base("SaprSimDbContext")
        {
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Model> models { get; set; }

        public DbSet<Resource> resources { get; set; }

        public DbSet<AbstractStatiscticsBean> statistics { get; set; }

    }
}
