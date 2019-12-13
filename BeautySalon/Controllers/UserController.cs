using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Contracts;
using Entities.Models;
using Entities.Extentsions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BeautySalon.Controllers
{
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly IAuthService _authService;
        private ILoggerManager _logger;
        public IRepositoryWrapper _repository;
        private readonly string _secret;

        public UserController(IOptions<AppSettings> appSettings, ILoggerManager logger, IRepositoryWrapper repository, IAuthService authService)
        {
            _logger = logger;
            _repository = repository;
            _secret = appSettings.Value.Secret;
            _authService = authService;
        }

        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = _repository.User.GetAllUsers();

                _logger.LogInfo($"Returned all users from database.");

                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllUsers action: {ex.Message}");
                return NotFound();
            }
        }

        [AllowAnonymous]
        [HttpGet("masters")]
        public IActionResult GetAllMasters()
        {
            try
            {
                var users = _repository.User.GetAllMasters();
                foreach(User user in users)
                {
                    user.Password = null;
                }

                _logger.LogInfo($"Returned all masters from database.");

                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllMasters action: {ex.Message}");
                return NotFound();
            }
        }

        [Authorize(Roles = Role.ClientMasterAdmin)]
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                if (!_authService.IsAdmin(Request.Headers["Authorization"]) && _authService.GetId(Request.Headers["Authorization"]) != id)
                {
                    _logger.LogError($"User with id: {id}, has tried to access restricted data in GetUserById.");
                    return Unauthorized();
                }

                var user = _repository.User.GetUserById(id);

                if (user.IsEmptyObject(id))
                {
                    _logger.LogError($"User with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned user with id: {id}");
                    return Ok(user);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetUserById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = Role.ClientMasterAdmin)]
        [HttpGet("{id}/reservations")]
        public IActionResult GetUserWithDetails(int id)
        {
            try
            {
                if (!_authService.IsAdmin(Request.Headers["Authorization"]) && _authService.GetId(Request.Headers["Authorization"]) != id)
                {
                    _logger.LogError($"User with id: {id}, has tried to access restricted data in GetUserById.");
                    return Unauthorized();
                }

                var user = _repository.User.GetUserWithDetails(id);

                if(user.IsEmptyObject(id))
                {
                    _logger.LogError($"User with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned user with details for id: {id}");
                    return Ok(user);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetUserWithDetails action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateUser([FromBody]User user)
        {
            try
            {
                //user.CreationDate = DateTime.UtcNow;
                if (user.IsObjectNull())
                {
                    _logger.LogError("User object sent from client is null.");
                    return BadRequest("User object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid user object sent from client.");
                    return BadRequest("Invalid model object");
                }

                _repository.User.CreateUser(user);
                _repository.Save();

                return Ok(user);

                //return CreatedAtRoute(nameof(IUserRepository.GetUserById), new { id = user.Id },user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateUser action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = Role.ClientMasterAdmin)]
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody]User user)
        {
            try
            {
                if (!_authService.IsAdmin(Request.Headers["Authorization"]) && _authService.GetId(Request.Headers["Authorization"]) != id)
                {
                    _logger.LogError($"User with id: {id}, has tried to access restricted data in GetUserById.");
                    return Unauthorized();
                }

                if (user.IsObjectNull())
                {
                    _logger.LogError("User object sent from client is null.");
                    return BadRequest("User object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid user object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var dbUser = _repository.User.GetUserById(id);
                if (dbUser.IsEmptyObject(id))
                {
                    _logger.LogError($"User with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                user.Id = id;
                _repository.User.UpdateUser(dbUser, user);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateUser action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = Role.ClientMasterAdmin)]
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                if (!_authService.IsAdmin(Request.Headers["Authorization"]) && _authService.GetId(Request.Headers["Authorization"]) != id)
                {
                    _logger.LogError($"User with id: {id}, has tried to access restricted data in GetUserById.");
                    return Unauthorized();
                }

                var user = _repository.User.GetUserById(id);
                if (user.IsEmptyObject(id))
                {
                    _logger.LogError($"User with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                if (_repository.Reservation.GetReservationsByUser(id).Any())
                {
                    _logger.LogError($"Cannot delete user with id: {id}. It has related reservations. Delete those reservations first");
                    return BadRequest("Cannot delete user. It has related reservations. Delete those reservations first");
                }

                _repository.User.DeleteUser(user);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteUser action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
