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
                new Booking { Id = 1, CustomerId = 1, EndDate = date.AddDays(35), StartDate = date.AddDays(30), IsActive = true, RoomId = 1 },
                new Booking { Id = 2, CustomerId = 2, EndDate = date.AddDays(35), StartDate = date.AddDays(30), IsActive = true, RoomId = 2  },
            };
            var room = new List<Room>
            {
                new Room { Id = 1, Description = "A"},
                new Room { Id = 2,  Description = "B"}
            };
            _fakeBookingManager = new Mock<IBookingManager>();
            _fakeRoomRepo = new Mock<IRepository<Room>>();
            _fakeBookingRepo = new Mock<IRepository<Booking>>();

            _fakeRoomRepo.Setup(x => x.GetAll()).Returns(room);
            _fakeBookingRepo.Setup(x => x.GetAll()).Returns(booking);


            _bookingManager = new BookingManager(_fakeBookingRepo.Object, _fakeRoomRepo.Object);
        }

        [Fact]
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException()
        {
            DateTime date = new DateTime();
            Assert.Throws<ArgumentException>(() => _bookingManager.FindAvailableRoom(date, date));
        }

        [Theory]
        [JsonData("testdata.json")]
        public void CreateBooking_IDIsLowerThanZero_ThrowsArgumentException(int id)
        {
            var booking = new Booking { CustomerId = id };
            Assert.Throws<ArgumentException>(() => _bookingManager.CreateBooking(booking));
        }

        [Fact]
        public void Create_Booking_If_StartDate_And_EndDate_Is_BeforeOccupied()
        {
            DateTime date = DateTime.Now;

            Booking booking = new Booking
            {       
                Id = 4, 
                CustomerId = 2,
                StartDate = date.AddDays(5),
                EndDate = date.AddDays(11), 
                IsActive = true, 
                RoomId = 1 
            };
            
            var nrOfRooms = _bookingManager.CreateBooking(booking);
            Assert.True(nrOfRooms);
        }

        [Fact]
        public void Create_Booking_If_StartDate_And_EndDate_Is_AfterOccupied()
        {
            DateTime date = DateTime.Now;

            Booking booking = new Booking
            {
                Id = 4,
                CustomerId = 2,
                StartDate = date.AddDays(36),
                EndDate = date.AddDays(39),
                IsActive = true,
                RoomId = 1
            };

            var nrOfRooms = _bookingManager.CreateBooking(booking);
            Assert.True(nrOfRooms);
        }

        [Fact]
        public void Create_Booking_If_StartDate_IsBefore_And_EndDate_Is_After_Ocupied()
        {
            DateTime date = DateTime.Now;

            Booking booking = new Booking
            {
                Id = 4,
                CustomerId = 2,
                StartDate = date.AddDays(29),
                EndDate = date.AddDays(32),
                IsActive = true,
                RoomId = 1
            };

            var nrOfRooms = _bookingManager.CreateBooking(booking);
            Assert.False(nrOfRooms);
        }

        [Fact]
        public void Create_Booking_If_StartDate_IsBefore_And_EndDate_Is_During_Ocupied()
        {
            DateTime date = DateTime.Now;

            Booking booking = new Booking
            {
                Id = 4,
                CustomerId = 2,
                StartDate = date.AddDays(29),
                EndDate = date.AddDays(39),
                IsActive = true,
                RoomId = 1
            };

            var nrOfRooms = _bookingManager.CreateBooking(booking);
            Assert.False(nrOfRooms);
        }

        //30  -   35   is ocupied
        [Fact]
        public void Create_Booking_If_StartDate_During_Ocupied_And_EndDate_Is_After_Ocupied()
        {
            DateTime date = DateTime.Now;

            Booking booking = new Booking
            {
                Id = 4,
                CustomerId = 2,
                StartDate = date.AddDays(32),
                EndDate = date.AddDays(37),
                IsActive = true,
                RoomId = 1
            };

            var nrOfRooms = _bookingManager.CreateBooking(booking);
            Assert.False(nrOfRooms);
        }

        [Fact]
        public void Create_Booking_If_StartDate_And_EndDate_IsDuring_Ocupied()
        {
            DateTime date = DateTime.Now;

            Booking booking = new Booking
            {
                Id = 4,
                CustomerId = 2,
                StartDate = date.AddDays(31),
                EndDate = date.AddDays(34),
                IsActive = true,
                RoomId = 1
            };

            var nrOfRooms = _bookingManager.CreateBooking(booking);
            Assert.False(nrOfRooms);
        }
    }
}
