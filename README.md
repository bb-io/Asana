# Blackbird.io Asana

Blackbird is the new automation backbone for the language technology industry. Blackbird provides enterprise-scale automation and orchestration with a simple no-code/low-code platform. Blackbird enables ambitious organizations to identify, vet and automate as many processes as possible. Not just localization workflows, but any business and IT process. This repository represents an application that is deployable on Blackbird and usable inside the workflow editor.

## Introduction

<!-- begin docs -->

Asana is a project management and team collaboration platform. Its suite of features comprises task management, team communication, project planning tools, and a Timeline view. This Asana application primarily centers around project and task management.

## Connecting

Our app supports 3 types of authentication: Personal Access Token and OAuth 2.0, and OAuth 2.0 with your own app. Choose whatever suits your needs.

### Personal access token (PAT) authentication

1.  Navigate to apps and search for Asana. If you cannot find Asana then click _Add App_ in the top right corner, select Asana and add the app to your Blackbird environment.
2.  Click _Add Connection_.
3.  Name your connection for future reference e.g. 'My client'.
4.  Go to Asana developer console (https://app.asana.com/0/my-apps) and click _Create token_.
5.  Input its name and agree to Asana's terms.
6.  Copy the API token and paste it into the appropriate field in Blackbird.
7.  Click _Connect_.
8.  Confirm that the connection has appeared and the status is _Connected_.

### OAuth 2.0 authentication

1.  Navigate to apps and search for Asana. If you cannot find Asana then click _Add App_ in the top right corner, select Asana and add the app to your Blackbird environment.
2. Click _Add Connection_.
3. Name your connection for future reference e.g. 'My client'.
4. Click _Connect_.
5. You will be redirected to Asana's website to authorize Blackbird to access your data.
6. After authorization, you will be redirected back to Blackbird.
7. Confirm that the connection has appeared and the status is _Connected_.

### OAuth 2.0 with your own app

In Asana you can limit the use of third-party apps and allow only your own OAuth apps created from the developer console (https://app.asana.com/0/my-apps). In this case, you will not be able to use our default OAuth authentication method and will have to create your own OAuth app in Asana and use its credentials to connect.

1. Go to Asana developer console (https://app.asana.com/0/my-apps) and click _Create new app_.
2. Input all mandatory fields and add Redirect URL: `https://bridge-dev.blackbird.io/api/AuthorizationCode`
3. Select the needed scopes for your app. They depend on your use case, so specify them based on the actions you are going to use.
4. After creating the app, copy its Client ID and Client Secret.
5. Go to Blackbird and start the authentication process.

Blackbird side authentication steps:

1. Navigate to apps and search for Asana. If you cannot find Asana then click _Add App_ in the top right corner, select Asana and add the app to your Blackbird environment.
2. Click _Add Connection_.
3. Name your connection for future reference e.g. 'My client'.
4. Select `OAuth 2.0 (own app)` authentication type.
5. Input the Client ID and Client Secret you received from Asana developer console.
6. Input the Scope(s) you specified for your app in Asana developer console. They should be separated by spaces, e.g. `attachments:read custom_fields:read projects:read`.
7. Click _Connect_.
8. You will be redirected to Asana's website to authorize Blackbird to access your data.
9. After authorization, you will be redirected back to Blackbird.
10. Confirm that the connection has appeared and the status is _Connected_.

## Actions

> **Note**: When you create or update items in Asana, there is a short delay before those changes are visible in our app. [Asana requires 10 to 60 seconds](https://developers.asana.com/reference/searchtasksforworkspace#user-content-eventual-consistency) to index new data and make it available via their API. If you don't see your recent changes immediately, please wait a minute and try again.

### Attachments

- **Search attachments** returns all attachments of the specified object. Object ID can stand for an ID of `Project`, `Project brief` or `Task`.
- **Get/upload/delete attachment**.

### Custom fields

- **Get text/date/enum/people custom field** returns the value of a custom field with text/date/enum type for a specific task.
- **Update text/date/enum/people custom field** sets the value of a custom field with text/date/enum type for a specific task.
- **Get multi-enum custom field** gets values of a custom field with multi-enum type (returns option names).

### Projects

- **Search projects** returns all projects. You can also specify `Workspace` or `Team` to retrieve projects of a specific workspace or team. You can also use the `Archived` variable to retrieve only archived projects or vice versa.
- **Get/create/update/delete project**.
- **Get project sections** returns all sections in the specified project.
- **Get project status** returns a complete record for a single status update.
- **Get project status updates** returns status update records for all updates on the project

### Sections

- **Search sections** returns all sections of specified project.
- **Get/create/update/delete section**.

### Tags

- **Search tags** returns all tags. You can also specify `Workspace` to retrieve tags of specific workspace.
- **Get/create/update/delete tag**.

### Tasks

- **Search tasks** returns all tasks of the specified `Workspace`, `Project`, `Tag`, `Section` or `User task list`.
- **Get/create/update/delete task**.
- **Get tasks from a user task list** returns list of tasks in a user’s My Tasks list.
- **Get tasks by tag** returns all tasks with the given tag.
- **Assign tag to task** adds a tag to a task.

> Whenever you use dropdowns to select `Task ID`, please specify `Workspace ID` and `Project ID` or `Manual project ID`


### Users

- **Search users** returns all users. You can also specify `Workspace` or `Team` to retrieve users of specific workspace or team.
- **Get user** returns details of a specified user.
- **Get user's task list** returns full record for a user's task list.
- **Get user's teams** returns all teams to which the given user is assigned.

### Workspaces

- **Search workspaces** returns all workspaces visible to the authorized user.
- **Get workspace** returns details of a specified workspace

## Events

### Projects 
- **On projects added** is triggered when projects are added.
- **On projects changed** is triggered when projects are changed.
- **On projects deleted** is triggered when projects are deleted.
- **On projects removed** is triggered when projects are removed.
- **On projects undeleted** is triggered when projects are undeleted.

### Tasks

- **On tasks added** is triggered when tasks are added.
- **On tasks changed** is triggered when tasks are changed.
- **On tasks deleted** is triggered when tasks are deleted.
- **On tasks removed** is triggered when tasks are removed.
- **On tasks undeleted** is triggered when tasks are undeleted.

### Tags

- **On tags added** is triggered when tags are added.
- **On tags changed** is triggered when tags are changed.
- **On tags deleted** is triggered when tags are deleted.
- **On tags undeleted** is triggered when tags are undeleted.

### Sections

- **On sections added** is triggered when sections are added.
- **On sections changed** is triggered when sections are changed.
- **On sections deleted** is triggered when sections are deleted.
- **On sections undeleted** is triggered when sections are undeleted.

### Stories

- **On stories added** is triggered when stories are added.
- **On stories removed** is triggered when stories are removed.
- **On stories undeleted** is triggered when stories are undeleted.
- **On stories comments added** is triggered when comments are added to stories.
- **On stories comments changed** is triggered when comments on stories are changed.
- **On stories comments removed** is triggered when comments are removed from stories.
- **On stories comments undeleted** is triggered when comments on stories are undeleted.

### Workspaces

- **On workspaces changed** is triggered when changes are made to workspaces.

### Goals

- **On goals added** is triggered when goals are added.
- **On goals changed** is triggered when goals are changed.
- **On goals removed** is triggered when goals are removed.
- **On goals deleted** is triggered when goals are deleted.
- **On goals undeleted** is triggered when goals are undeleted.

### Project memberships

- **On project memberships added** is triggered when project memberships are added.
- **On project memberships removed** is triggered when project memberships are removed.

### Teams

- **On teams added** is triggered when teams are added.
- **On teams changed** is triggered when teams are changed.
- **On teams deleted** is triggered when teams are deleted.
- **On team memberships added** is triggered when team memberships are added.
- **On team memberships removed** is triggered when team memberships are removed.

### Workspace memberships

- **On workspace memberships added** is triggered when workspace memberships are added.
- **On workspace memberships removed** is triggered when workspace memberships are removed.

## Example

![Asana example](/README/images/example.png)

In this example, we have a workflow that triggers when tasks are added to a project. The workflow then retrieves the task details and sends them to a Slack channel.

## Missing features

In the future we can add actions for:

- Portfolios
- Project briefs
- Teams

## Feedback

Feedback to our implementation of Asana is always very welcome. Reach out to us using the established channels or create an issue.

<!-- end docs -->
