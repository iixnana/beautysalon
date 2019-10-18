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
    [Route("api/article")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;

        public ArticleController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAllArticles()
        {
            try
            {
                var articles = _repository.Article.GetAllArticles();

                _logger.LogInfo($"Returned all articles from database.");

                return Ok(articles);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllArticles action: {ex.Message}");
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetArticleById(int id)
        {
            try
            {
                var article = _repository.Article.GetArticleById(id);

                if (article.IsEmptyObject(id))
                {
                    _logger.LogError($"Article with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned article with id: {id}");
                    return Ok(article);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetArticleById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult CreateArticle([FromBody]Article article)
        {
            try
            {
                //article.CreationDate = DateTime.UtcNow;
                if (article.IsObjectNull())
                {
                    _logger.LogError("Article object sent from client is null.");
                    return BadRequest("Article object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid article object sent from client.");
                    return BadRequest("Invalid model object");
                }

                _repository.Article.CreateArticle(article);
                _repository.Save();

                return Ok(article);

                //return CreatedAtRoute(nameof(IArticleRepository.GetArticleById), new { id = article.Id },article);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateArticle action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateArticle(int id, [FromBody]Article article)
        {
            try
            {
                if (article.IsObjectNull())
                {
                    _logger.LogError("Article object sent from client is null.");
                    return BadRequest("Article object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid article object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var dbArticle = _repository.Article.GetArticleById(id);
                if (dbArticle.IsEmptyObject(id))
                {
                    _logger.LogError($"Article with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.Article.UpdateArticle(dbArticle, article);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateArticle action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteArticle(int id)
        {
            try
            {
                var article = _repository.Article.GetArticleById(id);
                if (article.IsEmptyObject(id))
                {
                    _logger.LogError($"Article with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                //if (_repository.Reservation.ReservationsByArticle(id).Any())
                //{
                //    _logger.LogError($"Cannot delete article with id: {id}. It has related reservations. Delete those resrevations first");
                //    return BadRequest("Cannot delete article. It has related reservations. Delete those reservations first");
                //}

                _repository.Article.DeleteArticle(article);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteArticle action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}