namespace SalesManagement.Repository.Common;

public class ModelStateDictionary
{
    private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

    public void AddModelError(string key, string errorMessage)
    {
        if (!_errors.ContainsKey(key))
        {
            _errors[key] = new List<string>();
        }
        _errors[key].Add(errorMessage);
    }

    public bool IsValid => !_errors.Any();

    public Dictionary<string, List<string>> Errors => _errors;
}