using Apps.Asana.Dtos;
using Apps.Asana.Models.Tags.Requests;
using Apps.Asana.Models.Tags.Responses;
using Apps.Translate5;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;

namespace Apps.Asana.Actions
{
    [ActionList]
    public class TagActions
    {
        [Action("List tags", Description = "List tags")]
        public ListTagsResponse ListAllTags(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] ListTagsRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/tags?workspace={input.WorkspaceId}", Method.Get, authenticationCredentialsProviders);
            var tags = client.Get<ResponseWrapper<List<TagDto>>>(request);
            return new ListTagsResponse()
            {
                Tags = tags.Data
            };
        }

        [Action("Get tag", Description = "Get tag by Id")]
        public GetTagResponse GetTag(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] GetTagRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/tags/{input.TagId}", Method.Get, authenticationCredentialsProviders);
            var task = client.Get<ResponseWrapper<TagDto>>(request);
            return new GetTagResponse()
            {
                GId = task.Data.GId,
                Name = task.Data.Name,
                Color = task.Data.Color
            };
        }

        [Action("Update tag", Description = "Update tag by Id")]
        public void UpdateTag(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] UpdateTagRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/tags/{input.TagId}", Method.Put, authenticationCredentialsProviders);
            request.AddJsonBody(new
            {
                data = new
                {
                    name = input.NewName,
                    color = input.NewColor
                }
            });

            client.Execute(request);
        }

        [Action("Create tag", Description = "Create tag")]
        public void CreateTag(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] CreateTagRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/tags", Method.Post, authenticationCredentialsProviders);
            request.AddJsonBody(new
            {
                data = new
                {
                    name = input.TagName,
                    workspace = input.WorkspaceId,
                    color = input.TagColor
                }
            });
            client.Execute(request);
        }

        [Action("Delete tag", Description = "Delete tag")]
        public void DeleteTag(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] DeleteTagRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/tags/{input.TagId}", Method.Delete, authenticationCredentialsProviders);
            client.Execute(request);
        }
    }
}
