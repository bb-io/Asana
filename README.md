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

> **Note**: Changes can take 10 to 60 seconds to appear in [Asana API](https://developers.asana.com/reference/searchtasksforworkspace#user-content-eventual-consistency). Add a delay before searching for recently created or updated tasks.

Optional inputs are listed beneath the relevant action or event.

### Attachment

- **Search attachments**: Search attachments on a task, project, or project brief and output their IDs and names. The input labeled **Task ID** also accepts a project or project brief ID.
- **Get attachment**: Download an attachment by ID and output its file and details.
- **Delete attachment**: Delete the selected attachment.
- **Upload attachment**: Upload a file to the specified parent and output the attachment details.
   - **File name**: Override the uploaded file name. If omitted, the original file name is used.

### Custom fields

- **Get text custom field**: Output the text value of a custom field on the selected task.
- **Get people custom field**: Output the user IDs and names selected in a people custom field on the selected task.
- **Get date custom field**: Output the date value of a custom field on the selected task, including the time when available.
- **Get enum custom field**: Output the selected option name of an enum custom field on the selected task.
- **Get number custom field**: Output the number value of a custom field on the selected task.
- **Get multi-enum custom field**: Output the selected option names of a multi-enum custom field on the selected task.
- **Update text custom field**: Set the text value of a custom field on the selected task.
- **Update date custom field**: Set the date and time value of a custom field on the selected task.
- **Update number custom field**: Set the number value of a custom field on the selected task.
- **Update people custom field**: Replace the users in a people custom field on the selected task. Supports up to 20 users; an empty selection clears the field.
- **Update enum custom field**: Set the selected option of an enum custom field on the selected task.
- **Set multi-enum custom field**: Replace the selected options of a multi-enum custom field on the selected task using choice IDs, choice names, or both. At least one choice is required.
   - **Choice IDs**: Multiple option IDs to select. Can be combined with Choice names.
   - **Choice names**: Multiple option names to select. Names are matched without regard to capitalization or surrounding spaces; unknown or ambiguous names cause an error.

### Project

**Actions**

- **Get project**: Output the details of the selected project.
- **Delete project**: Delete the selected project.
- **Get project sections**: Output the IDs and names of sections in the selected project.
- **Get project status**: Output the details of a single project status update by its ID.
- **Get project status updates**: Output status updates for the selected project.
- **Create project**: Create a project for the specified team and output its details.
- **Search projects**: Search projects in the selected workspace, with optional team and archive filters, and output their IDs and names.
- **Update project**: Update the selected project and output its details.


### Section

- **Search sections**: Search sections in the selected project and output their IDs and names.
- **Get section**: Output the ID and name of the selected section.
- **Update section**: Rename or reposition the selected section and output its details.
- **Create section**: Create a named section in the selected project and output its details.
- **Delete section**: Delete the selected section.

### Tag

- **Search tags**: Search tags, optionally within a workspace, and output their IDs and names.
- **Get tag**: Output the details of the selected tag.
- **Update tag**: Update the name, color, or notes of the selected tag and output its details.
- **Create tag**: Create a tag in the selected workspace and output its details.
- **Delete tag**: Delete the selected tag.

### Task

- **Update task**: Update the name, notes, or assignee of the selected task, optionally move it to a section, and output its details.
- **Delete task**: Delete the selected task.
- **Get user tasks**: Output task IDs and names from the specified user task list (My Tasks).
- **Get tasks by tag**: Output the IDs and names of tasks with the selected tag.
- **Assign tag to task**: Add the selected tag to the selected task.
- **Get task**: Output the details of the selected task.
- **Create task**: Create a task with the supplied details, optionally as a subtask, and output its details.
- **Search tasks**: Search tasks in the selected project or section using assignment, tag, date, and custom field filters. Output matching task IDs and names. **Section ID** takes precedence over **Project ID**. Only tasks with direct membership in the selected project or section are included. Each "after" date must be no later than its corresponding "before" date.
   - **Tag**: Limit results to tasks with the selected tag.
   - **User task list ID**: Limit results to tasks in the specified My Tasks list.
   - **Text custom field ID**: Choose the text custom field used by the contains or equals filter.
   - **Text custom field contains**: Match text contained in the selected text custom field. Requires Text custom field ID.
   - **Text custom field equals**: Match an exact value in the selected text custom field. Requires Text custom field ID.
   - **Enum custom field ID**: Choose the enum custom field used by Enum option ID.
   - **Enum option ID**: Match the option with this ID in the selected enum custom field. Requires Enum custom field ID.
   - **Created after**: Include tasks created after this date and time.
   - **Created before**: Include tasks created before this date and time.
   - **Modified after**: Include tasks modified after this date and time.
   - **Modified before**: Include tasks modified before this date and time.
   - **Exclude subtasks**: Enable to output only top-level tasks. Disabled by default.
   - **Assignee ID**: Filter by assignee. For Create task and Update task, assign the task to the selected user.
   - **Section ID**: Limit results to the selected section. For Update task, move the task to the selected section.

> To populate the **Task ID** dropdown, specify **Workspace ID** and **Project ID**. You can also enter the project ID manually.

### User

- **Search users**: Search users, optionally by workspace or team, and output their IDs and names.
- **Get user**: Output the details of the selected user.
- **Get user's task list**: Output the ID and name of the selected user's My Tasks list in the selected workspace.
- **Get user's teams**: Output the IDs and names of teams the selected user belongs to in the selected workspace.

### Workspace

- **Search workspaces**: Search workspaces accessible to the connected user and output their IDs and names.
- **Get workspace**: Output the details of the selected workspace.

## Events

### Projects

- **On projects added**: Trigger when projects are added.
- **On projects changed**: Trigger when projects are changed.
- **On projects deleted**: Trigger when projects are deleted and output their IDs.
- **On projects removed**: Trigger when projects are removed.
- **On projects undeleted**: Trigger when projects are restored after deletion.

### Tasks

- **On tasks deleted**: Trigger when tasks are deleted and output their IDs.
- **On tasks removed**: Trigger when tasks are removed, optionally excluding subtasks.
- **On tasks undeleted**: Trigger when tasks are restored after deletion, optionally excluding subtasks.
- **On tasks added**: Trigger when tasks are added to the selected project, optionally filtered by section and with subtasks excluded.
   - **Section ID**: Only trigger for tasks added to the selected section. It's recommended to always set section ID for preventing new flights starting when task is moving between sections.

- **On tasks changed**: Trigger when tasks are changed, optionally limited to selected custom fields and with subtasks excluded.
   - **Custom fields**: Multiple custom fields to monitor. When set, only changes involving these fields trigger the event.


### Tags

- **On tags added**: Trigger when tags are added.
- **On tags changed**: Trigger when tags are changed.
- **On tags deleted**: Trigger when tags are deleted and output their IDs.
- **On tags undeleted**: Trigger when tags are restored after deletion.

### Sections

- **On sections added**: Trigger when sections are added.
- **On sections changed**: Trigger when sections are changed.
- **On sections deleted**: Trigger when sections are deleted and output their IDs.
- **On sections undeleted**: Trigger when sections are restored after deletion.

### Stories

- **On stories added**: Trigger when task activity entries are added.
- **On stories removed**: Trigger when task activity entries are removed.
- **On stories undeleted**: Trigger when task activity entries are restored after deletion.
- **On stories comments added**: Trigger when task comments are added.
- **On stories comments changed**: Trigger when task comments are edited.
- **On stories comments removed**: Trigger when task comments are removed.
- **On stories comments undeleted**: Trigger when task comments are restored after deletion.

### Workspaces

- **On workspaces changed**: Trigger when changes are made to workspaces.

### Goals

- **On goals added**: Trigger when goals are added.
- **On goals changed**: Trigger when goals are changed.
- **On goals removed**: Trigger when goals are removed.
- **On goals deleted**: Trigger when goals are deleted and output their IDs.
- **On goals undeleted**: Trigger when goals are restored after deletion.

### Project memberships

- **On project memberships added**: Trigger when project memberships are added.
- **On project memberships removed**: Trigger when project memberships are removed.

### Teams

- **On teams added**: Trigger when teams are added.
- **On teams changed**: Trigger when teams are changed.
- **On teams deleted**: Trigger when teams are deleted and output their IDs.
- **On team memberships added**: Trigger when team memberships are added.
- **On team memberships removed**: Trigger when team memberships are removed.

### Workspace memberships

- **On workspace memberships added**: Trigger when workspace memberships are added.
- **On workspace memberships removed**: Trigger when workspace memberships are removed.

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
