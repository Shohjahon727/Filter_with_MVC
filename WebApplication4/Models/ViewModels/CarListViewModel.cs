using WebApplication4.Entities;

namespace WebApplication4.Models.ViewModels;

public class CarListViewModel
{

    public List<Car> Cars { get; set; }
    public PaginationViewModel Pagination { get; set; }
    public FilterViewModel Filter { get; set; }

}
