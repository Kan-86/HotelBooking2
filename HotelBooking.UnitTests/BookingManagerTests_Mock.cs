using System;
using System.Collections.Generic;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using HotelBooking.WebApi.Controllers;
using Moq;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTests_Mock
    {
        private Mock<IBookingManager> _fakeBookingManager;
        private Mock<IRepository<Room>> _fakeRoomRepo;
        private Mock<IRepository<Booking>> _fakeBookingRepo;
        private BookingManager _bookingManager;
        public BookingManagerTests_Mock()
        {
            DateTime date = DateTime.Today;

            var booking = new List<Booking>
            {
                new Booking { Id = 1, CustomerId = 1, EndDate = date.AddDays(1), StartDate = date, IsActive = true, RoomId = 1 },
                new Booking { Id = 2, CustomerId = 2, EndDate = date.AddDays(2), StartDate = date, IsActive = true, RoomId = 2  },
            };
            _fakeBookingManager = new Mock<IBookingManager>();
            _fakeRoomRepo = new Mock<IRepository<Room>>();
            _fakeBookingRepo = new Mock<IRepository<Booking>>();

            // Get available rooms
            _fakeBookingManager.Setup(x => x.FindAvailableRoom(date, date.AddDays(1))).Verifiable();


            _bookingManager = new BookingManager(_fakeBookingRepo.Object, _fakeRoomRepo.Object);
        }

        [Fact]
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException()
        {
            DateTime date = new DateTime();
            Assert.Throws<ArgumentException>(() => _bookingManager.FindAvailableRoom(date, date));
        }
    }
}
