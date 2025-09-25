# Blackbird.io Asana

Blackbird is the new automation backbone for the language technology industry. Blackbird provides enterprise-scale automation and orchestration with a simple no-code/low-code platform. Blackbird enables ambitious organizations to identify, vet and automate as many processes as possible. Not just localization workflows, but any business and IT process. This repository represents an application that is deployable on Blackbird and usable inside the workflow editor.

## Introduction

<!-- begin docs -->

Asana is a project management and team collaboration platform. Its suite of features comprises task management, team communication, project planning tools, and a Timeline view. This Asana application primarily centers around project and task management.

## Connecting

1.  Navigate to apps and search for Asana. If you cannot find Asana then click _Add App_ in the top right corner, select Asana and add the app to your Blackbird environment.
2.  Click _Add Connection_.
3.  Name your connection for future reference e.g. 'My client'.
4.  Go to Asana developer console (https://app.asana.com/0/my-apps) and click _Create token_.
5.  Input its name, agree to Asana's terms
6.  Copy API token and paste it to the appropriate field in the BlackBird
7.  Click _Connect_.
8.  Confirm that the connection has appeared and the status is _Connected_.

## Actions

### Attachments

- **List attachments** returns all attachments of specified object. Object ID can stand for an ID of `Project`, `Project brief` or `Task`.
- **Get/upload/delete attachment**.

### Custom fields

- **Get text/date/enum custom field** returns the value of a custom field with text/date/enum type for a specific task.
- **Update text/date/enum custom field** sets the value of a custom field with text/date/enum type for a specific task.

### Projects

- **List projects** returns all projects. You can also specify `Workspace` or `Team` to retrieve projects of specific workspace or team. Also you can `Archived` variable to retrieve only archived projects or vice versa.
- **Get/create/update/delete project**.
- **Get project sections** returns all sections in the specified project.
- **Get project status** returns complete record for a single status update
- **Get project status updates** returns status update records for all updates on the project

### Sections

- **List sections** returns all sections of specified project.
- **Get/create/update/delete section**.

### Tags

- **List tags** returns all tags. You can also specify `Workspace` to retrieve tags of specific workspace.
- **Get/create/update/delete tag**.

### Tasks

- **List tasks** returns all tasks of specified `Workspace`, `Project`, `Tag`, `Section` or `User task list`.
- **Get/create/update/delete task**.
- **Get tasks from a user task list** returns list of tasks in a user’s My Tasks list.
- **Get tasks by tag** returns all tasks with the given tag.
- **Assign tag to task** adds a tag to a task.

> Whenever you use dropdowns to select `Task ID`, please specify `Workspace ID` and `Project ID` or `Manual project ID`

### Users

- **List users** returns all users. You can also specify `Workspace` or `Team` to retrieve users of specific workspace or team.
- **Get user** returns details of a specified user.
- **Get user's task list** returns full record for a user's task list.
- **Get user's teams** returns all teams to which the given user is assigned.

### Workspaces

- **List workspaces** returns all workspaces visible to the authorized user.
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
