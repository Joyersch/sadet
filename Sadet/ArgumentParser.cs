using Sadet.Actions;
using Sadet.Steam.DataObjects;
namespace Sadet;

public class ArgumentParser
{
    private readonly TextWriter _log;
    private readonly Library _library;

    public ArgumentParser(TextWriter log, Library library)
    {
        _log = log;
        _library = library;
    }

    public async Task ParseAsync(string[] args)
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
                case "la":
                case "-load-api":
                    if (args.Length - i < 3)
                    {
                        _log.WriteLine("missing parameters after {0}", args[i]);
                        return;
                    }

                    i += 2; // moved argument pointer 2 ahead since it expects 2 params for this function
                    
                    actions.Add(new WebApiAction(_log, _library, args[cache + 1], args[cache + 2]));
                    break;
                case "lf":
                case "-load-file":
                    if (args.Length - i < 2)
                        return;
                    
                    i += 1; // moved argument pointer 1 ahead since it expects 1 params for this function

                    actions.Add(new ReadFromFileAction(_library, args[cache + 1]));
                    break;
                case "le":
                case "-load-external":
                    if (args.Length - i < 2)
                        return;
                    
                    i += 1;// moved argument pointer 1 ahead since it expects 1 params for this function
                    
                    actions.Add(new ImportGamesAction(_library, args[cache + 1]));
                    break;
                case "d":
                case "-dump":
                    if (args.Length - i < 1)
                    {
                        _log.WriteLine("missing parameters after {0}", args[i]);
                        return;
                    }

                    i += 1; // moved argument pointer 1 ahead since it expects 1 params for this function
                    
                    actions.Add(new WriteToFileAction(_library, args[cache + 1]));
                    break;
                case "ds":
                case "-dataset":

                    if (args.Length - i < 1)
                    {
                        _log.WriteLine("missing parameters after {0}", args[i]);
                        return;
                    }

                    i += 1; // moved argument pointer 1 ahead since it expects 1 params for this function
                    
                    actions.Add(new DatasheetAction(_log, _library, args[cache + 1]));
                    break;
                default:
                    _log.WriteLine("Unrecognized argument: {0}", args[i]);
                    return;
            }
        }

        foreach (var action in actions)
            await action.ExecuteAsync();
    }
}