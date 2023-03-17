namespace Sadet.Actions.DataActions;

public class SetRetryAction : IAction
{
    private WebApiConnection _connection;
    private string _input;
    
    public SetRetryAction(WebApiConnection connection, string input)
    {
       _connection = connection;
       _input = input;
    }
    public async Task ExecuteAsync()
    {
        if (uint.TryParse(_input, out uint retries))
            _connection.SetRetries(retries);
        else
            _connection.SetDefaultRetries();
    }
}