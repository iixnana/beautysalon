using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Contracts;
using Entities.Models;
using Entities.Extentsions;
using Microsoft.AspNetCore.Authorization;

namespace BeautySalon.Controllers
{
    [Route("api/timetable")]
    [ApiController]
    public class TimetableController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;
        private readonly IAuthService _authService;

        public TimetableController(ILoggerManager logger, IRepositoryWrapper repository, IAuthService authService)
        {
            _logger = logger;
            _repository = repository;
            _authService = authService;
        }

        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public IActionResult GetAllTimetables()
        {
            try
            {
                var timetables = _repository.Timetable.GetAllTimetables();

                _logger.LogInfo($"Returned all timetables from database.");

                return Ok(timetables);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllTimetables action: {ex.Message}");
                return NotFound();
            }
        }

        [Authorize(Roles=Role.MasterAdmin)]
        [HttpGet("master/{id}")]
        public IActionResult GetTimetablesByMaster(int id)
        {
            try
            {
                var timetables = _repository.Timetable.GetTimetablesByMaster(id).OrderByDescending(x => x.Date);

                _logger.LogInfo($"Returned timetables by master id {id} from database.");

                return Ok(timetables);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllTimetables action: {ex.Message}");
                return NotFound();
            }
        }

        [Authorize(Roles = Role.MasterAdmin)]
        [HttpGet("{id}")]
        public IActionResult GetTimetableById(int id)
        {
            try
            {
                var timetable = _repository.Timetable.GetTimetableById(id);

                if (timetable.IsEmptyObject(id))
                {
                    _logger.LogError($"Timetable with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else if (_authService.IsMaster(Request.Headers["Authorization"]) && _authService.GetId(Request.Headers["Authorization"]) != timetable.MasterId)
                {
                    _logger.LogError($"User with id: {id}, has tried to access restricted data in GetTimetableById.");
                    return Unauthorized();
                }
                else
                {
                    _logger.LogInfo($"Returned timetable with id: {id}");
                    return Ok(timetable);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetTimetableById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpGet("times/{id}/{serviceId}")]
        public IActionResult GetTimes(int id, int serviceId)
        {
            try
            {
                var timetable = _repository.Timetable.GetTimes(id, serviceId);

                if (!timetable.Any())
                {
                    _logger.LogError($"Times for master with id: {id}, hasn't been found in db.");
                    return Ok();
                }
                else
                {
                    _logger.LogInfo($"Returned times for master id: {id}");
                    return Ok(timetable);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetTimes action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = Role.Master)]
        [HttpPost]
        public IActionResult CreateTimetable([FromBody]Timetable timetable)
        {
            try
            {
                timetable.MasterId = _authService.GetId(Request.Headers["Authorization"]);
                if (timetable.IsObjectNull())
                {
                    _logger.LogError("Timetable object sent from client is null.");
                    return BadRequest("Timetable object is null");
                }

                if (!_repository.Timetable.IsValid(timetable))
                {
                    _logger.LogError("Invalid timetable object sent from client.");
                    return BadRequest("Egzistuoja konfliktas su kitu tvarkaraščiu.");
                }

                _repository.Timetable.CreateTimetable(timetable);
                _repository.Save();

                return Ok(timetable);

                //return CreatedAtRoute(nameof(ITimetableRepository.GetTimetableById), new { id = timetable.Id },timetable);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateTimetable action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = Role.Master)]
        [HttpPut("{id}")]
        public IActionResult UpdateTimetable(int id, [FromBody]Timetable timetable)
        {
            try
            {
                if (timetable.IsObjectNull())
                {
                    _logger.LogError("Timetable object sent from client is null.");
                    return BadRequest("Timetable object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid timetable object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var dbTimetable = _repository.Timetable.GetTimetableById(id);
                if (dbTimetable.IsEmptyObject(id))
                {
                    _logger.LogError($"Timetable with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                timetable.Id = id;
                timetable.MasterId = dbTimetable.MasterId;
                _repository.Timetable.UpdateTimetable(dbTimetable, timetable);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateTimetable action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = Role.Master)]
        [HttpDelete("{id}")]
        public IActionResult DeleteTimetable(int id)
        {
            try
            {
                var timetable = _repository.Timetable.GetTimetableById(id);
                if (timetable.IsEmptyObject(id))
                {
                    _logger.LogError($"Timetable with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                if (_authService.IsMaster(Request.Headers["Authorization"]) && _authService.GetId(Request.Headers["Authorization"]) != timetable.MasterId)
                {
                    _logger.LogError($"User with id: {id}, has tried to access restricted data in DeleteTimetable.");
                    return Unauthorized();
                }

                if (_repository.Reservation.ReservationsByTimetable(timetable.Date, timetable.TimeStart, timetable.TimeEnd).Any())
                {
                    _logger.LogError($"Cannot delete timetable with id: {id}. It has related reservations. Delete those resrevations first");
                    return BadRequest("Cannot delete timetable. It has related reservations. Delete those reservations first");
                }

                _repository.Timetable.DeleteTimetable(timetable);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteTimetable action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}