using Apps.Asana.Dtos;
using Apps.Asana.Models.Sections.Requests;
using Apps.Asana.Models.Sections.Responses;
using Apps.Asana.Models.Tasks.Requests;
using Apps.Asana.Models.Tasks.Responses;
using Apps.Translate5;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Actions
{
    [ActionList]
    public class SectionActions
    {
        [Action("Get section", Description = "Get section by Id")]
        public GetSectionResponse GetSection(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] GetSectionRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/sections/{input.SectionId}", Method.Get, authenticationCredentialsProvider);
            var section = client.Get<ResponseWrapper<SectionDto>>(request);
            return new GetSectionResponse()
            {
                GId = section.Data.GId,
                Name = section.Data.Name,
            };
        }

        [Action("Update section", Description = "Update section by Id")]
        public void UpdateSection(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] UpdateSectionRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/sections/{input.SectionId}", Method.Put, authenticationCredentialsProvider);
            request.AddJsonBody(new
            {
                data = new
                {
                    name = input.NewName
                }
            });
            client.Execute(request);
        }

        [Action("Create section", Description = "Create section in project")]
        public void CreateSection(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] CreateSectionRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/projects/{input.ProjectId}/sections", Method.Post, authenticationCredentialsProvider);
            request.AddJsonBody(new
            {
                data = new
                {
                    name = input.SectionName,
                }
            });
            client.Execute(request);
        }

        [Action("Delete section", Description = "Delete section")]
        public void DeleteSection(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] DeleteSectionRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/sections/{input.SectionId}", Method.Delete, authenticationCredentialsProvider);
            client.Execute(request);
        }
    }
}
