using FluentValidation;
using LeadManagementSystem.Shared.Contracts.Request;
using PhoneNumbers;
using System.Text.RegularExpressions;

namespace LeadManagementSystem.Logic.Handler
{
    public class LeadValidationHandler : AbstractValidator<LeadRequest>
    {
        public LeadValidationHandler()
        {
            RuleFor(request => request.IDNumber)
            .NotEmpty()
           .WithMessage("The IDNumber must not be empty.")
            .DependentRules(() =>
            {
                RuleFor(request => request.IDNumber)
                    .Matches(@"^\d+$")
                    .WithMessage("The IDNumber must contain only numbers.")
                    .DependentRules(() =>
                    {
                        RuleFor(request => request.IDNumber)
                            .Length(13)
                            .WithMessage("The IDNumber must be exactly 13 characters long.");
                    });
            });
            RuleFor(request => request.FirstName)
              .NotEmpty().WithMessage("FirstName cannot be empty.")
              .MaximumLength(50).WithMessage("FirstName cannot exceed 50 characters.")
              .Matches(@"^[a-zA-Z\s]*$").WithMessage("FirstName cannot contain numbers or special characters.");

            RuleFor(request => request.Surname)
               .NotEmpty().WithMessage("SurName cannot be empty.")
               .MaximumLength(50).WithMessage("SurName cannot exceed 50 characters.")
              .Matches(@"^[a-zA-Z\s]*$").WithMessage("SurName cannot contain numbers or special characters.");


            RuleFor(request => request.SupplierSource)
                .MaximumLength(50).WithMessage("Supplier Source cannot exceed 50 characters.");


            RuleFor(request => request.UMID)
                    .NotEmpty().WithMessage("UMID cannot be empty.")
                   .Matches(@"^\d+$").WithMessage("UMID must contain only numbers.")
                    .DependentRules(() =>
                    {
                        RuleFor(request => request.UMID)
                     .Length(8).WithMessage("UMID must be of 8 digits.");
                    });

            RuleFor(request => request.SMSresponse);


            RuleFor(request => request.CellPhone)
                  .NotEmpty().WithMessage("Cell phone number provided is in invalid format.")
                  .DependentRules(() =>
                  {
                      RuleFor(request => request.CellPhone)
                          .Matches(@"^\d+$")
                          .WithMessage("Cell phone number can only contain digits.")
                          .DependentRules(() =>
                          {
                              RuleFor(request => request.CellPhone)
                                  .Must(cellPhone => cellPhone.StartsWith("07") || cellPhone.StartsWith("08"))
                                  .WithMessage("Cell phone number must begin with '07' or '08'.")
                                  .DependentRules(() =>
                                  {
                                      RuleFor(request => request.CellPhone)
                                          .Matches(@"^[0][7,8][0-9]{8}$")
                                          .WithMessage("Cell phone number must be 10 digits long.");
                                  });
                          });
                  });

            RuleFor(request => request.Email)
               .Cascade(CascadeMode.Stop)
               .Must(email => string.IsNullOrEmpty(email) || IsValidEmail(email))
               .WithMessage("Please provide a valid email address.")
               .When(request => !string.IsNullOrEmpty(request.Email));

            RuleFor(request => request.SupplierEmail)
               .Cascade(CascadeMode.Stop)
               .Must(email => string.IsNullOrEmpty(email) || IsValidEmail(email))
               .WithMessage("Please provide a valid Supplier Email address.")
               .When(request => request.SupplierEmail != null);


            RuleFor(x => x.AlternateNumber)
                           .Cascade(CascadeMode.Stop)
                           .NotEmpty().WithMessage("Alternate number is required.")
                           .Must(BeAValidPhoneNumber).WithMessage("Alternate number provided is in invalid format.");


            // WorkNumber validation
            RuleFor(x => x.WorkNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Work number is required.")
                .Must(BeAValidPhoneNumber).WithMessage("Work number provided is in invalid format.");


            // HomeNumber validation
            RuleFor(x => x.HomeNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Home number is required.")
                .Must(BeAValidPhoneNumber).WithMessage("Home number provided is in invalid format.");

            RuleFor(request => request.AllowsCreditCheck)
                .NotNull()
                .WithMessage("AllowsCreditCheck is required.")
                .Must(value => value == true || value == false)
                .WithMessage("AllowsCreditCheck must be 'True' or 'False'.");



            RuleFor(request => request.PermissionToPromote)
                 .Must(value => value == "yes" || value == "no")
                 .WithMessage("Permission to Promote must be 'yes' or 'no'.")
                 .When(request => !string.IsNullOrEmpty(request.PermissionToPromote));


            RuleFor(request => request.GrossIncome)
                .Must(BeValidDouble).WithMessage("Gross Income must be a valid number.")
                .InclusiveBetween(0, 999999).WithMessage("Gross Income must be between 0 and 999999.");


        }

        private bool BeAValidPhoneNumber(string number)
        {
            if (string.IsNullOrEmpty(number)) return false;

            // Split code and number
            var match = Regex.Match(number, @"^(0[0-9]{2,4})([0-9]{4,6})$");
            if (!match.Success) return false;

            var code = match.Groups[1].Value;
            var phoneNumber = match.Groups[2].Value;

            // Check if total length is 10
            return code.Length + phoneNumber.Length == 10;
        }

        private bool BeValidDouble(double value)
        {

            return !double.IsNaN(value) && !double.IsInfinity(value);
        }
        private bool BeAValidSouthAfricanCode(string code)
        {
            // Validate that the code is exactly 3 digits and starts with '0'
            return Regex.IsMatch(code, @"^0\d{2}$");
        }

        private bool BeAValidPhoneNumberFormat(string phoneNumber)
        {

            var code = phoneNumber.Substring(0, 3);
            var number = phoneNumber.Substring(3);

            // Validate the code and number
            return BeAValidSouthAfricanCode(code) && Regex.IsMatch(number, @"^\d{7}$");
        }


        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);

                return addr.Address == email && Regex.IsMatch(email,
                  @"^[^@\s]+@[^@\s]+\.(com|(net|org|gov|co)(\.(in|za))?)$", RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }

    }
}
