﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Models.Projects.Requests
{
    public class CreateProjectRequest
    {
        public string TeamId { get; set; }

        public string ProjectName { get; set; }
    }
}
