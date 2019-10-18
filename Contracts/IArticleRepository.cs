using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;

namespace Contracts
{
    public interface IArticleRepository : IRepositoryBase<Article>
    {
        IEnumerable<Article> GetAllArticles();
        Article GetArticleById(int articleId);
        void CreateArticle(Article article);
        void UpdateArticle(Article dbArticle, Article article);
        void DeleteArticle(Article article);
    }
}
