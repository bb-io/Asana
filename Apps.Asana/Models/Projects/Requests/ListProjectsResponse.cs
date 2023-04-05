using Apps.Asana.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Models.Projects.Requests
{
    public class ListProjectsResponse
    {
        public IEnumerable<ProjectDto> Projects { get; set; }
    }
}
