using Sadet.Steam.DataObjects;

namespace Sadet.Actions.DataActions;

public abstract class ChangeDataAction : IAction
{
    protected readonly Library _library;
    public ChangeDataAction(Library library)
    {
        _library = library;
    }

    public virtual async Task ExecuteAsync()
    {
        throw new NotImplementedException();
    }
}