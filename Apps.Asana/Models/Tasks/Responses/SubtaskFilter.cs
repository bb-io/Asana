using Apps.Asana.Dtos.Base;

namespace Apps.Asana.Models.Tasks.Responses;

public static class SubtaskFilter
{
    public static List<T> Apply<T>(IEnumerable<T> tasks, bool? excludeSubtasks) where T : ITaskWithParent =>
        excludeSubtasks == true
            ? tasks.Where(task => task.Parent is null).ToList()
            : tasks.ToList();
}
