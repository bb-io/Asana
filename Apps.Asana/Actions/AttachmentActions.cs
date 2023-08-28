using Apps.Asana.Actions.Base;
using Apps.Asana.Api;
using Apps.Asana.Constants;
using Apps.Asana.Dtos;
using Apps.Asana.Dtos.Base;
using Blackbird.Applications.Sdk.Common;
using RestSharp;
using Apps.Asana.Models.Attachments.Requests;
using Apps.Asana.Models.Attachments.Responses;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.String;

namespace Apps.Asana.Actions;

[ActionList]
public class AttachmentActions : AsanaActions
{
    public AttachmentActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    [Action("List attachments", Description = "List attachments from object")]
    public async Task<GetAttachmentsResponse> ListAttachments(
        [ActionParameter] ListAttachmentsRequest input)
    {
        var endpoint = ApiEndpoints.Attachments.SetQueryParameter("parent", input.ObjectId);
        var request = new AsanaRequest(endpoint, Method.Get, Creds);

        var attachments = await Client.ExecuteWithErrorHandling<IEnumerable<AsanaEntity>>(request);

        return new()
        {
            Attachments = attachments
        };
    }

    [Action("Get attachment", Description = "Get attachment by ID")]
    public Task<AttachmentDto> GetAttachment([ActionParameter] AttachmentRequest input)
    {
        var endpoint = $"{ApiEndpoints.Attachments}/{input.AttachmentId}";
        var request = new AsanaRequest(endpoint, Method.Get, Creds);

        return Client.ExecuteWithErrorHandling<AttachmentDto>(request);
    }

    [Action("Delete attachment", Description = "Delete attachment by ID")]
    public Task DeleteAttachment([ActionParameter] AttachmentRequest input)
    {
        var endpoint = $"{ApiEndpoints.Attachments}/{input.AttachmentId}";
        var request = new AsanaRequest(endpoint, Method.Delete, Creds);

        return Client.ExecuteWithErrorHandling(request);
    }

    [Action("Upload attachment", Description = "Upload a new attachment")]
    public Task<AttachmentDto> UploadAttachment(
        [ActionParameter] UploadAttachmentRequest input)
    {
        var request = new AsanaRequest(ApiEndpoints.Attachments, Method.Post, Creds);

        request.AddFile("file", input.File.Bytes, input.FileName ?? input.File.Name);
        request.AddParameter("parent", input.ParentId);

        return Client.ExecuteWithErrorHandling<AttachmentDto>(request);
    }
}