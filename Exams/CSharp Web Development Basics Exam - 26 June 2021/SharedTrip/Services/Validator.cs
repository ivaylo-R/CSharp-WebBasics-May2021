using SharedTrip.Data;
using SharedTrip.Models.Trips;
using SharedTrip.Models.Users;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SharedTrip.Services
{
    using static DataConstants;

    public class Validator : IValidator
    {
        public ICollection<string> ValidateTrip(AddTripFormModel model)
        {
            List<string> errors = new();

            if (model.Seats < SeatsMinValue || model.Seats > SeatsMaxValue)
            {
                errors.Add($"Seats must be between {SeatsMinValue} and {SeatsMaxValue}");
            }

            if (model.Description.Length > DescriptionMaxLength)
            {
                errors.Add($"Description is not valid. It must be no more than {DescriptionMaxLength} characters long.");
            }

            return errors;
        }

        public ICollection<string> ValidateUserRegistration(RegisterUserFormModel model)
        {
            List<string> errors = new();

            if (model.Username.Length < UsernameMinLength || model.Username.Length > UsernameMaxLength)
            {
                errors.Add($"Username '{model.Username}' is not valid! It must be between: {UsernameMinLength} and {UsernameMaxLength} characters long.");
            }
            if (model.Password.Length < PasswordMinLength || model.Password.Length > PasswordMaxLength)
            {
                errors.Add($"Password '{new string('*', model.Password.Length)}' is not valid! It must be between: {PasswordMinLength} and {PasswordMaxLength} characters long. ");
            }
            if (!Regex.IsMatch(model.Email, UserEmailRegularExpression))
            {
                errors.Add($"Email {model.Email} should be valid.");
            }
            if (model.ConfirmPassword != model.Password)
            {
                errors.Add($"Confirm Password is not valid! It should be the same with the original one.");
            }

            return errors;
        }
    }
}
