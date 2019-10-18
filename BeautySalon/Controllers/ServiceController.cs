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
    [Route("api/service")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;

        public ServiceController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAllServices()
        {
            try
            {
                var services = _repository.Service.GetAllServices();

                _logger.LogInfo($"Returned all services from database.");

                return Ok(services);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllServices action: {ex.Message}");
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetServiceById(int id)
        {
            try
            {
                var service = _repository.Service.GetServiceById(id);

                if (service.IsEmptyObject(id))
                {
                    _logger.LogError($"Service with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned service with id: {id}");
                    return Ok(service);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetServiceById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult CreateService([FromBody]Service service)
        {
            try
            {
                //service.CreationDate = DateTime.UtcNow;
                if (service.IsObjectNull())
                {
                    _logger.LogError("Service object sent from client is null.");
                    return BadRequest("Service object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid service object sent from client.");
                    return BadRequest("Invalid model object");
                }

                _repository.Service.CreateService(service);
                _repository.Save();

                return Ok(service);

                //return CreatedAtRoute(nameof(IServiceRepository.GetServiceById), new { id = service.Id },service);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateService action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateService(int id, [FromBody]Service service)
        {
            try
            {
                if (service.IsObjectNull())
                {
                    _logger.LogError("Service object sent from client is null.");
                    return BadRequest("Service object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid service object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var dbService = _repository.Service.GetServiceById(id);
                if (dbService.IsEmptyObject(id))
                {
                    _logger.LogError($"Service with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.Service.UpdateService(dbService, service);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateService action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteService(int id)
        {
            try
            {
                var service = _repository.Service.GetServiceById(id);
                if (service.IsEmptyObject(id))
                {
                    _logger.LogError($"Service with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                //if (_repository.Reservation.ReservationsByService(id).Any())
                //{
                //    _logger.LogError($"Cannot delete service with id: {id}. It has related reservations. Delete those resrevations first");
                //    return BadRequest("Cannot delete service. It has related reservations. Delete those reservations first");
                //}

                _repository.Service.DeleteService(service);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteService action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}