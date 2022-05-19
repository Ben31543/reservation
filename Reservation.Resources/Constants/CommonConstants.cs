using System;

namespace Reservation.Resources.Constants
{
    public static class CommonConstants
    {
        public static int CurrentHour = DateTime.Now.Hour;
        public static readonly string[] SupportedLanguages =
        {
            "hy", "arm", "hy-AM",
            "ru", "rus", "ru-RU",
            "en", "eng", "en-US"
        };
        public static readonly string[] ArmenianPhoneOperators = {"10", "11", "12", "33", "41", "43", "44", "55", "77", "91", "93", "94", "95", "96", "97", "98", "99"};
        
        public const int BankCardNumberLength = 16;
        public const int BankAccountNumberLength = 12;
    }

    public static class Cookies
    {
        public const string Language = "lang";
    }

    public static class ImageSaverConstants
    {
        public const string ImagesHostingPath = "https://lines98.am/Drive/Reservation";
        public const string ImageSavingServiceHost = "https://lines98.am/cseventlogger/";
    }

    public static class EmailCredentials
    {
        public const string Username = "onlinereservationservice@gmail.com";
        public const string Password = "onlinereservationservice2022_";

        public const string ReservationAdmin1Email = "benatanesyan2@gmail.com";
        public const string ReservationAdmin2Email = "lia.2003.tantushyan@gmail.com";
    }
}