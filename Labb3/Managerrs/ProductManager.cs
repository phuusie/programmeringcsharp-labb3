using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Labb3ProgTemplate.DataModels.Products;
using Labb3ProgTemplate.Enums;

namespace Labb3ProgTemplate.Managerrs;

public static class ProductManager
{
    private static readonly IEnumerable<Product>? _products = new List<Product>();

    public static IEnumerable<Product>? Products => _products;

    private static readonly Dictionary<ProductTypes, string?> TypeIconPaths = new()
    {
        { ProductTypes.Bread, "/Assets/bread.png" },
        { ProductTypes.DairyCheese, "/Assets/dairycheese.png" },
        { ProductTypes.FruitsVegetables, "/Assets/fruitsvegetables.png" },
        { ProductTypes.Meat, "/Assets/meat.png" },
        { ProductTypes.SweetsIcecream, "/Assets/sweetsicecream.png" }
    };

    public static event Action ProductListChanged;

    public static void AddProduct(Product product)
    {
        if (Products is List<Product> productsList)
        {
            productsList.Add(product);
        }

        ProductListChanged?.Invoke();
    }

    public static void RemoveProduct(Product product)
    {
        if (Products is List<Product> productsList)
        {
            productsList.Remove(product);
        }
        ProductListChanged?.Invoke();
    }

    public static async Task SaveProductsToFile()
    {
        var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Phu");
        Directory.CreateDirectory(directory);
        var fileName = "productDataBase.json";
        var filePath = Path.Combine(directory, fileName);

        var jsonOption = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string json = JsonSerializer.Serialize(_products, jsonOption);

        using (StreamWriter sw = new StreamWriter(filePath, append: false))
        {
            await sw.WriteLineAsync(json);
        }
    }

    public static async Task LoadProductsFromFile()
    {
        var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Phu");
        var fileName = "productDataBase.json";
        var filePath = Path.Combine(directory, fileName);

        if (File.Exists(filePath))
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                var json = await sr.ReadToEndAsync();

                var jsonDocument = JsonDocument.Parse(json);
                var root = jsonDocument.RootElement;

                if (root.ValueKind == JsonValueKind.Array)
                {
                    foreach (var productElement in root.EnumerateArray())
                    {
                        if (productElement.TryGetProperty("Name", out var nameProperty) && 
                            productElement.TryGetProperty("Price", out var priceProperty) &&
                            productElement.TryGetProperty("Type", out var typeProperty) &&
                            productElement.TryGetProperty("Icon", out var iconProperty))
                        {
                            var productName = nameProperty.GetString();
                            var productPrice = priceProperty.GetDouble();
                            var productType = (ProductTypes)typeProperty.GetInt32();
                            var productIcon = iconProperty.GetString();

                            if (Products is List<Product> productsList)
                            {
                                Uri productIconUri = null;

                                if (!string.IsNullOrWhiteSpace(productIcon))
                                {
                                    productIconUri = new Uri(productIcon, UriKind.RelativeOrAbsolute);
                                }

                                var product = new NewItems(productName, productPrice, productIconUri, productType);
                                productsList.Add(product);

                            }
                        }
                    }
                }
                ProductListChanged?.Invoke();
            }
        }
    }

    public static string GetIconPath(ProductTypes productType)
    {
        if (TypeIconPaths.TryGetValue(productType, out string? iconPath))
        {
            return iconPath;
        }

        return null;
    }

    public static string GetIconPathByType(ProductTypes productType)
    {
        return GetIconPath(productType);
    }
}