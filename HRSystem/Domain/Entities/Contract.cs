using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using static HRSystem.Domain.Enums;
public partial class Contract
{
    [Key]
    public int? Id { get; set; }
    [StringLength(7)]
    public string? ContractNumber { get; set; }
    public int StaffNo { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    [StringLength(20)]
    public string Position { get; set; } = null!;
    [Column(TypeName = "decimal(8, 2)")]
    public decimal Salary { get; set; }
    public DateOnly? CessationDate { get; set; }
    public Status Status { get; set; }
    [ForeignKey("StaffNo")]
    [InverseProperty("Contracts")]
    public virtual Staff StaffNoNavigation { get; set; } = null!;
}
