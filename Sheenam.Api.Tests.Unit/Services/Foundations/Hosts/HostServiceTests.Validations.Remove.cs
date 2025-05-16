//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using FluentAssertions;
using Moq;
using Sheenam.Api.Models.Foundations.Hosts;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions.BigExceptions;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions.SmallExceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Hosts
{
    public partial class HostServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvaledAndLogItAsync()
        {
            // given
            Guid invalidId = Guid.Empty;
            var invalidHostIdException = new InvalidHostException();

            invalidHostIdException.AddData(
                key: nameof(Host.Id),
                values: "Id is required");

            var expectedHostValidationException = new
                HostValidationException(invalidHostIdException);

            //when
            ValueTask<Host> removeHostTask =
                this.hostService.RemoveHostAsync(invalidId);

            HostValidationException actuaHostValidationException =
                await Assert.ThrowsAsync<HostValidationException>(removeHostTask.AsTask);

            //then
            actuaHostValidationException.Should().BeEquivalentTo(expectedHostValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedHostValidationException))),
                Times.Once());

            this.storageBrokerMock.Verify(broker =>
             broker.SelectByIdHostAsync(It.IsAny<Guid>()),
                Times.Never());

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteHostAsync(It.IsAny<Host>()),
                Times.Never());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRemoveHostByIdIsNotFoundAndLogItAsync()
        {
            Guid inputHostId = Guid.NewGuid();
            Host noHost = null;

            var notFoundHostException =
                new NotFoundHostException(inputHostId);

            var expectedHostValidationException =
                new HostValidationException(notFoundHostException);

            this.storageBrokerMock.Setup(broker =>
                     broker.SelectByIdHostAsync(inputHostId))
                    .ReturnsAsync(noHost);
            //when
            ValueTask<Host> removeHostTask =
                this.hostService.RemoveHostAsync(inputHostId);
            HostValidationException actualHostValidationException =
                await Assert.ThrowsAsync<HostValidationException>(removeHostTask.AsTask);
            //then
            actualHostValidationException.Should()
                .BeEquivalentTo(expectedHostValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedHostValidationException))),
                Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectByIdHostAsync(inputHostId),
                Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteHostAsync(It.IsAny<Host>()),
                Times.Never());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();

        }
    }
}
