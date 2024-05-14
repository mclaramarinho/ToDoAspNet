using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Services
{
    public interface IUsersServices
    {
        List<User> GetUsers();
        User GetUser(int id);
        Task<bool> CreateUser(User user);
        Task<bool> LoginUser(string email,  string password);
        Task<bool> UpdateUser(int id, User user);
        Task<bool> DeleteUser(int id);
        bool UserExists(int id);
        User? FindUserByEmail(string email);
    }
    public class UsersService : IUsersServices
    {
        public readonly ApplicationDbContext _db;

        public UsersService(ApplicationDbContext db) => _db = db;

        public bool UserExists(int id) => _db.Users.Any(u => u.UserID == id);

        public List<User> GetUsers() => _db.Users.ToList();

        public User GetUser(int id) {

            if (!UserExists(id))
            {
                throw new Exception("User not found");
            }

            return _db.Users.FirstOrDefault(u => u.UserID == id);
        }

        public Task<bool> CreateUser(User user)
        {
            var promise = new TaskCompletionSource<bool>();

            try { 
                _db.Users.Add(user);
                _db.SaveChanges();
                promise.SetResult(true);
                return promise.Task;
            }
            catch (DbUpdateException dbuex)
            {
                promise.SetException(new Exception("invalid email"));
                return promise.Task;
            }
            catch(SystemException sysex)
            {
                promise.SetException(new Exception("Unexpected error"));
                return promise.Task;
            }
            catch (Exception e)
            {
                promise.SetResult(false);
                return promise.Task;
            }
        }

        public Task<bool> LoginUser(string email, string password)
        {
            var promise = new TaskCompletionSource<bool>(false);

            bool exists = _db.Users.Any(u => u.Email == email);
            if(!exists)
            {
                promise.SetException(new Exception("User does not exist!"));
                return promise.Task;
            }

            User user = _db.Users.FirstOrDefault(u => u.Email == email);
            if(user != null) { 
                bool isPswdCorrect = user.Password == password;
                if (!isPswdCorrect)
                {
                    promise.SetException(new Exception("Invalid password"));
                    return promise.Task;
                }
                else
                {
                    promise.SetResult(true);
                }
            }
            else
            {
                promise.SetException(new Exception("An unexpected error occurred!"));
                return promise.Task;
            }

            return promise.Task;
        }

        public Task<bool> UpdateUser(int id, User user)
        {
            if (!UserExists(id))
            {
                throw new Exception("User not found");
            }

            var promise = new TaskCompletionSource<bool>();

            try
            {
                _db.Users.Update(user);
                _db.SaveChanges();
                promise.SetResult(true);
                return promise.Task;
            }catch(Exception ex)
            {
                promise.SetResult(false);
                return promise.Task;
            }
        }

        public Task<bool> DeleteUser(int id)
        {
            if (!UserExists(id))
            {
                throw new Exception("User not found");
            }

            var promise = new TaskCompletionSource<bool>();
            try
            {
                User u = _db.Users.FirstOrDefault(i => i.UserID == id);
                _db.Users.Remove(u);
                _db.SaveChanges();
                promise.SetResult(true);
                return promise.Task;
            }catch( Exception ex)
            {
                promise.SetResult(false);
                return promise.Task;
            }
        }

        public User? FindUserByEmail(string email)
        {
            return _db.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}
