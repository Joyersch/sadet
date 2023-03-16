namespace Sadet;

public class FormatSettings
{
    public string Format { get; private set; }
    public int? Minimum { get; private set; }
    public int? Maximum { get; private set; }


    public FormatSettings(string format = null, int? minimum = null, int? maximum = null)
    {
        Format = format;
        Minimum = minimum;
        Maximum = maximum;
    }

    public void ChangeFormat(string format)
        => Format = format;

    public void ChangeMinimum(int minimum)
        => Minimum = minimum;
    
    public void ChangeMaximum(int maximum)
        => Maximum = maximum;
}