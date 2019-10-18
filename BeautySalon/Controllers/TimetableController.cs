using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Contracts;
using Entities.Models;
using Entities.Extentsions;

namespace BeautySalon.Controllers
{
    [Route("api/timetable")]
    [ApiController]
    public class TimetableController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;

        public TimetableController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

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

        [HttpPost]
        public IActionResult CreateTimetable([FromBody]Timetable timetable)
        {
            try
            {
                //timetable.CreationDate = DateTime.UtcNow;
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

                //if (_repository.Reservation.ReservationsByTimetable(id).Any())
                //{
                //    _logger.LogError($"Cannot delete timetable with id: {id}. It has related reservations. Delete those resrevations first");
                //    return BadRequest("Cannot delete timetable. It has related reservations. Delete those reservations first");
                //}

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