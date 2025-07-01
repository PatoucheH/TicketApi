using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TicketApi.Data;
using TicketApi.Models;

namespace TicketApi.Services
{
    /// <summary>
    /// The interface for users
    /// </summary>
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserById(int id);
        Task<User> CreateUser(User user);
        Task DeleteUser(int id);
        Task UpdateAllInfoAboutOneUser(int id , User userDto);
    }

    /// <summary>
    /// Provides methods for retrieving user information from the database.
    /// </summary>
    /// <remarks>This service is responsible for accessing user data, including retrieving all users or a
    /// specific user by their ID. It interacts with the underlying database context to perform these
    /// operations.</remarks>
    /// <param name="context"></param>
    public class UserService(ContextDatabase context) : IUserService
    {
        private readonly ContextDatabase _context = context;

        /// <summary>
        /// Retrieves all users from the database, including their associated tickets.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <IEnumerable{T}> of <User>
        /// objects, where each user includes  their associated tickets.</returns>
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.Include(u => u.Tickets).ToListAsync();
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to retrieve.</param>
        /// <returns>A <see cref="User"/> object representing the user with the specified identifier,  or <see langword="null"/>
        /// if no user with the given identifier exists.</returns>
        public async Task<User?> GetUserById(int id)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
        }

        /// <summary>
        /// Asynchronously creates a new user and saves it to the database.
        /// </summary>
        /// <param name="user">The user entity to be created. Must not be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created user entity.</returns>
        public async Task<User> CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        /// <summary>
        /// Deletes a user with the specified identifier from the database.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeleteUser(int id)
        {
            var user = new User { Id = id };
            _context.Entry(user).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates all information for a specific user in the database.
        /// </summary>S
        /// <param name="id">The unique identifier of the user to update. This parameter is not used in the method but may be required
        /// for external context.</param>
        /// <param name="userDto">An object containing the updated user information. The object must not be null.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateAllInfoAboutOneUser(int id, User userDto)
        {
            _context.Entry(userDto).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }


    }
}

