using Xunit;
using EFGetStarted;
using EFGetStarted.BusinessLogic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using FluentAssertions;

namespace WebAPIStarter.Tests
{
    public class BlogServicesTests
    {
        [Fact]
        public void Add_WriterToDB(){
            var options = new DbContextOptionsBuilder<BloggingContext>()
                .UseInMemoryDatabase(databaseName: "Add_WritesToDB")
                .Options;

                using (var context = new BloggingContext(options))
                {
                    var service = new BlogService(context);
                    service.Add("http://sample.com");
                    // context.SaveChanges();
                }

                using (var context = new BloggingContext(options))
                {
                    Assert.Equal(1, context.Blogs.Count());
                    Assert.Equal("http://sample.com", context.Blogs.Single().Url);
                }
        }

        [Fact]

        public void Find_Searches_URL()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>()
                .UseInMemoryDatabase(databaseName: "Find_Searches_URL")
                .Options;

            using (var context = new BloggingContext(options))
            {
                context.Blogs.Add( new Blog(){ Url = "http://sample.com/doggos" } ); 
                context.Blogs.Add( new Blog(){ Url = "http://sample.com/cattos" } );
                context.Blogs.Add( new Blog(){ Url = "http://sample.com/catwabbits" } );
                context.SaveChanges(); 
            }
            using (var context = new BloggingContext(options))
            {
                var service = new BlogService(context);
                var result = service.Find("cat");
                // result.Count().Equals(2); //samesis
                Assert.Equal(2, result.Count());
            }
        }

        [Fact]
        public void Delete_ShouldDelete(){

            var options = new DbContextOptionsBuilder<BloggingContext>()
                .UseInMemoryDatabase(databaseName: "MAhDeleteDatabase").Options;
            
            using (var context = new BloggingContext(options))
            {
                var service = new BlogService(context);
                service.Add("http://Hi.I.Am.A.New.Blog");
                service.Add("http://Hi.Ill.BeDeleted");
            }
            
            using (var context = new BloggingContext(options))
            {
                var service = new BlogService(context);
                var toDelete = service.Find("Deleted").Single();
                context.Blogs.Remove(toDelete);
                context.Blogs.Count().Equals(1); //Yeeeeiii
            }

        }
        
    }
}