using Apps.Asana.DataSourceHandlers;
using Apps.Asana.DataSourceHandlers.CustomFields;
using Apps.Asana.Models.CustomFields.Requests;
using Apps.Asana.Models.Workspaces.Requests;
using Tests.Asana.Base;

namespace Tests.Asana;

[TestClass]
public class DataHandlerTests : TestBase
{
    [TestMethod]
    public async Task ProjectDataHandler_IsSuccess()
    {
        var handler = new ProjectDataHandler(InvocationContext, new Apps.Asana.Models.Workspaces.Requests.WorkspaceRequest { WorkspaceId= "11329706322538" });
        var data = await handler.GetDataAsync(new Blackbird.Applications.Sdk.Common.Dynamic.DataSourceContext { SearchString=""}, CancellationToken.None);

        foreach (var item in data)
        {
            Console.WriteLine($"{item.Value} - {item.Key}");
        }

        Assert.IsNotNull(data);
    }

    [TestMethod]
    public async Task WorkspaceDataHandler_IsSuccess()
    {
        var handler = new WorkspaceDataHandler(InvocationContext);
        var data = await handler.GetDataAsync(new Blackbird.Applications.Sdk.Common.Dynamic.DataSourceContext { SearchString = "" }, CancellationToken.None);

        foreach (var item in data)
        {
            Console.WriteLine($"{item.Value} - {item.Key}");
        }

        Assert.IsNotNull(data);
    }

    [TestMethod]
    public async Task UserDataHandler_IsSuccess()
    {
        var handler = new UserDataHandler(InvocationContext, new Apps.Asana.Models.Workspaces.Requests.WorkspaceRequest { WorkspaceId = "11329706322538" });
        var data = await handler.GetDataAsync(new Blackbird.Applications.Sdk.Common.Dynamic.DataSourceContext { SearchString = "" }, CancellationToken.None);

        foreach (var item in data)
        {
            Console.WriteLine($"{item.Value} - {item.Key}");
        }

        Assert.IsNotNull(data);
    }

    [TestMethod]
    public async Task TeamDataHandler_IsSuccess()
    {
        var handler = new TeamDataHandler(InvocationContext, new Apps.Asana.Models.Workspaces.Requests.WorkspaceRequest { WorkspaceId = "11329706322538" });
        var data = await handler.GetDataAsync(new Blackbird.Applications.Sdk.Common.Dynamic.DataSourceContext { SearchString = "" }, CancellationToken.None);

        foreach (var item in data)
        {
            Console.WriteLine($"{item.Value} - {item.Key}");
        }

        Assert.IsNotNull(data);
    }


    [TestMethod]
    public async Task TaskDataHandler_IsSuccess()
    {
        var handler = new TaskDataHandler(InvocationContext, new Apps.Asana.Models.Tasks.Requests.TaskRequest { WorkspaceId = "11329706322538", ProjectId= "1112702425163154" });
        var data = await handler.GetDataAsync(new Blackbird.Applications.Sdk.Common.Dynamic.DataSourceContext { SearchString = "" }, CancellationToken.None);

        foreach (var item in data)
        {
            Console.WriteLine($"{item.Value} - {item.Key}");
        }

        Assert.IsNotNull(data);
    }

    [TestMethod]
    public async Task EnumCustomFieldDataHandler_IsSuccess()
    {
        var handler = new EnumCustomFieldDataHandler(InvocationContext, new EnumCustomFieldRequest { WorkspaceId = "11329706322538"});
        var data = await handler.GetDataAsync(new Blackbird.Applications.Sdk.Common.Dynamic.DataSourceContext { SearchString = "" }, CancellationToken.None);

        foreach (var item in data)
        {
            Console.WriteLine($"{item.Value} - {item.Key}");
        }

        Assert.IsNotNull(data);
    }
}
