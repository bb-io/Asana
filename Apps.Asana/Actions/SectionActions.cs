using Apps.Asana.Dtos;
using Apps.Asana.Models.Sections.Requests;
using Apps.Asana.Models.Sections.Responses;
using Apps.Translate5;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;

namespace Apps.Asana.Actions
{
    [ActionList]
    public class SectionActions
    {
        [Action("Get section", Description = "Get section by Id")]
        public GetSectionResponse GetSection(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] GetSectionRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/sections/{input.SectionId}", Method.Get, authenticationCredentialsProviders);
            var section = client.Get<ResponseWrapper<SectionDto>>(request);
            return new GetSectionResponse()
            {
                GId = section.Data.GId,
                Name = section.Data.Name,
            };
        }

        [Action("Update section", Description = "Update section by Id")]
        public void UpdateSection(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] UpdateSectionRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/sections/{input.SectionId}", Method.Put, authenticationCredentialsProviders);
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
        public void CreateSection(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] CreateSectionRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/projects/{input.ProjectId}/sections", Method.Post, authenticationCredentialsProviders);
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
        public void DeleteSection(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] DeleteSectionRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/sections/{input.SectionId}", Method.Delete, authenticationCredentialsProviders);
            client.Execute(request);
        }
    }
}
