using Apps.Asana.Actions;
using Newtonsoft.Json;
using Tests.Asana.Base;

namespace Tests.Asana;

[TestClass]
public class CustomFieldsActionsTests : TestBase
{
    [TestMethod]
    public async Task GetEnumCustomField_ShouldWork()
    {
        var actions = new CustomFieldsActions(InvocationContext);
        
        var response = await actions.GetEnumCustomField(new()
        {
            TaskId = "1212209175879373",
            WorkspaceId = "9227808103590",
            CustomFieldId = "1208771758005734",
            ProjectId = "1206544924069863"
        });
        
        Assert.IsNotNull(response);
        Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
    }
}