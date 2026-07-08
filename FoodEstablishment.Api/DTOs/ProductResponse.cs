namespace FoodEstablishment.Api.DTOs;

public class ProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int VolumeOrWeight { get; set; }
    public string Unit { get; set; } = string.Empty;
    public int Calories { get; set; }
    public bool IsStopListed { get; set; }
    public int CategoryId { get; set; }
}