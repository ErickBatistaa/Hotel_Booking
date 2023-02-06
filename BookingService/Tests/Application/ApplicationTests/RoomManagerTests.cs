using Application;
using Application.Rooms;
using Application.Rooms.DTO;
using Application.Rooms.Requests;
using Domain.Guests.Enuns;
using Domain.Rooms.Entities;
using Domain.Rooms.Ports;
using Moq;

namespace ApplicationTests
{
    public class RoomManagerTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task Should_Create_Room_When_User_Is_Manager()
        {
            var repoMock = new Mock<IRoomRepository>();

            var request = new CreateRoomRequest()
            {
                UserRoles = new List<string>() { "Manager", "SomethingElse" },
                Data = new RoomDTO
                {
                    Level = 1,
                    InMaintenance = true,
                    Name = "Room Test",
                    Price = 100,
                    Currency = EAcceptedCurrencies.Dolar
                }
            }; 

            repoMock.Setup(x => x.Create(It.IsAny<Room>())).Returns(Task.FromResult(1));

            var manager = new RoomManager(repoMock.Object);
            var result = await manager.CreateRoom(request);
             
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
        }

        [Test]
        public async Task Should_Not_Create_Room_If_User_IsNot_Manager()
        {
            var repoMock = new Mock<IRoomRepository>();

            var request = new CreateRoomRequest()
            {
                UserRoles = new List<string>() { "NotManager" }
            };

            var manager = new RoomManager(repoMock.Object);
            var result = await manager.CreateRoom(request);

            Assert.False(result.Success);
            Assert.AreEqual(result.ErrorCode, ErrorCodes.ROOM_INVALID_PERMISSION);
            Assert.AreEqual(result.Message, "User does not have permission to perform this action");

        }
    }
}
