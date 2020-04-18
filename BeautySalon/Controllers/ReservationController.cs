using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.Extentsions;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BeautySalon.Controllers
{
    [Route("api/reservation")]
    public class ReservationController : Controller
    {
        private readonly IAuthService _authService;
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;

        public ReservationController(ILoggerManager logger, IRepositoryWrapper repository, IAuthService authService)
        {
            _logger = logger;
            _repository = repository;
            _authService = authService;
        }

        // GET: api/<controller>
        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public IActionResult GetAllReservations()
        {
            try
            {
                var reservations = _repository.Reservation.GetAllReservations();
                if (reservations.Any())
                {
                    _logger.LogInfo($"Returned all reservations from database.");

                    return Ok(reservations);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllReservations action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = Role.Master)]
        [HttpGet("timetable/{id}")]
        public IActionResult GetReservationsByTimetable(int id)
        {
            try
            {
                var reservations = _repository.Reservation.GetReservationsByTimetable(id);
                if (reservations.Any())
                {
                    _logger.LogInfo($"Returned all reservations from database.");

                    return Ok(reservations);
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetReservationsByTimetable action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = Role.Master)]
        [HttpGet("master/{id}")]
        public IActionResult GetReservationsByMaster(int id)
        {
            try
            {
                var reservations = _repository.Reservation.GetReservationsByMaster(id);
                if (reservations.Any())
                {
                    _logger.LogInfo($"Returned all reservations from database.");

                    return Ok(reservations);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllReservations action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = Role.ClientMasterAdmin)]
        [HttpGet("user/{id}")]
        public IActionResult GetReservationsByUser(int id)
        {
            try
            {
                if (_authService.IsClient(Request.Headers["Authorization"]) && _authService.GetId(Request.Headers["Authorization"]) != id)
                {
                    _logger.LogError($"User with id: {id}, has tried to access restricted data in GetReservationsByUser.");
                    return Unauthorized();
                }

                var reservations = _repository.Reservation.GetReservationsByUser(id);
                if (reservations.Any())
                {
                    _logger.LogInfo($"Returned all reservations from database.");

                    return Ok(reservations);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllReservations action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }



        // GET api/<controller>/5
        [Authorize(Roles = Role.ClientMaster)]
        [HttpGet("{id}")]
        public IActionResult GetReservationById(int id)
        {
            try
            {
                var reservation = _repository.Reservation.GetReservationById(id);

                if (_authService.IsClient(Request.Headers["Authorization"]) && _authService.GetId(Request.Headers["Authorization"]) != reservation.UserId)
                {
                    _logger.LogError($"User with id: {id}, has tried to access restricted data in GetReservationsById.");
                    return Unauthorized();
                }

                if (_authService.IsMaster(Request.Headers["Authorization"]) && _authService.GetId(Request.Headers["Authorization"]) != reservation.MasterId)
                {
                    _logger.LogError($"User with id: {id}, has tried to access restricted data in GetReservationsById.");
                    return Unauthorized();
                }

                if (reservation.IsEmptyObject(id))
                {
                    _logger.LogError($"Reservation with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned reservation with id: {id}");
                    return Ok(reservation);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetReservationById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = Role.ClientMasterAdmin)]
        [HttpGet("details/{id}")]
        public IActionResult GetReservationWithDetails(int id)
        {
            try
            {
                var reservation = _repository.Reservation.GetReservationWithDetails(id);

                if (reservation.IsEmptyObject(id))
                {
                    _logger.LogError($"Reservation with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned reservation with details for id: {id}");
                    return Ok(reservation);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetReservationWithDetails action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = Role.MasterAdmin)]
        [HttpGet("details")]
        public IActionResult GetReservationsWithDetails()
        {
            try
            {
                var reservations = _repository.Reservation.GetReservationsWithDetails();

                if (!reservations.Any())
                {
                    _logger.LogError($"No reservations have been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned extended reservations");
                    return Ok(reservations);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetReservationsWithDetails action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = Role.ClientMasterAdmin)]
        [HttpGet("details/user/{id}")]
        public IActionResult GetReservationsWithDetails(int id)
        {
            try
            {
                var reservations = _repository.Reservation.GetReservationsWithDetails(id);

                if (!reservations.Any())
                {
                    _logger.LogError($"No reservations have been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned extended reservations");
                    return Ok(reservations);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetReservationsWithDetails action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST api/<controller>
        [Authorize(Roles = Role.Client)]
        [HttpPost]
        public IActionResult CreateReservation([FromBody]Reservation reservation)
        {
            try
            {
                reservation.UserId = _authService.GetId(Request.Headers["Authorization"]);
                var timetable = _repository.Timetable.GetTimetableByMasterAndDateTime(reservation.MasterId, reservation.TimeStart.Date, reservation.TimeStart.TimeOfDay, reservation.TimeEnd.TimeOfDay);
                reservation.TimetableId = timetable.Id;
                

                if (reservation.IsObjectNull())
                {
                    _logger.LogError("Reservation object sent from client is null.");
                    return BadRequest("Reservation object is null");
                }

                _repository.Reservation.CreateReservation(reservation);
                _repository.Save();

                return Ok(reservation);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateReservation action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/<controller>/5
        [Authorize(Roles = Role.ClientMaster)]
        [HttpPut("{id}")]
        public IActionResult UpdateReservation(int id, [FromBody]Reservation reservation)
        {
            try
            {
                if (reservation.IsObjectNull())
                {
                    _logger.LogError("Reservation object sent from client is null.");
                    return BadRequest("Reservation object is null");
                }

                if (_authService.IsClient(Request.Headers["Authorization"]) && _authService.GetId(Request.Headers["Authorization"]) != reservation.UserId)
                {
                    _logger.LogError($"User with id: {id}, has tried to access restricted data in UpdateReservation.");
                    return Unauthorized();
                }

                if (!ModelState.IsValid || reservation.TimeStart.Date != reservation.TimeEnd.Date
                    || _repository.Timetable.GetTimetableByMasterAndDateTime(reservation.MasterId, reservation.TimeStart.Date, reservation.TimeStart.TimeOfDay, reservation.TimeEnd.TimeOfDay).IsObjectNull()
                    || !_repository.Reservation.IsValid(reservation.MasterId, reservation.TimeStart.Date, reservation.TimeStart.TimeOfDay, reservation.TimeEnd.TimeOfDay))
                {
                    _logger.LogError("Invalid reservation object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var dbReservation = _repository.Reservation.GetReservationById(id);
                if (dbReservation.IsEmptyObject(id))
                {
                    _logger.LogError($"Reservation with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.Reservation.UpdateReservation(dbReservation, reservation);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateReservation action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE api/<controller>/5
        [Authorize(Roles = Role.ClientMaster)]
        [HttpDelete("{id}")]
        public IActionResult DeleteReservation(int id)
        {
            try
            {
                var reservation = _repository.Reservation.GetReservationById(id);
                if (reservation.IsEmptyObject(id))
                {
                    _logger.LogError($"Reservation with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.Reservation.DeleteReservation(reservation);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteReservation action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
