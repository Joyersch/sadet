using System.ComponentModel.Design;
using Sadet.Steam.DataObjects;

namespace Sadet.Actions.DataActions.ChangeDataActions;

public class SortGamesAction : ChangeDataAction
{
    public bool _completion;
    public bool _ascending;
    
    public SortGamesAction(Library library, bool completion, bool ascending) : base(library)
    {
        _completion = completion;
        _ascending = ascending;
    }

    public override async Task ExecuteAsync()
    {
        if (_completion)
            _library.Games.SortedByCompletion(_ascending);
        else
            _library.Games.SortedByDifficulty(_ascending);
    }
}