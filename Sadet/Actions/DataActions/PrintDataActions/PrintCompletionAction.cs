using Sadet.Steam.DataObjects;

namespace Sadet.Actions.DataActions.PrintDataActions;

public class PrintCompletionAction : PrintDataAction
{
    public PrintCompletionAction(TextWriter log, Library library, FormatSettings formatSettings) : base(log, library,
        formatSettings)
    {
    }

    public override async Task ExecuteAsync()
    {
        string format = "{1}={0}";
        if (_formatSettings?.Format is not null)
            format = _formatSettings.Format;
        _library.Games
            .GetRange(_formatSettings.Minimum ?? 0, _formatSettings.Maximum ?? _library.Games.Count)
            .ForEach(g =>
            {
                _log.WriteLine(format
                    , g.Completion
                    , g.Name
                    , g.Id);
            });
    }
}