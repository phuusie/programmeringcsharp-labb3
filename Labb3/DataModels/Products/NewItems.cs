using System;

namespace Labb3ProgTemplate.DataModels.Products;

public class NewItems : Product
{

    public NewItems(string name, double price, Uri icon, Enums.ProductTypes type) 
        : base(name, price, icon, type)
    {
    }
}