using System;
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
    }
}
