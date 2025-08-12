using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Duo.Api.Persistence;
using Duo.Api.Models;
using Duo.Api.Repositories.Repos;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace TestProject1.Repositories
{
    public class HashtagRepositoryTests : IDisposable
    {
        private readonly DataContext _mockContext;
        private readonly HashtagRepository _repository;

        public HashtagRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _mockContext = new DataContext(options);
            _repository = new HashtagRepository(_mockContext);

            // Seed test data
            SeedTestData();
        }

        private void SeedTestData()
        {
            // Clear existing data
            _mockContext.Hashtags.RemoveRange(_mockContext.Hashtags);
            _mockContext.PostHashtags.RemoveRange(_mockContext.PostHashtags);
            _mockContext.Posts.RemoveRange(_mockContext.Posts);
            _mockContext.SaveChanges();

            // Add test posts
            var posts = new List<Post>
            {
                new Post { Id = 1, Title = "Test Post 1", Content = "Content 1" },
                new Post { Id = 2, Title = "Test Post 2", Content = "Content 2" }
            };

            _mockContext.Posts.AddRange(posts);
            _mockContext.SaveChanges();

            // Add test hashtags
            var hashtags = new List<Hashtag>
            {
                new Hashtag { Id = 1, Tag = "test" },
                new Hashtag { Id = 2, Tag = "tag1" },
                new Hashtag { Id = 3, Tag = "tag2" }
            };

            _mockContext.Hashtags.AddRange(hashtags);
            _mockContext.SaveChanges();

            // Add test post-hashtag relationships
            var postHashtags = new List<PostHashtags>
            {
                new PostHashtags { PostId = 1, HashtagId = 1 },
                new PostHashtags { PostId = 1, HashtagId = 2 },
                new PostHashtags { PostId = 1, HashtagId = 3 }
            };

            _mockContext.PostHashtags.AddRange(postHashtags);
            _mockContext.SaveChanges();
        }

        public void Dispose()
        {
            _mockContext.Dispose();
        }

 

        [Fact]
        public async Task GetHashtags_ReturnsAllHashtags()
        {
            // Act
            var result = await _repository.GetHashtags();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Contains(result, hashtag => hashtag.Tag == "test");
            Assert.Contains(result, hashtag => hashtag.Tag == "tag1");
            Assert.Contains(result, hashtag => hashtag.Tag == "tag2");
        }

        [Fact]
        public async Task CreateHashtag_WithValidHashtag_ReturnsId()
        {
            // Arrange
            var hashtag = new Hashtag { Tag = "newtag" };

            // Act
            var result = await _repository.CreateHashtag(hashtag);

            // Assert
            Assert.True(result > 0);
            var createdHashtag = await _mockContext.Hashtags.FindAsync(result);
            Assert.NotNull(createdHashtag);
            Assert.Equal("newtag", createdHashtag.Tag);
        }

        [Fact]
        public async Task CreateHashtag_WithNullHashtag_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.CreateHashtag(null));
        }

        [Fact]
        public async Task AddHashtagToPost_WithValidIds_AddsRelationship()
        {
            // Act
            await _repository.AddHashtagToPost(2, 1);

            // Assert
            var relationship = await _mockContext.PostHashtags
                .FirstOrDefaultAsync(postHashtag => postHashtag.PostId == 2 && postHashtag.HashtagId == 1);
            Assert.NotNull(relationship);
        }

        [Fact]
        public async Task AddHashtagToPost_WithNonExistentPost_ThrowsException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _repository.AddHashtagToPost(999, 1));
        }

        [Fact]
        public async Task AddHashtagToPost_WithNonExistentHashtag_ThrowsException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _repository.AddHashtagToPost(1, 999));
        }

        [Fact]
        public async Task RemoveHashtagFromPost_WithValidIds_RemovesRelationship()
        {
            // Act
            await _repository.RemoveHashtagFromPost(1, 1);

            // Assert
            var relationship = await _mockContext.PostHashtags
                .FirstOrDefaultAsync(postHashtag => postHashtag.PostId == 1 && postHashtag.HashtagId == 1);
            Assert.Null(relationship);
        }

        [Fact]
        public async Task GetAllPostHashtags_ReturnsAllRelationships()
        {
            // Act
            var result = await _repository.GetAllPostHashtags();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Contains(result, postHashtag => postHashtag.PostId == 1 && postHashtag.HashtagId == 1);
            Assert.Contains(result, postHashtag => postHashtag.PostId == 1 && postHashtag.HashtagId == 2);
            Assert.Contains(result, postHashtag => postHashtag.PostId == 1 && postHashtag.HashtagId == 3);
        }
    }
}