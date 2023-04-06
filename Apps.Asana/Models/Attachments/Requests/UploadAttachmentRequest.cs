using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Models.Attachments.Requests
{
    public class UploadAttachmentRequest
    {
        public string Filename { get; set; }

        public byte[] File { get; set; }

        public string ParentId { get; set; }
    }
}
