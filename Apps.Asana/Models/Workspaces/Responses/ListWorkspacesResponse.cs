﻿using Apps.Asana.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Models.Workspaces.Responses
{
    public class ListWorkspacesResponse
    {
        public IEnumerable<WorkspaceDto> Workspaces { get; set; }
    }
}
