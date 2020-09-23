using HotelBooking.Core;
using HotelBooking.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class BookingControllerTests
    {
        private BookingsController controller;
        private Mock<IRepository<Booking>> fakeBookingRepo;
        private Mock<IBookingManager> fakeBookingManager;

        public BookingControllerTests()
        {
            DateTime date = DateTime.Today;
            var booking = new List<Booking>
            {
                new Booking { Id = 1, CustomerId = 1, EndDate = date, StartDate = date, IsActive = true, RoomId = 1 },
                new Booking { Id = 2, CustomerId = 2, EndDate = date, StartDate = date, IsActive = true, RoomId = 2  },
            };

            // Create fake RoomRepository. 
            fakeBookingRepo = new Mock<IRepository<Booking>>();
            fakeBookingManager = new Mock<IBookingManager>();

            // Implement fake GetAll() method.
            fakeBookingRepo.Setup(x => x.GetAll()).Returns(booking);

            // Integers from 1 to 2 (using a range)
            fakeBookingRepo.Setup(x => x.Get(It.IsInRange<int>(1, 2, Moq.Range.Inclusive))).Returns(booking[1]);


            // Create BookingController
            controller = new BookingsController(fakeBookingRepo.Object, 
                fakeBookingManager.Object);
        }

        [Fact]
        public void GetAll_ReturnsListWithCorrectNumberOfBookings()
        {
            // Act
            var result = controller.Get() as List<Booking>;
            var noOfRooms = result.Count;

            // Assert
            Assert.Equal(2, noOfRooms);
        }

        [Fact]
        public void GetById_BookingExists_ReturnsIActionResultWithRoom()
        {
            // Act
            var result = controller.Get(2) as ObjectResult;
            var room = result.Value as Booking;
            var roomId = room.Id;

            // Assert
            Assert.InRange<int>(roomId, 1, 2);
        }
    }
}
