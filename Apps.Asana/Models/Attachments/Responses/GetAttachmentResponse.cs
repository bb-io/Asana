using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Models.Attachments.Responses
{
    public class GetAttachmentResponse
    {
        public string GId { get; set; }

        public string Name { get; set; }

        public string Download_url { get; set; }

        public string Permanent_url { get; set; }
    }
}
