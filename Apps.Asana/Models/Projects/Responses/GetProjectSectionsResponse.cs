using Apps.Asana.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Models.Projects.Responses
{
    public class GetProjectSectionsResponse
    {
        public IEnumerable<SectionDto> Sections { get; set; }
    }
}
