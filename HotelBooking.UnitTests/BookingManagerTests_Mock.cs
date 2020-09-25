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
                new Booking { Id = 1, CustomerId = 1, EndDate = date.AddDays(20), StartDate = date.AddDays(10), IsActive = true, RoomId = 1 },
                new Booking { Id = 2, CustomerId = 2, EndDate = date.AddDays(21), StartDate = date.AddDays(11), IsActive = true, RoomId = 2  },
            };
            _fakeBookingManager = new Mock<IBookingManager>();
            _fakeRoomRepo = new Mock<IRepository<Room>>();
            _fakeBookingRepo = new Mock<IRepository<Booking>>();

            _fakeBookingRepo.Setup(x => x.GetAll()).Returns(booking);


            _bookingManager = new BookingManager(_fakeBookingRepo.Object, _fakeRoomRepo.Object);
        }

        [Fact]
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException()
        {
            DateTime date = new DateTime();
            Assert.Throws<ArgumentException>(() => _bookingManager.FindAvailableRoom(date, date));
        }

        [Fact]
        public void GetFullyOccupiedDates_NotNull()
        {
            DateTime date = DateTime.Now;
            var nrOfRooms =_bookingManager.GetFullyOccupiedDates(date.AddDays(10), date.AddDays(20));
            Assert.NotNull(nrOfRooms);
        }

        [Theory]
        [JsonData("D:/Skoli/7Onn/Test/HotelBooking2/HotelBooking.UnitTests/testdata.json")]
        public void CreateBooking_IDIsLowerThanZero_ThrowsArgumentException(int id)
        {
            var booking = new Booking { CustomerId = id };
            Assert.Throws<ArgumentException>(() => _bookingManager.CreateBooking(booking));
        }
    }
}
