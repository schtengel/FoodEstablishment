using FoodEstablishment.Api.Common.Entities;

namespace FoodEstablishment.Api.Modules.Inventory.Entities;

public class StorageZone : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal RecommendedTemperature { get; set; }

    public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
}