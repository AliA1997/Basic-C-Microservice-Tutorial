using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace CreditRatingService
{
    //The CreditRatingCheckService inherits from the CreditRatingCheck.CreditRatingCheckBase
    //Which is generated from the credit-rating-service.proto file.
    //It is built from the rpc CreditRatingCheck definition
    //After implementatiion integrate into gRPC infrastructure, open Startup.cs file to implement this.
    public class CreditRatingCheckService : CreditRatingCheck.CreditRatingCheckBase
    {
        private readonly ILogger<CreditRatingCheckService> _logger;

        private static readonly Dictionary<string, Int32> customerTrustedCredit = new Dictionary<string, Int32>() 
        {
            {"id0201", 10000},
            {"id0417", 5000},
            {"id0306", 15000}
        };

        public CreditRatingCheckService(ILogger<CreditRatingCheckService> logger)
        {
            _logger = logger;
        }
        //Implement the rpc method definition in the credit-rating-service proto file definition.
        public override Task<CreditReply> CheckCreditRequest(CreditRequest request, ServerCallContext context) 
        {
            return Task.FromResult(new CreditReply
            {
                IsAccepted = IsEligibleForCredit(request.CustomerId, request.Credit)
            });
        }

        private bool IsEligibleForCredit(string customerId, Int32 credit) 
        {
            bool isEligible = false;

            if(customerTrustedCredit.TryGetValue(customerId, out Int32 maxCredit))
            {
                isEligible = credit <= maxCredit;
            }

            return isEligible;
        }

    }
}
