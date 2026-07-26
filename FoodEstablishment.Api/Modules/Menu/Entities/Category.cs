using FoodEstablishment.Api.Common.Entities;

namespace FoodEstablishment.Api.Modules.Menu.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}