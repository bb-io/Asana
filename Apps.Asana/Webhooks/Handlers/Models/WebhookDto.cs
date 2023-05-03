using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Webhooks.Handlers.Models
{
    public class WebhookDto
    {
        public string GId { get; set; }
        public string ResourceType { get; set; }
    }
}
