using Apps.Asana.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Models.Tags.Responses
{
    public class ListTagsResponse
    {
        public IEnumerable<TagDto> Tags { get; set; }
    }
}
