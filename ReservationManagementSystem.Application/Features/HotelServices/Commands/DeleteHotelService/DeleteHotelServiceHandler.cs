using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.HotelServices.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.HotelServices.Commands.DeleteHotelService;

public sealed class DeleteHoteServicelHandler : IRequestHandler<DeleteHotelServiceRequest, Result<HotelServiceResponse>>
{
    private readonly IHotelServiceRepository _hotelServiceRepository;
    private readonly IMapper _mapper;

    public DeleteHoteServicelHandler(IHotelServiceRepository hotelRepository, IMapper mapper)
    {
        _hotelServiceRepository = hotelRepository;
        _mapper = mapper;
    }

    public async Task<Result<HotelServiceResponse>> Handle(DeleteHotelServiceRequest request, CancellationToken cancellationToken)
    {
        var hotelService = await _hotelServiceRepository.Delete(request.Id, cancellationToken);

        if (hotelService is null)
        {
            return Result<HotelServiceResponse>.Failure(HotelServiceErrors.NotFound(request.Id));
        }

        var response = _mapper.Map<HotelServiceResponse>(hotelService);

        return Result<HotelServiceResponse>.Success(response);
    }
}
