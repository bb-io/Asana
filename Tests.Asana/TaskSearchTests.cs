using Apps.Asana.Actions;
using Apps.Asana.Api;
using Apps.Asana.Dtos;
using Apps.Asana.Dtos.Base;
using Apps.Asana.Models.Sections.Requests;
using Apps.Asana.Models.Tasks.Requests;
using RestSharp;
using Tests.Asana.Base;

namespace Tests.Asana;

[TestClass]
public class TaskSearchTests : TestBase
{
    private const string WorkspaceId = "11357835420802";
    private const string ProjectId = "1212686181749273";
    private const string SectionId = "1212686181749277";

    [TestMethod]
    public async Task ListAllTasks_WithSection_ReturnsOnlyTasksCurrentlyInThatSection()
    {
        var expected = await GetSectionTaskIds(SectionId);

        var result = await new TaskActions(InvocationContext).ListAllTasks(
            new SectionRequest { WorkspaceId = WorkspaceId, ProjectId = ProjectId, SectionId = SectionId },
            new ListTasksRequest(),
            new SubtaskFilterRequest());

        var returned = result.Tasks.Select(x => x.Gid).ToList();
        var unexpected = returned.Except(expected).ToList();

        Assert.AreEqual(0, unexpected.Count, $"Tasks not in the section: {string.Join(", ", unexpected)}");
        CollectionAssert.AreEquivalent(expected, returned);
    }

    [TestMethod]
    public async Task ListAllTasks_WithoutSection_ReturnsOnlyTasksInThatProject()
    {
        var result = await new TaskActions(InvocationContext).ListAllTasks(
            new SectionRequest { WorkspaceId = WorkspaceId, ProjectId = ProjectId },
            new ListTasksRequest(),
            new SubtaskFilterRequest());

        Assert.IsTrue(result.Tasks.Any());

        foreach (var task in result.Tasks)
        {
            var projectIds = await GetTaskProjectIds(task.Gid);
            CollectionAssert.Contains(projectIds, ProjectId, $"Task {task.Gid} is not in the project.");
        }
    }

    [TestMethod]
    public async Task ListAllTasks_ExcludeSubtasksEnabled_ReturnsNoSubtasks()
    {
        var scope = new SectionRequest { WorkspaceId = WorkspaceId };
        var input = new ListTasksRequest
        {
            CreatedAfter = new DateTime(2026, 9, 19, 0, 0, 0, DateTimeKind.Utc),
            CreatedBefore = new DateTime(2026, 9, 21, 0, 0, 0, DateTimeKind.Utc)
        };

        var withSubtasks = await new TaskActions(InvocationContext)
            .ListAllTasks(scope, input, new SubtaskFilterRequest());
        var withoutSubtasks = await new TaskActions(InvocationContext)
            .ListAllTasks(scope, input, new SubtaskFilterRequest { ExcludeSubtasks = true });

        Assert.IsTrue(withSubtasks.Tasks.Any(), "No tasks were found in the test window.");
        Assert.IsTrue(withoutSubtasks.Tasks.Count() < withSubtasks.Tasks.Count(), "No subtasks were excluded.");

        foreach (var task in withoutSubtasks.Tasks)
            Assert.IsNull(await GetTaskParentId(task.Gid), $"Task {task.Gid} is a subtask.");
    }

    private async Task<List<string>> GetSectionTaskIds(string sectionId)
    {
        var request = new AsanaRequest($"/sections/{sectionId}/tasks", Method.Get, Creds);
        request.AddQueryParameter("opt_fields", "gid");

        var tasks = await new AsanaClient().Paginate<AsanaEntity>(request);
        return tasks.Select(x => x.Gid).ToList();
    }

    private async Task<string?> GetTaskParentId(string taskId)
    {
        var request = new AsanaRequest($"/tasks/{taskId}", Method.Get, Creds);
        request.AddQueryParameter("opt_fields", "parent.gid");

        var task = await new AsanaClient().ExecuteWithErrorHandling<TaskDto>(request);
        return task.Parent?.Gid;
    }

    private async Task<List<string>> GetTaskProjectIds(string taskId)
    {
        var request = new AsanaRequest($"/tasks/{taskId}", Method.Get, Creds);
        request.AddQueryParameter("opt_fields", "projects.gid");

        var task = await new AsanaClient().ExecuteWithErrorHandling<TaskDto>(request);
        return task.Projects.Select(x => x.Gid).ToList();
    }
}
