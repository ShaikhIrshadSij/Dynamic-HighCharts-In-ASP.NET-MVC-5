using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DynamicHighCharts.Entity.POCO
{
    public class CodeHubsContext : DbContext
    {
        public CodeHubsContext() : base("DBConnectionString") { }
        public DbSet<Analysis> Analyses { get; set; }
    }
}