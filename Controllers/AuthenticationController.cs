using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using LJGHistoryService.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using LJGHistoryService.Repositories;

namespace LJGHistoryService.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private IConfiguration config;
        public IAuthenticationRepository authRepo { get; }

        public AuthenticationController(IConfiguration iConfig, IAuthenticationRepository authRepository)
        {
            config = iConfig;
            authRepo = authRepository;
        }



        [HttpPost]        
        public ActionResult<User> GetToken(User user)
        {
            return Ok(authRepo.GetJWTToken(user));
        }

       
    }
}
