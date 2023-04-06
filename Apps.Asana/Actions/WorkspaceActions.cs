using Apps.Asana.Dtos;
using Apps.Asana.Models.Tasks.Requests;
using Apps.Asana.Models.Tasks.Responses;
using Apps.Translate5;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Asana.Models.Workspaces.Responses;

namespace Apps.Asana.Actions
{
    [ActionList]
    public class WorkspaceActions
    {
        [Action("List workspaces", Description = "List workspaces")]
        public ListWorkspacesResponse ListAllWorkspaces(AuthenticationCredentialsProvider authenticationCredentialsProvider)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/workspaces", Method.Get, authenticationCredentialsProvider);
            var workspaces = client.Get<ResponseWrapper<List<WorkspaceDto>>>(request);
            return new ListWorkspacesResponse()
            {
                Workspaces = workspaces.Data
            };
        }
    }
}
