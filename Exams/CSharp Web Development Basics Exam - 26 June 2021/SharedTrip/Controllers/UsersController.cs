using MyWebServer.Controllers;
using MyWebServer.Http;
using SharedTrip.Data;
using SharedTrip.Models;
using SharedTrip.Models.Users;
using SharedTrip.Services;
using System.Linq;

namespace SharedTrip.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext data;
        private readonly IPasswordHasher passwordHasher;
        private readonly IValidator validator;

        public UsersController(
            ApplicationDbContext data,
            IPasswordHasher passwordHasher, IValidator validator)
        {
            this.passwordHasher = passwordHasher;
            this.data = data;
            this.validator = validator;
        }

        [HttpGet]
        public HttpResponse Login()
        {
            if (this.User.IsAuthenticated)
            {
                return Redirect("/Trips/All");
            }

            return View();
        }


        [HttpPost]
        public HttpResponse Login(UserLoginFormModel model)
        {
            var hashedPassword = passwordHasher.HashPassword(model.Password);
            var userId = this.data
                .Users
                .Where(u => u.Username == model.Username
                && u.Password == hashedPassword)
                .Select(u => u.Id)
                .FirstOrDefault();

            if (userId == null)
            {
                return Redirect("/Users/Login");
            }

            this.SignIn(userId);


            return Redirect("/Trips/All");
        }

        [HttpGet]
        public HttpResponse Register()
        {
            if (this.User.IsAuthenticated)
            {
                return Redirect("/Trips/All");
            }

            return View();
        }

        [HttpPost]
        public HttpResponse Register(RegisterUserFormModel model)
        {
            var errors = validator.ValidateUserRegistration(model);

            if (errors.Any())
            {
                return Redirect("/Users/Register");
            }

            if (this.data.Users.Any(u => u.Username == model.Username))
            {
                return Redirect("/Users/Register");
            }

            if (this.data.Users.Any(u => u.Email == model.Email))
            {
                return Redirect("/Users/Register");
            }

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                Password = passwordHasher.HashPassword(model.Password),
            };

            this.data.Users.Add(user);

            this.data.SaveChanges();

            return Redirect("/Users/Login");
        }

        [Authorize]
        public HttpResponse Logout()
        {
            this.SignOut();

            return Redirect("/");
        }
    }
}
