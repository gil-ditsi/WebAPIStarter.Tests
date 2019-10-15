using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPIStarter.Controllers;
using WebAPIStarter.Models;
using WebAPIStarter.Services.CustomerService;
using Xunit;

namespace WebAPIStarter.Tests
{
    public class CustomerControllerTest
    {
        [Fact]
        public void GetAll_WhenCalled_ReturnsOKObjectResult()
        {
            var mockService = new Mock<IService<Customer>>();
            //Arrange
            CustomerController customerController = new CustomerController(mockService.Object);

            //Act
            var getResults = customerController.GetAll();

            //Assert
            Assert.IsType<OkObjectResult>(getResults);

        }

        [Fact]
        public void CreateCustomer_WhenCalled_WithValidCustomer_ReturnsCreatedResult(){

            //Given
            var mockService = new Mock<IService<Customer>>();
            CustomerController customerController = new CustomerController(mockService.Object);
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
            var mockService = new Mock<IService<Customer>>();
            CustomerController customerController = new CustomerController(mockService.Object);
            int nonExistingId = 25;

            var getResults = customerController.Read(nonExistingId);

            Assert.IsType<NotFoundResult>(getResults);
        }

        [Fact]
        public void DeleteCustomer_WhenCalledWithAnID_ReturnsGoneResult(){

            var mockService = new Mock<IService<Customer>>();
            var fakeCustomer = new Customer() { Id = 1, FirstName = "Gil", LastName = "Hdz", Email = "mah.mail@man.com" };
            mockService.Setup(serv => serv.GetOne(1)).Returns(fakeCustomer);

            CustomerController customerController = new CustomerController(mockService.Object);
            int existingId = 1;

            var getResults = customerController.Delete(existingId);

            Assert.IsType<StatusCodeResult>(getResults);
        }

        [Fact]
        public void GetOne_WhenCalledWithID_ReturnsCustomer()
        {

            //Arrange
            var mockService = new Mock<IService<Customer>>();
            var fakeCustomer = new Customer() { Id = 1, FirstName = "Gil", LastName = "Hdz", Email = "mah.mail@man.com" };
            mockService.Setup(serv => serv.GetOne(1)).Returns(fakeCustomer);

            CustomerController customerController = new CustomerController( mockService.Object );
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
            var customerService = new InMemoryCustomerService(new List<Customer>{
                new Customer() { Id = 1, FirstName = "Gil", LastName = "Hdz", Email = "mah.mail@man.com" },
                new Customer() { Id = 2, FirstName = "Gil2", LastName = "Hdz", Email = "mah.mail@man.com" },
                new Customer() { Id = 3, FirstName = "Gil3", LastName = "Hdz", Email = "mah.mail@man.com" }
            });
            CustomerController SUT = new CustomerController(customerService); //System Under Test
            var expected = new Customer() { Id = 1, FirstName = "Gil", LastName = "Hdz", Email = "mah.mail@man.com" };

            //Act
            var getResult = (OkObjectResult) SUT.Read(1);
            
            //Assert

            //This one takes into account the values of the object
            getResult.Value.Should().BeEquivalentTo(expected);
            // getResult.Value.Should().Equals(customers[0]);
            // Assert.Equal(getResult.Value, customers[0]); //This one is referential

        }

        [Fact]
        public void CreateCustomer_WhenCalled_WithValidCustomer_ReturnsCustomer(){

            //Given
            // var mockService = new Mock<IService<Customer>>();
            // CustomerController customerController = new CustomerController(mockService.Object);
            CustomerController customerController = new CustomerController();
            Customer newCustomer = new Customer {
                FirstName = "Gil",
                LastName = "Hdz",
                Email = "some@other.net"
            };
            
            Customer[] testDouble = {new Customer {
                FirstName = "Gil",
                Id = 1,
                LastName = "Hdz",
                Email = "some@other.net"
            }, null};

            //When
            var getResults = (CreatedResult) customerController.Create(newCustomer);
            

            //Then
            var res = getResults.Value as Customer;
            Customer[] originalSet = {null, res};
            originalSet.Should().BeEquivalentTo(testDouble); //Le vale el orden
            // originalSet.Should().Equal(testDouble); //Le importa el orden
            //Completed

        }
    }

}
