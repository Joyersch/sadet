using System.Net;
using System.Net.Http.Json;
using Newtonsoft.Json;
using Sadet.Steam.API.achievements.globalAchievements;
using Sadet.Steam.DataObjects;
using Achievement = Sadet.Steam.DataObjects.Achievement;

namespace Sadet;

public class WebApiConnection
{
    private readonly string _key;
    private readonly string _userId;

    private uint _retries;

    public WebApiConnection(string key, string userId)
    {
        _key = key;
        _userId = userId;
        SetDefaultRetries();
    }

    public void SetRetries(uint tries)
        => _retries = tries;

    public void SetDefaultRetries()
        => _retries = 10;

    /// <summary>
    /// pull data from the steam api
    /// </summary>
    /// <param name="key">steam api key</param>
    /// <param name="userId">steam user id</param>
    /// <returns>Steam Game data. Can be null</returns>
    public async Task<Library> GetFromApi(ApiOptions _apiOptions)
    {
        HttpClient client = new();

        #region Games

        string userGamesUrl = Global.SteamAPI.GetUserGamesUrl(_key, _userId);

        // Send request to the api for all games
        HttpResponseMessage userGamesResponse;

        int tries = 0;
        do
        {
            userGamesResponse = await client.SendAsync(Global.Http.Get(userGamesUrl));
            tries++;
        } while (userGamesResponse.StatusCode == HttpStatusCode.InternalServerError && tries <= _retries);

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
            var gameAsync = GetGameAsync(client, apiGame.appid, _apiOptions.OnlyAchievements);
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

    public async Task<Game> GetGameAsync(HttpClient client, int appId, bool onlyAchievements)
    {
        Game game = new Game();
        game.Id = appId;

        string achievementUrl = Global.SteamAPI.GetGameAchievementUrl(_key, _userId, appId.ToString());

        // Send request for all achievements of a given game
        HttpResponseMessage gameAchievementResponse;

        int tries = 0;
        do
        {
            gameAchievementResponse = await client.SendAsync(Global.Http.Get(achievementUrl));
            tries++;
        } while (gameAchievementResponse.StatusCode == HttpStatusCode.InternalServerError && tries <= _retries);

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

        Achievements? globalAchievementsObject = null;

        if (!onlyAchievements)
        {
            string globalAchievementUrl = Global.SteamAPI.GetgameGlobalAchievementsUrl(appId.ToString());
            HttpResponseMessage globalAchievementResponse;

            tries = 0;

            do
            {
                globalAchievementResponse = await client.SendAsync(Global.Http.Get(globalAchievementUrl));
                tries++;
            } while (globalAchievementResponse.StatusCode == HttpStatusCode.InternalServerError && tries <= _retries);

            if (!globalAchievementResponse.IsSuccessStatusCode)
                return null;

            // Send request for all completion rates of the achievements for a given game
            globalAchievementsObject = await globalAchievementResponse.Content
                .ReadFromJsonAsync<Steam.API.achievements.globalAchievements.Achievements>();


        }

        globalAchievementsObject ??= new Achievements()
        {
            achievementpercentages = new Percentages()
            {
                achievements = Array.Empty<Steam.API.achievements.globalAchievements.Achievement>()
            }
        };

        if (!gameAchievementObject.playerstats.success || gameAchievementObject.playerstats.achievements is null)
            return game;
        // Write Achievements into the game object (for the library)
        foreach (var apiAchievement in gameAchievementObject.playerstats.achievements)
        {
            var steamAchievement = apiAchievement;

            Achievement libAchievement = new()
            {
                Achieved = apiAchievement.achieved == 1,
                // First can fail here if a game is really new or recently got achievements which therefor there will be no global statistics
                Percent = globalAchievementsObject.achievementpercentages.achievements
                    .FirstOrDefault(a => a.name == steamAchievement.apiname, new() { percent = 0 }).percent,
                Name = apiAchievement.apiname
            };

            game.Achievements.Add(libAchievement);
        }

        #endregion

        return game;
    }
}