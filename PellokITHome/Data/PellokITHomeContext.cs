using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PellokITHome.Models;

namespace PellokITHome.Data
{
    public class PellokITHomeContext : DbContext
    {
        public PellokITHomeContext (
            DbContextOptions<PellokITHomeContext> options)
            : base(options)
        {
        }

        public DbSet<PellokITHome.Models.Article> Articles { get; set; }

        #region snippet1
        public async virtual Task<List<Article>> GetArticlesAsync()
        {
            return await Articles
                .OrderBy(Article => Article.Title)
                .AsNoTracking()
                .ToListAsync();
        }
        #endregion

        #region snippet2
        public async virtual Task AddArticleAsync(Article Article)
        {
            await Articles.AddAsync(Article);
            await SaveChangesAsync();
        }
        #endregion

        #region snippet3
        public async virtual Task DeleteAllArticlesAsync()
        {
            foreach (Article Article in Articles)
            {
                Articles.Remove(Article);
            }
            
            await SaveChangesAsync();
        }
        #endregion

        #region snippet4
        public async virtual Task DeleteArticleAsync(int id)
        {
            var Article = await Articles.FindAsync(id);

            if (Article != null)
            {
                Articles.Remove(Article);
                await SaveChangesAsync();
            }
        }
        #endregion

        public void Initialize()
        {
            Articles.AddRange(GetSeedingArticles());
            SaveChanges();
        }

        public static List<Article> GetSeedingArticles()
        {
            return new List<Article>()
            {
                new Article(){ Title = "Day01 Azure 的自我修煉" },
                new Article(){ Title = "Day02 申請Azure帳號" },
                new Article(){ Title = "Day03 Resource Group 資源群組" }
            };
        }
    }
}