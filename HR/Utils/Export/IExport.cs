namespace HR.Utils.Export
{
    public interface IExport
    {
        Dictionary<string, string> GetDataDictionary(string id);
    }
}
