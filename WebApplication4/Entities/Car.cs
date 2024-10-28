using WebApplication4.Enums;

namespace WebApplication4.Entities
{
	public class Car
	{
		public int Id { get; set; }
		public Manufacturers Manufacturer { get; set; }
		public string Model { get; set; }
		public Colors Color { get; set; }
		public decimal Price { get; set; }
	}
}
