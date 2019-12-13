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
    [Route("api/article")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;
        private readonly IAuthService _authService;

        public ArticleController(ILoggerManager logger, IRepositoryWrapper repository, IAuthService authService)
        {
            _logger = logger;
            _repository = repository;
            _authService = authService;
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpGet("author/{id}")]
        public IActionResult GetArticlesByAuthor(int id)
        {
            try
            {
                var articles = _repository.Article.GetArticlesByMaster(id);

                _logger.LogInfo($"Returned all articles from database.");

                return Ok(articles);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllArticles action: {ex.Message}");
                return NotFound();
            }
        }

        [AllowAnonymous]
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

        [Authorize(Roles = Role.Master)]
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

                if (_authService.IsMaster(Request.Headers["Authorization"]) && _authService.GetId(Request.Headers["Authorization"]) != article.UserId)
                {
                    _logger.LogError($"User with id: {_authService.GetId(Request.Headers["Authorization"])}, has tried to access restricted data in CreateArticle.");
                    return Unauthorized();
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

        [Authorize(Roles = Role.MasterAdmin)]
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

                if (_authService.IsMaster(Request.Headers["Authorization"]) && _authService.GetId(Request.Headers["Authorization"]) != article.UserId)
                {
                    _logger.LogError($"User with id: {_authService.GetId(Request.Headers["Authorization"])}, has tried to access restricted data in UpdateArticle.");
                    return Unauthorized();
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

        [Authorize(Roles = Role.MasterAdmin)]
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

                if (_authService.IsMaster(Request.Headers["Authorization"]) && _authService.GetId(Request.Headers["Authorization"]) != article.UserId)
                {
                    _logger.LogError($"User with id: {_authService.GetId(Request.Headers["Authorization"])}, has tried to access restricted data in DeleteArticle.");
                    return Unauthorized();
                }

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