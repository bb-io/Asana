using Apps.Asana.Actions;
using Apps.Asana.Models.Attachments.Requests;
using Apps.Asana.Models.CustomFields.Requests;
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
                WorkspaceId= "11329706322538"
            });

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine(json);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task SearchTasks_IssSuccess()
        {
            var action = new Apps.Asana.Actions.TaskActions(InvocationContext);

            var result = await action.ListAllTasks(new Apps.Asana.Models.Sections.Requests.SectionRequest { WorkspaceId = "11329706322538",  IncludeArchived=false }, 
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

        [TestMethod]
        public async Task SearchAttachment_IssSuccess()
        {
            var action = new AttachmentActions(InvocationContext, FileManager);

            var result = await action.ListAttachments(
                new ListAttachmentsRequest
                {
                   ObjectId = "1212502671744490",
                    
                });

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine(json);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetAttachment_IssSuccess()
        {
            var action = new AttachmentActions(InvocationContext, FileManager);

            var result = await action.GetAttachment(
                new AttachmentRequest
                {
                    AttachmentId = "1212464178656683",
                });

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine(json);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task UpdatePeopleCustomField_IssSuccess()
        {
            var action = new CustomFieldsActions(InvocationContext);

            await action.UpdatePeopleCustomField(
                new PeopleCustomFieldRequest
                {
                    TaskId = "1212502671744490",
                    ProjectId= "1212502671744475",
                    CustomFieldId = "1212464178656695",
                }, new List<string> { "1212502676955414" });

            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task GetPeopleCustomField_IssSuccess()
        {
            var action = new CustomFieldsActions(InvocationContext);

            var result = await action.GetPeopleCustomField(
                new PeopleCustomFieldRequest
                {
                    ProjectId= "1212502671744475",
                    TaskId = "1212502671744490",
                    CustomFieldId = "1212464178656695",
                });

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine(json);
            Assert.IsNotNull(result);
        }
    }
}
