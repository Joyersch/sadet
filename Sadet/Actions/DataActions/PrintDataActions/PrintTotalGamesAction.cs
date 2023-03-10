using Sadet.Steam.DataObjects;

namespace Sadet.Actions.DataActions.PrintDataActions;

public class PrintTotalGamesAction : PrintDataAction
{
    public PrintTotalGamesAction(TextWriter log, Library library, FormatSettings formatSettings) : base(log, library, formatSettings)
    {
    }

    public override async Task ExecuteAsync()
    {
        _log.WriteLine(
            _formatSettings.Format is null
                ? "TotalGames={0}"
                : _formatSettings.Format
            , _library.Games.Count);
    }
}