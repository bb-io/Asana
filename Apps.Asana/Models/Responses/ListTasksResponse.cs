using Apps.Asana.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Models.Responses
{
    public class ListTasksResponse
    {
        public IEnumerable<TaskDto> Tasks { get; set; }
    }
}
