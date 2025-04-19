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
       public async Task ShoulThrowHostValidationExceptionOnRetrieveByIdIfIdIsInvalidLogItAsync()
        {
            //given
            Guid invalidGuid = Guid.Empty;
            var invalidHostException = new InvalidHostException();

            invalidHostException.AddData(
                key:nameof(Host.Id),
                values:"Id is required");
            
            var expectedHostValidationException = new HostValidationException(invalidHostException);

            //when
            ValueTask<Host> retriveByIdHostTask = this.hostService.RetrieveByIdHostAsync(invalidGuid);

            HostValidationException actualHostValidationException =
                await Assert.ThrowsAsync<HostValidationException>(retriveByIdHostTask.AsTask);

            //then
            actualHostValidationException.Should().BeEquivalentTo(expectedHostValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedHostValidationException))), 
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectByIdHostAsync(It.IsAny<Guid>()),
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();

        }
    }
}
