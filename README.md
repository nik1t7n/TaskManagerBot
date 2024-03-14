# Task Manager Bot

Task Manager Bot is a Telegram bot that helps users manage their tasks efficiently. Users can perform various task management operations such as viewing all tasks, adding new tasks, updating tasks, deleting tasks, and more.

## Features

- View all tasks
- View a specific task by its ID
- Add a new task
- Update a task by its ID
- Delete a task by its ID
- Get information about available commands and usage

## Setup Instructions

1. Clone the repository to your local machine.
2. Install the necessary dependencies by running `dotnet restore`.
3. Create a `sensitive.env` file and add your Telegram bot API key as `API_KEY`.
4. Update the `appsettings.json` file with your database connection string.
5. Run the database migrations using FluentMigrator by executing the migration scripts.
6. Build the project using `dotnet build`.
7. Start the bot by running the application.

## Usage

- To view all tasks: `/alltasks`
- To view a specific task by its ID: `/taskbyid {id}`
- To add a new task: `/addtask {description}`
- To update a task by its ID: `/updatetask {id} {changed_description}`
- To delete a task by its ID: `/deletetask {id}`
- To get information about available commands and usage: `/info`

## Note

- Ensure you provide correct inputs when using commands that require IDs.
- Use commands without braces (e.g., `/taskbyid 5`) for better results.

Enjoy managing your tasks with Task Manager Bot!

## Contributors

- [Your Name]
- [Your Email]

Feel free to contribute to this project by submitting pull requests or reporting issues.

Thank you for using Task Manager Bot!
