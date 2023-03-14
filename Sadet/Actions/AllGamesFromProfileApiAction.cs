using System.Net.Http.Json;
using Newtonsoft.Json;
using Sadet.Steam.DataObjects;

namespace Sadet.Actions;

public class AllGamesFromProfileApiAction : IAction
{
    private readonly TextWriter _log;
    private readonly Library _library;
    private readonly WebApiConnection _webApiConnection;

    public AllGamesFromProfileApiAction(TextWriter log, Library library, WebApiConnection webApiConnection)
    {
        _log = log;
        _library = library;
        _webApiConnection = webApiConnection;
    }

    public async Task ExecuteAsync()
    {
        Library newLibary = new Library();
        try
        {
            newLibary = await _webApiConnection.GetFromApi();
            if (newLibary is null)
            {
                await _log.WriteLineAsync("Api return invalid values!");
                return;
            }

            if (newLibary.Games.Count == 0)
                await _log.WriteLineAsync("This steam user might have no games!");
        }
        catch (Exception ex)
        {
            _log.WriteLine("Something went wrong accessing the api!\n{0}", ex);
            return;
        }

        _library.Games = newLibary.Games;
    }
}