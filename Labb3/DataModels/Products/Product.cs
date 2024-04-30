using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Labb3ProgTemplate.DataModels.Products;

public abstract class Product : INotifyPropertyChanged
{

    private string _name;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Name
    {
        get { return _name; }
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    private double _price;

    public double Price
    {
        get { return _price; }
        set
        {
            _price = value;
            OnPropertyChanged();
        }
    }

    private Uri _icon;

    public Uri Icon
    {
        get { return _icon; }
        set
        {
            _icon = value;
            OnPropertyChanged();
        }
    }

    private Enums.ProductTypes _type;

    public Enums.ProductTypes Type
    {
        get { return _type; }
        set
        {
            _type = value; 
            OnPropertyChanged();
        }
    }

    protected Product(string name, double price, Uri icon, Enums.ProductTypes type)
    {
        Name = name;
        Price = price;
        Type = type;
        Icon = icon;
        
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}