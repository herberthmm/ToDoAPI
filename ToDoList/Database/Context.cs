using ToDoList.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.Database
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) 
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        //// Avisando ao Core quais objetos são criados ao executar o migration, e quais serão gerenciados pelo Core
        public DbSet<ToDo> ToDo { get; set; }
        public DbSet<User> User { get; set; }
    }
}