using HotelBooking.Core;
using Moq;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Xunit;

namespace HotelBooking.SpecFlowTest.Steps
{
    [Binding]
    public class CreateBookingSteps
    {
        private Booking _booking = new Booking();

        private Mock<IBookingManager> _fakeBookingManager;
        private Mock<IRepository<Room>> _fakeRoomRepo;
        private Mock<IRepository<Booking>> _fakeBookingRepo;
        private BookingManager _bookingManager;
        private bool result;

        private CreateBookingSteps()
        {
            var booking = new List<Booking>
            {
                new Booking { Id = 2, CustomerId = 2, StartDate = new DateTime(2020, 11, 10), EndDate = new DateTime(2020, 11, 15), IsActive = true, RoomId = 2  }
            };

            var rooms = new List<Room>
            {
                new Room {Id =2 }
            };

            _fakeBookingManager = new Mock<IBookingManager>();
            _fakeRoomRepo = new Mock<IRepository<Room>>();
            _fakeBookingRepo = new Mock<IRepository<Booking>>();

            _fakeBookingRepo.Setup(x => x.GetAll()).Returns(booking);
            _fakeRoomRepo.Setup(x => x.GetAll()).Returns(rooms);

            _bookingManager = new BookingManager(_fakeBookingRepo.Object, _fakeRoomRepo.Object);
        }

        [Given(@"I have entered a start date (.*)")]
        public void GivenIHaveEnteredAStartDate(string startDate)
        {
            _booking.StartDate = DateTime.Parse(startDate);
        }
        
        [Given(@"I have entered an end date (.*)")]
        public void GivenIHaveEnteredAnEndDate(string endDate)
        {
            _booking.EndDate = DateTime.Parse(endDate);
        }
        
        [When(@"I book a room")]
        public void WhenIBookARoom()
        {
            result = _bookingManager.CreateBooking(_booking);
        }
        
        [Then(@"I want to know if there are available rooms (.*)")]
        public void ThenIWantToKnowIfThereAreAvailableRooms(bool expectedResult)
        {
            Assert.Equal(expectedResult, result);
        }
    }
}
