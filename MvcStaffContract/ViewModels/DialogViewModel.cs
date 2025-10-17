using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcStaffContract.Cores;
using MvcStaffContract.Helpers;
using static MvcStaffContract.Cores.Enums;
using static MvcStaffContract.Helpers.ValidationHelper;

namespace MvcStaffContract.ViewModels;

public record DialogViewModel
{
    public Dialog dialog { get; set; }
    public string message { get; set; }
}
