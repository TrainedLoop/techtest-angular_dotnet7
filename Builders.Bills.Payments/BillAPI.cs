using System.Text.Json;

namespace Builders.Bills.Payments
{
    public class BillAPI : APIBase
    {
        public BillAPI(HttpClient httpClient) : base(httpClient)
        {
        }

        public virtual Task<APIResponse<AuthInfo>> Auth(string clientId, string clientSecret)
        {
            var jsonBody = JsonSerializer.Serialize(new
            {
                client_id = clientId,
                client_secret = clientSecret
            });

            return SendPost<AuthInfo>("https://vagas.builders/api/builders/auth/tokens", jsonBody);
        }

        public virtual Task<APIResponse<Bill>> GetInfo(string code, AuthInfo auth)
        {
            var jsonBody = JsonSerializer.Serialize(new
            {
                code
            });

            return SendPost<Bill>("https://vagas.builders/api/builders/bill-payments/codes", jsonBody, auth);
        }
    }
}