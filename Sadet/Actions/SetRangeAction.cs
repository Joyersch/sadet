namespace Sadet.Actions;

public class SetRangeAction : IAction
{
    private FormatSettings _formatSettings;
    private int _min;
    private int _max;

    public SetRangeAction(FormatSettings formatSettings, int min, int max)
    {
        _formatSettings = formatSettings;
        _min = min;
        _max = max;
    }

    public async Task ExecuteAsync()
    {
        if (_min >= 0)
            _formatSettings.ChangeMinimum(_min);
        if (_max >= 0)
            _formatSettings.ChangeMinimum(_max);
    }
}