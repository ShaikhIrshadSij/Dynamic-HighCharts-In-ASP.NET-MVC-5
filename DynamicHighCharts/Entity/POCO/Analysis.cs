using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DynamicHighCharts.Entity.POCO
{
    public class Analysis
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public string Value { get; set; }
    }
}