using Sadet.Steam.DataObjects;

namespace Sadet.Actions.DataActions.PrintDataActions;

public class PrintDifficultyAction : PrintDataAction
{
    private readonly DifficultySettings _difficultySettings;

    public PrintDifficultyAction(TextWriter log, Library library, FormatSettings formatSettings, DifficultySettings difficultySettings) : base(log, library, formatSettings)
    {
        _difficultySettings = difficultySettings;
    }
    
    public override async Task ExecuteAsync()
    {
        _library.Games
            .GetRange(_formatSettings.Minimum ?? 0, _formatSettings.Maximum ?? _library.Games.Count)
            .ForEach(g =>
            {
                _log.WriteLine(
                    _formatSettings.Format is null
                        ? "{1}={0}"
                        : _formatSettings.Format
                    , _difficultySettings.ShowSingleTargetDifficulty ? g.MaxSingleDifficulty : g.Difficulty
                    , g.Name
                    , g.Id);
            });
    }
}