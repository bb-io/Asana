using Apps.Asana.Models.Tasks.Requests;

namespace Apps.Asana.Models.Tasks.Responses;

// Have to filter client-side too because the server-side filter behaves weird when it comes to dash-separated values
// Example:
//  - Custom field value: '1234-5678-910'
//  - Server-side filter: '910-5678-1234'
//  - Result: the task is returned because the '5678' part matches in the middle
public static class CustomFieldSearchFilter
{
    private const StringComparison IgnoreCase = StringComparison.OrdinalIgnoreCase;

    public static List<TaskSearchResultDto> Apply(IEnumerable<TaskSearchResultDto> tasks, ListTasksRequest input)
    {
        return tasks.Where(task => MatchesText(task, input)).ToList();
    }

    private static bool MatchesText(TaskSearchResultDto task, ListTasksRequest input)
    {
        if (string.IsNullOrWhiteSpace(input.TextCustomFieldId))
            return true;

        var value = task.CustomFields?.FirstOrDefault(field => field.Gid == input.TextCustomFieldId)?.TextValue ?? string.Empty;

        return (string.IsNullOrWhiteSpace(input.TextCustomFieldContains) || value.Contains(input.TextCustomFieldContains, IgnoreCase))
               && (string.IsNullOrWhiteSpace(input.TextCustomFieldEquals) || value.Equals(input.TextCustomFieldEquals, IgnoreCase));
    }
}