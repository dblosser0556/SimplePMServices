using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimplePMServices.Models.Entities;

namespace SimplePMServices.Models.Entities
{
    public class LoggedInUser
    {
        public User CurrentUser;
        public List<string> Roles;
    }
}
