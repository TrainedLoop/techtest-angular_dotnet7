using Newtonsoft.Json;

namespace Builders.Bills.Payments
{
    public class AuthInfo
    {
        public AuthInfo(string token, DateTime expiresIn)
        {
            if (expiresIn < DateTime.Now)
                throw new ArgumentException($"{nameof(expiresIn)}:{expiresIn} has expired");

            Token = token ?? throw new ArgumentNullException(nameof(token));
            ExpiresIn = expiresIn;
        }

        public string Token { get; }

        [JsonProperty(PropertyName = "expires_in")]
        public DateTime ExpiresIn { get; }
    }
}