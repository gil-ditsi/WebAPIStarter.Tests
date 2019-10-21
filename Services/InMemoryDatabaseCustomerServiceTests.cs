using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using WebAPIStarter.Data;
using WebAPIStarter.Data.Models;
using WebAPIStarter.Services.CustomerService;
using FluentAssertions;

namespace WebAPIStarter.Tests.Services
{
    public class InMemoryDatabaseCustomerServiceTests : IDisposable
    {
        private WebAPIStarterContext Context { get; set; }
        
        public InMemoryDatabaseCustomerServiceTests()
        {
            var contextOptions = new DbContextOptionsBuilder<WebAPIStarterContext>()
                .UseInMemoryDatabase(databaseName: "mockDB_CustomerService")
                .Options;
            this.Context = new WebAPIStarterContext(contextOptions);
        }

        [Fact]
        public void Add_WhenCalled_AddsCustomerToContext(){

            this.Context.Database.EnsureDeleted();

            Customer testC = new Customer(){
                FirstName = "Dude",
                LastName = "Damn",
                Email = "mail.test@something"
            };

            // // var newC = this.Context.Customers.Add(testC).Entity;

            var SUT = new InMemoryDatabaseCustomerService(this.Context);
            var newC = SUT.Add(testC);

            this.Context.Customers.Find(newC.Id).Should().BeEquivalentTo(testC); //BREAKS HAAAARD with SUT

        }

        [Fact]
        public void Update_ShouldModifyMembers(){

            this.Context.Database.EnsureDeleted();

            Customer testC = new Customer(){
                FirstName = "Dude",
                LastName = "Damn",
                Email = "mail.test@something"
            };

            var SUT = new InMemoryDatabaseCustomerService(this.Context);
            var newC = SUT.Add(testC);

            var testEmail = "notaRealEmail@notemail.com"; 
            newC.Email = testEmail;

            SUT.Update(newC);

            this.Context.Customers.Find(newC.Id).Email.Should().BeEquivalentTo(testEmail); //BREAKS HAAAARD with SUT

        }

        [Fact]
        public void Delete_ShouldRemoveFromContext(){

            this.Context.Database.EnsureDeleted();

            Customer testC = new Customer(){
                FirstName = "Dude",
                LastName = "Damn",
                Email = "mail.test@something"
            };

            var SUT = new InMemoryDatabaseCustomerService(this.Context);
            var newC = SUT.Add(testC);

            SUT.Delete(newC);

            this.Context.Customers.Find(newC.Id).Should().BeNull();

        }

        public void Dispose()
        {
            this.Context.Dispose();
        }
    }
}