using Sadet.Steam.DataObjects;

namespace Sadet.Actions.DataActions.PrintDataActions;

public class PrintDifficultyAction : PrintDataAction
{
    public PrintDifficultyAction(TextWriter log, Library library, FormatSettings formatSettings) : base(log, library, formatSettings)
    {
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
                    , g.Difficulty
                    , g.Name
                    , g.Id);
            });
    }
}