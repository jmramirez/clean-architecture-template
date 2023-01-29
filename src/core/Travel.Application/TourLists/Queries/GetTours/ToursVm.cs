using Travel.Application.Dtos;

namespace Travel.Application.TourLists.Queries.GetTours;

public class ToursVm
{
    public IList<TourListDto> Lists { get; set; } = new List<TourListDto>();
}