using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.Extentsions;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BeautySalon.Controllers
{
    [Route("api/reservation")]
    public class ReservationController : Controller
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;

        public ReservationController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        // GET: api/<controller>
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
                return NotFound();
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public IActionResult GetReservationById(int id)
        {
            try
            {
                var reservation = _repository.Reservation.GetReservationById(id);

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

        // POST api/<controller>
        [HttpPost]
        public IActionResult CreateReservation([FromBody]Reservation reservation)
        {
            try
            {
                //reservation.CreationDate = DateTime.UtcNow;
                if (reservation.IsObjectNull())
                {
                    _logger.LogError("Reservation object sent from client is null.");
                    return BadRequest("Reservation object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid reservation object sent from client.");
                    return BadRequest("Invalid model object");
                }

                _repository.Reservation.CreateReservation(reservation);
                _repository.Save();

                return Ok(reservation);

                //return CreatedAtRoute(nameof(IReservationRepository.GetReservationById), new { id = reservation.Id },reservation);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateReservation action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/<controller>/5
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

                if (!ModelState.IsValid)
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
