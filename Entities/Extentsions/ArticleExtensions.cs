using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;

namespace Entities.Extentsions
{
    public static class ArticleExtensions
    {
        public static void Map(this Article dbArticle, Article article)
        {
            dbArticle.Id = article.Id;
            dbArticle.UserId = article.UserId;
            dbArticle.PublishingTime = article.PublishingTime;
            dbArticle.Title = article.Title;
            dbArticle.Picture = article.Picture;
            dbArticle.Text = article.Text;
        }
    }
}
