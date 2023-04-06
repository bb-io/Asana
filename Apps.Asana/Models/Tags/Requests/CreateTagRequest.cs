using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Models.Tags.Requests
{
    public class CreateTagRequest
    {
        public string WorkspaceId { get; set; }

        public string TagName { get; set; }

        public string TagColor { get; set; }
    }
}
