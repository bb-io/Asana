using System.Net.Mime;
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
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Extensions.String;

namespace Apps.Asana.Actions;

[ActionList("Attachment")]
public class AttachmentActions : AsanaActions
{
    private readonly IFileManagementClient _fileManagementClient;

    public AttachmentActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : base(
        invocationContext)
    {
        _fileManagementClient = fileManagementClient;
    }

    [Action("Search attachments", Description = "List attachments from object")]
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
    public async Task<AttachmentDto> GetAttachment([ActionParameter] AttachmentRequest input)
    {
        var endpoint = $"{ApiEndpoints.Attachments}/{input.AttachmentId}";
        var request = new AsanaRequest(endpoint, Method.Get, Creds);

        var response = await Client.ExecuteWithErrorHandling<AttachmentResponse>(request);

        var contentType = MimeTypes.TryGetMimeType(response.Name, out var mimeType)
            ? mimeType
            : MediaTypeNames.Application.Octet;
        return new(response)
        {
            File = new FileReference(new HttpRequestMessage(HttpMethod.Get, response.DownloadUrl), response.Name, contentType)
        };
    }

    [Action("Delete attachment", Description = "Delete attachment by ID")]
    public Task DeleteAttachment([ActionParameter] AttachmentRequest input)
    {
        var endpoint = $"{ApiEndpoints.Attachments}/{input.AttachmentId}";
        var request = new AsanaRequest(endpoint, Method.Delete, Creds);

        return Client.ExecuteWithErrorHandling(request);
    }

    [Action("Upload attachment", Description = "Upload a new attachment")]
    public async Task<AttachmentDto> UploadAttachment(
        [ActionParameter] UploadAttachmentRequest input)
    {
        var request = new AsanaRequest(ApiEndpoints.Attachments, Method.Post, Creds);

        var file = await _fileManagementClient.DownloadAsync(input.File);
        request.AddFile("file", () => file, input.FileName ?? input.File.Name!);
        request.AddParameter("parent", input.ParentId);

        return await Client.ExecuteWithErrorHandling<AttachmentDto>(request);
    }
}