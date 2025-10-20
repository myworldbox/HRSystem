using HRSystem.Cores;
using HRSystem.Models;
using HRSystem.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace HRSystem.Helpers;

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

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AttrRange : ValidationAttribute
    {
        private readonly string? _lower, _upper;

        public AttrRange(string lower, string? upper = null)
        {
            _lower = lower;
            _upper = upper;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
        {
            if (value is not IComparable v) return ValidationResult.Success;

            object? lo = string.IsNullOrEmpty(_lower) ? null : ctx.ObjectType.GetProperty(_lower)?.GetValue(ctx.ObjectInstance);
            object? hi = string.IsNullOrEmpty(_upper) ? null : ctx.ObjectType.GetProperty(_upper)?.GetValue(ctx.ObjectInstance);

            bool below = lo is IComparable l && v.GetType() == l.GetType() && v.CompareTo(l) < 0;
            bool above = hi is IComparable h && v.GetType() == h.GetType() && v.CompareTo(h) > 0;

            if (below || above)
            {
                string msg = _lower != null && _upper != null && lo != null && hi != null
                    ? $"{ctx.MemberName} must be between {_lower} ({lo}) and {_upper} ({hi})."
                    : below ? $"{ctx.MemberName} must be ≥ {_lower} ({lo})."
                    : $"{ctx.MemberName} must be ≤ {_upper} ({hi}).";
                return new ValidationResult(msg);
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