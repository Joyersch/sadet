using Newtonsoft.Json;
using Sadet.Steam.DataObjects;

namespace Sadet.Actions;

public class ImportGamesAction : IAction
{
    private string _filePath;
    private Library _library;


    public ImportGamesAction(string filePath, Library library)
    {
        _filePath = filePath;
        _library = library;
    }

    public async Task ExecuteAsync()
    {
        using var sr = new StreamReader(_filePath);
        var externalGames = JsonConvert.DeserializeObject<ExternalGame[]>(sr.ReadToEnd()) ??
                            Array.Empty<ExternalGame>();
        foreach (ExternalGame externalGame in externalGames)
        {
            var game = new Game();
            game.Id = externalGame.Id;
            game.Name = externalGame.Name;
            for (int j = 0; j < externalGame.Achievements; j++)
            {
                var achievement = new Achievement();
                achievement.Name = "dummy" + j;
                achievement.Percent = 0;
                achievement.Achieved = j < externalGame.Unlocked;
                game.Achievements.Add(achievement);
            }

            _library.Games.Add(game);
        }
    }
}