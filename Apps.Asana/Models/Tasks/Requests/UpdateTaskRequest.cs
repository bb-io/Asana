using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Models.Tasks.Requests
{
    public class UpdateTaskRequest
    {
        public string TaskId { get; set; }

        public string NewName { get; set; }
    }
}
