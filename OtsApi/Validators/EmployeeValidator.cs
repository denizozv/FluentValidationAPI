using FluentValidation;
using OtsApi.Models;

namespace OtsApi.Validators;

public class EmployeeValidator : AbstractValidator<Employee>
{
    public EmployeeValidator()
    {

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(3, 10).WithMessage("Name must be between 3 and 10 characters");


        RuleFor(x => x.Surname)
            .NotEmpty().WithMessage("Surname is required")
            .Length(3, 15).WithMessage("Surname must be between 3 and 15 characters");


        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required").Must(email => email.Contains("@")).WithMessage("Email must contain '@' character").EmailAddress().WithMessage("Invalid Email Address");


        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^\d{10,15}$").WithMessage("Invalid phone number format. Must be 10-15 digits.");


        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required");


        RuleFor(x => x.Age)
            .NotEmpty().WithMessage("Age is required.").InclusiveBetween(18, 60).WithMessage("Age must be between 18 and 60");


        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of Birth is required")
            .LessThan(DateTime.Today).WithMessage("Date of Birth cannot be in the future");


        RuleFor(x => x.Age).Must((employee, age) => age == CalculateAge(employee.DateOfBirth))
    .WithMessage("Age must match the calculated age based on Date of Birth");

    }

    //age with date of birth 
    private static int CalculateAge(DateTime birthDate)
    {
        int age = DateTime.Today.Year - birthDate.Year;
        if (birthDate.Date > DateTime.Today.AddYears(-age)) age--;
        return age;
    }
}
