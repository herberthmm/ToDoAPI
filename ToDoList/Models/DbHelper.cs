using Microsoft.EntityFrameworkCore;
using System.Text;
using ToDoList.Database;
using ToDoList.Enums;

namespace ToDoList.Models
{
    public class DbHelper
    {
        private Context _context;

        public DbHelper(Context context)
        {
            _context = context;
        }

        #region [ Login ]

        public bool VerifyLogin(LoginModel login)
        {
            var userDb = _context.User.Where(c => c.Username == login.username).FirstOrDefault();

            //From here is the same process as BaseAtuhenticationHandler
            var credentialBytes = Convert.FromBase64String(login.username);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(":");
            var user = credentials[0];
            var password = credentials[1];

            if (userDb != null)
            {
                //if (user == "Admin" && password == "Admin1234")
                if (user == userDb.Username && password == userDb.PasswordHash)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region [ Users ]       

        public UserModel GetUserById(int userId)
        {
            UserModel response = new UserModel();
            var data = _context.User.Include(p => p.ToDo).Where(c => c.UserId == userId).FirstOrDefault();

            if (data != null)
            {
                response.Username = data.Username;
                response.Id = data.UserId;
                response.CreatedAt = data.CreatedAt;

                if (data.ToDo != null && data.ToDo.Any())
                {
                    response.ToDos = new List<ToDoModel>();

                    data.ToDo.ToList().ForEach(row => response.ToDos.Add(new ToDoModel()
                    {
                        Category = row.Category,
                        CreatedAt = row.CreatedAt,
                        Description = row.Description,
                        Id = row.Id,
                        IsCompleted = row.IsCompleted,
                        Title = row.Title,
                        UpdatedAt = row.UpdatedAt,
                        UserId = row.UserId
                    }));
                }
            }

            return response;
        }

        public void SaveUser(UserModel user)
        {
            if (user.Id > 0)
            {
                // PUT
                var userData = _context.User.Where(c => c.UserId == user.Id).FirstOrDefault();

                if (userData != null)
                {
                    userData.Username = user.Username;
                    userData.PasswordHash = user.PasswordHash;

                    _context.User.Update(userData);
                }
            }
            else
            {
                User newUser = new User()
                {
                    UserId = user.Id,
                    Username = user.Username,
                    CreatedAt = user.CreatedAt,
                    PasswordHash = user.PasswordHash
                };

                _context.User.Add(newUser);
            }

            _context.SaveChanges();
        }

        public void DeleteUser(int userId)
        {
            var userTable = _context.User.Where(c => c.UserId == userId).FirstOrDefault();

            if (userTable != null)
            {
                _context.User.Remove(userTable);

                _context.SaveChanges();
            }
        }

        public LoginModel GetUserByUsername(string username)
        {
            var data = _context.User.Where(c => c.Username == username).FirstOrDefault();
            LoginModel user = new LoginModel();

            if (data != null)
            {
                user.username = data.Username;
                username = data.Username;
            }

            return user;
        }

        #endregion

        #region [ To Do ]

        public List<ToDoModel> GetToDos()
        {
            List<ToDoModel> response = new List<ToDoModel>();
            var data = _context.ToDo.ToList();

            data.ForEach(row => response.Add(new ToDoModel()
            {
                Id = row.Id,
                Category = row.Category,
                CreatedAt = row.CreatedAt,
                Description = row.Description,
                IsCompleted = row.IsCompleted,
                Title = row.Title,
                UpdatedAt = row.UpdatedAt,
                UserId = row.UserId,
            }));

            return response;
        }

        public List<ToDoModel> GetToDoByUser(int userId)
        {
            List<ToDoModel> response = new List<ToDoModel>();
            var data = _context.ToDo.Where(c => c.UserId == userId).ToList();

            data.ForEach(row => response.Add(new ToDoModel()
            {
                Id = row.Id,
                Category = row.Category,
                CreatedAt = row.CreatedAt,
                Description = row.Description,
                IsCompleted = row.IsCompleted,
                Title = row.Title,
                UpdatedAt = row.UpdatedAt,
                UserId = row.UserId,
            }));

            return response;
        }

        public ToDoModel GetToDoById(int Id)
        {
            ToDoModel response = new ToDoModel();

            var data = _context.ToDo.Where(c => c.Id == Id).FirstOrDefault();

            if (data != null)
            {

                response.Id = data.Id;
                response.Category = data.Category;
                response.CreatedAt = data.CreatedAt;
                response.Description = data.Description;
                response.IsCompleted = data.IsCompleted;
                response.Title = data.Title;
                response.UpdatedAt = data.UpdatedAt;
                response.UserId = data.UserId;
            }

            return response;
        }

        public ToDoModel GetToDoByCategory(int userId, int category)
        {
            ToDoModel response = new ToDoModel();

            var data = _context.ToDo.Where(c => c.UserId == userId && c.Category == (Category)category).FirstOrDefault();

            if (data != null)
            {
                response.Id = data.Id;
                response.UserId = data.UserId;
                response.Title = data.Title;
                response.Description = data.Description;
                response.Category = data.Category;
                response.IsCompleted = data.IsCompleted;
                response.UpdatedAt = data.UpdatedAt;
                response.CreatedAt = data.CreatedAt;
            }

            return response;
        }

        public void SaveToDo(ToDoModel toDo)
        {
            if (toDo.Id > 0)
            {
                // PUT
                var toDoTable = _context.ToDo.Where(c => c.Id == toDo.Id).FirstOrDefault();

                if (toDoTable != null)
                {
                    toDoTable.Title = toDo.Title;
                    toDoTable.Description = toDo.Description;
                    toDoTable.Category = toDo.Category;
                    toDoTable.IsCompleted = toDo.IsCompleted;
                    toDoTable.UpdatedAt = DateTime.Now;

                    _context.ToDo.Update(toDoTable);
                }
            }
            else
            {
                ToDo item = new ToDo();

                item.Title = toDo.Title;
                item.Description = toDo.Description;
                item.Category = toDo.Category;
                item.IsCompleted = toDo.IsCompleted;
                item.CreatedAt = DateTime.Now;
                item.UserId = toDo.UserId;

                _context.ToDo.Add(item);
            }

            _context.SaveChanges();
        }

        public void DeleteToDo(int toDoId)
        {
            var toDoTable = _context.ToDo.Where(c => c.Id == toDoId).FirstOrDefault();

            if (toDoTable != null)
            {
                _context.ToDo.Remove(toDoTable);

                _context.SaveChanges();
            }
        }

        #endregion
    }
}