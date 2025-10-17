using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MvcStaffContract.Models;

public partial class StaffModel
{
    [Key]
    public int StaffNo { get; set; }

    [StringLength(20)]
    public string StaffName { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    [StringLength(50)]
    public string? Address { get; set; }

    [StringLength(20)]
    public string EmailAddress { get; set; } = null!;

    [StringLength(15)]
    public string? PhoneNumber { get; set; }

    [InverseProperty("StaffNoNavigation")]
    public virtual ICollection<ContractModel> Contracts { get; set; } = new List<ContractModel>();
}
