using Application;
using Application.Rooms.Queries;
using Domain.Guests.ValueObjects;
using Domain.Rooms.Entities;
using Domain.Rooms.Ports;
using Moq;

namespace ApplicationTests
{
    public class GetRoomQueryHandlerTests
    {
        public void Setup()
        {

        }

        [Test]
        public async Task Should_Return_Room()
        {
            var query = new GetRoomsQuery() { Id = 1 };
            var repoMock = new Mock<IRoomRepository>();
            var fakeRoom = new Room() 
            {
                Id = 1, 
                Price = new Price { Value = 130 }
            };

            repoMock.Setup(x => x.Get(query.Id)).Returns(Task.FromResult(fakeRoom));

            var handler = new GetRoomsQueryHandler(repoMock.Object);
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
        }

        [Test]
        public async Task Should_Return_ProperError_Message_WhenRoom_NotFound()
        {
            var query = new GetRoomsQuery { Id = 1 };
            var repoMock = new Mock<IRoomRepository>();

            var handler = new GetRoomsQueryHandler(repoMock.Object);
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.False(result.Success);
            Assert.AreEqual(result.ErrorCode, ErrorCodes.ROOM_NOT_FOUND);
            Assert.AreEqual(result.Message, "Could not find a Room with the given Id");
        }
    }
}
