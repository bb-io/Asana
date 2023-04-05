using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Dtos
{
    public class UserDto
    {
        public string GId { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public IEnumerable<WorkspaceDto> Workspaces { get; set; }
    }
}
