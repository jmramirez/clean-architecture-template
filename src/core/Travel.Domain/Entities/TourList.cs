namespace Travel.Domain.Entities;

public class TourList
{
    public IList<TourPackage> Tours { get; set; } = new List<TourPackage>();
    
    public int Id { get; set; }
    public string City { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string About { get; set; } = default!;
}