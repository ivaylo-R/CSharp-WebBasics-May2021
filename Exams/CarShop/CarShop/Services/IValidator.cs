using CarShop.Models.Cars;
using CarShop.Models.Users;
using System.Collections.Generic;

namespace CarShop.Services
{
    public interface IValidator
    {
        ICollection<string> ValidateUserRegistration(RegisterUserFormModel model);
        ICollection<string> ValidateCar(AddCarFormModel model);
    }
}
