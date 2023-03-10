using Sadet.Steam.DataObjects;

namespace Sadet.Actions.DataActions.PrintDataActions;

public class PrintCompletionAction : PrintDataAction
{
    public PrintCompletionAction(TextWriter log, Library library, FormatSettings formatSettings) : base(log, library, formatSettings)
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
                    , g.Completion
                    , g.Name);
            });
    
    }
}