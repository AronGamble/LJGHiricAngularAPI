using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using LJGHistoryService.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace LJGHistoryService.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        public AuthenticationRepository(IConfiguration config)
        {
            IConfig = config;
        }

        public IConfiguration IConfig { get; }

        public User GetJWTToken(User user)
        {
            var payload = new Dictionary<string, object> { { "claim1", user.Username } };

            string secret = IConfig.GetSection("LJGConfig").GetSection("JwtSecret").Value;

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(payload, secret);
            user.Token = token;


            return user;
        }
    }
}
