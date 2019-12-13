using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeautySalon.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private ILoggerManager _logger;
        public AuthController(ILoggerManager logger, IAuthService authService)
        {
            _authService = authService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost, Route("request")]
        public IActionResult RequestToken([FromBody]AuthData request)
        {
            try
            {
                if (request == null) return BadRequest("Invalid client request");

                string token;
                if (_authService.IsAuthenticated(request, out token))
                {
                    _logger.LogInfo($"Token generated.");
                    return Ok(token);
                }
                _logger.LogInfo($"Invalid data.");
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside RequestToken method: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public IActionResult ReadToken()
        {
            try
            {
                string receivedToken = Request.Headers["Authorization"];
                var token = _authService.ReadToken(receivedToken);
                _logger.LogInfo($"Authorized token.");
                return Ok(token.Payload);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Bad token: {ex.Message} {Request.Headers["Authorization"]}");
                return Unauthorized();
            }
        }
    }
}