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
            new ListTasksRequest());

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
            new ListTasksRequest());

        Assert.IsTrue(result.Tasks.Any());

        foreach (var task in result.Tasks)
        {
            var projectIds = await GetTaskProjectIds(task.Gid);
            CollectionAssert.Contains(projectIds, ProjectId, $"Task {task.Gid} is not in the project.");
        }
    }

    private async Task<List<string>> GetSectionTaskIds(string sectionId)
    {
        var request = new AsanaRequest($"/sections/{sectionId}/tasks", Method.Get, Creds);
        request.AddQueryParameter("opt_fields", "gid");

        var tasks = await new AsanaClient().Paginate<AsanaEntity>(request);
        return tasks.Select(x => x.Gid).ToList();
    }

    private async Task<List<string>> GetTaskProjectIds(string taskId)
    {
        var request = new AsanaRequest($"/tasks/{taskId}", Method.Get, Creds);
        request.AddQueryParameter("opt_fields", "projects.gid");

        var task = await new AsanaClient().ExecuteWithErrorHandling<TaskDto>(request);
        return task.Projects.Select(x => x.Gid).ToList();
    }
}
