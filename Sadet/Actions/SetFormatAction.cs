namespace Sadet.Actions;

public class SetFormatAction : IAction
{
    private FormatSettings _formatSettings;
    private string _format;
    
    public SetFormatAction(FormatSettings formatSettings, string format)
    {
        _formatSettings = formatSettings;
        _format = format;
    }
    
    public async Task ExecuteAsync()
    {
        _formatSettings.ChangeFormat(_format);
    }
}