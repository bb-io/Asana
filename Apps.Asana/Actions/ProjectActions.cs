using Apps.Asana.Dtos;
using Apps.Asana.Models.Projects.Requests;
using Apps.Asana.Models.Projects.Responses;
using Apps.Translate5;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using Blackbird.Applications.Sdk.Common.Actions;

namespace Apps.Asana.Actions
{
    [ActionList]
    public class ProjectActions
    {
        [Action("List projects", Description = "List projects")]
        public ListProjectsResponse ListAllProjects(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] ListProjectsRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/projects", Method.Get, authenticationCredentialsProviders);
            var projects = client.Get<ResponseWrapper<List<ProjectDto>>>(request); 
            return new ListProjectsResponse()
            {
                Projects = projects.Data
            };
        }

        [Action("Get project", Description = "Get project by Id")]
        public GetProjectResponse GetProject(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] GetProjectRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/projects/{input.ProjectId}", Method.Get, authenticationCredentialsProviders);
            var project = client.Get<ResponseWrapper<ProjectDto>>(request);
            return new GetProjectResponse()
            {
                GId = project.Data.GId,
                Name = project.Data.Name,
            };
        }

        [Action("Update project", Description = "Update project by Id")]
        public void UpdateProject(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] UpdateProjectRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/projects/{input.ProjectId}", Method.Put, authenticationCredentialsProviders);
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
        public void CreateProject(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] CreateProjectRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/projects", Method.Post, authenticationCredentialsProviders);
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
        public void DeleteProject(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] DeleteProjectRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/projects/{input.ProjectId}", Method.Delete, authenticationCredentialsProviders);
            client.Execute(request);
        }

        [Action("Get project sections", Description = "Get project sections")]
        public GetProjectSectionsResponse GetProjectSections(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] GetProjectSectionsRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/projects/{input.ProjectId}/sections", Method.Get, authenticationCredentialsProviders);
            var sections = client.Get<ResponseWrapper<List<SectionDto>>>(request);
            return new GetProjectSectionsResponse()
            {
                Sections = sections.Data
            };
        }

        [Action("Get project status", Description = "Get project status by Id")]
        public GetProjectStatusResponse GetProjectStatus(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] GetProjectStatusRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/project_statuses/{input.ProjectStatusId}", Method.Get, authenticationCredentialsProviders);
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
        public GetProjectStatusUpdatesResponse GetProjectStatusUpdates(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] GetProjectStatusUpdatesRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/status_updates?parent={input.ProjectId}", Method.Get, authenticationCredentialsProviders);
            var updates = client.Get<ResponseWrapper<List<ProjectStatusUpdateDto>>>(request);
            return new GetProjectStatusUpdatesResponse()
            {
                Updates = updates.Data
            };
        }

    }
}
