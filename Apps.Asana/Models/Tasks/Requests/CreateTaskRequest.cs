﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Models.Tasks.Requests
{
    public class CreateTaskRequest
    {
        public string ProjectId { get; set; }

        public string TaskName { get; set; }
    }
}
