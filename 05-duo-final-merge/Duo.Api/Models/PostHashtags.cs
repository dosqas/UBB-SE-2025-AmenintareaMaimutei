using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duo.Api.Models
{
    public class PostHashtags
    {
        public PostHashtags() { }
        public int PostId { get; set; }
        
        public int HashtagId { get; set; }
    }
}
