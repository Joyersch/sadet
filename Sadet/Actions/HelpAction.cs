namespace Sadet.Actions;

public class HelpAction : IAction
{
    private readonly TextWriter _log;

    public HelpAction(TextWriter log)
    {
        _log = log;
    }

    public async Task ExecuteAsync()
        => await _log.WriteAsync(@"Args
-----
-h --help                                   Displays this info
-ac --api-connection    [apikey] [userid]   Add an api connection (required for api actions)
-ar --api-retries       [number]            Set amount of retries for api calls if they run into Internal Server Errors. Default value is 10 
-lf --load-file         [file]              Loads user game data from a given file
-le --load-external     [file]              Loads external file to add to the library. This is for unlisted games
-lag --load-api-games                       Loads user game data with the achievements from the api
-lfg --load-file-games  [file]              Loads game id's from a file and queries the data for each games from the api and adds said data
-d --dump               [file]              Dumps the user game data to a file (for -lf)                 
-f --format             [format]            Set a format for output. {0} = value | {1} = name {2} = id
-r --range              [min] [max]         Set a min and max for output. default for both is -1

Change Data:
-----------
-fou --filter-only-unfinished           Only Unfinished (remove all finished (100%))
-fos --filter-only-started              Only Started (remove all not started (0%))
-s --sort           [arg1] [arg2]       Sort Games
    arg1=[c|comp|completion]            Sorted by completion
    arg1=[d|dif|difficulty]             Sorted by difficulty
    arg2=[a|asc|ascending]              Sort Ascending
    arg2=[d|desc|descending]            Sort Descending

Output data:
------------
-pc --print-completion                  Print completion
-pd --print-difficulty                  Print difficulty
-pca --print-completion-average         Print average completion
-ptg --print-total-games                Print total game count
-pta --print-total-achievements         Print total achievement count
");
}