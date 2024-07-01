using Apps.Asana.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.Teams.Requests;

public class TeamRequest
{
    [Display("Team ID"), DataSource(typeof(TeamDataHandler))]
    public string TeamId { get; set; }
}