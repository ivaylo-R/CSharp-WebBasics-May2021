using CarShop.Data;
using CarShop.Models.Cars;
using CarShop.Models.Users;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CarShop.Services
{
    using static DataConstants;

    public class Validator : IValidator
    {
        public ICollection<string> ValidateCar(AddCarFormModel model)
        {
            List<string> errors = new();

            if (model.Model.Length<CarModelMinLength || model.Model.Length>DefaultMaxLength)
            {
                errors.Add($"Model '{model.Model}' is not valid.It must be between: {UserMinUsername} and {DefaultMaxLength} characters long.");
            }
            if (!Regex.IsMatch(model.PlateNumber,CarPlateNumberRegularExpression))
            {
                errors.Add($"Platenumber is not valid.");
            }
            if (model.Year<CarYearMinValue && model.Year> CarYearMaxValue)
            {
                errors.Add($"Year '{model.Year}' is not valid!");
            }
            if (!Uri.IsWellFormedUriString(model.Image,UriKind.Absolute))
            {
                errors.Add($"Image url {model.Image} is not valid");
            }

            return errors;
        }

        public ICollection<string> ValidateUserRegistration(RegisterUserFormModel model)
        {
            List<string> errors = new();


            if (model.Username.Length < UserMinUsername || model.Username.Length > DefaultMaxLength)
            {
                errors.Add($"Username '{model.Username}' is not valid! It must be between: {UserMinUsername} and {DefaultMaxLength} characters long.");
            }
            if (model.Password.Length < UserMinPassword || model.Password.Length > DefaultMaxLength)
            {
                errors.Add($"Password '{model.Password[0] + new string('*',model.Password.Length)}' is not valid! It must be between: {UserMinPassword} and {DefaultMaxLength} characters long. ");
            }
            if (!Regex.IsMatch(model.Email, UserEmailRegularExpression))
            {
                errors.Add($"Email {model.Email} should be valid.");
            }
            if (model.ConfirmPassword != model.Password)
            {
                errors.Add($"Confirm Password is not valid! It should be the same with the original one.");
            }
            if (model.UserType != UserTypeMechanic && model.UserType != UserTypeClient)
            {
                errors.Add($"User must be {UserTypeMechanic} or {UserTypeClient}");
            }

            return errors;
        }
    }
}
