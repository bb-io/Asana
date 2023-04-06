using Apps.Asana.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Models.Projects.Responses
{
    public class GetProjectStatusUpdatesResponse
    {
        public IEnumerable<ProjectStatusUpdateDto> Updates { get; set; }
    }
}
