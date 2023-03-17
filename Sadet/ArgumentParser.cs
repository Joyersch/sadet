using Sadet.Actions;
using Sadet.Actions.DataActions;
using Sadet.Actions.DataActions.ChangeDataActions;
using Sadet.Actions.DataActions.PrintDataActions;
using Sadet.Steam.DataObjects;

namespace Sadet;

public class ArgumentParser
{
    private readonly TextWriter _log;
    private readonly Library _library;
    private readonly FormatSettings _formatSettings;
    private WebApiConnection _webApiConnection;

    public ArgumentParser(TextWriter log, Library library)
    {
        _log = log;
        _library = library;
        _formatSettings = new FormatSettings();
    }

    public async Task<List<IAction>> ParseAsync(string[] args)
    {
        List<IAction> actions = new List<IAction>();
        for (int i = 0; i < args.Length; i++)
        {
            int cache = i; // to cache the i index in case multiple arguments are given the the procedures

            if (!args[i].StartsWith("-"))
                continue;

            // code below will only be run if args[i] start with '-'
            switch (args[i][1..].ToLower()) // Substring(1)
            {
                case "h":
                case "-help":
                    actions.Add(new HelpAction(_log));
                    break;
                case "ac":
                case "-api-connection":
                    if (args.Length - i < 3)
                    {
                        _log.WriteLine("missing parameters after {0}", args[i]);
                        return new List<IAction>();
                    }

                    i += 2; // moved argument pointer 2 ahead since it expects 2 params for this function

                    _webApiConnection = new WebApiConnection(args[cache + 1], args[cache + 2]);

                    break;
                case "lf":
                case "-load-file":
                    if (args.Length - i < 2)
                        return new List<IAction>();

                    i += 1; // moved argument pointer 1 ahead since it expects 1 params for this function

                    actions.Add(new ReadFromFileAction(_library, args[cache + 1]));
                    break;
                case "le":
                case "-load-external":
                    if (args.Length - i < 2)
                        return new List<IAction>();

                    i += 1; // moved argument pointer 1 ahead since it expects 1 params for this function

                    actions.Add(new ImportGamesAction(_library, args[cache + 1]));
                    break;
                case "d":
                case "-dump":
                    if (args.Length - i < 1)
                    {
                        _log.WriteLine("missing parameters after {0}", args[i]);
                        return new List<IAction>();
                    }

                    i += 1; // moved argument pointer 1 ahead since it expects 1 params for this function

                    actions.Add(new WriteToFileAction(_library, args[cache + 1]));
                    break;
                case "f":
                case "-format":
                    if (args.Length - i < 1)
                    {
                        _log.WriteLine("missing parameters after {0}", args[i]);
                        return new List<IAction>();
                    }

                    i += 1; // moved argument pointer 1 ahead since it expects 1 params for this function

                    actions.Add(new SetFormatAction(_formatSettings, args[cache + 1]));
                    break;
                case "r":
                case "-range":
                    if (args.Length - i < 2)
                    {
                        _log.WriteLine("missing parameters after {0}", args[i]);
                        return new List<IAction>();
                    }

                    i += 2; // moved argument pointer 2 ahead since it expects 2 params for this function

                    if (!int.TryParse(args[cache + 1], out int min))
                    {
                        _log.WriteLine("Argument for minimum \"{0}\" is not a number!", args[cache + 1]);
                        return new List<IAction>();
                    }

                    if (!int.TryParse(args[cache + 2], out int max))
                    {
                        _log.WriteLine("Argument for maximum \"{0}\" is not a number!", args[cache + 2]);
                        return new List<IAction>();
                    }

                    actions.Add(new SetRangeAction(_formatSettings, min, max));
                    break;
                case "pca":
                case "-print-completion-average":

                    actions.Add(new PrintAverageCompletionAction(_log, _library, _formatSettings));
                    break;
                case "fou":
                case "-filter-only-unfinished":

                    actions.Add(new UnfinishedGamesAction(_library));
                    break;
                case "fos":
                case "-filter-only-started":

                    actions.Add(new StartedGamesAction(_library));
                    break;
                case "pc":
                case "-print-completion":

                    actions.Add(new PrintCompletionAction(_log, _library, _formatSettings));
                    break;
                case "pd":
                case "-print-difficulty":

                    actions.Add(new PrintDifficultyAction(_log, _library, _formatSettings));
                    break;
                case "ptg":
                case "-print-total-games":

                    actions.Add(new PrintTotalGamesAction(_log, _library, _formatSettings));
                    break;
                case "pta":
                case "-print-total-achievements":

                    actions.Add(new PrintTotalAchievementsAction(_log, _library, _formatSettings));
                    break;
                case "s":
                case "-sort":
                    if (args.Length - i < 2)
                    {
                        _log.WriteLine("missing parameters after {0}", args[i]);
                        return new List<IAction>();
                    }

                    i += 2; // moved argument pointer 2 ahead since it expects 2 params for this function

                    bool completion = false, asc = false;

                    switch (args[cache + 1]) // check extra argument 1
                    {
                        case "c":
                        case "comp":
                        case "completion":
                            completion = true;
                            break;
                        case "d":
                        case "dif":
                        case "difficulty":
                            break; // value is already false;
                        default:
                            _log.WriteLine("Unknown argument {0} for sorting!", args[cache + 1]);
                            return new List<IAction>();
                    }

                    switch (args[cache + 2]) // check extra argument 2
                    {
                        case "asc":
                        case "ascending":
                        case "a":
                            asc = true;
                            break;
                        case "desc":
                        case "descending":
                        case "d":
                            break; // value is already false;
                        default:
                            _log.WriteLine("Unknown argument {0} for sorting!", args[cache + 1]);
                            return new List<IAction>();
                    }

                    actions.Add(new SortGamesAction(_library, completion, asc));
                    break;
                case "lag":
                case "-load-api-games":
                    if (_webApiConnection is null)
                    {
                        _log.WriteLine($"Parameter {args[i]} required an api connection to be provided");
                        return new List<IAction>();
                    }

                    actions.Add(new AllGamesFromProfileApiAction(_log, _library, _webApiConnection));
                    break;
                case "lfg":
                case "-load-file-games":
                    if (_webApiConnection is null)
                    {
                        _log.WriteLine($"Parameter {args[i]} required an api connection to be provided");
                        return new List<IAction>();
                    }

                    if (args.Length - i < 1)
                    {
                        _log.WriteLine("missing parameters after {0}", args[i]);
                        return new List<IAction>();
                    }

                    i += 1; // moved argument pointer 1 ahead since it expects 1 params for this function

                    actions.Add(new AllGamesFromFileApiAction(_log, _library, _webApiConnection, args[cache + 1]));
                    break;
                case "ar":
                case "-api-retries":
                    if (_webApiConnection is null)
                    {
                        _log.WriteLine($"Parameter {args[i]} required an api connection to be provided");
                        return new List<IAction>();
                    }

                    if (args.Length - i < 1)
                    {
                        _log.WriteLine("missing parameters after {0}", args[i]);
                        return new List<IAction>();
                    }

                    i += 1; // moved argument pointer 1 ahead since it expects 1 params for this function

                    actions.Add(new SetRetryAction(_webApiConnection, args[cache + 1]));
                    break;
                default:
                    _log.WriteLine("Unrecognized argument: {0}", args[i]);
                    return new List<IAction>();
            }
        }

        return actions;
    }
}