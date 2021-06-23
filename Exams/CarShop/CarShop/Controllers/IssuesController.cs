using CarShop.Data;
using CarShop.Data.Models;
using CarShop.Models.Issues;
using CarShop.Services;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Collections.Generic;
using System.Linq;

namespace CarShop.Controllers
{
    public class IssuesController : Controller
    {
        private readonly CarShopDbContext data;
        private readonly IUserService userService;

        public IssuesController(
            CarShopDbContext data,
            IUserService userService)
        {
            this.data = data;
            this.userService = userService;
        }

        [Authorize]
        public HttpResponse Add()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public HttpResponse Add(string carId,string description)
        {
            var car = this.data.Cars.FirstOrDefault(c => c.Id == carId);

            if (car == null)
            {
                return Error("This car does not exists.");
            }

            var issue = new Issue
            {
                Car = car,
                CarId = carId,
                Description = description,
                IsFixed = false,
            };

            this.data.Issues.Add(issue);

            this.data.SaveChanges();

            return Redirect($"/Issues/CarIssues?carId={carId}");
        }

        [Authorize]
        public HttpResponse Delete(string issueId,string carId)
        {
            var issue = this.data.Issues.FirstOrDefault(i => i.Id == issueId && i.CarId == carId);
            if (issue==null)
            {
                return Error("Issue does not exists.");
            }

            this.data.Issues.Remove(issue);

            this.data.SaveChanges();

            return Redirect($"/Issues/CarIssues?carId={carId}");
        }

        [Authorize]
        public HttpResponse Fix(string issueId, string carId)
        {
            var issue = this.data.Issues.FirstOrDefault(i => i.Id == issueId && i.CarId == carId);
            if (issue == null)
            {
                return Error("Issue does not exists.");
            }

            if (issue.IsFixed==true)
            {
                return Redirect($"/Issues/CarIssues?carId={carId}");
            }

            if (!userService.IsMechanic(this.User.Id))
            {
                return Unauthorized();
            }

            issue.IsFixed = true;

            this.data.SaveChanges();

            return Redirect($"/Issues/CarIssues?carId={carId}");
        }

        public HttpResponse CarIssues(string carId)
        {
            if (!this.userService.IsMechanic(this.User.Id))
            {
                var userOwnsCar = this.data.Cars
                    .Any(c => c.Id == carId && c.OwnerId == this.User.Id);

                if (!userOwnsCar)
                {
                    return Error("You do not have access to this car.");
                }
            }

            var carWithIssues = this.data
                .Cars
                .Where(c => c.Id == carId)
                .Select(c => new CarIssueViewModel
                {
                    Id = c.Id,
                    Model = c.Model,
                    Year = c.Year,
                    Issues = c.Issues.Select(i => new IssueListingViewModel
                    {
                        Id = i.Id,
                        IsFixed = i.IsFixed,
                        Description = i.Description,
                    })
                })
                .FirstOrDefault();

            if (carWithIssues == null)
            {
                return Error($"Car with ID: {carWithIssues.Id} does not exists.");
            }

            return View(carWithIssues);
        }
    }
}
