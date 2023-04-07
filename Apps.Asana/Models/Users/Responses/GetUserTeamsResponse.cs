using Apps.Asana.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Models.Users.Responses
{
    public class GetUserTeamsResponse
    {   
        public IEnumerable<WorkspaceDto> Teams { get; set; }
    }
}
