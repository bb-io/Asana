using Apps.Asana.Dtos;
using Apps.Asana.Models.Projects.Requests;
using Apps.Asana.Models.Projects.Responses;
using Apps.Translate5;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Asana.Actions
{
    [ActionList]
    public class ProjectActions
    {
        [Action("List projects", Description = "List projects")]
        public ListProjectsResponse ListAllProjects(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] ListProjectsRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/projects", Method.Get, authenticationCredentialsProvider);
            var projects = client.Get<ResponseWrapper<List<ProjectDto>>>(request); 
            return new ListProjectsResponse()
            {
                Projects = projects.Data
            };
        }

        [Action("Get project", Description = "Get project by Id")]
        public GetProjectResponse GetProject(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] GetProjectRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/projects/{input.ProjectId}", Method.Get, authenticationCredentialsProvider);
            var project = client.Get<ResponseWrapper<ProjectDto>>(request);
            return new GetProjectResponse()
            {
                GId = project.Data.GId,
                Name = project.Data.Name,
            };
        }

        [Action("Update project", Description = "Update project by Id")]
        public void UpdateProject(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] UpdateProjectRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/projects/{input.ProjectId}", Method.Put, authenticationCredentialsProvider);
            request.AddJsonBody(new
            {
                data = new
                {
                    name = input.NewName
                }
            });
            client.Execute(request);
        }

        [Action("Create project", Description = "Create project")]
        public void CreateProject(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] CreateProjectRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/projects", Method.Post, authenticationCredentialsProvider);
            request.AddJsonBody(new
            {
                data = new
                {
                    name = input.ProjectName,
                    team = input.TeamId
                }
            });
            client.Execute(request);
        }

        [Action("Delete project", Description = "Delete project")]
        public void DeleteProject(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] DeleteProjectRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/projects/{input.ProjectId}", Method.Delete, authenticationCredentialsProvider);
            client.Execute(request);
        }

        [Action("Get project sections", Description = "Get project sections")]
        public GetProjectSectionsResponse GetProjectSections(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] GetProjectSectionsRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/projects/{input.ProjectId}/sections", Method.Get, authenticationCredentialsProvider);
            var sections = client.Get<ResponseWrapper<List<SectionDto>>>(request);
            return new GetProjectSectionsResponse()
            {
                Sections = sections.Data
            };
        }

        [Action("Get project status", Description = "Get project status by Id")]
        public GetProjectStatusResponse GetProjectStatus(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] GetProjectStatusRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/project_statuses/{input.ProjectStatusId}", Method.Get, authenticationCredentialsProvider);
            var statuses = client.Get<ResponseWrapper<ProjectStatusDto>>(request);
            return new GetProjectStatusResponse()
            {
                GId = statuses.Data.GId,
                Color = statuses.Data.Color,
                Text = statuses.Data.Text,
                Title = statuses.Data.Title
            };
        }

        [Action("Get project status updates", Description = "Get project status updates")]
        public GetProjectStatusUpdatesResponse GetProjectStatusUpdates(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] GetProjectStatusUpdatesRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/status_updates?parent={input.ProjectId}", Method.Get, authenticationCredentialsProvider);
            var updates = client.Get<ResponseWrapper<List<ProjectStatusUpdateDto>>>(request);
            return new GetProjectStatusUpdatesResponse()
            {
                Updates = updates.Data
            };
        }

    }
}
