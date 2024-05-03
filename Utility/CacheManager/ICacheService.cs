namespace BlazorLoanModule.Utility.CacheManager
{
    public interface ICacheService
    {
        Task<T> GetData<T>(string key);
        Task SetData(string key, object value, int ExpireMinute = 0);
        Task RemoveData(string key);
        Task<List<string>> GetKeylistAsync(string pattern);
        Task<string?> GetValue(string key);
    }
}
