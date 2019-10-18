using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;
using Entities.ExtendedModels;
using Microsoft.AspNetCore.Mvc;
using Entities.Extentsions;

namespace Repository
{
    public class ArticleRepository: RepositoryBase<Article>, IArticleRepository
    {
        public ArticleRepository(RepositoryContext repositoryContext) :base(repositoryContext)
        {

        }

        public IEnumerable<Article> GetAllArticles()
        {
            return FindAll().OrderBy(x => x.Id).ToList();
        }

        [HttpGet("{id}", Name = nameof(GetArticleById))]
        public Article GetArticleById(int articleId)
        {
            return FindByCondition(article => article.Id.Equals(articleId)).DefaultIfEmpty(new Article()).FirstOrDefault();
        }

        public void CreateArticle(Article article)
        {
            article.PublishingTime = DateTime.UtcNow;
            Create(article);
        }

        public void UpdateArticle(Article dbArticle, Article article)
        {
            dbArticle.Map(article);
            Update(dbArticle);
        }

        public void DeleteArticle(Article article)
        {
            Delete(article);
        }
    }
}
