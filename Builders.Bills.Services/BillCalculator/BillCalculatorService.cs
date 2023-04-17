using Builders.Bills.Database;
using Builders.Bills.Payments;
using Builders.Bills.Services.Interfaces;
using Builders.Bills.Shared;
using Builders.Bills.Shared.Enums;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Builders.Bills.Services.BillCalculator
{
    public class BillCalculatorService : IBillCalculatorService
    {
        private readonly decimal fineRate;
        private readonly decimal interestRate;
        private readonly string clientId;
        private readonly string clientSecret;

        public BillAPI API { get; set; }

        private readonly BillsDbContext billsDb;

        //should use other solution for cache if the app will run multiple instances
        private readonly IMemoryCache cache;

        private const string CACHE_KEY = "BillAPI:AuthInfo";

        public BillCalculatorService(IConfiguration configuration, BillsDbContext billsDb, HttpClient httpClient, IMemoryCache cache)
        {
            var config = new BillCalculatorServiceConfig();
            configuration.GetSection("BillCalculator").Bind(config);
            fineRate = config.FineRate
                ?? throw new NullReferenceException($"{nameof(fineRate)} configuration");
            interestRate = config.InterestRate
                ?? throw new NullReferenceException($"{nameof(interestRate)} configuration");
            clientId = config.ClientId
                ?? throw new NullReferenceException($"{nameof(clientId)} configuration");
            clientSecret = config.ClientSecret
                ?? throw new NullReferenceException($"{nameof(clientSecret)} configuration");

            API = new(httpClient);
            this.billsDb = billsDb;
            this.cache = cache;
        }

        public async Task<IBillCalculated?> GetCalculatedBill(string barcode, DateTime paymentDate)
        {
            var authInfo = await GetAuth();
            var billInfoResponse = await API.GetInfo(barcode, authInfo);
            if (billInfoResponse.Success == false || billInfoResponse.Result == null)
            {
                if (billInfoResponse.Code == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                throw new InvalidOperationException(
                    @$"Error to fetch {nameof(GetCalculatedBill)}: Code: {billInfoResponse.Code},
                      {billInfoResponse.Reason}");
            }
            IBillCalculated calculatedBill = billInfoResponse.Result.Type switch
            {
                BillType.NPC => new BillCalculatedNPC(billInfoResponse.Result, paymentDate)
                                        .CalculateFees(interestRate, fineRate),
                BillType.NORMAL => new BillCalculatedNormal(billInfoResponse.Result, paymentDate),

                //this code is not reachable the response from the infrastructure layer does not allow invalid types
                _ => throw new NotImplementedException($"Bill Type {billInfoResponse.Result.Type}"),
            };

            billsDb.Bills.Add(new(calculatedBill, fineRate, interestRate));
            await billsDb.SaveChangesAsync();
            return calculatedBill;
        }

        private async Task<AuthInfo> GetAuth()
        {
            var authInfo = cache.Get<AuthInfo>(CACHE_KEY);
            if (authInfo != null)
                return authInfo;

            var authInfoResponse = await API.Auth(clientId, clientSecret);

            if (authInfoResponse.Success == false || authInfoResponse.Result == null)
            {
                throw new InvalidOperationException(
                   @$"Error to fetch {nameof(GetAuth)}: Code: {authInfoResponse.Code},
                      {authInfoResponse.Reason}");
            }

            var expire = (authInfoResponse.Result.ExpiresIn - DateTime.Now);
            cache.Set(CACHE_KEY, authInfoResponse.Result, expire);

            return authInfoResponse.Result;
        }
    }
}