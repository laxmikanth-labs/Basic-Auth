
using System.Text;
using System.Text.Json;

public class UserDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class Program
{
   
    private static readonly string baseUrl = "http://localhost:5297";
    private static readonly string username = "laxmikanth@gmail.com";
    private static readonly string password = "passwrod123";
    private static async Task Main(string[] args)
    {
        using var httpClient = new HttpClient();

        var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("basic", authToken);

        Console.WriteLine("Basic Auth Console client");

        await GetAllUsersAsync(httpClient);
        await GetUserByIdAsync(httpClient, 1);
        var newUserId = await CreateUserAsync(httpClient);
        await UpdateUserAsync(httpClient, newUserId);
        await DeleteUserAsync(httpClient, newUserId);

        Console.WriteLine("All Operations are Completed");
        Console.ReadKey();


    }

    private static async Task GetAllUsersAsync(HttpClient httpClient)
    {
        Console.WriteLine("\n---- Get All Users ----");

        try
        {
            var response = await httpClient.GetAsync($"{baseUrl}/api/User");
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<UserDTO>>(responseBody);

            Console.WriteLine("Users");

            if (users != null)
            {
                foreach (var user in users)
                {
                    Console.WriteLine($"Id: {user.Id}, Email: {user.Email}, Name: {user.FirstName} {user.LastName}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching users: {ex.Message}");
        }
    }
    private static async Task GetUserByIdAsync(HttpClient httpClient, int id)
    {
        Console.WriteLine($"\n--- GET USER BY ID: {id} ---");
        try
        {
            var response = await httpClient.GetAsync($"{baseUrl}/api/User/{id}");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to get user. HTTP Status: {response.StatusCode}");
                return;
            }
            var responseBody = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<UserDTO>(responseBody);
            if (user != null)
            {
                Console.WriteLine($"User found - Id: {user.Id}, Email: {user.Email}, Name: {user.FirstName} {user.LastName}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching user by Id: {ex.Message}");
        }
    }
    private static async Task<int> CreateUserAsync(HttpClient httpClient)
    {
        Console.WriteLine("\n--- CREATE NEW USER (POST) ---");
        var newUser = new UserDTO
        {
            FirstName = "Laxmi kanth",
            LastName = "Munagala",
            Email = $"laxmikanth@gmail.com",
            Password = "password123"
        };
        try
        {
            var payload = JsonSerializer.Serialize(newUser);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{baseUrl}/api/User", content);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to create user. HTTP Status: {response.StatusCode}");
                return 0;
            }
            var responseBody = await response.Content.ReadAsStringAsync();
            var createdUser = JsonSerializer.Deserialize<UserDTO>(responseBody);
            if (createdUser != null)
            {
                Console.WriteLine($"Created user Id: {createdUser.Id}, Email: {createdUser.Email}");
                return createdUser.Id;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating user: {ex.Message}");
        }
        return 0;
    }
    private static async Task UpdateUserAsync(HttpClient httpClient, int userId)
    {
        if (userId <= 0) return;
        Console.WriteLine("\n--- UPDATE USER (PUT) ---");
        var updatedUser = new UserDTO
        {
            Id = userId,
            FirstName = "Jane (updated)",
            LastName = "Smith (updated)",
            Email = $"jane.smith.updated_{Guid.NewGuid()}@example.com",
            Password = "MyUpdatedPassword123"
        };
        try
        {
            var payload = JsonSerializer.Serialize(updatedUser);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync($"{baseUrl}/api/User/{userId}", content);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to update user. HTTP Status: {response.StatusCode}");
                return;
            }
            Console.WriteLine("User successfully updated.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating user: {ex.Message}");
        }
    }
    private static async Task DeleteUserAsync(HttpClient httpClient, int userId)
    {
        if (userId <= 0) return;
        Console.WriteLine("\n--- DELETE USER ---");
        try
        {
            var response = await httpClient.DeleteAsync($"{baseUrl}/api/User/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to delete user. HTTP Status: {response.StatusCode}");
                return;
            }
            Console.WriteLine($"User with Id: {userId} deleted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting user: {ex.Message}");
        }
    }
}