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
-h --help                               Displays this info
-la --load-api      [apikey] [userid]   Loads user game data with the achievements from the api
-lf --load-file     [file]              Loads user game data from a given file
-le --load-external [file]              Loads external file to add to the library. This is for unlisted games.
-d --dump           [file]              Dumps the user game data to a file (for -lf)
-ds --dataset       [dataset]           Outputs data                       

Datasets:

Print output with
------------------------------
c       Print total completion
r       Print total game count
y       Print completion
x       Print difficulty
z       Print total achievement count

sort // limit the dataset with
------------------------------
u       Only Unfinished (remove all finished (100%))
a       Only Started (remove all not started (0%))
g       Sorted by completion ASC
h       Sorted by completion DESC
i       Sorted by difficulty ASC
j       Sorted by difficulty DESC

Examples:
-la [Key] [userid] -d cache.json -ds c      This will print out the total completion average.
-lf cache.json -ds auix                     This will print out a all games and there difficulty to 100% sorted by the difficulty.
");
}