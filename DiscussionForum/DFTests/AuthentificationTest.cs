using System;
using Xunit;
using Microsoft.AspNetCore.Authorization;
//using System.Web.;
using Moq;
using DFBL;
using DiscussionForumREST.Controllers;
using Microsoft.Extensions.Options;

namespace DFTests
{
    public class AuthentificationTest
    {

         [Fact]
        public void CreateForum_Should_require_Autorization(){
            var mockBL=new Mock<IForum>();

            ForumController controller=new ForumController(mockBL.Object);
     
            var actualAtributes=controller.GetType().GetMethod("AddForum").GetCustomAttributes(typeof(AuthorizeAttribute),true);
            Assert.Equal(typeof(AuthorizeAttribute),actualAtributes[0].GetType());
        }

         [Fact]
        public void UpdateForum_Should_require_Autorization(){
            var mockBL=new Mock<IForum>();

            ForumController controller=new ForumController(mockBL.Object);
     
            var actualAtributes=controller.GetType().GetMethod("UpdateForum").GetCustomAttributes(typeof(AuthorizeAttribute),true);
            Assert.Equal(typeof(AuthorizeAttribute),actualAtributes[0].GetType());
        }
        
        
         [Fact]
        public void DeleteForum_Should_require_Autorization(){
            var mockBL = new Mock<IForum>();
            ForumController controller = new ForumController(mockBL.Object);

            var actualAtributes=controller.GetType().GetMethod("DeleteForum").GetCustomAttributes(typeof(AuthorizeAttribute),true);
            Assert.Equal(typeof(AuthorizeAttribute),actualAtributes[0].GetType());
        }


        [Fact]
        public void CreateComment_Should_require_Autorization()
        {
            var mockBL = new Mock<IComment>();
            var settings = Options.Create(new ApiSettings());

            CommentController controller = new CommentController(mockBL.Object, settings);

            var actualAtributes = controller.GetType().GetMethod("AddComment").GetCustomAttributes(typeof(AuthorizeAttribute), true);
            Assert.Equal(typeof(AuthorizeAttribute), actualAtributes[0].GetType());
        }

        [Fact]
        public void UpdateComment_Should_require_Autorization()
        {
            var mockBL = new Mock<IComment>();
            var settings = Options.Create(new ApiSettings());

            CommentController controller = new CommentController(mockBL.Object,settings);

            var actualAtributes = controller.GetType().GetMethod("UpdateComment").GetCustomAttributes(typeof(AuthorizeAttribute), true);
            Assert.Equal(typeof(AuthorizeAttribute), actualAtributes[0].GetType());
        }


        [Fact]
        public void DeleteComment_Should_require_Autorization()
        {
            var mockBL = new Mock<IComment>();
            var settings = Options.Create(new ApiSettings());

            CommentController controller = new CommentController(mockBL.Object,settings);

            var actualAtributes = controller.GetType().GetMethod("DeleteComment").GetCustomAttributes(typeof(AuthorizeAttribute), true);
            Assert.Equal(typeof(AuthorizeAttribute), actualAtributes[0].GetType());
        }



        [Fact]
        public void CreateForumPost_Should_require_Autorization()
        {
            var mockBL = new Mock<IForumPost>();
            var settings = Options.Create(new ApiSettings());

            ForumPostController controller = new ForumPostController(mockBL.Object, settings);

            var actualAtributes = controller.GetType().GetMethod("AddPost").GetCustomAttributes(typeof(AuthorizeAttribute), true);
            Assert.Equal(typeof(AuthorizeAttribute), actualAtributes[0].GetType());
        }

        [Fact]
        public void UpdateForumPost_Should_require_Autorization()
        {
            var mockBL = new Mock<IForumPost>();
            var settings = Options.Create(new ApiSettings());

            ForumPostController controller = new ForumPostController(mockBL.Object, settings);

            var actualAtributes = controller.GetType().GetMethod("UpdatePost").GetCustomAttributes(typeof(AuthorizeAttribute), true);
            Assert.Equal(typeof(AuthorizeAttribute), actualAtributes[0].GetType());
        }


        [Fact]
        public void DeleteForumPost_Should_require_Autorization()
        {
            var mockBL = new Mock<IForumPost>();
            var settings = Options.Create(new ApiSettings());

            ForumPostController controller = new ForumPostController(mockBL.Object, settings);

            var actualAtributes = controller.GetType().GetMethod("DeletePost").GetCustomAttributes(typeof(AuthorizeAttribute), true);
            Assert.Equal(typeof(AuthorizeAttribute), actualAtributes[0].GetType());
        }






    }
}