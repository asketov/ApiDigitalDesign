using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDigitalDesign.Models.PostModels
{
    public class GetPostsSubscriptionsRequest
    {
        [Range(0, Int32.MaxValue)]
        public int SkipPosts { get; set; } = 0;
        [Range(1, 100)]
        public int TakePosts { get; set; } = 10;
    }
}
