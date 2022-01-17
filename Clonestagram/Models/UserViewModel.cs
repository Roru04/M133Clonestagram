using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clonestagram.Models
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public virtual ICollection<RoleViewModel> Roles { get; set; }

    }
}