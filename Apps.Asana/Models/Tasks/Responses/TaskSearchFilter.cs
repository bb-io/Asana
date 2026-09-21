namespace Apps.Asana.Models.Tasks.Responses;

public static class TaskSearchFilter
{
    public static List<TaskSearchResultDto> ByMembership(
        IEnumerable<TaskSearchResultDto> tasks,
        string? projectId,
        string? sectionId)
    {
        if (!string.IsNullOrWhiteSpace(sectionId))
            return tasks.Where(task => HasMembership(task, membership => membership.Section?.Gid == sectionId)).ToList();

        if (!string.IsNullOrWhiteSpace(projectId))
            return tasks.Where(task => HasMembership(task, membership => membership.Project?.Gid == projectId)).ToList();

        return tasks.ToList();
    }

    private static bool HasMembership(TaskSearchResultDto task, Func<TaskMembershipDto, bool> predicate) =>
        task.Memberships?.Any(predicate) == true;
}
