using System.Text;

namespace Builders.Bills.Payments
{
    public abstract class APIBase
    {
        protected readonly HttpClient httpClient;

        public APIBase(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public virtual async Task<APIResponse<T>> SendPost<T>(string url, string jsonBody, AuthInfo? auth = null)
        {
            var data = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = data,
            };
            if (auth != null)
            {
                request.Headers.Add("Authorization", auth.Token);
            }
            var response = await httpClient.SendAsync(request);

            return new APIResponse<T>(response);
        }
    }
}