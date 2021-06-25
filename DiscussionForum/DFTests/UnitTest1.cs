using System;
using Xunit;
using DFDL;
using DFBL;
using DFModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Rest = DiscussionForumREST;


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
        public void GetForumsShouldGetAllForums()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IForum _BL = new ForumBL(_repo);
    
                var ForCont = new Rest.Controllers.ForumController(_BL);

                //Act
                var returnedValue = ForCont.GetForums();
                var returnedStatus = returnedValue.Result as OkObjectResult;

                //Assert
                Assert.NotNull(returnedValue.Result);
                Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status200OK);
                Assert.IsType<List<Forum>>(returnedStatus.Value);
            }
        }

        [Fact]
        public void GetForumShouldReturnAForum()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IForum _BL = new ForumBL(_repo);

                int forumID = 631;

                var ForCont = new Rest.Controllers.ForumController(_BL);

                //Act
                var returnedValue = ForCont.GetForum(forumID);
                var returnedStatus = returnedValue.Result as ObjectResult;

                //Assert
                Assert.NotNull(returnedValue.Result);
                Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status200OK);
                Assert.IsType<Forum>(returnedStatus.Value);
            }

        }

        [Fact]
        public void AddForumsShouldReturnCreated()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IForum _BL = new ForumBL(_repo);

                var ForCont = new Rest.Controllers.ForumController(_BL);

                Forum test = new Forum()
                {
                    ForumID = 2,
                    Description = "Testing",
                    Topic = "test"
                };

                //Act
                var returnedValue = ForCont.AddForum(test);
                var returnedStatus = returnedValue.Result as NoContentResult;

                //Assert
                Assert.NotNull(returnedValue.Result);
                Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status204NoContent);
            }
        }

        [Fact]
        public void AddForumsShouldReturnBadRequestIfExists()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IForum _BL = new ForumBL(_repo);

                var ForCont = new Rest.Controllers.ForumController(_BL);

                Forum test = new Forum
                {
                    ForumID = 631,
                    Topic = "Found dogs",
                    Description = "Used for people to post about finding dogs"
                };

                //Act
                var returnedValue = ForCont.AddForum(test);
                var returnedStatus = returnedValue.Result as BadRequestResult;

                //Assert
                Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status400BadRequest);
            }
        }

        [Fact]
        public void UpdateForumShouldReturnNoContent()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IForum _BL = new ForumBL(_repo);

                var ForCont = new Rest.Controllers.ForumController(_BL);

                Forum test = new Forum()
                {
                    ForumID = 631,
                    Description = "Testing",
                    Topic = "test"
                };

                //Act
                var returnedValue = ForCont.UpdateForum(test);
                var returnedStatus = returnedValue.Result as NoContentResult;

                //Assert
                Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status204NoContent);
            }
        }

        [Fact]
        public void RemoveForumsShouldReturnNoContent()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IForum _BL = new ForumBL(_repo);

                var ForCont = new Rest.Controllers.ForumController(_BL);

                Forum test = new Forum()
                {
                    ForumID = 631,
                    Topic = "Found dogs",
                    Description = "Used for people to post about finding dogs"
                };

                //Act
                var returnedValue = ForCont.DeleteForum(test);
                var returnedStatus = returnedValue.Result as NoContentResult;

                //Assert
                Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status204NoContent);
            }
        }

        [Fact]
        public void AddPostShouldCreateAPostAndReturnCreated()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IForumPost _BL = new ForumPostBL(_repo);

                var PostCont = new Rest.Controllers.ForumPostController(_BL);

                Posts test = new Posts()
                {
                    PostID = 1,
                    ForumID = 631,
                    Topic = "test",
                    UserCreator = "Cesar_19"
                };

                //Act
                var returnedValue = PostCont.AddPost(test);
                var returnedStatus = returnedValue.Result as ObjectResult;

                //Assert
                Assert.NotNull(returnedValue.Result);
                Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status201Created);
                Assert.Equal(returnedStatus.Value, test);
            }
        }

        [Fact]
        public void GetAllPostsShouldReturnAListOfResults()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IForumPost _BL = new ForumPostBL(_repo);

                var ForCont = new Rest.Controllers.ForumPostController(_BL);

                //Act
                var returnedValue = ForCont.GetPosts();
                var returnedStatus = returnedValue.Result as ObjectResult;

                //Assert
                Assert.NotNull(returnedValue.Result);
                Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status200OK);
                Assert.IsType<List<Posts>>(returnedStatus.Value);
            }
        }

        [Fact]
        public void GetPostsShouldReturnAForumOfResults()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IForumPost _BL = new ForumPostBL(_repo);

                int ForumID = 631;

                var ForCont = new Rest.Controllers.ForumPostController(_BL);

                //Act
                var returnedValue = ForCont.GetPost(ForumID);
                var returnedStatus = returnedValue.Result as ObjectResult;

                //Assert
                Assert.NotNull(returnedValue.Result);
                Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status200OK);
                Assert.IsType<List<Posts>>(returnedStatus.Value);
            }
        }

        [Fact]
        public void DeletePostsShouldReturnNoContent()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IForumPost _BL = new ForumPostBL(_repo);

                var PostCont = new Rest.Controllers.ForumPostController(_BL);

                Posts test = new Posts()
                {
                    PostID = 7771,
                    Topic = "Lost Dogs",
                    UserCreator = "Cesar_19",
                    ForumID = 631
                };

                //Act
                var returnedValue = PostCont.DeletePost(test);
                var returnedStatus = returnedValue.Result as NoContentResult;

                //Assert
                Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status204NoContent);
            }
        }
        [Fact]
        public void UpdatePostsShouldReturnNoContent()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IForumPost _BL = new ForumPostBL(_repo);

                var PostCont = new Rest.Controllers.ForumPostController(_BL);

                Posts test = new Posts()
                {
                    PostID = 1648,
                    Topic = "Found Dog",
                    UserCreator = "Cesar_19",
                    ForumID = 631
                };

                //Act
                var returnedValue = PostCont.UpdatePost(test);
                var returnedStatus = returnedValue.Result as NoContentResult;

                //Assert
                Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status204NoContent);
            }
        }
        [Fact]
        public void AddCommentsShouldCreateAPostAndReturnCreated()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IComment _BL = new CommentBL(_repo);

                var CommCont = new Rest.Controllers.CommentController(_BL);

                Comments test = new Comments()
                {
                    CommentID = 0,
                    Created = DateTime.Now,
                    Message = "Testing message",
                    UserName = "Cesar_19",
                    PostID = 1648
                };

                //Act
                var returnedValue = CommCont.AddComment(test);
                var returnedStatus = returnedValue.Result as ObjectResult;

                //Assert
                Assert.NotNull(returnedValue.Result);
                Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status201Created);
                Assert.Equal(returnedStatus.Value, test);
            }
        }

        [Fact]
        public void AddCommentsShouldReturnBadRequestIfExists()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IComment _BL = new CommentBL(_repo);

                var CommCont = new Rest.Controllers.CommentController(_BL);

                Comments test = new Comments
                {
                    CommentID = 753,
                    PostID = 123,
                    UserName = "Cesar_19",
                    Created = new DateTime(2015, 12, 31, 5, 10, 20, DateTimeKind.Utc),
                    Message = "I just got a new dog!"
                };

                //Act
                var returnedValue = CommCont.AddComment(test);
                var returnedStatus = returnedValue.Result as BadRequestResult;

                //Assert
                Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status400BadRequest);

            }
        }

        [Fact]
        public void GetAllCommentsShouldReturnAListOfResults()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IComment _BL = new CommentBL(_repo);

                var ForCont = new Rest.Controllers.CommentController(_BL);

                //Act
                var returnedValue = ForCont.GetAllComments();
                var returnedStatus = returnedValue.Result as ObjectResult;

                //Assert
                Assert.NotNull(returnedValue.Result);
                Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status200OK);
                Assert.IsType<List<Comments>>(returnedStatus.Value);
            }
        }

        [Fact]
        public void GetCommentShouldReturnACommentOfResults()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IComment _BL = new CommentBL(_repo);

                int CommentID = 753;

                var CommCont = new Rest.Controllers.CommentController(_BL);

                //Act
                var returnedValue = CommCont.GetComment(CommentID);
                var returnedStatus = returnedValue.Result as ObjectResult;

                //Assert
                Assert.NotNull(returnedValue.Result);
                Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status200OK);
                Assert.IsType<List<Comments>>(returnedStatus.Value);
            }
        }

        [Fact]
        public void DeleteCommentsShouldReturnNoContent()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IComment _BL = new CommentBL(_repo);

                var CommentCont = new Rest.Controllers.CommentController(_BL);

                Comments test = new Comments
                {
                    CommentID = 753,
                    PostID = 123,
                    UserName = "Cesar_19",
                    Created = new DateTime(2015, 12, 31, 5, 10, 20, DateTimeKind.Utc),
                    Message = "I just got a new dog!"
                };

                //Act
                var returnedValue = CommentCont.DeleteComment(test);
                var returnedStatus = returnedValue.Result as NoContentResult;

                //Assert
                Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status204NoContent);
            }
        }
        [Fact]
        public void UpdateCommentsShouldReturnNoContent()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IComment _BL = new CommentBL(_repo);

                var PostCont = new Rest.Controllers.CommentController(_BL);

                Comments test = new Comments
                {
                    CommentID = 753,
                    PostID = 123,
                    UserName = "Cesar_19",
                    Created = new DateTime(2015, 12, 31, 5, 10, 20, DateTimeKind.Utc),
                    Message = "New Test Message!"
                };

                //Act
                var returnedValue = PostCont.UpdateComment(test);
                var returnedStatus = returnedValue.Result as NoContentResult;

                //Assert
                Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status204NoContent);
            }
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
                            UserCreator = "Cesar_19",
                            ForumID = 631
                        },
                        new Posts
                        {
                            PostID = 1648,
                            Topic = "Found Dogs",
                            UserCreator = "Cesar_19",
                            ForumID = 631
                        }

                    );
                    context.SaveChanges();
            }
        }
     }
}
