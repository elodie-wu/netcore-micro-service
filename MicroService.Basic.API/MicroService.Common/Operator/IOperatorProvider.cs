using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace MicroService.Common.Operator
{
    public interface IOperatorProvider
    {
        string Name { get; }
        string ID { get; }
        bool IsAuthenticated();
        IEnumerable<Claim> GetClaimsIdentity();
        List<string> GetClaimValueByType(string ClaimType);

        string GetToken();
    }
}
