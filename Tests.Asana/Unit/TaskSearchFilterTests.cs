using Apps.Asana.Dtos.Base;
using Apps.Asana.Models.Tasks.Responses;

namespace Tests.Asana.Unit;

[TestClass]
public class TaskSearchFilterTests
{
    private static TaskSearchResultDto Task(string gid, params (string Project, string Section)[] memberships) =>
        new()
        {
            Gid = gid,
            Memberships = memberships
                .Select(m => new TaskMembershipDto
                {
                    Project = new AsanaEntity { Gid = m.Project },
                    Section = new AsanaEntity { Gid = m.Section }
                })
                .ToList()
        };

    [TestMethod]
    public void ByMembership_SectionSpecified_KeepsOnlyTasksInThatSection()
    {
        var tasks = new[]
        {
            Task("in-section", ("p1", "s1")),
            Task("other-section", ("p1", "s2"))
        };

        var result = TaskSearchFilter.ByMembership(tasks, "p1", "s1");

        CollectionAssert.AreEqual(new[] { "in-section" }, result.Select(x => x.Gid).ToArray());
    }

    [TestMethod]
    public void ByMembership_SectionSpecified_DropsSubtasksWithoutMemberships()
    {
        var subtask = new TaskSearchResultDto { Gid = "subtask", Memberships = [] };

        var result = TaskSearchFilter.ByMembership([subtask, Task("in-section", ("p1", "s1"))], "p1", "s1");

        CollectionAssert.AreEqual(new[] { "in-section" }, result.Select(x => x.Gid).ToArray());
    }

    [TestMethod]
    public void ByMembership_NoSectionSpecified_KeepsOnlyTasksInThatProject()
    {
        var tasks = new[]
        {
            Task("in-project", ("p1", "s1")),
            Task("other-project", ("p2", "s9")),
            new TaskSearchResultDto { Gid = "subtask", Memberships = null }
        };

        var result = TaskSearchFilter.ByMembership(tasks, "p1", null);

        CollectionAssert.AreEqual(new[] { "in-project" }, result.Select(x => x.Gid).ToArray());
    }

    [TestMethod]
    public void ByMembership_NoProjectAndNoSection_KeepsEverything()
    {
        var tasks = new[] { Task("a", ("p1", "s1")), Task("b", ("p2", "s2")) };

        var result = TaskSearchFilter.ByMembership(tasks, null, null);

        CollectionAssert.AreEqual(new[] { "a", "b" }, result.Select(x => x.Gid).ToArray());
    }
}
