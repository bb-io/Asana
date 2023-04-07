using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Models.Sections.Requests
{
    public class CreateSectionRequest
    {
        public string ProjectId { get; set; }

        public string SectionName { get; set; }
    }
}
