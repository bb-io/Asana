using Apps.Asana.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Models.Attachments.Responses
{
    public class GetAttachmentsResponse
    {
       public IEnumerable<AttachmentDto> Attachments { get; set; }
    }
}
