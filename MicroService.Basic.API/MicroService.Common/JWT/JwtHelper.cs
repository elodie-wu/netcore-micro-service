using MicroService.Common.Config;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace MicroService.Common.JWT
{
    public class JwtHelper
    {

        /// <summary>
        /// 颁发JWT字符串
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public static string IssueJwt(TokenModel tokenModel)
        {
            /* 
            iss (issuer)：签发人 
            exp (expiration time)：过期时间 
            sub (subject)：主题 
            aud (audience)：受众 
            nbf (Not Before)：生效时间 
            iat (Issued At)：签发时间 
            jti (JWT ID)：编号 
             */
            string iss = Appsettings.Get(new string[] { "Audience", "Issuer" });
            string aud = Appsettings.Get(new string[] { "Audience", "Audience" });
            string secret = "asjdhfjkasdhkflhkashd";
             
            var claims = new List<Claim>
            {
                 new Claim(JwtRegisteredClaimNames.Jti, tokenModel.Id.ToString()),
                 new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                 new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") , 
                 new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(1200)).ToUnixTimeSeconds()}"),
                 new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(1200).ToString()),
                 new Claim(JwtRegisteredClaimNames.Iss,iss),
                 new Claim(JwtRegisteredClaimNames.Aud,aud),
                 new Claim(ClaimTypes.Name,tokenModel.UserName)

            };
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: iss,
                claims: claims,
                signingCredentials: creds);
             
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt); 
            return encodedJwt;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static TokenModel SerializeJwt(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
            object name;
            try
            {
                jwtToken.Payload.TryGetValue(ClaimTypes.Name, out name);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            var tm = new TokenModel
            {
                Id = (jwtToken.Id).ToString(),
                UserName = name != null ? name.ToString() : "",
            };
            return tm;
        } 
        /// <summary>
        /// 令牌
        /// </summary>
        public class TokenModel
        {
            /// <summary>
            /// Id
            /// </summary>
            public string Id { get; set; }
            /// <summary>
            /// 名称
            /// </summary>
            public string UserName { get; set; }  

        } 
    }
}