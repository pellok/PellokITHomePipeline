using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using PellokITHome.Data;
using PellokITHome.Models;

namespace Tests.UnitTests
{
    public class DataAccessLayerTest
    {
        [Fact]
        public async Task GetMessagesAsync_MessagesAreReturned()
        {
            using (var db = new PellokITHomeContext(Utilities.TestDbContextOptions()))
            {
                // Arrange
                var expectedMessages = PellokITHomeContext.GetSeedingArticles();
                await db.AddRangeAsync(expectedMessages);
                await db.SaveChangesAsync();

                // Act
                var result = await db.GetArticlesAsync();

                // Assert
                var actualMessages = Assert.IsAssignableFrom<List<Article>>(result);
                Assert.Equal(
                    expectedMessages.OrderBy(m => m.ID).Select(m => m.Title), 
                    actualMessages.OrderBy(m => m.ID).Select(m => m.Title));
            }
        }

        [Fact]
        public async Task AddMessageAsync_MessageIsAdded()
        {
            using (var db = new PellokITHomeContext(Utilities.TestDbContextOptions()))
            {
                // Arrange
                var recId = 10;
                var expectedMessage = new Article() { ID = recId, Title = "Message" };

                // Act
                await db.AddArticleAsync(expectedMessage);

                // Assert
                var actualMessage = await db.FindAsync<Article>(recId);
                Assert.Equal(expectedMessage, actualMessage);
            }
        }

        [Fact]
        public async Task DeleteAllMessagesAsync_MessagesAreDeleted()
        {
            using (var db = new PellokITHomeContext(Utilities.TestDbContextOptions()))
            {
                // Arrange
                var seedMessages = PellokITHomeContext.GetSeedingArticles();
                await db.AddRangeAsync(seedMessages);
                await db.SaveChangesAsync();

                // Act
                await db.DeleteAllArticlesAsync();

                // Assert
                Assert.Empty(await db.Articles.AsNoTracking().ToListAsync());
            }
        }

        [Fact]
        public async Task DeleteMessageAsync_MessageIsDeleted_WhenMessageIsFound()
        {
            using (var db = new PellokITHomeContext(Utilities.TestDbContextOptions()))
            {
                #region snippet1
                // Arrange
                var seedMessages = PellokITHomeContext.GetSeedingArticles();
                await db.AddRangeAsync(seedMessages);
                await db.SaveChangesAsync();
                var recId = 1;
                var expectedMessages = 
                    seedMessages.Where(message => message.ID != recId).ToList();
                #endregion

                #region snippet2
                // Act
                await db.DeleteArticleAsync(recId);
                #endregion

                #region snippet3
                // Assert
                var actualMessages = await db.Articles.AsNoTracking().ToListAsync();
                Assert.Equal(
                    expectedMessages.OrderBy(m => m.ID).Select(m => m.Title), 
                    actualMessages.OrderBy(m => m.ID).Select(m => m.Title));
                #endregion
            }
        }

        #region snippet4
        [Fact]
        public async Task DeleteMessageAsync_NoMessageIsDeleted_WhenMessageIsNotFound()
        {
            using (var db = new PellokITHomeContext(Utilities.TestDbContextOptions()))
            {
                // Arrange
                var expectedMessages = PellokITHomeContext.GetSeedingArticles();
                await db.AddRangeAsync(expectedMessages);
                await db.SaveChangesAsync();
                var recId = 4;

                // Act
                try
                {
                    await db.DeleteArticleAsync(recId);
                }
                catch
                {
                    // recId doesn't exist
                }

                // Assert
                var actualMessages = await db.Articles.AsNoTracking().ToListAsync();
                Assert.Equal(
                    expectedMessages.OrderBy(m => m.ID).Select(m => m.Title), 
                    actualMessages.OrderBy(m => m.ID).Select(m => m.Title));
            }
        }
        #endregion
    }
}