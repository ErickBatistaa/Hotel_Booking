using Application;
using Application.Guests;
using Application.Guests.DTO;
using Application.Guests.Requests;
using Domain.Guests.Entities;
using Domain.Guests.Enuns;
using Domain.Guests.Ports;
using Domain.Guests.ValueObjects;
using Moq;

namespace ApplicationTests
{
    public class Tests
    {
        private GuestManager _guestManager;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task ShouldCreateAGuest()
        {
            var guestDTO = new GuestDTO
            {
                Name = "Ciclano",
                Surname = "Teste",
                Email = "abc@teste.com.br",
                IdNumber = "abcd",
                IdTypeCode = 1
            };

            int expectedId = 222;

            var request = new CreateGuestRequest
            {
                Data = guestDTO
            };

            var fakeRepo = new Mock<IGuestRepository>(); // when we use Moq lib, just declare the properties that will be use and passed this implementattion through an object.
            fakeRepo.Setup(
                x => x.Create(
                It.IsAny<Guest>())).
                Returns(Task.FromResult(expectedId));

            _guestManager = new GuestManager(fakeRepo.Object);

            var result = await _guestManager.CreateGuest(request);

            Assert.IsNotNull(result);
            Assert.True(result.Success);
            Assert.AreEqual(result.Data.Id, expectedId);
            Assert.AreEqual(result.Data.Name, guestDTO.Name);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("a")]
        [TestCase("ab")]
        [TestCase("abc")]
        public async Task Should_Return_InvalidPersonDocumentIdException_When_Docs_Are_Invalid(string docNumber)
        {
            var guestDTO = new GuestDTO
            {
                Name = "Ciclano",
                Surname = "Teste",
                Email = "abc@teste.com.br",
                IdNumber = docNumber,
                IdTypeCode = 1
            };

            var request = new CreateGuestRequest
            {
                Data = guestDTO
            };

            var fakeRepo = new Mock<IGuestRepository>();

            fakeRepo.Setup(
                x => x.Create(
                It.IsAny<Guest>())).
                Returns(Task.FromResult(222));

            _guestManager = new GuestManager(fakeRepo.Object);

            var result = await _guestManager.CreateGuest(request);

            Assert.IsNotNull(result);
            Assert.False(result.Success);
            Assert.AreEqual(result.ErrorCode, ErrorCodes.GUEST_INVALID_PERSON_ID);
            Assert.AreEqual(result.Message, "The ID passed is not valid");
        }

        [TestCase("", "surnametest", "etset@teste.com.br")]
        [TestCase(null, "surnametest", "etset@teste.com.br")]
        [TestCase("Fulano", "", "etset@teste.com.br")]
        [TestCase("Fulano", null, "etset@teste.com.br")]
        [TestCase("Fulano", "surnametest", "")]
        [TestCase("Fulano", "surnametest", null)]
        public async Task Should_Return_MissingRequiredInformation_When_Docs_Are_Invalid(string name, string surname, string email)
        {
            var guestDTO = new GuestDTO
            {
                Name = name,
                Surname = surname,
                Email = email,
                IdNumber = "abcd",
                IdTypeCode = 1
            };

            var request = new CreateGuestRequest
            {
                Data = guestDTO
            };

            var fakeRepo = new Mock<IGuestRepository>();

            fakeRepo.Setup(
                x => x.Create(
                It.IsAny<Guest>())).
                Returns(Task.FromResult(222));

            _guestManager = new GuestManager(fakeRepo.Object);

            var result = await _guestManager.CreateGuest(request);

            Assert.IsNotNull(result);
            Assert.False(result.Success);
            Assert.AreEqual(result.ErrorCode, ErrorCodes.GUEST_MISSING_REQUIRED_INFORMATION);
            Assert.AreEqual(result.Message, "Missing Required Information passed");
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("b@b.com")]
        public async Task Should_Return_InvalidEmailException_When_Docs_Are_Invalid(string email)
        {
            var guestDTO = new GuestDTO
            {
                Name = "Teste",
                Surname = "Teste",
                Email = email,
                IdNumber = "abcd",
                IdTypeCode = 1
            };

            var request = new CreateGuestRequest
            {
                Data = guestDTO
            };

            var fakeRepo = new Mock<IGuestRepository>();

            fakeRepo.Setup(
                x => x.Create(
                It.IsAny<Guest>())).
                Returns(Task.FromResult(222));

            _guestManager = new GuestManager(fakeRepo.Object);

            var result = await _guestManager.CreateGuest(request);

            Assert.IsNotNull(result);
            Assert.False(result.Success);
            Assert.AreEqual(result.ErrorCode, ErrorCodes.GUEST_INVALID_EMAIL);
            Assert.AreEqual(result.Message, "The given email is not valid");
        }

        [Test]
        public async Task Should_Return_GuestNotFound_When_GuestDoesntExist()
        {
            var fakeRepo = new Mock<IGuestRepository>();

            fakeRepo.Setup(
                x => x.Get(333)).
                Returns(Task.FromResult<Guest?>(null));

            _guestManager = new GuestManager(fakeRepo.Object);

            var result = await _guestManager.GetGuest(333);

            Assert.IsNotNull(result);
            Assert.False(result.Success);
            Assert.AreEqual(result.ErrorCode, ErrorCodes.GUEST_NOT_FOUND);
            Assert.AreEqual(result.Message, "No guest record was found with the given ID");
        }

        [Test]
        public async Task Should_Return_Guest_Success()
        {
            var fakeRepo = new Mock<IGuestRepository>();

            var fakeGuest = new Guest
            {
                id = 333,
                Name = "Teste",
                Document = new PersonId
                {
                    DocumentType = EDocumentType.DriveLicence,
                    IdNumber = "123"
                }
            };

            fakeRepo.Setup(
                x => x.Get(333)).
                Returns(Task.FromResult((Guest?)fakeGuest));

            _guestManager = new GuestManager(fakeRepo.Object);

            var result = await _guestManager.GetGuest(333);

            Assert.IsNotNull(result);
            Assert.True(result.Success);
            Assert.AreEqual(result.Data.Id, fakeGuest.id);
            Assert.AreEqual(result.Data.Name, fakeGuest.Name);
        }
    }
}