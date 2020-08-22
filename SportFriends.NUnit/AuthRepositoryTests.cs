using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using SportFriends.API.Data;
using SportFriends.API.Models;

namespace SportFriends.NUnit
{
    [TestFixture]
    public class Tests
    {
        private DataContext _context;
        
        [SetUp]
        public void Setup()
        {
            this._context = DbHelper.GetDatabaseContext();
        }

        [TestCase("user1", "password1")]
        [TestCase("user2", "password2")]
        public async Task Login_Should_Return_User_When_Login_Successfull(string username, string password)
        {
            //Arrange
            var dbContext = DbHelper.GetDatabaseContext();
            var repo = new AuthRepository(dbContext);
            //Act
            var one = await dbContext.Users.FirstAsync(x => x.UserName == username);
            var user = await repo.Login(username, password);
            //Assert
            Assert.NotNull(user);
        }
        
        [TestCase("user", "password")]
        public async Task Login_Should_Return_NULL_When_Login_Failed(string username, string password)
        {
            //Arrange
            var dbContext = DbHelper.GetDatabaseContext();
            var repo = new AuthRepository(dbContext);
            //Act
            var user = await repo.Login(username, password);
            //Assert
            Assert.IsNull(user);
        }
        
        [TestCase("user", "password")]
        public async Task Register_Should_Add_User_To_Context(string username, string password)
        {
            //Arrange
            var dbContext = DbHelper.GetDatabaseContext();
            var repo = new AuthRepository(dbContext);
            //Act
            var user = await repo.Register(new User() { UserName = username}, password);
            //Assert
            Assert.NotNull(user);
            Assert.IsTrue(user.UserName == username);
        }
    }
}