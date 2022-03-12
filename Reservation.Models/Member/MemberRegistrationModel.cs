using Reservation.Resources.Contents;
using System;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.Member
{
	public class MemberRegistrationModel
	{
		[Required(ErrorMessage = LocalizationKeys.ErrorMessages.ThisFieldIsRequired)]
		[StringLength(20)]
		public string Name { get; set; }

		[Required(ErrorMessage = LocalizationKeys.ErrorMessages.ThisFieldIsRequired)]
		[StringLength(20)]
		public string Surname { get; set; }

		[Required(ErrorMessage = LocalizationKeys.ErrorMessages.ThisFieldIsRequired)]
		public DateTime BirthDate { get; set; }

		[Required(ErrorMessage = LocalizationKeys.ErrorMessages.ThisFieldIsRequired)]
		public string Email { get; set; }

		[Required(ErrorMessage = LocalizationKeys.ErrorMessages.ThisFieldIsRequired)]
		public string Phone { get; set; }

		[Required(ErrorMessage = LocalizationKeys.ErrorMessages.ThisFieldIsRequired)]
		[MinLength(8), MaxLength(12)]
		public string Password { get; set; }

		[Required(ErrorMessage = LocalizationKeys.ErrorMessages.ThisFieldIsRequired)]
		public string ConfirmPassword { get; set; }
	}
}
