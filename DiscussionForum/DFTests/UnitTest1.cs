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
        public async void GetForumsExceptionShouldReturnNotFound()
        {
            var mockBL = new Mock<IForum>();
            mockBL.Setup(x => x.GetAllForums()).Throws(new Exception("exception test"));



            var controller = new Rest.Controllers.ForumController(mockBL.Object);
            var result = await controller.GetForums();
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void GetForumExceptionShouldReturnNotFound()
        {
            var mockBL = new Mock<IForum>();
            mockBL.Setup(x => x.GetForum(It.IsAny<int>())).Throws(new Exception("exception test"));



            var controller = new Rest.Controllers.ForumController(mockBL.Object);
            var result = await controller.GetForum(1);
            Assert.IsType<NotFoundResult>(result);
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
        public async void UpdateForumExceptionShouldReturnBadRequest()
        {
            var mockBL = new Mock<IForum>();
            mockBL.Setup(x => x.UpdateForum(It.IsAny<Forum>())).Throws(new Exception("exception test"));



            var controller = new Rest.Controllers.ForumController(mockBL.Object);
            var result = await controller.UpdateForum(new Forum());
            Assert.IsType<BadRequestObjectResult>(result);
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
                var returnedValue = ForCont.DeleteForum(test.ForumID);
                var returnedStatus = returnedValue.Result as NoContentResult;

                //Assert
                Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status204NoContent);
            }
        }

        [Fact]
        public async void RemoveForumsExceptionShouldReturnBadRequest()
        {
            var mockBL = new Mock<IForum>();
            mockBL.Setup(x => x.RemoveForum(It.IsAny<int>())).Throws(new Exception("exception test"));



            var controller = new Rest.Controllers.ForumController(mockBL.Object);
            var result = await controller.DeleteForum(1);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void AddPostShouldCreateAPostAndReturnCreated()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IForumPost _BL = new ForumPostBL(_repo);
                var mockUserSettings = new Mock<IOptions<ApiSettings>>();

                var PostCont = new Rest.Controllers.ForumPostController(_BL, mockUserSettings.Object);

                DTO.AddForumPostInput test = new DTO.AddForumPostInput()
                {
                    ForumID = 631,
                    Topic = "test",
                    Description = "test description"
                };

                //Act
                var returnedValue = PostCont.AddPost(test);
                var returnedStatus = returnedValue.Result as ObjectResult;

                //Assert
                Assert.NotNull(returnedValue.Result);
                //Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status201Created);
                //Assert.Equal(returnedStatus.Value, test);
            }
        }

        [Fact]
        public async void AddPostExceptionShouldReturnBadRequest()
        {
            var mockBL = new Mock<IForumPost>();
            mockBL.Setup(x => x.AddPost(It.IsAny<Posts>())).Throws(new Exception("exception test"));
            var mockUserSettings = new Mock<IOptions<ApiSettings>>();



            var controller = new Rest.Controllers.ForumPostController(mockBL.Object,mockUserSettings.Object);
            var result = await controller.AddPost(new DTO.AddForumPostInput());
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void GetAllPostsShouldReturnAListOfResults()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IForumPost _BL = new ForumPostBL(_repo);
                var mockUserSettings = new Mock<IOptions<ApiSettings>>();

                var ForCont = new Rest.Controllers.ForumPostController(_BL, mockUserSettings.Object);

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
        public async void GetAllPostsExceptionShouldReturnNotFound()
        {
            var mockBL = new Mock<IForumPost>();
            mockBL.Setup(x => x.GetAllPosts()).Throws(new Exception("exception test"));
            var mockUserSettings = new Mock<IOptions<ApiSettings>>();



            var controller = new Rest.Controllers.ForumPostController(mockBL.Object, mockUserSettings.Object);
            var result = await controller.GetPosts();
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetPostsShouldReturnAForumOfResults()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IForumPost _BL = new ForumPostBL(_repo);
                var mockUserSettings = new Mock<IOptions<ApiSettings>>();

                int ForumID = 631;

                var ForCont = new Rest.Controllers.ForumPostController(_BL, mockUserSettings.Object);

                //Act
                var returnedValue = ForCont.GetPost(ForumID);
                var returnedStatus = returnedValue.Result as ObjectResult;

                //Assert
                Assert.NotNull(returnedValue.Result);
                //Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status200OK);
                //Assert.IsType<List<Posts>>(returnedStatus.Value);
            }
        }

        [Fact]
        public async void GetPostsExceptionShouldReturnNotFound()
        {
            var mockBL = new Mock<IForumPost>();
            mockBL.Setup(x => x.GetPostForForumWithID(It.IsAny<int>())).Throws(new Exception("exception test"));
            var mockUserSettings = new Mock<IOptions<ApiSettings>>();



            var controller = new Rest.Controllers.ForumPostController(mockBL.Object, mockUserSettings.Object);
            var result = await controller.GetPost(1);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeletePostsShouldReturnNoContent()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IForumPost _BL = new ForumPostBL(_repo);
                var mockUserSettings = new Mock<IOptions<ApiSettings>>();

                var PostCont = new Rest.Controllers.ForumPostController(_BL, mockUserSettings.Object);

                Posts test = new Posts()
                {
                    PostID = 7771,
                    Topic = "Lost Dogs",
                    UserName = "Cesar_19",
                    ForumID = 631
                };

                //Act
                var returnedValue = PostCont.DeletePost(test.PostID);
                var returnedStatus = returnedValue.Result as NoContentResult;

                //Assert
                //Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status204NoContent);
            }
        }

        [Fact]
        public async void DeletePostsExceptionShouldReturnBadRequest()
        {
            var mockBL = new Mock<IForumPost>();
            mockBL.Setup(x => x.RemovePost(new Posts())).Throws(new Exception("exception test"));
            var mockUserSettings = new Mock<IOptions<ApiSettings>>();



            var controller = new Rest.Controllers.ForumPostController(mockBL.Object, mockUserSettings.Object);
            var result = await controller.DeletePost(1);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void UpdatePostsShouldReturnNoContent()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IForumPost _BL = new ForumPostBL(_repo);
                var mockUserSettings = new Mock<IOptions<ApiSettings>>();

                var PostCont = new Rest.Controllers.ForumPostController(_BL, mockUserSettings.Object);

                DTO.ForumPostInput test = new DTO.ForumPostInput()
                {
                    PostID = 1648,
                    Topic = "Found Dog",
                    Description = "test description",
                    ForumID = 631
                };

                //Act
                var returnedValue = PostCont.UpdatePost(test);
                var returnedStatus = returnedValue.Result as NoContentResult;

                //Assert
                //Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status204NoContent);
            }
        }

        [Fact]
        public async void UpdatePostsExceptionShouldReturnBadRequest()
        {
            var mockBL = new Mock<IForumPost>();
            mockBL.Setup(x => x.UpdatePost(It.IsAny<Posts>())).Throws(new Exception("exception test"));
            var mockUserSettings = new Mock<IOptions<ApiSettings>>();



            var controller = new Rest.Controllers.ForumPostController(mockBL.Object, mockUserSettings.Object);
            var result = await controller.UpdatePost(new DTO.ForumPostInput());
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void AddCommentsShouldCreateAPostAndReturnCreated()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IComment _BL = new CommentBL(_repo);

                var CommCont = new Rest.Controllers.CommentController(_BL);

                Rest.DTO.AddCommnetInput test = new Rest.DTO.AddCommnetInput
                {
                    PostID = 123,
                    Created = new DateTime(2015, 12, 31, 5, 10, 20, DateTimeKind.Utc),
                    Message = "I just got a new dog!"
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

                Rest.DTO.AddCommnetInput test = new Rest.DTO.AddCommnetInput
                {
                    PostID = 123,                   
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
        public async void GetAllCommentsExceptionShouldReturnNotFound()
        {
            var mockBL = new Mock<IComment>();
            mockBL.Setup(x => x.GetAllComments()).Throws(new Exception("exception test"));



            var controller = new Rest.Controllers.CommentController(mockBL.Object);
            var result = await controller.GetAllComments();
            Assert.IsType<NotFoundResult>(result);
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
        public async void GetCommentExceptionShouldReturnNotFound()
        {
            var mockBL = new Mock<IComment>();
            mockBL.Setup(x => x.GetComment(It.IsAny<int>())).Throws(new Exception("exception test"));



            var controller = new Rest.Controllers.CommentController(mockBL.Object);
            var result = await controller.GetComment(1);
            Assert.IsType<NotFoundResult>(result);
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
                var returnedValue = CommentCont.DeleteComment(test.CommentID);
                var returnedStatus = returnedValue.Result as NoContentResult;

                //Assert
                Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status204NoContent);
            }
        }

        [Fact]
        public async void DeleteCommentExceptionShouldReturnBadRequest()
        {
            var mockBL = new Mock<IComment>();
            mockBL.Setup(x => x.RemoveComments(It.IsAny<int>())).Throws(new Exception("exception test"));



            var controller = new Rest.Controllers.CommentController(mockBL.Object);
            var result = await controller.DeleteComment(1);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void UpdateCommentsShouldReturnNoContent()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IComment _BL = new CommentBL(_repo);

                var PostCont = new Rest.Controllers.CommentController(_BL);

                Rest.DTO.UpdateCommentInput test = new Rest.DTO.UpdateCommentInput
                {
                    PostID = 123,
                    Created = new DateTime(2015, 12, 31, 5, 10, 20, DateTimeKind.Utc),
                    Message = "I just got a new dog!"
                };

                //Act
                var returnedValue = PostCont.UpdateComment(test);
                var returnedStatus = returnedValue.Result as NoContentResult;

                //Assert
                Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status204NoContent);
            }
        }

        [Fact]
        public async void UpdateCommentExceptionShouldReturnBadRequest()
        {
            var mockBL = new Mock<IComment>();
            mockBL.Setup(x => x.UpdateComment(It.IsAny<Comments>())).Throws(new Exception("exception test"));



            var controller = new Rest.Controllers.CommentController(mockBL.Object);
            var result = await controller.UpdateComment(new Rest.DTO.UpdateCommentInput());
            Assert.IsType<BadRequestResult>(result);
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
