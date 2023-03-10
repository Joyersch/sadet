using Sadet.Steam.DataObjects;

namespace Sadet.Actions.DataActions;

public abstract class PrintDataAction : IAction
{
    protected readonly TextWriter _log;
    protected readonly Library _library;
    protected readonly FormatSettings _formatSettings;
    public PrintDataAction(TextWriter log, Library library, FormatSettings formatSettings)
    {
        _log = log;
        _library = library;
        _formatSettings = formatSettings;
    }
    public virtual async Task ExecuteAsync()
    {
        throw new NotImplementedException();
    }
}