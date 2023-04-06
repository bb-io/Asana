using Apps.Asana.Dtos;
using Apps.Asana.Models.Projects.Requests;
using Apps.Asana.Models.Projects.Responses;
using Apps.Translate5;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Asana.Models.Attachments.Requests;
using Apps.Asana.Models.Attachments.Responses;

namespace Apps.Asana.Actions
{
    [ActionList]
    public class AttachmentsActions
    {
        [Action("Get attachments", Description = "Get attachments from object")]
        public GetAttachmentsResponse GetAttachments(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] GetAttachmentsRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/attachments?parent={input.ObjectId}", Method.Get, authenticationCredentialsProvider);
            var projects = client.Get<ResponseWrapper<List<AttachmentDto>>>(request);
            return new GetAttachmentsResponse()
            {
                Attachments = projects.Data
            };
        }

        [Action("Get attachment", Description = "Get attachment by Id")]
        public GetAttachmentResponse GetAttachment(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] GetAttachmentRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/attachments/{input.AttachmentId}", Method.Get, authenticationCredentialsProvider);
            var attachment = client.Get<ResponseWrapper<FullAttachmentDto>>(request);
            return new GetAttachmentResponse()
            {
                GId = attachment.Data.GId,
                Name = attachment.Data.Name,
                Download_url = attachment.Data.Download_url,
                Permanent_url = attachment.Data.Permanent_url,
            };
        }

        [Action("Delete attachment", Description = "Delete attachment by Id")]
        public void DeleteAttachment(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] DeleteAttachmentRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/attachments/{input.AttachmentId}", Method.Delete, authenticationCredentialsProvider);
            client.Execute(request);
        }

        [Action("Upload attachment", Description = "Upload attachment")]
        public void UploadAttachment(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] UploadAttachmentRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/attachments", Method.Post, authenticationCredentialsProvider);

            request.AddFile("file", input.File, input.Filename);
            request.AddParameter("parent", input.ParentId);
            client.Execute(request);
        }
    }
}
