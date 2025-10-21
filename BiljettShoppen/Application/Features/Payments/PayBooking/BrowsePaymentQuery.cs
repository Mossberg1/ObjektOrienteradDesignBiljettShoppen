using MediatR;
using Models.Entities;

namespace Application.Features.Payments.PayBooking;
 
public record BrowsePaymentQuery () : IRequest<List<Payment>>;