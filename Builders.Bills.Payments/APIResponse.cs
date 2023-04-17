using Newtonsoft.Json;
using System.Net;

namespace Builders.Bills.Payments
{
    public class APIResponse<T>
    {
        public bool Success { get; set; }
        public HttpStatusCode Code { get; set; }
        public string? Reason { get; set; }
        public T? Result { get; set; }

        public APIResponse(HttpResponseMessage message)
        {
            Success = message.IsSuccessStatusCode;
            Code = message.StatusCode;

            var contentStr = message.Content.ReadAsStringAsync().Result;
            if (Success)
            {
                try
                {
                    Result = JsonConvert.DeserializeObject<T>(contentStr);
                }
                catch (JsonException ex)
                {
                    throw new JsonException($"" +
                        $"An error ocours on json desserialize when fetch {message.RequestMessage?.RequestUri} \r\n" +
                        $"Json :{contentStr}", ex);
                }
            }
            else
            {
                Reason = contentStr;
            }
        }
    }
}