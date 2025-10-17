using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcStaffContract.Cores;
using MvcStaffContract.Helpers;
using static MvcStaffContract.Cores.Enums;
using static MvcStaffContract.Helpers.ValidationHelper;

namespace MvcStaffContract.ViewModels;

[Table("Contract")]
public record ContractViewModel
{
    [Key]
    public int? Id { get; set; }

    [Ignore]
    [Display(Name = "Contract Number")]
    public string? ContractNumber { get; set; }

    [Display(Name = "Staff No")]
    public int StaffNo { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Start Date")]
    [MonthRange(-6, 6, ErrorMessage = "Date must be within the last 6 months and the next 6 months.")]
    public DateOnly StartDate { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "End Date")]
    [DateAfterProperty(nameof(StartDate))]
    public DateOnly EndDate { get; set; }
    public Position Position { get; set; }

    [Range(0, 999999.99, ErrorMessage = "Salary must be a positive value")]
    [Column(TypeName = "decimal(8, 2)")]
    [SalaryByPosition]
    public decimal Salary { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Cessation Date")]
    public DateOnly? CessationDate { get; set; }
    public Status Status { get; set; }

    [Ignore]
    public SelectList? ListStaffNo { get; set; }
}
