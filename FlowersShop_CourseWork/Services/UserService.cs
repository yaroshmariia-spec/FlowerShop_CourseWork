using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using FlowersShop_CourseWork.Models;

namespace FlowersShop_CourseWork.Services;

public class UserService
{
    private readonly string _filePath = "users.json";
    
    private List<User> _users;
    
    private Dictionary<string, string> _userCredentials;

    public UserService()
    {
        LoadUsers();
    }
    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }
    private void LoadUsers()
    {
        _users = new List<User>();
        _userCredentials = new Dictionary<string, string>();

        if (File.Exists(_filePath))
        {
            try
            {
                string json = File.ReadAllText(_filePath);
                _users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            catch
            {
                _users = new List<User>();
            }
        }
        
        foreach (var user in _users)
        {
            _userCredentials[user.Email] = user.Password;
        }
    }

    private void SaveUsers()
    {
        string json = JsonSerializer.Serialize(_users);
        File.WriteAllText(_filePath, json);
    }

    public bool Register(string email, string password, bool isAdmin)
    {
        if (_userCredentials.ContainsKey(email))
        {
            return false;
        }
        string hashedPassword = HashPassword(password);
        var newUser = new User
        {
            Email = email,
            Password = hashedPassword,
            Role = isAdmin ? "Admin" : "User"
        };

        _users.Add(newUser);
        _userCredentials[email] = password; 

        SaveUsers();
        return true;
    }

    public User Authenticate(string email, string password)
    {
        string hashedInput = HashPassword(password);
        if (_userCredentials.TryGetValue(email, out string storedPassword))
        {
            if (storedPassword == hashedInput)
            {
                return _users.FirstOrDefault(u => u.Email == email);
            }
        }

        return null; 
    }
}