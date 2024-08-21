using Apps.Asana.Actions.Base;
using Apps.Asana.Api;
using Apps.Asana.Constants;
using Apps.Asana.Dtos;
using Apps.Asana.Dtos.Base;
using Apps.Asana.Models;
using Apps.Asana.Models.Projects.Requests;
using Apps.Asana.Models.Sections.Requests;
using Apps.Asana.Models.Sections.Response;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Asana.Actions;

[ActionList]
public class SectionActions : AsanaActions
{
    public SectionActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    [Action("List sections", Description = "List all project sections")]
    public async Task<ListSectionsResponse> ListSections([ActionParameter] ProjectRequest input)
    {
        var endpoint = $"{ApiEndpoints.Projects}/{input.GetProjectId()}{ApiEndpoints.Sections}";
        var request = new AsanaRequest(endpoint, Method.Get, Creds);

        var sections = await Client.ExecuteWithErrorHandling<IEnumerable<AsanaEntity>>(request);

        return new()
        {
            Sections = sections
        };
    }

    [Action("Get section", Description = "Get section by ID")]
    public Task<AsanaEntity> GetSection([ActionParameter] SectionRequest input)
    {
        var endpoint = $"{ApiEndpoints.Sections}/{input.SectionId}";
        var request = new AsanaRequest(endpoint, Method.Get, Creds);

        return Client.ExecuteWithErrorHandling<AsanaEntity>(request);
    }

    [Action("Update section", Description = "Update section by ID")]
    public Task<SectionDto> UpdateSection(
        [ActionParameter] SectionRequest section,
        [ActionParameter] ManageSectionRequest input)
    {
        var payload = new ResponseWrapper<ManageSectionRequest>()
        {
            Data = input
        };

        var endpoint = $"{ApiEndpoints.Sections}/{section.SectionId}";
        var request = new AsanaRequest(endpoint, Method.Put, Creds)
            .WithJsonBody(payload, JsonConfig.Settings);

        return Client.ExecuteWithErrorHandling<SectionDto>(request);
    }

    [Action("Create section", Description = "Create section in project")]
    public Task<SectionDto> CreateSection(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] ManageSectionRequest input)
    {
        var payload = new ResponseWrapper<ManageSectionRequest>()
        {
            Data = input
        };

        var endpoint = $"{ApiEndpoints.Projects}/{project.GetProjectId()}{ApiEndpoints.Sections}";
        var request = new AsanaRequest(endpoint, Method.Post, Creds)
            .WithJsonBody(payload, JsonConfig.Settings);

        return Client.ExecuteWithErrorHandling<SectionDto>(request);
    }

    [Action("Delete section", Description = "Delete specific section")]
    public Task DeleteSection([ActionParameter] SectionRequest input)
    {
        var endpoint = $"{ApiEndpoints.Sections}/{input.SectionId}";
        var request = new AsanaRequest(endpoint, Method.Delete, Creds);

        return Client.ExecuteWithErrorHandling(request);
    }
}