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
    public class ControllerTest
    {
        private readonly DbContextOptions<DFDBContext> options;

        //Xunit creates new instances of test classes, you need to make sure that you seed your db for each class
        public ControllerTest()
        {
            options = new DbContextOptionsBuilder<DFDBContext>().UseSqlite("Filename=CTest.db").Options;
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
        public void GetCommentsShouldGetAllComments()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IComment _BL = new CommentBL(_repo);

                //var ForCont = new Rest.Controllers.ForumController(_BL);

                //Act
                List<Comments> returnedValue = _BL.GetAllComments().Result;
                int expected = 2;

                //Assert
                Assert.NotNull(returnedValue);
                Assert.Equal(returnedValue.Count, expected);

            }
        }

        [Fact]
        public void GetPostsShouldGetAllPosts()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IForumPost _BL = new ForumPostBL(_repo);

                //var ForCont = new Rest.Controllers.ForumController(_BL);

                //Act
                List<Posts> returnedValue = _BL.GetAllPosts().Result;
                int expected = 2;

                //Assert
                Assert.NotNull(returnedValue);
                Assert.Equal(returnedValue.Count, expected);

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
                var settings = Options.Create(new ApiSettings());

                var PostCont = new Rest.Controllers.ForumPostController(_BL, settings);

                var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "BZ")
                }));

                PostCont.ControllerContext = new ControllerContext();
                PostCont.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

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
                Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status201Created);
                Assert.IsType<Posts>(returnedStatus.Value);
            }
        }

        [Fact]
        public async void AddPostExceptionShouldReturnBadRequest()
        {
            var mockBL = new Mock<IForumPost>();
            mockBL.Setup(x => x.AddPost(It.IsAny<Posts>())).Throws(new Exception("exception test"));
            var mockUserSettings = new Mock<IOptions<ApiSettings>>();



            var controller = new Rest.Controllers.ForumPostController(mockBL.Object, mockUserSettings.Object);
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
                var settings = Options.Create(new ApiSettings());

                int ForumID = 631;

                var ForCont = new Rest.Controllers.ForumPostController(_BL, settings);

                var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "BZ")
                }));


                ForCont.ControllerContext = new ControllerContext();
                ForCont.ControllerContext.HttpContext = new DefaultHttpContext { User = user };



                //Act
                var returnedValue = ForCont.GetPost(ForumID);
                var returnedStatus = returnedValue.Result as ObjectResult;

                //Assert
                Assert.NotNull(returnedValue);
                Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status200OK);
                Assert.IsType<List<ForumPostOutput>>(returnedStatus.Value);
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
        public async void DeletePostsShouldReturnNoContent()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IForumPost _BL = new ForumPostBL(_repo);
                var mockUserSettings = new Mock<IOptions<ApiSettings>>();

                var PostCont = new Rest.Controllers.ForumPostController(_BL, mockUserSettings.Object);

                var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "BZ")
                }));


                PostCont.ControllerContext = new ControllerContext();
                PostCont.ControllerContext.HttpContext = new DefaultHttpContext { User = user };


                Posts test = new Posts()
                {
                    PostID = 2020,
                    Topic = "Test",
                    UserName = "test",
                    ForumID = 2
                };

                Posts added = await _BL.AddPost(test);

                //Act
                var returnedValue = PostCont.DeletePost(added.PostID);
                var returnedStatus = returnedValue.Result as NoContentResult;

                //Assert
                Assert.IsType<NoContentResult>(returnedValue.Result);
                Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status204NoContent);
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
                var settings = Options.Create(new ApiSettings());

                var PostCont = new Rest.Controllers.ForumPostController(_BL, settings);

                var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "BZ")
                }));

                PostCont.ControllerContext = new ControllerContext();
                PostCont.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

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
                Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status204NoContent);
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
                var settings = Options.Create(new ApiSettings());

                var CommCont = new Rest.Controllers.CommentController(_BL, settings);

                var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "BZ")
                }));

                CommCont.ControllerContext = new ControllerContext();
                CommCont.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

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
                Assert.IsType<Comments>(returnedStatus.Value);
            }
        }

        [Fact]
        public void AddCommentsShouldReturnBadRequestIfExists()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IComment _BL = new CommentBL(_repo);

                var mockUserSettings = new Mock<IOptions<ApiSettings>>();

                var CommCont = new Rest.Controllers.CommentController(_BL, mockUserSettings.Object);

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

                var settings = Options.Create(new ApiSettings());

                var CommCont = new Rest.Controllers.CommentController(_BL, settings);

                var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "BZ")
                }));

                CommCont.ControllerContext = new ControllerContext();
                CommCont.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

                //Act
                var returnedValue = CommCont.GetAllComments();
                var returnedStatus = returnedValue.Result as ObjectResult;

                //Assert
                Assert.NotNull(returnedValue.Result);
                Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status200OK);
                Assert.IsType<List<CommentOutput>>(returnedStatus.Value);
            }
        }

        [Fact]
        public async void GetAllCommentsExceptionShouldReturnNotFound()
        {
            var mockBL = new Mock<IComment>();
            mockBL.Setup(x => x.GetAllComments()).Throws(new Exception("exception test"));
            var mockUserSettings = new Mock<IOptions<ApiSettings>>();

            var CommCont = new Rest.Controllers.CommentController(mockBL.Object, mockUserSettings.Object);
            var result = await CommCont.GetAllComments();
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetCommentShouldReturnACommentOfResults()
        {
            using (var context = new DFDBContext(options))
            {

                var _BL = new Mock<IComment>();
                _BL.Setup(x => x.GetComment(It.IsAny<int>())).ReturnsAsync(
                        new List<Comments>
                        {
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
                        }
                    );

                int CommentID = 753;

                var settings = Options.Create(new ApiSettings());

                var CommCont = new Rest.Controllers.CommentController(_BL.Object, settings);

                var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "BZ")
                }));

                CommCont.ControllerContext = new ControllerContext();
                CommCont.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

                //Act
                var returnedValue = CommCont.GetComment(CommentID);
                var returnedStatus = returnedValue.Result as ObjectResult;

                //Assert
                Assert.NotNull(returnedValue.Result);
                Assert.Equal(returnedStatus.StatusCode, StatusCodes.Status200OK);
                Assert.IsType<List<CommentOutput>>(returnedStatus.Value);
            }
        }

        [Fact]
        public async void GetCommentExceptionShouldReturnNotFound()
        {
            var mockBL = new Mock<IComment>();
            mockBL.Setup(x => x.GetComment(It.IsAny<int>())).Throws(new Exception("exception test"));



            var mockUserSettings = new Mock<IOptions<ApiSettings>>();

            var CommCont = new Rest.Controllers.CommentController(mockBL.Object, mockUserSettings.Object);
            var result = await CommCont.GetComment(1);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteCommentsShouldReturnNoContent()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IComment _BL = new CommentBL(_repo);

                var settings = Options.Create(new ApiSettings());

                var CommCont = new Rest.Controllers.CommentController(_BL, settings);

                var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "BZ")
                }));

                CommCont.ControllerContext = new ControllerContext();
                CommCont.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

                Comments test = new Comments
                {
                    CommentID = 753,
                    PostID = 123,
                    UserName = "Cesar_19",
                    Created = new DateTime(2015, 12, 31, 5, 10, 20, DateTimeKind.Utc),
                    Message = "I just got a new dog!"
                };

                //Act
                var returnedValue = CommCont.DeleteComment(test.CommentID);
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



            var mockUserSettings = new Mock<IOptions<ApiSettings>>();

            var CommCont = new Rest.Controllers.CommentController(mockBL.Object, mockUserSettings.Object);
            var result = await CommCont.DeleteComment(1);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void UpdateCommentsShouldReturnNoContent()
        {
            using (var context = new DFDBContext(options))
            {
                IRepo _repo = new Repo(context);
                IComment _BL = new CommentBL(_repo);
                var settings = Options.Create(new ApiSettings());

                var CommCont = new Rest.Controllers.CommentController(_BL, settings);

                var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "BZ")
                }));

                CommCont.ControllerContext = new ControllerContext();
                CommCont.ControllerContext.HttpContext = new DefaultHttpContext { User = user };


                Rest.DTO.UpdateCommentInput test = new Rest.DTO.UpdateCommentInput
                {
                    PostID = 123,
                    Created = new DateTime(2015, 12, 31, 5, 10, 20, DateTimeKind.Utc),
                    Message = "I just got a new dog!"
                };

                //Act
                var returnedValue = CommCont.UpdateComment(test);
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



            var mockUserSettings = new Mock<IOptions<ApiSettings>>();

            var CommCont = new Rest.Controllers.CommentController(mockBL.Object, mockUserSettings.Object);
            var result = await CommCont.UpdateComment(new Rest.DTO.UpdateCommentInput());
            Assert.IsType<BadRequestResult>(result);
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
