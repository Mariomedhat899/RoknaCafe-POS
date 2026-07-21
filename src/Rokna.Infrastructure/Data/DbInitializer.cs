using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Rokna.Domain.Entities;

namespace Rokna.Infrastructure.Data;

public class CategoryDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class MenuItemDto
{
    [JsonPropertyName("categoryName")]
    public string CategoryName { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("price")]
    public decimal Price { get; set; }
}

public static class DbInitializer
{
    public static async Task SeedAsync(RoknaDbContext context)
    {
        if (await context.Categories.AnyAsync())
            return;

        var assembly = typeof(DbInitializer).Assembly;
        var root = assembly.GetName().Name!;

        var categoriesJson = await ReadEmbeddedResourceAsync(assembly, $"{root}.Data.Seeding.categories.json");
        var menuItemsJson = await ReadEmbeddedResourceAsync(assembly, $"{root}.Data.Seeding.menuitems.json");

        var categoryDtos = JsonSerializer.Deserialize<List<CategoryDto>>(categoriesJson);
        var menuItemDtos = JsonSerializer.Deserialize<List<MenuItemDto>>(menuItemsJson);

        var categoryEntities = categoryDtos!
            .Select((dto, index) => new Category
            {
                Name = dto.Name,
                DisplayOrder = index
            })
            .ToList();

        await context.Categories.AddRangeAsync(categoryEntities);
        await context.SaveChangesAsync();

        var categoryLookup = categoryEntities.ToDictionary(c => c.Name, c => c.Id);

        var menuItemEntities = menuItemDtos!
            .Where(m => categoryLookup.ContainsKey(m.CategoryName))
            .Select(m => new MenuItem
            {
                Name = m.Name,
                Price = m.Price,
                CategoryId = categoryLookup[m.CategoryName]
            })
            .ToList();

        if (menuItemEntities.Any())
        {
            await context.MenuItems.AddRangeAsync(menuItemEntities);
            await context.SaveChangesAsync();
        }
    }

    private static async Task<string> ReadEmbeddedResourceAsync(Assembly assembly, string resourceName)
    {
        await using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new FileNotFoundException($"Embedded resource not found: {resourceName}");
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}
