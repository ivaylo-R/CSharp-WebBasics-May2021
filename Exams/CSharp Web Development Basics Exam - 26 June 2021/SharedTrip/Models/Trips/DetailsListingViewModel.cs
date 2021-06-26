using System;

namespace SharedTrip.Models.Trips
{
    public class DetailsListingViewModel
    {
        public string TripId { get; init; }

        public string ImagePath { get; init; }

        public string StartPoint { get; init; }

        public string EndPoint { get; init; }

        public string DepartureTime { get; init; }

        public int Seats { get; init; }

        public string Description { get; init; }

    }
}
