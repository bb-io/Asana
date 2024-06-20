using Apps.Asana.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.Users.Requests;

public class GetUserItemsRequest
{
    [Display("User ID")]
    [DataSource(typeof(UserDataHandler))]
    public string UserId { get; set; }

    [Display("Workspace ID")]
    [DataSource(typeof(WorkspaceDataHandler))]
    public string WorkspaceId { get; set; }
}