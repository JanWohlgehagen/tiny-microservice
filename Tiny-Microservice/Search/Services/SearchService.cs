using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Search.Entities;
using Search.Services.Interfaces;
using SharedModels;

namespace Services;

public class SearchService : ISearchService
{
    public static List<UserSimpleDTO> UserList = new();

    public SearchService()
    {

    }

    // metode som rammes fra messaging, der tilføjer UserSimpleDTO til UserList
    internal void HandleCreateUser(UserFullDTO e)
    {
        Console.WriteLine("Entering HandleCreateUser()");
        if (e != null)
        {
            UserList.Add(new UserSimpleDTO
            {
                id = e.id,
                name = e.name,
                imageURL = e.imageURL,
            });
            Console.WriteLine("UserList updated");
            Console.WriteLine("UserList.Count = " + UserList.Count);
            Console.WriteLine("UserList = " + UserList);
        }
        else
        {
            Console.WriteLine("Received null response from the message bus (createUserResult).");
        }
    }

    // metode som rammes fra messaging, der opdaterer UserSimpleDTO i UserList
    internal void HandleUpdateUser(UserFullDTO e)
    {
        if (e != null)
        {
            var userToUpdate = UserList.FirstOrDefault(u => u.id == e.id);
            if (userToUpdate != null)
            {
                // Update properties of the user
                userToUpdate.name = e.name;
                userToUpdate.imageURL = e.imageURL;
            }
            else
            {
                Console.WriteLine($"User with ID {e.id} not found in the UserList.");
            }
        }
        else
        {
            Console.WriteLine("Received null response from the message bus (updateUserResult).");
        }
    }

    // Metode der finder users fra UserList
    internal Task<List<UserSimpleDTO>> FindUser(string searchString)
    {
        Console.WriteLine("UserList inside FindUser() = " + UserList);
        Console.WriteLine("UserList.Count inside FindUser() = " + UserList.Count);
        var filteredUsers = UserList.Where(user =>
            user.name.Contains(searchString, StringComparison.OrdinalIgnoreCase)
        ).ToList();

        return Task.FromResult(filteredUsers);
    }

    internal void AddUserAsync(UserSimpleDTO user)
    {
        UserList.Add(user);
    }

}

