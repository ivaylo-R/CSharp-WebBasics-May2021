using CarShop.Data;
using CarShop.Data.Models;
using CarShop.Models.Users;
using CarShop.Services;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Linq;

namespace CarShop.Controllers
{
    using static DataConstants;

    public class UsersController : Controller
    {
        private readonly IValidator validator;
        private readonly CarShopDbContext data;
        private readonly IPasswordHasher passwordHasher;

        public UsersController(
            IValidator validator,
            CarShopDbContext data,
            IPasswordHasher passwordHasher)
        {
            this.validator = validator;
            this.data = data;
            this.passwordHasher = passwordHasher;
        }

        public HttpResponse Register()
            => View();

        [HttpPost]
        public HttpResponse Register(RegisterUserFormModel model)
        {
            var modelErrors = validator.ValidateUserRegistration(model);

            if (this.data.Users.Any(u => u.Username == model.Username))
            {
                modelErrors.Add($"User with '{model.Username}' already exists.");
            }

            if (this.data.Users.Any(u => u.Email == model.Email))
            {
                modelErrors.Add($"User with '{model.Email}' already exists.");
            }

            if (modelErrors.Any())
            {
                return Error(modelErrors);
            }

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                IsMechanic = model.UserType == UserTypeMechanic,
                Password = passwordHasher.HashPassword(model.Password),
            };

            data.Users.Add(user);

            data.SaveChanges();

            return Redirect("/Users/Login");
        }

        public HttpResponse Login()
        {
            return View();
        }

        [HttpPost]
        public HttpResponse Login(LoginUserFormModel model)
        {
            var hashedPassword = passwordHasher.HashPassword(model.Password);
            var userId = this.data
                .Users
                .Where(u => u.Username == model.Username
                && u.Password==hashedPassword)
                .Select(u => u.Id)
                .FirstOrDefault();

            if (userId==null)
            {
                return Error("Password or username are not valid.");
            }

            this.SignIn(userId);
            

            return Redirect("/Cars/All");
        }

        public HttpResponse Logout()
        {
            this.SignOut();

            return Redirect("/");
        }

    }
}
