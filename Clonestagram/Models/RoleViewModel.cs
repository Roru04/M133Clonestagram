using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clonestagram.Models
{
    public class RoleViewModel
    {
        public bool HasRole { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<UserViewModel> Users { get; set; }
    }
}