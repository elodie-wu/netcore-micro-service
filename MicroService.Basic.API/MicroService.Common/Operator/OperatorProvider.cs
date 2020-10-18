using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace MicroService.Common.Operator
{
    public class OperatorProvider : IOperatorProvider
    {
        private readonly IHttpContextAccessor _accessor;
        public OperatorProvider(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        public string Name => _accessor.HttpContext.User.Identity.Name;

        public string ID => GetClaimValueByType("jti").FirstOrDefault();

        public bool IsAuthenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public string GetToken()
        {
            return _accessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        }

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return _accessor.HttpContext.User.Claims;
        }

        public List<string> GetClaimValueByType(string claimType)
        {
            return (from item in GetClaimsIdentity()
                    where item.Type == claimType
                    select item.Value).ToList();
        }
    }
}
