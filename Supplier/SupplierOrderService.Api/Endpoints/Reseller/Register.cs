using FluentValidation;
using MediatR;
using SharedKernel;
using SupplierOrderService.Application.Dtos;
using SupplierOrderService.Application.Reseller.Register;
using SupplierOrderService.Extensions;
using SupplierOrderService.Infrastructure;

namespace SupplierOrderService.Api.Endpoints.Reseller;

internal sealed class Register : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/resellers", async (ResellerDto request,
                                         IValidator<ResellerDto> validator,
                                         ISender sender,
                                         CancellationToken cancellationToken) =>
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            var command = new RegisterResellerCommand(request);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(id => Results.Created($"/resellers/{id}", new { id }), CustomResults.Problem);
        })
        .WithTags("resellers");
    }
}
