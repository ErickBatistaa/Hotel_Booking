using Application;
using Application.Bookings.Commands;
using Application.Bookings.DTO;
using Application.Bookings.Requests;
using Domain.Bookings.Entities;
using Domain.Bookings.Ports;
using Domain.Guests.Entities;
using Domain.Guests.Enuns;
using Domain.Guests.Ports;
using Domain.Guests.ValueObjects;
using Domain.Rooms.Entities;
using Domain.Rooms.Ports;
using Moq;

namespace ApplicationTests
{
    [TestFixture]   
    internal class CreateBookingCommandHandlerTests
    {
        [Test]
        public async Task Should_Not_CreateBooking_If_RoomIsMissing()
        {
            var command = new CreateBookingCommand
            {
                BookingDto = new CreateBookingRequest
                {
                    Data = new BookingDTO
                    {
                        GuestId = 1,
                        Start = DateTime.Now,
                        End = DateTime.Now.AddDays(2)
                    }
                }
            };

            var fakeGuest = new Guest
            {
                id = command.BookingDto.Data.GuestId,
                Document = new PersonId
                {
                    DocumentType = EDocumentType.Passport,
                    IdNumber = "abc1234"
                },
                Email = "teste@teste.com.br",
                Name = "Fake Guest",
                Surname = "TEST Guest"
            };

            var guestRepository = new Mock<IGuestRepository>();
            guestRepository.Setup(x => x.Get(command.BookingDto.Data.GuestId))
                           .Returns(Task.FromResult(fakeGuest));

            var fakeBooking = new Booking
            {
                Id = 1
            };

            var bookingRepository = new Mock<IBookingRepository>();
            bookingRepository.Setup(x => x.CreateBooking(It.IsAny<Booking>()))
                             .Returns(Task.FromResult(fakeBooking));

            var handler = GetCommandMock(null, guestRepository, bookingRepository);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.NotNull(result.ErrorCode == ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION);
            Assert.NotNull(result.Message == "Room is a required information");
        }

        [Test]
        public async Task Should_Not_CreateBooking_If_StartDateIsMissing()
        {
            var command = new CreateBookingCommand
            {
                BookingDto = new CreateBookingRequest
                {
                    Data = new BookingDTO
                    {
                        RoomId = 1,
                        GuestId = 1,
                        End = DateTime.Now.AddDays(2)
                    }
                }
            };

            var fakeGuest = new Guest
            {
                id = command.BookingDto.Data.GuestId,
                Document = new PersonId
                {
                    DocumentType = EDocumentType.Passport,
                    IdNumber = "abc1234"
                },
                Email = "teste@teste.com.br",
                Name = "Fake Guest",
                Surname = "TEST Guest"
            };

            var guestRepository = new Mock<IGuestRepository>();
            guestRepository.Setup(x => x.Get(command.BookingDto.Data.GuestId))
                           .Returns(Task.FromResult(fakeGuest));

            var fakeRoom = new Room
            {
                Id = command.BookingDto.Data.RoomId,
                InMaintenance = false,
                Price = new Price
                {
                    Currency = EAcceptedCurrencies.Dolar,
                    Value = 100
                },
                Name = "Fake Room 01",
                Level = 1
            };

            var roomRepository = new Mock<IRoomRepository>();
            roomRepository.Setup(x => x.GetAggregate(command.BookingDto.Data.RoomId))
                          .Returns(Task.FromResult(fakeRoom));

            var handler = GetCommandMock(roomRepository, guestRepository);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.NotNull(result.ErrorCode == ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION);
            Assert.NotNull(result.Message == "PlaceAt is a required information");
        }

        [Test]
        public async Task Should_CreateBooking()
        {
            var command = new CreateBookingCommand
            {
                BookingDto = new CreateBookingRequest
                {
                    Data = new BookingDTO
                    {
                        RoomId = 1,
                        GuestId = 1,
                        Start = DateTime.Now,
                        End = DateTime.Now.AddDays(2)
                    }
                }
            };

            var fakeGuest = new Guest
            {
                id = command.BookingDto.Data.GuestId,
                Document = new PersonId
                {
                    DocumentType = EDocumentType.Passport,
                    IdNumber = "abc1234"
                },
                Email = "teste@teste.com.br",
                Name = "Fake Guest",
                Surname = "TEST Guest"
            };

            var guestRepository = new Mock<IGuestRepository>();
            guestRepository.Setup(x => x.Get(command.BookingDto.Data.GuestId))
                           .Returns(Task.FromResult(fakeGuest));

            var fakeRoom = new Room
            {
                Id = command.BookingDto.Data.RoomId,
                InMaintenance = false,
                Price = new Price
                {
                    Currency = EAcceptedCurrencies.Dolar,
                    Value = 100
                },
                Name = "Fake Room 01",
                Level = 1
            };

            var roomRepository = new Mock<IRoomRepository>();
            roomRepository.Setup(x => x.GetAggregate(command.BookingDto.Data.RoomId))
                          .Returns(Task.FromResult(fakeRoom));

            var fakeBooking = new Booking
            {
                Id = 1
            };

            var bookingRepository = new Mock<IBookingRepository>();
            bookingRepository.Setup(x => x.CreateBooking(It.IsAny<Booking>()))
                             .Returns(Task.FromResult(fakeBooking));

            var handler = GetCommandMock(roomRepository, guestRepository, bookingRepository);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.AreEqual(result.Data.Id, fakeBooking.Id);
        }

        private CreateBookingCommandHandler GetCommandMock(
            Mock<IRoomRepository> roomRepository = null,
            Mock<IGuestRepository> guestRepository = null,
            Mock<IBookingRepository> bookingRepository = null)
        {
            var _bookingRepository = bookingRepository ?? new Mock<IBookingRepository>();
            var _roomRepository = roomRepository ?? new Mock<IRoomRepository>();
            var _guestRepository = guestRepository ?? new Mock<IGuestRepository>();

            var commandHandler = new CreateBookingCommandHandler(
                _bookingRepository.Object,
                _roomRepository.Object,
                _guestRepository.Object);

            return commandHandler;
        }
    }
}
