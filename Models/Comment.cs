using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MarvelUniverse.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Body { get; set; }
        public int PostId { get; set; }
    }

    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Author { get; set; }
        
        [AllowHtml]
        public string Body { get; set; }
    }
}
