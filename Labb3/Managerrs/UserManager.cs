using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Labb3ProgTemplate.DataModels.Users;
using Labb3ProgTemplate.Enums;

namespace Labb3ProgTemplate.Managerrs;

public static class UserManager
{
    private static readonly IEnumerable<User>? _users = new List<User>();
    
    private static User _currentUser;

    public static IEnumerable<User>? Users => _users;

    public static User CurrentUser  
    {
        get => _currentUser;
        set
        { 
            _currentUser = value;
            CurrentUserChanged?.Invoke();
        }
    }

    public static event Action CurrentUserChanged;

    public static event Action UserListChanged;

    public static bool IsAdminLoggedIn => CurrentUser != null && CurrentUser.Type is UserTypes.Admin;

    public static void ChangeCurrentUser(string name, string password, UserTypes type)
    {
        if (_users != null && _users.Any(u => u.Name == name))
        {
            MessageBox.Show("Name already exist. Try a different name.");
            return;
        }

        User newUser;

        if (type == UserTypes.Admin)
        {
            newUser = new Admin(name, password);
        }
        else if (type == UserTypes.Customer)
        {
            newUser = new Customer(name, password);
        }
        else
        {
            return;
        }

        if (Users is List<User> users)
        {
            users.Add(newUser);
        }

        UserListChanged?.Invoke();
    }

    public static bool LoginUser(string name, string password)
    {
        if (Users is List<User> users)
        {
            var user = users.Find(u => u.Name == name && u.Authenticate(password));
            if (user != null)
            {
                CurrentUser = user;
                return true;
            }
            
        }
        return false;
    }

    public static void LogOut()
    {
        MainWindow mainWindow = new MainWindow();
        Application.Current.MainWindow = mainWindow;
        mainWindow.Show();
    }

    public static async Task SaveUsersToFile()
    {
        var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Phu");
        Directory.CreateDirectory(directory);
        var fileName = "userDataBase.json";
        var filePath = Path.Combine(directory, fileName);

        var jsonOption = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string json = JsonSerializer.Serialize(_users, jsonOption);

        using (StreamWriter sw = new StreamWriter(filePath, append: false))
        {
            await sw.WriteLineAsync(json);
        }
    }

    public static async Task LoadUsersFromFile()
    {
        var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Phu");
        var fileName = "userDataBase.json";
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
                    foreach (var userElement in root.EnumerateArray())
                    {
                        if (userElement.TryGetProperty("Name", out var nameProperty) &&
                            userElement.TryGetProperty("Password", out var passwordProperty) &&
                            userElement.TryGetProperty("Type", out var typeProperty))
                        {
                            var name = nameProperty.GetString();
                            var password = passwordProperty.GetString();
                            var type = (UserTypes)typeProperty.GetInt32();

                            ChangeCurrentUser(name, password, type);
                        }
                    }
                }
            }
        }
    }
}