namespace GymManagement.Api.Extensions
{
    public static class HybridCacheExtension
    {
        public static void AddCache(this IServiceCollection services)
        {
#pragma warning disable EXTEXP0018
            services.AddHybridCache(options =>
            {
                options.DefaultEntryOptions = new Microsoft.Extensions.Caching.Hybrid.HybridCacheEntryOptions()
                {
                    LocalCacheExpiration = TimeSpan.FromMinutes(2),
                    Expiration = TimeSpan.FromMinutes(2)
                };
            });
#pragma warning restore EXTEXP0018
        }
    }
}