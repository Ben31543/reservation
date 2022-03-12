﻿namespace Reservation.Data.Constants
{
	public static class CommonConstants
	{
		public static string[] PhoneCodes = { "33", "41", "43", "44", "55", "77", "91", "93", "94", "95", "96", "97", "98", "99" };
	}

	public static class Localizations
	{
		public static class Errors
		{
			public const string WrongIncomingParameters = "WrongIncomingParameters";
			public const string BankAccountDoesNotExist = "BankAccountDoesNotExist";
			public const string BankCardDoesNotExist = "BankCardDoesNotExist";
			public const string BankCardExpired = "BankCardExpired";
			public const string DishDoesNotExist = "DishDoesNotExist";
			public const string MemberDoesNotExist = "MemberDoesNotExist";
			public const string WrongCredientials = "WrongCredientials";
			public const string BranchNotFound = "BranchDoesNotExist";
			public const string ServiceMemberDoesNotExist = "ServiceMemberDoesNotExist";
			public const string BankAccountNotAttachedToServiceMember = "BankAccountNotAttachedToServiceMember";
			public const string InvalidPhoneNumber = "InvalidPhoneNumber";
			public const string InvalidEmail = "InvalidEmail";
			public const string InvalidDate = "InvalidDate";
			public const string ThisFieldIsRequired = "ThisFieldIsRequired";
			public const string PasswordDoNotMatch = "PasswordDoNotMatch";
			public const string OpenTimeMustBeEarlierThanCloseTime = "OpenTimeMustBeEarlierThanCloseTime";
		}
	}
}
