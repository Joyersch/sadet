using Sadet.Steam.DataObjects;

namespace Sadet.Actions.DataActions.ChangeDataActions;

public class UnfinishedGamesAction : ChangeDataAction
{
    public UnfinishedGamesAction(Library library) : base(library)
    {
    }

    public override async Task ExecuteAsync()
    {
        _library.Games.Unfinished();
    }
}