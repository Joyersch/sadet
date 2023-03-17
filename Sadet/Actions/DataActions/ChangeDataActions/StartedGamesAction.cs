using Sadet.Steam.DataObjects;

namespace Sadet.Actions.DataActions.ChangeDataActions;

public class StartedGamesAction : ChangeDataAction
{
    public StartedGamesAction(Library library) : base(library)
    {
    }

    public override async Task ExecuteAsync()
    {
        _library.Games.Started();
    }
}