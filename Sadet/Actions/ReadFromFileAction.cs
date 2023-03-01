using Newtonsoft.Json;
using Sadet.Steam.DataObjects;

namespace Sadet.Actions;

public class ReadFromFileAction : IAction
{
    public readonly Library _library;
    public readonly string _fileName;

    public ReadFromFileAction(Library library, string fileName)
    {
        _library = library;
        _fileName = fileName;
    }

    public async Task ExecuteAsync()
    {
        using var streamReader = new StreamReader(_fileName);
        var library = JsonConvert.DeserializeObject<Library>(await streamReader.ReadToEndAsync()) ?? new Library();
        _library.Games = library.Games;
    }
}