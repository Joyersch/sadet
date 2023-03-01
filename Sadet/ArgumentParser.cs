using Sadet.Actions;
using Sadet.Steam.DataObjects;
using System;
using Microsoft.Extensions.Logging;

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
                    HelpAction helpAction = new HelpAction(_log);
                    helpAction.ExecuteAsync();
                    break;
                case "la":
                case "-load-api":
                    if (args.Length - i < 3)
                    {
                        _log.WriteLine("missing parameters after {0}", args[i]);
                        return;
                    }

                    i += 2; // moved argument pointer 2 ahead since it expects 2 params for this function
                    WebApiAction webApiAction =
                        new WebApiAction(_log, _library, args[cache + 1], args[cache + 2]);
                    await webApiAction.ExecuteAsync();
                    break;
                case "lf":
                case "-load-file":
                    if (args.Length - i < 2)
                        return;
                    i += 1;

                    ReadFromFileAction readFromFileAction = new ReadFromFileAction(_library, args[cache + 1]);
                    readFromFileAction.ExecuteAsync();
                    break;
                case "le":
                case "-load-external":
                    if (args.Length - i < 2)
                        return;
                    i += 1;
                    ImportGamesAction importGamesAction = new ImportGamesAction(args[cache + 1], _library);
                    importGamesAction.ExecuteAsync();
                    break;
                case "d":
                case "-dump":
                    if (args.Length - i < 1)
                    {
                        _log.WriteLine("missing parameters after {0}", args[i]);
                        return;
                    }

                    i += 1; // moved argument pointer 1 ahead since it expects 1 params for this function
                    WriteToFileAction writeToFileAction = new WriteToFileAction(_library, args[cache + 1]);
                    writeToFileAction.ExecuteAsync();
                    break;
                case "ds":
                case "-dataset":

                    if (args.Length - i < 1)
                    {
                        _log.WriteLine("missing parameters after {0}", args[i]);
                        return;
                    }

                    i += 1; // moved argument pointer 1 ahead since it expects 1 params for this function
                    DatasheetAction datasheetAction = new DatasheetAction(_log, _library, args[cache + 1]);
                    datasheetAction.ExecuteAsync();
                    break;
                default:
                    _log.WriteLine("Unrecognized argument: {0}", args[i]);
                    break;
            }
        }
    }
}