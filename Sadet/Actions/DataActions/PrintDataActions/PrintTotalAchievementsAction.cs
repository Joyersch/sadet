using Sadet.Steam.DataObjects;

namespace Sadet.Actions.DataActions.PrintDataActions;

public class PrintTotalAchievementsAction : PrintDataAction
{
    public PrintTotalAchievementsAction(TextWriter log, Library library, FormatSettings formatSettings) : base(log, library, formatSettings)
    {
    }
    
    public override async Task ExecuteAsync()
    {
        _log.WriteLine(
            _formatSettings.Format is null
                ? "TotalAchievements={0}"
                : _formatSettings.Format
            , _library.Games.Sum(g => g.Achievements.Count(a => a.Achieved)));
    }
}