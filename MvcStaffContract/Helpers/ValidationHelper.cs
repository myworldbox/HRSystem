using MvcStaffContract.Cores;
using MvcStaffContract.Models;
using MvcStaffContract.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MvcStaffContract.Helpers;

public class ValidationHelper
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MonthRange : ValidationAttribute
    {
        private readonly int _lowerMonths;
        private readonly int _upperMonths;

        public MonthRange(int lowerMonths, int upperMonths)
        {
            _lowerMonths = lowerMonths;
            _upperMonths = upperMonths;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not DateOnly date) return ValidationResult.Success;

            var today = DateOnly.FromDateTime(DateTime.Today);
            var lowerBound = today.AddMonths(_lowerMonths);
            var upperBound = today.AddMonths(_upperMonths);

            if (date < lowerBound || date > upperBound)
            {
                var message = ErrorMessage ??
                              $"Date must be between {lowerBound:yyyy-MM-dd} and {upperBound:yyyy-MM-dd}.";
                return new ValidationResult(message);
            }

            return ValidationResult.Success;
        }
    }

    public class DateAfterProperty : ValidationAttribute
    {
        private readonly string _otherProperty;

        public DateAfterProperty(string otherProperty)
        {
            _otherProperty = otherProperty;
            ErrorMessage = $"It must be after the {_otherProperty}.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateOnly date)
            {
                var otherPropertyInfo = validationContext.ObjectType.GetProperty(_otherProperty);
                if (otherPropertyInfo == null)
                {
                    return new ValidationResult($"Unknown property {_otherProperty}.");
                }

                var otherValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance);
                if (otherValue is DateOnly otherDate)
                {
                    if (date <= otherDate)
                    {
                        return new ValidationResult(ErrorMessage);
                    }
                }
                else
                {
                    return new ValidationResult($"Invalid date format for {_otherProperty}.");
                }
            }
            else if (value != null)
            {
                return new ValidationResult("Invalid date format.");
            }

            return ValidationResult.Success;
        }
    }

    public class SalaryByPosition : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var contract = (ContractViewModel)validationContext.ObjectInstance;

            decimal min = 0, max = decimal.MaxValue;

            switch (contract.Position)
            {
                case Enums.Position.Clerk:
                    min = 15000; max = 25000;
                    break;
                case Enums.Position.Teacher:
                    min = 30000; max = 60000;
                    break;
                case Enums.Position.Principal:
                    min = 80000; max = 200000;
                    break;
                case Enums.Position.HRAssistant:
                    min = 20000; max = 40000;
                    break;
            }
            if (contract.Salary < min || contract.Salary > max)
            {
                return new ValidationResult(
                    $"{contract.Position.ToString()} must have a salary between {min:N0} and {max:N0}.",
                    [nameof(contract.Salary)]);
            }
            return ValidationResult.Success;
        }
    }
}