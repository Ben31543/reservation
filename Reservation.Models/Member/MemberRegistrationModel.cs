using Reservation.Resources.Contents;
using System;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.Member
{
	public class MemberRegistrationModel
	{
		[Required(ErrorMessage = LocalizationKeys.Contents.Name + LocalizationKeys.Errors.FieldIsRequired)]
		[StringLength(20)]
		public string Name { get; set; }

		[Required(ErrorMessage = LocalizationKeys.Contents.Surname + LocalizationKeys.Errors.FieldIsRequired)]
		[StringLength(20)]
		public string Surname { get; set; }

		[Required(ErrorMessage = LocalizationKeys.Contents.Email + LocalizationKeys.Errors.FieldIsRequired)]
		public string Email { get; set; }

		[Required(ErrorMessage = LocalizationKeys.Contents.Phone + LocalizationKeys.Errors.FieldIsRequired)]
		public string Phone { get; set; }

		[Required(ErrorMessage = LocalizationKeys.Contents.Password + LocalizationKeys.Errors.FieldIsRequired)]
		[MinLength(8), MaxLength(12)]
		public string Password { get; set; }

		[Required(ErrorMessage = LocalizationKeys.Contents.Password + LocalizationKeys.Errors.FieldIsRequired)]
		public string ConfirmPassword { get; set; }
	}
}
