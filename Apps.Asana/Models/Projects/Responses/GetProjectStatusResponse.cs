using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Models.Projects.Responses
{
    public class GetProjectStatusResponse
    {
        public string GId { get; set; }

        public string Color { get; set; }

        public string Text { get; set; }

        public string Title { get; set; }
    }
}
