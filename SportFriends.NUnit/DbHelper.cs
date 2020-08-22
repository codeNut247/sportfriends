using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportFriends.API.Data;
using SportFriends.API.Models;

namespace SportFriends.NUnit
{
    public class DbHelper
    {
        public static DataContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new DataContext(options);
            databaseContext.Database.EnsureCreated();
            if (databaseContext.Users.Count() <= 0)
            {
                for (int i = 1; i <= 10; i++)
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash($"password{i}", out passwordHash, out passwordSalt); 
                    databaseContext.Users.Add(new User()
                    {
                        Id = i,
                        UserName = $"user{i}",
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt
                    });
                    databaseContext.SaveChanges();
                }
            }
            return databaseContext;
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}