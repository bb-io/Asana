using Apps.Asana.Dtos.Base;

namespace Apps.Asana.Models.Tasks.Responses
{
    public class TaskMembershipDto
    {
        public AsanaEntity Project { get; set; }
        public AsanaEntity Section { get; set; }
    }
}
