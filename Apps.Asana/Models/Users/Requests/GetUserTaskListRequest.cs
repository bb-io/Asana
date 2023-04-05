using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Models.Users.Requests
{
    public class GetUserTaskListRequest
    {
        public string UserId { get; set; }

        public string WorkspaceId { get; set; }
    }
}
