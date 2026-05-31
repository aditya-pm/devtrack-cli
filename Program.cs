using DevTrackCLI.Views;
using DevTrackCLI.Applications;

class Program
{
    static async Task Main(string[] args)
    {
        GithubApplication githubApp = new();
        TaskApplication taskApp = new();

        bool running = true;

        while (running)
        {
            string choice = MainMenuView.Show();

            switch (choice)
            {
                case "GitHub Repository Summary":
                    await githubApp.RunAsync();
                    break;

                case "Task Management":
                    await taskApp.RunAsync();
                    break;

                case "Exit":
                    running = false;
                    break;
            }
        }
    }
}
