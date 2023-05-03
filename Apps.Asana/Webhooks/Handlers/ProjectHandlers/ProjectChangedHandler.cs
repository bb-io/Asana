using Apps.Asana.Webhooks.Handlers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Webhooks.Handlers.ProjectHandlers
{
    public class ProjectChangedHandler : BaseWebhookHandler
    {

        const string ResourceId = "1204337900073971";
        const string ResourceType = "project";
        const string Action = "changed";
        public  ProjectChangedHandler() : base(ResourceId, ResourceType, Action) { }
    }
}
