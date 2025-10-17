using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using static MvcStaffContract.Helpers.ValidationHelper;

namespace MvcStaffContract.ViewModels;

public record StaffViewModel
{
    [Key]
    [Display(Name = "Staff No")]
    public int StaffNo { get; set; }

    [StringLength(20)]
    [Display(Name = "Staff Name")]
    public string StaffName { get; set; } = null!;

    [DataType(DataType.Date)]
    [Display(Name = "Birth Date")]
    [MonthRange(-12 * 65, 0, ErrorMessage = "Birthday must be within 65 years.")]
    public DateOnly BirthDate { get; set; }

    [StringLength(12)]
    [Display(Name = "Address 1")]
    public string? Address1 { get; set; }

    [StringLength(12)]
    [Display(Name = "Address 2")]
    public string? Address2 { get; set; }

    [StringLength(12)]
    [Display(Name = "Address 3")]
    public string? Address3 { get; set; }

    [StringLength(12)]
    [Display(Name = "Address 4")]
    public string? Address4 { get; set; }

    [StringLength(20)]
    [Display(Name = "Email Address")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string EmailAddress { get; set; } = null!;

    [RegularExpression(@"^\+?[0-9\s\-]{7,15}$", ErrorMessage = "Phone number must be 7–15 digits and may include +, spaces, or dashes.")]
    [Display(Name = "Phone Number")]
    [StringLength(15)]
    public string? PhoneNumber { get; set; }
}
