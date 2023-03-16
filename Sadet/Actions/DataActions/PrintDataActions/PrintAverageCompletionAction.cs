using Sadet.Steam.DataObjects;

namespace Sadet.Actions.DataActions.PrintDataActions;

public class PrintAverageCompletionAction : PrintDataAction
{
    public PrintAverageCompletionAction(TextWriter log, Library library, FormatSettings formatSettings) :
        base(log, library, formatSettings)
    {
    }

    public override async Task ExecuteAsync()
    {
        _log.WriteLine(
            _formatSettings.Format is null
                ? "AverageCompletion={0}"
                : _formatSettings.Format
            , _library.Games.TotalCompletion());
    }
}