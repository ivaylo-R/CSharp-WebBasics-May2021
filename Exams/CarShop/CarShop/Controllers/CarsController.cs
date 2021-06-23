using CarShop.Data;
using CarShop.Data.Models;
using CarShop.Models.Cars;
using CarShop.Services;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Collections.Generic;
using System.Linq;

namespace CarShop.Controllers
{
    public class CarsController : Controller
    {
        private readonly CarShopDbContext data;
        private readonly IValidator validator;
        private readonly IUserService userService;

        public CarsController(
            CarShopDbContext data, 
            IValidator validator, 
            IUserService userService)
        {
            this.data = data;
            this.validator = validator;
            this.userService = userService;
        }

        [Authorize]
        public HttpResponse Add()
        {
            if (userService.IsMechanic(this.User.Id))
            {
                return Unauthorized();
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public HttpResponse Add(AddCarFormModel model)
        {
            if (userService.IsMechanic(this.User.Id))
            {
                return Unauthorized();
            }

            var errors = validator.ValidateCar(model);

            if (errors.Any())
            {
                return Error(errors);
            }

            var car = new Car
            {
                Model = model.Model,
                Year = model.Year,
                PlateNumber = model.PlateNumber,
                PictureUrl = model.Image,
                OwnerId = this.User.Id,
            };

            this.data.Cars.Add(car);

            this.data.SaveChanges();

            return Redirect("/Cars/All");
        }


        [Authorize]
        public HttpResponse All()
        {
            var carsQuery = this.data.Cars.AsQueryable();

            if (userService.IsMechanic(this.User.Id))
            {
                carsQuery = this.data.Cars
                    .Where(c => c.Issues.Any(i => !i.IsFixed));
            }
            else
            {
                carsQuery = this.data.Cars
                    .Where(c => c.OwnerId == this.User.Id);
            }

            var cars = carsQuery
                .Select(c => new CarListingViewModel
                {
                    Model = c.Model,
                    Year = c.Year,
                    Id = c.Id,
                    Image = c.PictureUrl,
                    PlateNumber = c.PlateNumber,
                    FixedIssues = c.Issues.Where(i => i.IsFixed).Count(),
                    RemainingIssues = c.Issues.Where(i => !i.IsFixed).Count(),
                })
                    .ToList();

            return View(cars);
        }

        
    }
}
