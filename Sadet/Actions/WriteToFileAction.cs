using Newtonsoft.Json;
using Sadet.Steam.DataObjects;

namespace Sadet.Actions;

public class WriteToFileAction : IAction
{
    public readonly Library _library;
    public readonly string _fileName;

    public WriteToFileAction(Library library, string fileName)
    {
        _library = library;
        _fileName = fileName;
    }

    public async Task ExecuteAsync()
    {
        await using var streamWriter = new StreamWriter(string.Format(_fileName));
        await streamWriter.WriteAsync(JsonConvert.SerializeObject(_library));
    }
}