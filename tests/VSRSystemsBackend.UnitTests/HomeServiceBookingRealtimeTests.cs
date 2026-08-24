using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using VSRSystemsBackend.Api.Controllers;
using VSRSystemsBackend.Application.HomeServices.DTOs;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Application.Platform.Realtime;
using VSRSystemsBackend.Core.Common;
using Xunit;

namespace VSRSystemsBackend.UnitTests;

public sealed class HomeServiceBookingRealtimeTests
{
    [Fact]
    public async Task SuccessfulBookingStateMutationsPublishAfterServiceSuccess()
    {
        var bookingService = new Mock<IBookingService>();
        var assignmentService = new Mock<IAssignmentService>();
        var publisher = new Mock<IRealtimePublisher>();
        var published = new List<(string Group, RealtimeEventEnvelope<BookingStatusChangedPayload> Message)>();

        bookingService.Setup(service => service.CancelAsync("booking-1", "changed plans", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<BookingDto>.Success(Booking("cancelled")));
        bookingService.Setup(service => service.ConfirmAsync("booking-1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<BookingDto>.Success(Booking("scheduled")));
        bookingService.Setup(service => service.StartAsync("booking-1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<BookingDto>.Success(Booking("in_progress")));
        bookingService.Setup(service => service.CompleteAsync("booking-1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<BookingDto>.Success(Booking("completed")));
        bookingService.Setup(service => service.RescheduleAsync("booking-1", It.IsAny<RescheduleBookingDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<BookingDto>.Success(Booking("rescheduled")));
        assignmentService.Setup(service => service.AssignAsync("booking-1", "professional-1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<BookingAssignmentDto>.Success(new BookingAssignmentDto
            {
                BookingId = "booking-1",
                ProfessionalId = "professional-1"
            }));
        publisher.Setup(service => service.SendToGroupAsync(
                It.IsAny<string>(),
                It.IsAny<RealtimeEventEnvelope<BookingStatusChangedPayload>>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, RealtimeEventEnvelope<BookingStatusChangedPayload>, CancellationToken>(
                (group, message, _) => published.Add((group, message)))
            .Returns(Task.CompletedTask);

        var controller = CreateController(bookingService, assignmentService, publisher);

        await controller.Cancel("booking-1", new CancelBookingDto { Reason = "changed plans" });
        await controller.Assign("booking-1", "professional-1");
        await controller.Confirm("booking-1");
        await controller.Start("booking-1");
        await controller.Complete("booking-1");
        await controller.Reschedule("booking-1", new RescheduleBookingDto
        {
            ScheduledStart = DateTime.UtcNow.AddDays(1),
            Reason = "new time"
        });

        Assert.Equal(6, published.Count);
        Assert.All(published, item =>
        {
            Assert.Equal(RealtimeGroups.HomeServicesBooking("booking-1"), item.Group);
            Assert.Equal(RealtimeEventTypes.HomeServicesBookingStatusChanged, item.Message.EventType);
            Assert.Equal(1, item.Message.Version);
            Assert.Equal("trace-1", item.Message.CorrelationId);
        });
        Assert.Equal(
            new[] { "cancelled", "assigned", "scheduled", "in_progress", "completed", "rescheduled" },
            published.Select(item => item.Message.Payload.Status));
    }

    [Fact]
    public async Task FailedBookingStateMutationDoesNotPublish()
    {
        var bookingService = new Mock<IBookingService>();
        var publisher = new Mock<IRealtimePublisher>();
        bookingService.Setup(service => service.ConfirmAsync("booking-1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<BookingDto>.Failure("invalid transition"));

        var controller = CreateController(
            bookingService,
            new Mock<IAssignmentService>(),
            publisher);

        await controller.Confirm("booking-1");

        publisher.Verify(service => service.SendToGroupAsync(
            It.IsAny<string>(),
            It.IsAny<RealtimeEventEnvelope<BookingStatusChangedPayload>>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    private static HomeServiceBookingsController CreateController(
        Mock<IBookingService> bookingService,
        Mock<IAssignmentService> assignmentService,
        Mock<IRealtimePublisher> publisher)
    {
        var controller = new HomeServiceBookingsController(
            bookingService.Object,
            Mock.Of<IPriceQuoteService>(),
            assignmentService.Object,
            publisher.Object,
            NullLogger<HomeServiceBookingsController>.Instance);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            TraceIdentifier = "trace-1"
        };
        return controller;
    }

    private static BookingDto Booking(string status) => new()
    {
        Id = "booking-1",
        Status = status,
        AssignedProfessionalId = "professional-1",
        ScheduledStart = DateTime.UtcNow.AddHours(1)
    };
}
