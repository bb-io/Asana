using Apps.Asana.Models.Tasks.Requests;
using Tests.Asana.Base;

namespace Tests.Asana
{
    [TestClass]
    public class ActionTest : TestBase
    {
        [TestMethod]
        public async Task GetTask_IssSuccess()
        {
            var action = new Apps.Asana.Actions.TaskActions(InvocationContext);

            var result = await action.GetTask(new Apps.Asana.Models.Tasks.Requests.TaskRequest
            {
                TaskId = "1116269553954953",
                WorkspaceId= "11329706322538",
                ProjectId = "1112702425163154"
            });

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine(json);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task SearchTasks_IssSuccess()
        {
            var action = new Apps.Asana.Actions.TaskActions(InvocationContext);

            var result = await action.ListAllTasks(new Apps.Asana.Models.Projects.Requests.ProjectRequest { },new ListTasksRequest
            {
               
            });

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine(json);
            Assert.IsNotNull(result);
        }
    }
}
