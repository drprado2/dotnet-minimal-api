namespace MinimalApi.Observability;

public class MapDiagnosticContext
{
    private const string CidKey = "Cid";
    private const string IdentityUserIdKey = "IdentityUserId";
    private const string CompanyIdKey = "CompanyId";
    
    private IEnumerable<KeyValuePair<string, object?>> _attributes = Enumerable.Empty<KeyValuePair<string, object?>>();
    public string? Cid { get; set; }
    public Guid? IdentityUserId { get; set; }
    public Guid? CompanyId { get; set; }
    public Guid? EmployeeId { get; set; }

    public IEnumerable<KeyValuePair<string, object?>> Attributes
    {
        get => _attributes
            .Append(new KeyValuePair<string, object?>(CidKey, Cid))
            .Append(new KeyValuePair<string, object?>(IdentityUserIdKey, IdentityUserId))
            .Append(new KeyValuePair<string, object?>(CompanyIdKey, CompanyId));
    }
    
    public object?[] LogArgs
    {
        get
        {
            var args = _attributes
                .Append(new KeyValuePair<string, object?>(CidKey, Cid))
                .Append(new KeyValuePair<string, object?>(IdentityUserIdKey, IdentityUserId))
                .Append(new KeyValuePair<string, object?>(CompanyIdKey, CompanyId)).ToArray();

            var result = new List<object?>(6);

            foreach (var a in args)
            {
                if (a.Value is null)
                    continue;
                
                result.Add(a.Key);
                result.Add(a.Value);
            }

            return result.ToArray();
        }
    }

    public MapDiagnosticContext Add(string key, object? value)
    {
        _attributes = _attributes.Append(new KeyValuePair<string, object?>(key, value));
        return this;
    }
}
