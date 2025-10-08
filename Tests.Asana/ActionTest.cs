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

            var result = await action.ListAllTasks(new Apps.Asana.Models.Projects.Requests.ProjectRequest { WorkspaceId = "11329706322538",  IncludeArchived=false }, 
            new ListTasksRequest
            {
                EnumOptionId= "1203932706107047",
                CustomFieldId = "1203932706107045"
            });

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine(json);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetMultiEnumCustomField_IssSuccess()
        {
            var action = new Apps.Asana.Actions.CustomFieldsActions(InvocationContext);

            var result = await action.GetMultiEnumCustomField(
                new Apps.Asana.Models.CustomFields.Requests.MultipleCustomFieldRequest { WorkspaceId = "11329706322538",
                TaskId= "1199694846659121",
                    CustomFieldId = "1201483098611179"
                });

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine(json);
            Assert.IsNotNull(result);
        }
    }
}
