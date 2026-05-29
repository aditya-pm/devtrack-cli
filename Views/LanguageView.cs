using Spectre.Console;

namespace DevTrackCLI.Views;

static class LanguageView
{
    public static void ShowLanguages(Dictionary<string, double> languagePercentages)
    {
        var topLanguages = languagePercentages.OrderByDescending(x => x.Value).Take(5);
        var table = new Table();

        table.Border(TableBorder.DoubleEdge);
        table.AddColumn("Language");
        table.AddColumn("Percentage");
        foreach (KeyValuePair<string, double> language in topLanguages)
        {
            table.AddRow(language.Key, language.Value.ToString("F2"));
        }

        AnsiConsole.Write(table);
    }
}