using Apps.Asana.Actions.Base;
using Apps.Asana.Api;
using Apps.Asana.Constants;
using Apps.Asana.Dtos;
using Apps.Asana.Dtos.Base;
using Apps.Asana.Models.Attachments.Requests;
using Apps.Asana.Models.Attachments.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;
using System.Net.Http.Headers;
using System.Net.Mime;

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

        using var httpClient = new HttpClient();
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, response.DownloadUrl);

        using var httpResponse = await httpClient.SendAsync(httpRequest,HttpCompletionOption.ResponseHeadersRead);

        httpResponse.EnsureSuccessStatusCode();

        await using var stream = await httpResponse.Content.ReadAsStreamAsync();
        var uploaded = await _fileManagementClient.UploadAsync(stream, contentType, response.Name);

        return new(response)
        {
            File = uploaded
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