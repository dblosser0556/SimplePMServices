using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePMServices.ViewModels
{
    public class CodeWithType
    {
        public int ID { get; set; }
        public int CodeTypeID { get; set; }
        public string CodeName { get; set; }
        public string CodeDesc { get; set; }
        public string TypeName { get; set; }
        public string TypeDesc { get; set; }
    }
}
