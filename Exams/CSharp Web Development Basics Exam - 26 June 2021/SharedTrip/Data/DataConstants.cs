﻿namespace SharedTrip.Data
{
    public class DataConstants
    {
        public const int UsernameMaxLength = 20;
        public const int UsernameMinLength = 5;

        //I set the MaxPasswordLength to 100, because of hashing
        public const int PasswordMaxLength = 100;
        public const int PasswordMinLength = 6;

        public const int SeatsMinValue = 2;
        public const int SeatsMaxValue = 6;

        public const string UserEmailRegularExpression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        public const int DescriptionMaxLength = 80;
        public const string DefaulDateTimeFormat = "dd.MM.yyyy HH:mm";

    }
}