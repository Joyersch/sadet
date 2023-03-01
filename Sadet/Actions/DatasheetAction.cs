using Sadet.Steam.DataObjects;

namespace Sadet.Actions;

public class DatasheetAction : IAction
{
    private readonly TextWriter _log;
    private readonly Library _library;
    private readonly IEnumerable<Parameter> _parameters;

    public DatasheetAction(TextWriter log, Library library, string parameters)
    {
        _log = log;
        _library = library;
        _parameters = parameters.ToCharArray().Select(Parse);
    }

    public async Task ExecuteAsync()
    {
        foreach (var param in _parameters)
        {
            switch (param)
            {
                case Parameter.PrintTotalCompletion:
                    Console.WriteLine($"AverageCompletion={_library.Games.TotalCompletion()}");
                    break;
                case Parameter.FilterOnlyUnfished:
                    _library.Games.Unfinished();
                    break;
                case Parameter.FilterOnlyStarted:
                    _library.Games.Started();
                    break;
                case Parameter.PrintCompletion:
                    _library.Games.ForEach(n => _log.WriteLine("{0}={1}", n.Name, n.Completion));
                    break;
                case Parameter.PrintDifficulty:
                    _library.Games.ForEach(n => _log.WriteLine("{0}={1}", n.Name, n.Difficulty));
                    break;
                case Parameter.SortByCompletionAsc:
                    _library.Games.SortedByCompletion(true);
                    break;
                case Parameter.SortByCompletionDes:
                    _library.Games.SortedByCompletion(false);
                    break;
                case Parameter.SortByDifficultyAsc:
                    _library.Games.SortedByDifficulty(true);
                    break;
                case Parameter.SortByDifficultyDes:
                    _library.Games.SortedByDifficulty(false);
                    break;
                case Parameter.PrintTotalGames:
                    _log.WriteLine("TotalGames={0}", _library.Games.Count);
                    break;
                case Parameter.PrintTotalAchievements:
                    _log.WriteLine("TotalAchievements={0}",
                        _library.Games.Sum(g => g.Achievements.Count(a => a.Achieved)));
                    break;
            }
        }
    }

    private static Parameter Parse(char param)
        => param switch
        {
            'c' => Parameter.PrintTotalCompletion,
            'u' => Parameter.FilterOnlyUnfished,
            'a' => Parameter.FilterOnlyStarted,
            'y' => Parameter.PrintCompletion,
            'x' => Parameter.PrintDifficulty,
            'g' => Parameter.SortByCompletionAsc,
            'h' => Parameter.SortByCompletionDes,
            'i' => Parameter.SortByDifficultyAsc,
            'j' => Parameter.SortByDifficultyDes,
            'z' => Parameter.PrintTotalGames,
            'r' => Parameter.PrintTotalAchievements,
            _ => Parameter.Unknown
        };

    private enum Parameter
    {
        PrintTotalCompletion,
        FilterOnlyUnfished,
        FilterOnlyStarted,
        PrintCompletion,
        PrintDifficulty,
        SortByCompletionAsc,
        SortByCompletionDes,
        SortByDifficultyAsc,
        SortByDifficultyDes,
        PrintTotalGames,
        PrintTotalAchievements,
        Unknown
    }
}