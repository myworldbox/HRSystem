using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static HRSystem.Domain.Enums;

namespace HRSystem.Domain.Entities;
public record Staff
{
    [Key]
    public int StaffNo { get; set; }
    public string UserId { get; set; }
    [StringLength(20)]
    public string StaffName { get; set; } = null!;
    public DateOnly BirthDate { get; set; }
    [StringLength(50)]
    public string? Address { get; set; }
    [StringLength(20)]
    public string EmailAddress { get; set; } = null!;
    [StringLength(15)]
    public string? PhoneNumber { get; set; }
    public Status Status { get; set; } = Status.Draft;
    [ForeignKey("UserId")]
    public virtual IdentityUser User { get; set; }
    [InverseProperty("StaffNoNavigation")]
    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public void Confirm() => Status = Status.Confirmed;
    public void Delete() => Status = Status.Deleted;
}