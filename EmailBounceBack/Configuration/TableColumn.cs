using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailBounceBack.Configuration
{
    public class TableColumn
    {
        public String Name { get; set; }
        public String Member { get; set; }
        public String Storage { get; set; }
        public String DbType { get; set; }
        public Boolean IsPrimaryKey { get; set; }
    }
}
