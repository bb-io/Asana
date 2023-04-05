using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Models.Tasks.Responses
{
    public class GetTaskResponse
    {
        public string GId { get; set; }

        public string Name { get; set; }

        public string Notes { get; set; }
    }
}
