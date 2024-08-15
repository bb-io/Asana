using Apps.Asana.DataSourceHandlers;
using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.Users.Requests;

public class UserRequest : WorkspaceRequest
{
    [Display("User ID")]
    [DataSource(typeof(UserDataHandler))]
    public string UserId { get; set; }
}