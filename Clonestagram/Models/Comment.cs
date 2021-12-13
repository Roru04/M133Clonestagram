using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Clonestagram.Models
{
    public class Comment
    {
        public string CommentContent { get; set; }

        public int Id { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual Post Post { get; set; }

        public int PostId { get; set; }

        public string ApplicationUserId { get; set; }

        public string ApplicationUserName { get; set; }

        [Timestamp] 
        public Byte[] RowVersion { get; set; }


        public DateTime date { get; set; }
    }
}