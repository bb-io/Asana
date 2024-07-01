using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Models.Teams.Requests;

public class TeamRequest
{
    [Display("Team ID")]
    public string TeamId { get; set; }
}