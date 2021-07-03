using System;
using Xunit;
using DFDL;
using DFBL;
using DFModels;
using DTO = DiscussionForumREST.DTO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Rest = DiscussionForumREST;
using Microsoft.Extensions.Options;
using Moq;
using DiscussionForumREST;
using System.Security.Claims;
using DiscussionForumREST.DTO;

namespace DFTests
{
    public class UnitTest1
    {
        private readonly DbContextOptions<DFDBContext> options;

        //Xunit creates new instances of test classes, you need to make sure that you seed your db for each class
        public UnitTest1()
        {
            options = new DbContextOptionsBuilder<DFDBContext>().UseSqlite("Filename=Test.db").Options;
            Seed();
        }

        [Fact]
        public void GetAllComments()
        {

            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                var results = _repo.GetAllCommentsAsync();
                Assert.Equal(2, results.Result.Count);
            }
        }
        [Fact]
        public void GetAllForums()
        {

            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                var results = _repo.GetAllForumsAsync();
                Assert.Equal(1, results.Result.Count);
            }
        }
        [Fact]
        public void GetAllPosts()
        {

            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                var results = _repo.GetAllPostsAsync();
                Assert.Equal(2, results.Result.Count);
            }
        }
        [Fact]
        public void ValidAddComment()
        {
            int CommentID = 9642;
            int PostID = 5631;
            string UserName = "Miggy_Cubbies";
            DateTime Created = new DateTime(2017, 8, 4, 8, 16, 21, DateTimeKind.Utc);
            string Message = "I just lost my dog!";

            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                _repo.AddCommentsAsync
                (
                    new Comments(CommentID, PostID, UserName, Created, Message)
                );
            }
            using (var assertContext = new DFDBContext(options))
            {

                var result = assertContext.Comments.FirstOrDefault(comm => comm.CommentID == CommentID);
                Assert.NotNull(result);
                Assert.Equal(UserName, result.UserName);
            }
        }
        [Fact]
        public void ValidAddForum()
        {
            int ForumID = 761;
            string Topic = "Lost dogs";
            string Description = "Used for people to post about losing dogs";

            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                _repo.AddForumAsync
                (
                    new Forum(ForumID, Topic, Description)
                );
            }
            using (var assertContext = new DFDBContext(options))
            {

                var result = assertContext.Forums.FirstOrDefault(foru => foru.ForumID == ForumID);
                Assert.NotNull(result);
                Assert.Equal(ForumID, result.ForumID);
            }
        }
        [Fact]
        public void ValidAddPosts()
        {
            int PostID = 7623;
            string Topic = "Found Dogs";
            string UserCreator = "Pepe_Rios";
            int ForumID = 3486;

            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                _repo.AddPostsAsync
                (
                    new Posts(PostID, Topic, UserCreator, ForumID)
                );
            }
            using (var assertContext = new DFDBContext(options))
            {

                var result = assertContext.Posts.FirstOrDefault(pst => pst.PostID == PostID);
                Assert.NotNull(result);
                Assert.Equal(ForumID, result.ForumID);
            }
        }
        [Fact]
        public void NotValidAddComment()
        {
            int CommentID = 753;
            int PostID = 123;
            string UserName = "Pepe_Rios";
            DateTime Created = new DateTime(2015, 12, 31, 5, 10, 20, DateTimeKind.Utc);
            string Message = "I just got a new dog!";

            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                _repo.AddCommentsAsync
                (
                    new Comments(CommentID, PostID, UserName, Created, Message)
                );
            }
            using (var assertContext = new DFDBContext(options))
            {

                var result = assertContext.Comments.FirstOrDefault(comm => comm.CommentID == CommentID);
                Assert.NotNull(result);
                Assert.NotEqual(UserName, result.UserName);
            }
        }
        [Fact]
        public void NotValidAddForum()
        {
            int ForumID = 631;
            string Topic = "Dogs";
            string Description = "Used for people to post about dogs";

            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                _repo.AddForumAsync
                (
                    new Forum(ForumID, Topic, Description)
                );
            }
            using (var assertContext = new DFDBContext(options))
            {

                var result = assertContext.Forums.FirstOrDefault(foru => foru.ForumID == ForumID);
                Assert.NotNull(result);
                Assert.NotEqual(Topic, result.Topic);
            }
        }
        [Fact]
        public void NotValidAddPosts()
        {
            int PostID = 7771;
            string Topic = "Lost Dogs";
            string UserCreator = "Cesar_19";
            int ForumID = 6479;

            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                _repo.AddPostsAsync
                (
                    new Posts(PostID, Topic, UserCreator, ForumID)
                );
            }
            using (var assertContext = new DFDBContext(options))
            {

                var result = assertContext.Posts.FirstOrDefault(pst => pst.PostID == PostID);
                Assert.NotNull(result);
                Assert.NotEqual(ForumID, result.ForumID);
            }
        }

        [Fact]
        public async void GetForumAsyncShouldWork()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                Forum toFind = await _repo.AddForumAsync(new Forum(12, "test 1", "test"));

                Forum found = await _repo.GetForumAsync(toFind);

                Assert.Equal(toFind.ForumID, found.ForumID);
                Assert.Equal(toFind.Topic, found.Topic);
            }
        }

        [Fact]
        public async void  GetPostsByIdAsyncShouldWork()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                Posts toFind = await _repo.AddPostsAsync(new Posts(1, "test", "new user", 1));

                Posts found = await _repo.GetPostsByIdAsync(1);

                Assert.Equal(toFind.PostID, found.PostID);
                Assert.Equal(toFind.Topic, found.Topic);
            }
        }

        [Fact]
        public async void GetPostsAsyncShouldWork()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                Posts toFind = await _repo.AddPostsAsync(new Posts(1, "test", "new user", 1));

                Posts found = await _repo.GetPostsAsync(toFind);

                Assert.Equal(toFind.PostID, found.PostID);
                Assert.Equal(toFind.Topic, found.Topic);
            }
        }

        [Fact]
        public async void GetCommentsAsyncShouldWork()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                Comments toFind = await _repo.AddCommentsAsync(new Comments(1, 1, "test", DateTime.Now, "test"));

                Comments found = await _repo.GetCommentsAsync(toFind);

                Assert.Equal(toFind.CommentID, found.CommentID);
                Assert.Equal(toFind.PostID, found.PostID);
                
            }
        }

        [Fact]
        public void CheckScopeAuthShouldWork()
        {
            HasScopeRequirement scope = new HasScopeRequirement("testScope", "testIssuer");
            string expectedScope = "testScope";
            string expectedIssue = "testIssuer";
            Assert.Equal(scope.Scope, expectedScope);
            Assert.Equal(scope.Issuer, expectedIssue);

        }

        private void Seed()
        {
            using (var context = new DFDBContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Comments.AddRange
                    (
                        new Comments
                        {
                            CommentID = 753,
                            PostID = 123,
                            UserName = "Cesar_19",
                            Created = new DateTime(2015, 12, 31, 5, 10, 20, DateTimeKind.Utc),
                            Message = "I just got a new dog!"
                        },
                        new Comments
                        {
                            CommentID = 867,
                            PostID = 5309,
                            UserName = "Pepe_Rios",
                            Created = new DateTime(2019, 1, 3, 5, 1, 2, DateTimeKind.Utc),
                            Message = "I just lost my dog!"
                        }

                    );
                context.Forums.AddRange
                    (
                        new Forum
                        {
                            ForumID = 631,
                            Topic = "Found dogs",
                            Description = "Used for people to post about finding dogs"
                        }

                    );
                context.Posts.AddRange
                    (
                        new Posts
                        {
                            PostID = 7771,
                            Topic = "Lost Dogs",
                            UserName = "Cesar_19",
                            ForumID = 631
                        },
                        new Posts
                        {
                            PostID = 1648,
                            Topic = "Found Dogs",
                            UserName = "Cesar_19",
                            ForumID = 631
                        }

                    );
                    context.SaveChanges();
            }
        }
     }
}
