using Newtonsoft.Json;
using Sadet.Steam.DataObjects;

namespace Sadet.Actions;

public class AllGamesFromFileApiAction : IAction
{
    private readonly TextWriter _log;
    private readonly Library _library;
    private readonly WebApiConnection _webApiConnection;
    private readonly string _fileName;
    private readonly ApiOptions _apiOptions;

    public AllGamesFromFileApiAction(TextWriter log, Library library, WebApiConnection webApiConnection,
        string fileName, ApiOptions apiOptions)
    {
        _log = log;
        _library = library;
        _webApiConnection = webApiConnection;
        _fileName = fileName;
        _apiOptions = apiOptions;
    }

    public async Task ExecuteAsync()
    {
        using var streamReader = new StreamReader(_fileName);
        int[] games = JsonConvert.DeserializeObject<int[]>(await streamReader.ReadToEndAsync());
        var client = new HttpClient();
        foreach (var game in games)
        {
            var value = await _webApiConnection.GetGameAsync(client, game, _apiOptions.OnlyAchievements);
            if (value is not null)
                _library.Games.Add(value);
        }
    }
}