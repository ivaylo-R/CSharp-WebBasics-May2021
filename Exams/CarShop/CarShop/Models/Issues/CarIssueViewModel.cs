using System.Collections.Generic;

namespace CarShop.Models.Issues
{
    public class CarIssueViewModel
    {
        public string Id { get; init; }

        public string Model { get; init; }

        public int Year { get; init; }

        public IEnumerable<IssueListingViewModel> Issues { get; init; }
    }
}
