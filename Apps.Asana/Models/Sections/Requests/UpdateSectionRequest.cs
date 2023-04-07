using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Models.Sections.Requests
{
    public class UpdateSectionRequest
    {
        public string SectionId { get; set;}

        public string NewName { get; set;}
    }
}
