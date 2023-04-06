using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Models.Tags.Requests
{
    public class UpdateTagRequest
    {
        public string TagId { get; set; }

        public string NewName { get; set; }

        public string NewColor { get; set; }
    }
}
