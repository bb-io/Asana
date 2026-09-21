using Apps.Asana.Dtos;
using Apps.Asana.Dtos.Base;
using Apps.Asana.Models.Tasks.Responses;

namespace Tests.Asana.Unit;

[TestClass]
public class SubtaskFilterTests
{
    private static readonly TaskSearchResultDto TopLevel = new() { Gid = "top-level" };

    private static readonly TaskSearchResultDto Subtask =
        new() { Gid = "subtask", Parent = new AsanaEntity { Gid = "top-level" } };

    [TestMethod]
    public void Apply_ExcludeSubtasksEnabled_KeepsOnlyTopLevelTasks()
    {
        var result = SubtaskFilter.Apply([TopLevel, Subtask], true);

        CollectionAssert.AreEqual(new[] { "top-level" }, result.Select(x => x.Gid).ToArray());
    }

    [TestMethod]
    public void Apply_ExcludeSubtasksDisabled_KeepsSubtasks()
    {
        var result = SubtaskFilter.Apply([TopLevel, Subtask], false);

        CollectionAssert.AreEqual(new[] { "top-level", "subtask" }, result.Select(x => x.Gid).ToArray());
    }

    [TestMethod]
    public void Apply_ExcludeSubtasksNotSet_KeepsSubtasks()
    {
        var result = SubtaskFilter.Apply([TopLevel, Subtask], null);

        CollectionAssert.AreEqual(new[] { "top-level", "subtask" }, result.Select(x => x.Gid).ToArray());
    }

    [TestMethod]
    public void Apply_ExcludeSubtasksEnabled_FiltersTasksReturnedByEvents()
    {
        var tasks = new List<TaskDto>
        {
            new() { Gid = "top-level" },
            new() { Gid = "subtask", Parent = new AsanaEntity { Gid = "top-level" } }
        };

        var result = SubtaskFilter.Apply(tasks, true);

        CollectionAssert.AreEqual(new[] { "top-level" }, result.Select(x => x.Gid).ToArray());
    }
}
