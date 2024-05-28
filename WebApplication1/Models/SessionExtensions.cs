using Newtonsoft.Json;

namespace WebApplication1.Models
{
    public static class SessionExtensions
    {
        public static T GetObject<T>(this ISession session, string key) where T : class
        {
            var value = session.GetString(key);
            return value == null ? null : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
