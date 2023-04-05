using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Models.Projects.Requests
{
    public class UpdateProjectRequest
    {
        public string ProjectId { get; set; }

        public string NewName { get; set; }
    }
}
