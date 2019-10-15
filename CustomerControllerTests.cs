using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using WebAPIStarter.Controllers;
using WebAPIStarter.Models;
using Xunit;

namespace WebAPIStarter.Tests
{
    public class CustomerControllerTest
    {
        [Fact]
        public void GetAll_WhenCalled_ReturnsOKObjectResult()
        {

            //Arrange
            CustomerController customerController = new CustomerController();

            //Act
            var getResults = customerController.GetAll();

            //Assert
            Assert.IsType<OkObjectResult>(getResults);

        }

        [Fact]
        public void CreateCustomer_WhenCalled_WithValidCustomer_ReturnsCreatedResult(){

            //Given
            CustomerController customerController = new CustomerController();
            Customer newCustomer = new Customer {
                FirstName = "Gil",
                LastName = "Hdz",
                Email = "some@other.net"
            };

            //When
            var getResults = customerController.Create(newCustomer);

            //Then
            Assert.IsType<CreatedResult>(getResults);
            //Completed

        }

        [Fact]
        public void GetCustomer_WhenCalledWithInvalidID_ReturnsNotFoundResult(){
            CustomerController customerController = new CustomerController();
            int nonExistingId = 25;

            var getResults = customerController.Read(nonExistingId);

            Assert.IsType<NotFoundResult>(getResults);
        }

        [Fact]
        public void DeleteCustomer_WhenCalledWithAnID_ReturnsGoneResult(){
            CustomerController customerController = new CustomerController();
            int existingId = 1;

            var getResults = customerController.Delete(existingId);

            Assert.IsType<StatusCodeResult>(getResults);
        }

        [Fact]
        public void GetOne_WhenCalledWithID_ReturnsCustomer()
        {

            //Arrange
            var customers = new List<Customer>{
                new Customer() { Id = 1, FirstName = "Gil", LastName = "Hdz", Email = "mah.mail@man.com" },
                new Customer() { Id = 2, FirstName = "Gil2", LastName = "Hdz", Email = "mah.mail@man.com" },
                new Customer() { Id = 3, FirstName = "Gil3", LastName = "Hdz", Email = "mah.mail@man.com" }
            };
            CustomerController customerController = new CustomerController(customers);
            int existingId = 1;

            //Act
            var getResults = (OkObjectResult)customerController.Read(existingId);

            //Assert
            Assert.NotNull((getResults.Value as Customer));

        }

        [Fact]
        public void GetOne_WhenCalled_ReturnsCustomer()
        {

            //Arrange
            var customers = new List<Customer>{
                new Customer() { Id = 1, FirstName = "Gil", LastName = "Hdz", Email = "mah.mail@man.com" },
                new Customer() { Id = 2, FirstName = "Gil2", LastName = "Hdz", Email = "mah.mail@man.com" },
                new Customer() { Id = 3, FirstName = "Gil3", LastName = "Hdz", Email = "mah.mail@man.com" }
            };
            CustomerController SUT = new CustomerController(customers); //System Under Test
            var expected = new Customer() { Id = 1, FirstName = "Gil", LastName = "Hdz", Email = "mah.mail@man.com" };

            //Act
            var getResult = (OkObjectResult) SUT.Read(1);
            
            //Assert

            //This one takes into account the values of the object
            getResult.Value.Should().BeEquivalentTo(expected);
            // getResult.Value.Should().Equals(customers[0]);
            // Assert.Equal(getResult.Value, customers[0]); //This one is referential

        }
    }
}
