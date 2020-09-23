using HotelBooking.Core;
using HotelBooking.WebApi.Controllers;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBooking.UnitTests
{
    public class BookingControllerTests
    {
        private BookingsController controller;
        private Mock<IRepository<Booking>> fakeBookingRepo;
        private Mock<IRepository<Room>> fakeRoomRepo;
        private Mock<IRepository<Customer>> fakeCustomerRepo;
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

            // Implement fake GetAll() method.
            fakeBookingRepo.Setup(x => x.GetAll()).Returns(booking);


            // Implement fake Get() method.
            //fakeRoomRepository.Setup(x => x.Get(2)).Returns(rooms[1]);


            // Alternative setup with argument matchers:

            // Any integer:
            //roomRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(rooms[1]);

            // Integers from 1 to 2 (using a predicate)
            // If the fake Get is called with an another argument value than 1 or 2,
            // it returns null, which corresponds to the behavior of the real
            // repository's Get method.
            //fakeRoomRepository.Setup(x => x.Get(It.Is<int>(id => id > 0 && id < 3))).Returns(rooms[1]);

            // Integers from 1 to 2 (using a range)
            fakeBookingRepo.Setup(x => x.Get(It.IsInRange<int>(1, 2, Moq.Range.Inclusive))).Returns(booking[1]);


            // Create RoomsController
            controller = new BookingsController(fakeBookingRepo.Object, 
                fakeRoomRepo.Object, 
                fakeCustomerRepo.Object,
                fakeBookingManager.Object);
        }
    }
}
