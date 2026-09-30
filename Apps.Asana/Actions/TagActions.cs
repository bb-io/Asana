using Apps.Asana.Actions.Base;
using Apps.Asana.Api;
using Apps.Asana.Constants;
using Apps.Asana.Dtos;
using Apps.Asana.Dtos.Base;
using Apps.Asana.Models;
using Apps.Asana.Models.Tags.Requests;
using Apps.Asana.Models.Tags.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using RestSharp;

namespace Apps.Asana.Actions;

[ActionList("Tag")]
public class TagActions(InvocationContext invocationContext) : AsanaActions(invocationContext)
{
    [Action("Search tags", Description = "Search tags, optionally within a workspace, and output their IDs and names.")]
    public async Task<ListTagsResponse> ListAllTags([ActionParameter] ListTagsRequest input)
    {
        var endpoint = ApiEndpoints.Tags.WithQuery(input);
        var request = new AsanaRequest(endpoint, Method.Get, Creds);

        var tags = await Client.ExecuteWithErrorHandling<IEnumerable<AsanaEntity>>(request);

        return new()
        {
            Tags = tags
        };
    }

    [Action("Get tag", Description = "Output the details of the selected tag.")]
    public Task<TagDto> GetTag([ActionParameter] TagRequest input)
    {
        var endpoint = $"{ApiEndpoints.Tags}/{input.TagId}";
        var request = new AsanaRequest(endpoint, Method.Get, Creds);

        return Client.ExecuteWithErrorHandling<TagDto>(request);
    }

    [Action("Update tag", Description = "Update the name, color, or notes of the selected tag and output its details.")]
    public Task<TagDto> UpdateTag(
        [ActionParameter] TagRequest tag,
        [ActionParameter] UpdateTagRequest input)
    {
        var payload = new ResponseWrapper<UpdateTagRequest>()
        {
            Data = input
        };
        var endpoint = $"{ApiEndpoints.Tags}/{tag.TagId}";
        var request = new AsanaRequest(endpoint, Method.Put, Creds)
            .WithJsonBody(payload, JsonConfig.Settings);

        return Client.ExecuteWithErrorHandling<TagDto>(request);
    }

    [Action("Create tag", Description = "Create a tag in the selected workspace and output its details.")]
    public Task<TagDto> CreateTag([ActionParameter] CreateTagRequest input)
    {
        var payload = new ResponseWrapper<CreateTagRequest>()
        {
            Data = input
        };
        var request = new AsanaRequest(ApiEndpoints.Tags, Method.Post, Creds)
            .WithJsonBody(payload, JsonConfig.Settings);

        return Client.ExecuteWithErrorHandling<TagDto>(request);
    }

    [Action("Delete tag", Description = "Delete the selected tag.")]
    public Task DeleteTag([ActionParameter] TagRequest input)
    {
        var endpoint = $"{ApiEndpoints.Tags}/{input.TagId}";
        var request = new AsanaRequest(endpoint, Method.Delete, Creds);
        
        return Client.ExecuteWithErrorHandling(request);
    }
}