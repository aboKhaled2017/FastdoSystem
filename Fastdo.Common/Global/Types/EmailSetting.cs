using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastDo.Common
{
    public class EmailSetting
    {
        public string from { get; set; }
        public string password { get; set; }
        public bool writeAsFile { get; set; }
    }
}
