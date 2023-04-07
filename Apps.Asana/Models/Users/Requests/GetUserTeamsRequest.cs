using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Models.Users.Requests
{
    public class GetUserTeamsRequest
    {
        public string WorkspaceId { get; set; }

        public string UserId { get; set; }
    }
}
