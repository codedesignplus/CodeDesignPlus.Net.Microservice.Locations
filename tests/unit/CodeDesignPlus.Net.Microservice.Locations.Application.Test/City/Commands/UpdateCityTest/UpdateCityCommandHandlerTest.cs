using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.UpdateCity;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;
using CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;
using Moq;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.City.Commands.UpdateCityTest;

public class UpdateCityCommandHandlerTest
{
    private readonly Mock<ICityRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly UpdateCityCommandHandler handler;
    private readonly FakeData fakeData = new();

    public UpdateCityCommandHandlerTest()
    {
        repositoryMock = new Mock<ICityRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new UpdateCityCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(null!, CancellationToken.None));

        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_CityNotFound_ThrowsCodeDesignPlusException()
    {
        var command = fakeData.UpdateCityCommand;
        repositoryMock.Setup(r => r.FindAsync<CityAggregate>(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync((CityAggregate)null!);

        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(command, CancellationToken.None));

        Assert.Equal(Errors.CityNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.CityNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_StateNotFound_ThrowsCodeDesignPlusException()
    {
        var command = fakeData.UpdateCityCommand;
        var city = fakeData.CityAggregate;
        repositoryMock.Setup(r => r.FindAsync<CityAggregate>(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(city);
        repositoryMock.Setup(r => r.ExistsAsync<StateAggregate>(command.IdState, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(command, CancellationToken.None));

        Assert.Equal(Errors.StateNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.StateNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_UpdatesCityAndPublishesEvents()
    {
        var command = fakeData.UpdateCityCommand;
        var city = fakeData.CityAggregate;

        repositoryMock.Setup(r => r.FindAsync<CityAggregate>(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(city);
        repositoryMock.Setup(r => r.ExistsAsync<StateAggregate>(command.IdState, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        userContextMock.Setup(u => u.IdUser).Returns(Guid.NewGuid());

        await handler.Handle(command, CancellationToken.None);

        repositoryMock.Verify(r => r.UpdateAsync(city, It.IsAny<CancellationToken>()), Times.Once);
        pubSubMock.Verify(p => p.PublishAsync(It.IsAny<List<CityUpdatedDomainEvent>>(), It.IsAny<CancellationToken>()), Times.AtMostOnce);
    }
}
