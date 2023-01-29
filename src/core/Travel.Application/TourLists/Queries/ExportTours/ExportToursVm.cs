namespace Travel.Application.TourLists.Queries.ExportTours;

public class ExportToursVm
{
    public string FileName { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public byte[] Content { get; set; } = default!;
}