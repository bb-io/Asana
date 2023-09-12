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

### Users

- **List users** returns all users. You can also specify `Workspace` or `Team` to retrieve users of specific workspace or team.
- **Get user** returns details of a specified user.
- **Get user's task list** returns full record for a user's task list.
- **Get user's teams** returns all teams to which the given user is assigned.

### Workspaces

- **List workspaces** returns all workspaces visible to the authorized user.
- **Get workspace** returns details of a specified workspace

## Events

- **On project changed** is triggered when any project was changed.

## Missing features

In the future we can add actions for:

- Portfolios
- Project briefs
- Teams

## Feedback

Feedback to our implementation of Asana is always very welcome. Reach out to us using the established channels or create an issue.

<!-- end docs -->
