using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastDo.Common
{
    public class UserIdentifier
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; } = false;
        public string Role { get; set; }
    }
}
