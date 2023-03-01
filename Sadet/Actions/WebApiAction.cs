using System.Net.Http.Json;
using Newtonsoft.Json;
using Sadet.Steam.DataObjects;

namespace Sadet.Actions;

public class WebApiAction : IAction
{
    private readonly TextWriter _log;
    private readonly Library _library;
    private readonly string _key;
    private readonly string _userId;

    public WebApiAction(TextWriter log, Library library, string key, string userId)
    {
        _log = log;
        _library = library;
        _key = key;
        _userId = userId;
    }

    public async Task ExecuteAsync()
    {
        Library newLibary = new Library();
        try
        {
            newLibary = await GetFromApi(_key, _userId);
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

    /// <summary>
    /// pull data from the steam api
    /// </summary>
    /// <param name="key">steam api key</param>
    /// <param name="userId">steam user id</param>
    /// <returns>Steam Game data. Can be null</returns>
    private static async Task<Library> GetFromApi(string key, string userId)
    {
        HttpClient client = new();

        #region Games

        string userGamesUrl = Global.SteamAPI.GetUserGamesUrl(key, userId);

        // Send request to the api for all games
        var userGamesResponse = await client.SendAsync(Global.Http.Get(userGamesUrl));

        if (!userGamesResponse.IsSuccessStatusCode)
            return null;
        // Response Object
        var userGamesObject = JsonConvert
                                  .DeserializeObject<Steam.API.userGames.UserGames>(await userGamesResponse
                                      .Content
                                      .ReadAsStringAsync()) ??
                              new Steam.API.userGames.UserGames();

        // Check if the response object contains data
        if (userGamesObject.Equals(new Steam.API.userGames.UserGames()))
            return null;

        #endregion // Games

        List<Task<Game>> games = new();
        for (var index = 0; index < userGamesObject.response.games.Length; index++)
        {
            var apiGame = userGamesObject.response.games[index];
            var gameAsync = GetGameAsync(client, apiGame.appid, key, userId);
            if (gameAsync is null)
                return null;
            games.Add(gameAsync);
        }

        await Task.WhenAll(games);
        Library library = new Library();
        foreach (var game in games)
        {
            var g = await game;
            if (g is null)
                continue;
            library.Games.Add(g);
        }

        return library;
    }

    private static async Task<Game> GetGameAsync(HttpClient client, int appId, string apikey, string userId)
    {
        Game game = new Game();
        game.Id = appId;

        string achievementUrl = Global.SteamAPI.GetGameAchievementUrl(apikey, userId, appId.ToString());

        // Send request for all achievements of a given game
        var gameAchievementResponse = await client.SendAsync(Global.Http.Get(achievementUrl));

        if (!gameAchievementResponse.IsSuccessStatusCode)
            return null;
        // Response Object
        var gameAchievementObject = await gameAchievementResponse.Content
            .ReadFromJsonAsync<Steam.API.achievements.userAchievements.Achievements>();

        // Check if the response object contains data
        if (gameAchievementObject is null)
            return null;

        #region Name

        game.Name = gameAchievementObject.playerstats.gameName;

        #endregion

        #region Achievements

        string globalAchievementUrl = Global.SteamAPI.GetgameGlobalAchievementsUrl(appId.ToString());
        var globalAchievementResponse = await client.SendAsync(Global.Http.Get(globalAchievementUrl));

        if (!globalAchievementResponse.IsSuccessStatusCode)
            return null;

        // Send request for all completion rates of the achievements for a given game
        var globalAchievementsObject = await globalAchievementResponse.Content
            .ReadFromJsonAsync<Steam.API.achievements.globalAchievements.Achievements>();

        if (!gameAchievementObject.playerstats.success || gameAchievementObject.playerstats.achievements is null)
            return null;


        // Write Achievements into the game object (for the library)
        foreach (var apiAchievement in gameAchievementObject.playerstats.achievements)
        {
            var achievement1 = apiAchievement;
            Achievement achievement = new()
            {
                Achieved = apiAchievement.achieved == 1,
                Percent = globalAchievementsObject.achievementpercentages.achievements
                    .First(a => a.name == achievement1.apiname).percent,
                Name = apiAchievement.apiname
            };

            game.Achievements.Add(achievement);
        }

        #endregion

        return game;
    }
}