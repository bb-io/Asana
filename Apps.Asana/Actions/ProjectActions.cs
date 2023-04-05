﻿using Apps.Asana.Dtos;
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
            var request = new AsanaRequest($"https://app.asana.com/api/1.0/projects", Method.Get, authenticationCredentialsProvider);
            dynamic content = JsonConvert.DeserializeObject(client.Get(request).Content);
            JArray projectsArray = content.data;
            var projects = projectsArray.ToObject<List<ProjectDto>>();
            return new ListProjectsResponse()
            {
                Projects = projects
            };
        }

        [Action("Get project", Description = "Get project by Id")]
        public GetProjectResponse GetProject(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] GetProjectRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"https://app.asana.com/api/1.0/projects/{input.ProjectId}", Method.Get, authenticationCredentialsProvider);
            dynamic content = JsonConvert.DeserializeObject(client.Get(request).Content);
            JObject projectObj = content.data;
            var project = projectObj.ToObject<ProjectDto>();
            return new GetProjectResponse()
            {
                GId = project.GId,
                Name = project.Name,
            };
        }

        [Action("Update project", Description = "Update project by Id")]
        public void UpdateProject(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] UpdateProjectRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"https://app.asana.com/api/1.0/projects/{input.ProjectId}", Method.Put, authenticationCredentialsProvider);
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
            var request = new AsanaRequest($"https://app.asana.com/api/1.0/projects", Method.Post, authenticationCredentialsProvider);
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
            var request = new AsanaRequest($"https://app.asana.com/api/1.0/projects/{input.ProjectId}", Method.Delete, authenticationCredentialsProvider);
            client.Execute(request);
        }
    }
}
