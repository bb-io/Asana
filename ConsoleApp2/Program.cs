
using Apps.Asana.Actions;
using Apps.Asana.Connections;
using Apps.Asana.DataSourceHandlers.CustomFields;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.SDK.Extensions.FileManagement.Models;
using System;
using System.Data;

Console.WriteLine("Hello, World!");

string apikey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpYXQiOjE3NjExMzM1NDIsInNjb3BlIjoiZGVmYXVsdCBpZGVudGl0eSIsInN1YiI6MTIxMTU1NzM2ODI3NTAyNiwicmVmcmVzaF90b2tlbiI6MTIxMTYwMTA2MzM1NzIyMywidmVyc2lvbiI6MiwiYXBwIjoxMjA1Mzg1MTA0ODY5ODM1LCJleHAiOjE3NjExMzcxNDJ9.-ioCXjbtZY_vrb3SnPSAOneVF5L0-wh9y-043WldptE";

//var creds = new OAuth2ConnectionDefinition().CreateAuthorizationCredentialsProviders(new Dictionary<string, string> { { "apiKey", apikey }, { "tenantId", TenantID } });
//var creds = new AuthenticationCredentialsProvider(new AuthenticationCredentialsRequestLocation { }, "test1", "test2");
AuthenticationCredentialsProvider[] creds = { new AuthenticationCredentialsProvider(new AuthenticationCredentialsRequestLocation { }, "AccessToken", apikey) };


var context = new Blackbird.Applications.Sdk.Common.Invocation.InvocationContext { AuthenticationCredentialsProviders = creds };

var ttt = new FileManagementClient();
//var actions = new Apps.LanguageCloud.Actions.FileActions(ttt);

//var handlers = new DateCustomFieldDataHandler(context, new Apps.Asana.Models.CustomFields.Requests.DateCustomFieldRequest { ProjectId = "1211601161389560", WorkspaceId = "157902303643426" });

//var response = await handlers.GetDataAsync(new Blackbird.Applications.Sdk.Common.Dynamic.DataSourceContext(), CancellationToken.None);

var actions = new Apps.Asana.Actions.CustomFieldsActions(context);

//var response = await actions.GetDateCustomField(new Apps.Asana.Models.CustomFields.Requests.DateCustomFieldRequest { WorkspaceId = "157902303643426", ProjectId = "1211601161389560", TaskId = "1211601161389563", CustomFieldId = "1211601164667507" });
var response = await actions.GetMultiEnumCustomField(new Apps.Asana.Models.CustomFields.Requests.MultipleCustomFieldRequest
{
    WorkspaceId = "157902303643426",
    ProjectId = "1211601161389560",
    TaskId = "1211601161389563",
    CustomFieldId = "1211601161389572"
}
);
//var respose = actions.GetProject(creds, new Apps.LanguageCloud.Models.Projects.Requests.GetProjectRequest { Project = "65c537da956f0450deb6ec9a" });


//ffile.File.Name = "Test_file_3.txt";

//var response = await actions.TranslateDocument(new Apps.Asana.Requests.DocumentTranslationRequest
//{
//    File = ffile,
//    TargetLanguage = "nl",
//    SourceLanguage = "en"

//});

Console.WriteLine("The end");

//var response = actions.DownloadTargetFile(creds, new Apps.LanguageCloud.Models.Files.Requests.DownloadFileRequest
//{
//    FileId = "65ce1d3d8744825dc053d475",
//    ProjectId = "65cd60db14554c0d4b74e7dd"
//});

//var response = actions.ListTargetFiles(creds, new Apps.LanguageCloud.Models.Files.Requests.ListSourceFilesRequest
//{ ProjectId = "65cd60db14554c0d4b74e7dd" });


//var response = actions.CreateProjectFromTemplate(creds, new Apps.LanguageCloud.Models.Projects.Requests.CreateFromTemplateRequest
//{
//    Name = "testFromTemplate3",
//    DueBy = "2024-02-29T12:00:00.000Z",
//    Location = "7ee9d51124664daaa46fa8f848b1028e",
//    Template = "65cbb82fa59d787ed55edcf9",
//    Workflow = "65cbb813aeb4d82e0f160700",
//    FileProcessingConfiguration = "82232491-616f-4e07-8fc5-fd5aebe7ffa8",
//    TranslationEngine = "65cbc488f3a46935174e448f",
//    SourceLanguage = "en-us",
//    TargetLanguages = new List<string> { "nl-nl", "fr-fr" }
//});


Console.WriteLine("END");

public class FileManagementClient : IFileManagementClient
{
    async Task<Stream> IFileManagementClient.DownloadAsync(FileReference reference)
    {
        Stream file = File.Open(@"c:\Users\cenis\Downloads\terminology2.tbx", FileMode.Open);
        return file;
    }

    Task<FileReference> IFileManagementClient.UploadAsync(Stream stream, string contentType, string fileName)
    {
        Console.WriteLine(fileName);
        StreamReader reader = new StreamReader(stream);
        string text = reader.ReadToEnd();
        File.WriteAllText(@"c:\Users\cenis\Downloads\TestFile_result.xlf", text);
        Console.WriteLine(text);
        return Task.FromResult(new FileReference());
    }
}