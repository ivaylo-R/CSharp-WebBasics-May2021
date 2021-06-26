using MyWebServer.Controllers;
using MyWebServer.Http;
using SharedTrip.Data;
using SharedTrip.Models;
using SharedTrip.Models.Trips;
using SharedTrip.Services;
using System;
using System.Globalization;
using System.Linq;

namespace SharedTrip.Controllers
{
    using static DataConstants;

    public class TripsController : Controller
    {
        private readonly ApplicationDbContext data;
        private readonly IValidator validator;

        public TripsController(ApplicationDbContext data, 
            IValidator validator)
        {
            this.data = data;
            this.validator = validator;
        }

        [Authorize]
        public HttpResponse All()
        {
            var trips = this.data.Trips
                .Select(c => new TripsListingViewModel
                {
                     Id=c.Id,
                     StartPoint=c.StartPoint,
                     EndPoint=c.EndPoint,
                     DepartureTime=c.DepartureTime.ToString("dd.MM.yyyy HH:mm"),
                     Seats=c.Seats,
                })
                .ToList();

            return View(trips);
        }

        [Authorize]
        [HttpGet]
        public HttpResponse Add() => View();

        [Authorize]
        [HttpPost]
        public HttpResponse Add(AddTripFormModel model)
        {
            var errors = validator.ValidateTrip(model);

            var tryParseDepartureTime = DateTime.TryParseExact(
                model.DepartureTime,
                DefaulDateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var departureTime
                );

            if (!tryParseDepartureTime)
            {
                errors.Add("Invalid DateTime format.");
            }

            if (errors.Any())
            {
                return Redirect("/Trips/All");
            }

            var trip = new Trip
            {
                StartPoint = model.StartPoint,
                EndPoint = model.EndPoint,
                DepartureTime = departureTime,
                ImagePath = model.ImagePath,
                Seats=model.Seats,
                Description=model.Description,
            };

            this.data.Trips.Add(trip);

            this.data.SaveChanges();

            return Redirect("/Trips/All");
        }

        [Authorize]
        public HttpResponse Details(string tripId)
        {
            var trip = this.data.Trips.FirstOrDefault(t => t.Id == tripId);

            if (trip==null)
            {
                return BadRequest();
            }

            if (trip.Seats==0)
            {
                return Redirect($"/Trips/Details?tripId={tripId}");
            }

            var details = new DetailsListingViewModel
            {
                TripId=tripId,
                ImagePath=trip.ImagePath,
                StartPoint = trip.StartPoint,
                EndPoint = trip.EndPoint,
                DepartureTime = trip.DepartureTime.ToString(DefaulDateTimeFormat),
                Seats = trip.Seats,
                Description = trip.Description,
            };

            return View(details);
        }

        [Authorize]
        public HttpResponse AddUserToTrip(string tripId)
        {
            var trip = this.data.Trips.FirstOrDefault(t => t.Id == tripId);

            if (trip == null)
            {
                return BadRequest();
            }

            var user = this.data.Users.FirstOrDefault(u => u.Id == this.User.Id);

            if (user == null)
            {
                return BadRequest();
            }

            if (this.data.UserTrips.Any(ut => ut.UserId == user.Id 
                        && ut.TripId==trip.Id))
            {
                return Redirect($"/Trips/Details?tripId={trip.Id}");
            }

            trip.Seats -= 1;

            this.data.UserTrips.Add(new UserTrip { User = user, Trip = trip, });

            this.data.SaveChanges();

            return Redirect("/Trips/All");
        }
    }
}
